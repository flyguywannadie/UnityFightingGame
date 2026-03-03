using System;
using System.Collections.Generic;
using UnityEngine;

public enum CommonAnimations
{
	CUSTOM = 0,
	IDLE = 1,
	WALKING = 2,
	BLOCKSTUN = 3,
	HITSTUN = 4,
	JUMP = 5,
	INAIR = 6,
	INAIRKNOCKDOWNUP = 7,
	INAIRKNOCKDOWNMID = 8,
	INAIRKNOCKDOWNDOWN = 9,
	KNOCKDOWN = 10,
	ONGROUND = 11,
	GETUP = 12,
	CROUCH = 13,
	BACKWALKING = 14,
	CROUCHBLOCK = 15,
	CROUCHHIT = 16,
	AIRBLOCK = 17,
	AIRHIT = 18,
	RUN = 19,
}

public enum CharacterState
{
	STANDING = 0,
	WALKING = 1,
	CROUCHING = 2,
	JUMP = 3,
	INAIR = 4,
	ATTACK = 5,
	BLOCKSTUN = 6,
	HITSTUN = 7,
	KNOCKDOWN = 8,
	ONGROUND = 9,
	NOACTION = 10,
	RUNNING = 11,
}

public abstract class BaseCharacter : MonoBehaviour
{
	[SerializeField] protected int maxHealth = 200;
	[SerializeField] protected int health = 200;
	[SerializeField] protected int speed = 5;
	[SerializeField] protected float jumpPower = 15;
	private float gravity = -40f;
	[SerializeField] protected int combo = 0;
	[SerializeField] public Vector2 motion { get; protected set; }
	[SerializeField] protected float knockback;
	[SerializeField] protected float weight = 12;
	[SerializeField] protected Transform whoIMove;
	[SerializeField] protected SpriteRenderer myVisuals;
	[SerializeField] protected CharacterAnimator animator;
	//[SerializeField] protected Animation anims;
	[SerializeField] protected bool inControl;
	//[SerializeField] protected bool onGround = true;
	[SerializeField] protected int hitstun = 0;
	[SerializeField] protected int knocked = 0;
	[SerializeField] private Transform otherPerson;
	[SerializeField] private BaseState[] states;
	[SerializeField] private int stateIndex = 0;
	[SerializeField] private int queuedState = 0;
	[SerializeField] private BufferedInput myLastInput;
	[SerializeField] private List<CharacterMove> moves;
	[SerializeField] private string movePath;
	[SerializeField] private bool cancelable;
	[SerializeField] private int priority;

	[SerializeField] private InputBuffer inputBuffer;

	[SerializeField] private List<GameObject> Spawnables;

	public void Start()
	{
		inControl = true;
		if (whoIMove == null)
		{
			whoIMove = transform;
		}
		myLastInput = new BufferedInput();

		health = maxHealth;

		InitializeStates();
		editorHitboxes = false;
		SetState(CharacterState.STANDING);
        moves = new List<CharacterMove>(Resources.LoadAll<CharacterMove>(movePath));
    }

	public void SetPlayerStatus(bool player1)
	{
		if (player1)
		{
			this.tag = "Player1";
		} else
		{
			this.tag = "Player2";
		}
	}

	protected virtual void InitializeStates()
	{
		states = new BaseState[]
		{
			new State_Standing(),
			new State_Walking(),
			new State_Crouching(),
			new State_JumpCrouch(),
			new State_InAir(),
			new State_Attack(),
			new State_Blockstun(),
			new State_Hitstun(),
			new State_Knockdown(),
			new State_OnGround(),
			new State_NoAction(),
			new State_Running(),
		};
	}

