using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Windows;
using UnityEngine.WSA;
using static Unity.Collections.AllocatorManager;

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
	LIGHT = 19,
	LIGHTC = 20,
	LIGHTA = 21,
	HEAVY = 22,
	HEAVYC = 23,
	HEAVYA = 24,
	SPECIAL = 25,
	SPECIALC = 26,
	SPECIALA = 27,
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
	[SerializeField] private MoveDefinition[] moves;

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
		
		if (inControl)
		{
			TryAttacks();
		}

		animator.AnimatorUpdate(this);

		MoveCharacter();

		if (queuedState != stateIndex)
		{
			ChangeState();
		}
	}

	protected virtual void MoveCharacter()
	{
		bool currentlyGrounded = IsOnGround();
		Vector2 usedMotion = motion;

		usedMotion.x += knockback;

		whoIMove.Translate(usedMotion * Time.fixedDeltaTime);

		whoIMove.position = new Vector3(Mathf.Clamp(whoIMove.position.x, -9.0f, 9.0f), whoIMove.position.y,0);

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

	protected virtual void TryAttacks()
	{
		if (queuedState != stateIndex)
		{
			ChangeState();
			if (!inControl)
			{
				return;
			}
		}

		int animID = 0;

		switch(stateIndex)
		{
			case (int)CharacterState.STANDING:
			case (int)CharacterState.WALKING:
				animID += 100;
				break;
			case (int)CharacterState.CROUCHING:
				animID += 200;
				break;
			case (int)CharacterState.INAIR:
				animID += 300;
				break;
		}

		if (myLastInput.Light())
		{
			SetState(CharacterState.ATTACK);
			SetAnimation(animID);
		}

		if (myLastInput.Heavy())
		{
			SetState(CharacterState.ATTACK);
			SetAnimation(animID + 1);
		}

		if (myLastInput.Special())
		{
			SetState(CharacterState.ATTACK);
			SetAnimation(animID + 2);
		}
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

	public void SetAnimation(int animID)
	{
		if (animator.GetCurrentAnimationID() == animID)
		{
			return;
		}
		animator.ChangeAnimationToID(animID);
	}

	public void SetState(CharacterState state)
	{
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

			ProcessHit(0, 0, push);
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
			knockback *= 0.5f;
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

		animator.currentFrame = Mathf.Clamp(currentFrame, 0, current.GetAnimationDuration());

		animator.visuals.sprite = current.GetCurrentSprite(currentFrame);

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