	public virtual void CharUpdate(BufferedInput input)
	{
		if (health <= 0)
		{
			input.Clear();
		}

		bool faceBack = AmIFacingBackward();
		if (faceBack)
		{
			input.FlipForwardBack();
		}

		myLastInput.CopyInput(input);

		bool currentlyGrounded = IsOnGround();
		if (currentlyGrounded)
		{
			if (inControl)
			{
				Vector3 s = whoIMove.localScale;
				if (faceBack)
				{
					whoIMove.localScale = new Vector3(Mathf.Abs(s.x) * -1, s.y, s.z);
				}
				else
				{
					whoIMove.localScale = new Vector3(Mathf.Abs(s.x), s.y, s.z);
				}
					
				//myVisuals.flipX = faceBack;
			}

			knockback = Mathf.MoveTowards(knockback, 0.0f, weight * Time.fixedDeltaTime);
		}
		else
		{
			AddMotion(0, gravity * Time.fixedDeltaTime);
			//if (!(stateIndex == (int)CharacterState.INAIR))
			//{
			//	SetState(CharacterState.INAIR);
			//	ChangeState();
			//}
		}

		if (knocked > 0)
		{
			if (currentlyGrounded)
			{
				knocked -= 1;
				
				if (knocked <= 0)
				{
					SetAnimation(CommonAnimations.GETUP);
					GainControl();
				}
			}
		}
		else if (hitstun > 0)
		{
			hitstun -= 1;

			if (hitstun <= 0)
			{
				if (!currentlyGrounded)
				{
					SetState(CharacterState.INAIR);
				}
				else if (input.Down())
				{
					SetState(CharacterState.CROUCHING);
				}
				else
				{
					SetState(CharacterState.STANDING);
				}
				GainControl();
			}
		}

		states[stateIndex].StateUpdate(this, input);
		
		if ((inControl || cancelable) && input.PressingAttacks())
		{
			TryMoves();
		}
		//else if (CompareCurrentState(CharacterState.ATTACK))
		//{

		//}

		animator.AnimatorUpdate(this);

		MoveCharacter();

		if (queuedState != stateIndex)
		{
			ChangeState();
		}
	}

	protected virtual void SpawnProjectile(int index, Vector3 offset)
	{
		if (Spawnables.Count <= 0)
		{
			Debug.LogError("Spawnables must have objects if you want to use the SpawnProjectile function");
			return;
		}

		if (index < 0 || index >= Spawnables.Count)
		{
			Debug.LogWarning("The index of " + index + "is outside the bounds of Spawnables and has been clamped\nYour used projectile may not be correct");
		}

		index = Mathf.Clamp(index, 0, Spawnables.Count - 1);

		if (Spawnables[index] == null)
		{
            Debug.LogError("Spawnable at index " + index + " is null");
            return;
		}

		ProjectileScript proj = Instantiate(Spawnables[index], transform.position, Quaternion.identity).GetComponent<ProjectileScript>();

		if (AmIFacingBackward())
		{
			offset.x *= -1;
			proj.transform.localScale = new Vector3(Mathf.Abs(proj.transform.localScale.x) * -1, proj.transform.localScale.y, proj.transform.localScale.z);
        }

		proj.gameObject.transform.position += offset;
		proj.tag = this.tag;
		proj.SetInstigator(this);

		GameManager.instance.AddProjectile(proj);
	}

	protected virtual void MoveCharacter()
	{
		bool currentlyGrounded = IsOnGround();
		Vector2 usedMotion = motion;

		usedMotion.x += knockback;

		whoIMove.Translate(usedMotion * Time.fixedDeltaTime);

		float xclamp = Mathf.Clamp(whoIMove.position.x, -9.0f, 8.8f);
		if (AmIFacingBackward())
		{
			xclamp = Mathf.Clamp(whoIMove.position.x, -8.8f, 9.0f);
		}

		whoIMove.position = new Vector3(xclamp, whoIMove.position.y,0);

		if (!currentlyGrounded && IsOnGround())
		{
			if (hitstun <= 0)
			{
				if (myLastInput.Down())
				{
					SetState(CharacterState.CROUCHING);
				}
				else
				{
					SetState(CharacterState.STANDING);
				}
			}

			LandFromAir();
		}
	}

    protected virtual bool TryMoves()
    {
        if (queuedState != stateIndex)
        {
            ChangeState();
            if (!inControl)
            {
                return false;
            }
        }

		CharacterMove move = null;
        List<CharacterMove> usedMoves = new List<CharacterMove>();

        foreach (CharacterMove m in moves)
        {
            if (m.cancelPriority > priority)
            {
                if (!IsOnGround() == m.inAir)
                {
                    bool correctButtons;

					if (m.AnyOfTheRequiredInputs)
					{
                        correctButtons = !(m.Light ^ myLastInput.Light()) || !(m.Heavy ^ myLastInput.Heavy()) || !(m.Special ^ myLastInput.Special());
                    } 
					else
					{
                        correctButtons = !(m.Light ^ myLastInput.Light()) && !(m.Heavy ^ myLastInput.Heavy()) && !(m.Special ^ myLastInput.Special());
                    }

                    //Debug.Log(m.Light + " " + myLastInput.Light() + " " + m.Heavy + " " + myLastInput.Heavy() + " " + m.Special + " " + myLastInput.Special());

                    //Debug.Log(!(m.Light ^ myLastInput.Light()) + " " + !(m.Heavy ^ myLastInput.Heavy()) + " " + !(m.Special ^ myLastInput.Special()));

                    //Debug.Log(correctButtons);

                    if (correctButtons)
					{
						bool correctMotion = inputBuffer.ReadBufferForMotion(m.motion, AmIFacingBackward());
						if (correctMotion)
						{
                            usedMoves.Add(m);
						}
					}
                }
            }
        }

        if (usedMoves.Count <= 0)
        {
            return false;
        }
		move = usedMoves[0];

		//Debug.Log(usedMoves.Count);

		foreach (CharacterMove m2 in usedMoves)
		{
			if (m2.GetMovePriority() > move.GetMovePriority())
			{
				move = m2;
			}
		}

        priority = move.cancelPriority;
        cancelable = false;
        SetState(CharacterState.ATTACK);
        SetAnimation(move.anim.GetAnimID(), false);
        return true;
    }

    public virtual void LoseControl()
	{
		inControl = false;
	}

	public virtual void GainControl()
	{
		inControl = true;
	}

	public void LoseCombo()
	{
		combo = 0;
	}

	public void AnimStop()
	{
		SetMotion(0, 0);
	}

	public virtual void JumpAction()
	{
		AddMotion(0, jumpPower);
		whoIMove.Translate(motion * Time.fixedDeltaTime);
		SetState(CharacterState.INAIR);
	}

	public virtual void LandFromAir()
	{
		whoIMove.position = new Vector3(whoIMove.position.x, 0, 0);
		SetMotion(0, 0);
	}

	public virtual bool IsOnGround()
	{
		return (whoIMove.position.y <= 0.0f);
	}

	public virtual void SetMotion(float x, float y)
	{
		motion = new Vector2(x, y);
	}

	public virtual void AddMotion(float x, float y)
	{
		motion += new Vector2(x, y);
	}

	public void SetAnimation(CommonAnimations animID)
	{
		SetAnimation((int)animID);
	}

	public void SetAnimation(int animID, bool ignoreSameID = true)
	{
		if (animator.GetCurrentAnimationID() == animID && ignoreSameID)
		{
			return;
		}
		animator.ChangeAnimationToID(animID);
	}

	public void SetState(CharacterState state)
	{
		if (state != CharacterState.ATTACK)
		{
			cancelable = false;
			priority = 0;
		}

		SetState((int)state);
	}

	public void SetState(int state)
	{
		queuedState = state;
		//ChangeState();
	}

	private void ChangeState()
	{
		states[stateIndex].OnExitState(this, myLastInput);
		stateIndex = queuedState;
		states[stateIndex].OnEnterState(this, myLastInput);
	}

	public virtual void GetHit(HurtboxProperties property)
	{
		bool flipKnockback = !AmIFacingBackward();

		if (property.HasTag(AttackTags.ONLYPUSH))
		{
			float push = property.knockback.x;

			if (flipKnockback)
			{
				push *= -1;
			}

			ProcessHit(0, hitstun, push);
			return;
		}

		int damage = property.damage;
		bool currentlyGrounded = IsOnGround();
		if (CompareCurrentState(CharacterState.ONGROUND))
		{
			if (currentlyGrounded && property.HasTag(AttackTags.OTG))
			{
				knocked = 1;
				ProcessHit(damage, 0, 0);
				combo += 1;
			}
			return;
		}

		bool blocked = false;
		if (property.attackHeight != AttackHeight.UNBLOCKABLE) {
			blocked = states[stateIndex].WasAttackBlocked(myLastInput, property);
		}

		int stun = property.hitstun;
		float knockback = property.knockback.x;
		float yknockback = property.knockback.y / (float)Mathf.Max(combo, 1);
		//Debug.Log(yknockback + " - " + combo + " - " + Mathf.Max(combo, 1));
		if (blocked)
		{
			damage = 0;
			stun = property.blockstun;
			//knockback *= 0.5f;
		} else
		{
			if (currentlyGrounded)
			{
				if (property.HasTag(AttackTags.LAUNCH))
				{
					SetMotion(motion.x, Mathf.Max(yknockback, 0.0f));
				}
			} else
			{
				SetMotion(motion.x, yknockback);
			}
		}

		if (flipKnockback)
		{
			knockback *= -1;
		}

		if (CompareCurrentState(CharacterState.KNOCKDOWN) ||
				(currentlyGrounded && property.HasTag(AttackTags.KNOCKDOWN)) ||
				(!currentlyGrounded && property.HasTag(AttackTags.AIRKNOCK)))
		{
			SetState(CharacterState.KNOCKDOWN);
			knocked = 30;
			combo += 1;
		}
		else if (blocked)
		{
			SetState(CharacterState.BLOCKSTUN);
		}
		else
		{
			SetState(CharacterState.HITSTUN);
			combo += 1;
		}

		ProcessHit(damage, stun, knockback);
	}

	protected virtual void ProcessHit(int damage, int stun, float knockback)
	{
		this.health -= damage;
		this.hitstun = stun;
		this.knockback = knockback;

		ChangeState();
	}

	public virtual void ResetChar()
	{
		combo = 0;
		health = maxHealth;
		hitstun = 0;
		knocked = 0;
		knockback = 0;
		motion = Vector2.zero;
		SetState(CharacterState.STANDING);
        SetAnimation(CommonAnimations.IDLE);
	}

	public int GetHitstun()
	{
		return hitstun;
	}

	public int GetSpeed()
	{
		return speed;
	}

	public int GetMaxHealth()
	{
		return maxHealth;
	}

	public int GetCombo()
	{
		return combo;
	}

	public int GetHealth()
	{
		return health;
	}

	public bool AmIFacingBackward()
	{
		return (otherPerson.position.x < whoIMove.position.x);
	}

	public bool CompareCurrentState(CharacterState state)
	{
		return (int)state == stateIndex;
	}

	public void SetCancelable(bool yn)
	{
		cancelable = yn;
	}

	[SerializeField] private bool editorHitboxes;

	private void OnDrawGizmos()
	{
		int currentAnimation = animator.currentAnimation;
		int currentFrame = animator.currentFrame;

		if (animator.animations.Count <= 0 || animator.animations[currentAnimation] == null)
		{
			return;
		}

		if (!editorHitboxes)
		{
			return;
		}

		CharacterAnimation current = animator.animations[currentAnimation];

		//animator.currentFrame = Mathf.Clamp(currentFrame, 0, current.GetAnimationDuration());

		//animator.visuals.sprite = current.GetCurrentSprite(currentFrame);

		CharacterAnimation.FrameData frameData = current.GetHitboxData(current.GetHitboxDataIndex(currentFrame));

		DrawBoxes(frameData);
	}

	private void DrawBoxes(CharacterAnimation.FrameData frameData)
	{
		var scaled = whoIMove.lossyScale;

		foreach (BaseBoxData box in frameData.hitboxes)
		{
			Vector3 usedPos = box.position * scaled;
			if (myVisuals.flipX)
			{
				usedPos.x *= -1;
			}

			Gizmos.color = Color.cyan;
			Gizmos.DrawWireCube(transform.position + usedPos, box.size * scaled);
		}

		foreach (HurtBoxData box in frameData.hurtboxes)
		{
			Vector3 usedPos = box.position * scaled;
			if (myVisuals.flipX)
			{
				usedPos.x *= -1;
			}

			Gizmos.color = Color.red;
			Gizmos.DrawWireCube(transform.position + usedPos, box.size * scaled);
		}
	}
}
