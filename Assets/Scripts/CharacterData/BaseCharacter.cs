using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Windows;
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
	NOACTION = 8,
}

public abstract class BaseCharacter : MonoBehaviour
{
	[SerializeField] protected int maxHealth = 200;
	[SerializeField] protected int health = 200;
	[SerializeField] protected int speed = 5;
	[SerializeField] protected float jumpHeight = 3;
	[SerializeField] protected int myState = 0;
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
			new State_NoAction(),
		};
	}

	public virtual void CharUpdate(BufferedInput input)
	{
		if (health <= 0)
		{
			return;
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

		if (hitstun > 0)
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
				//LoseControl();
			}

			if (stateIndex == (int)CharacterState.BLOCKSTUN || stateIndex == (int)CharacterState.HITSTUN)
			{
				states[stateIndex].StateUpdate(this, input);
			}
		}

		if (inControl)
		{
			states[stateIndex].StateUpdate(this, input);

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
		combo = 0;
	}

	public void AnimStop()
	{
		SetMotion(0, 0);
	}

	public virtual void JumpAction()
	{
		AddMotion(0, jumpHeight);
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
		bool blocked = false;
		if (property.attackHeight != AttackHeight.UNBLOCKABLE) {
			blocked = states[stateIndex].WasAttackBlocked(myLastInput, property);
		}

		int damage = property.damage;
		int stun = property.hitstun;
		float knock = property.knockback.x;
		if (blocked)
		{
			damage = 0;
			stun = property.blockstun;
			knock *= 0.5f;
		} else
		{
			if (IsOnGround())
			{
				SetMotion(motion.x, Mathf.Max(property.knockback.y, 0.0f));
			} else
			{
				SetMotion(motion.x, property.knockback.y);
			}
		}

		if (!AmIFacingBackward())
		{
			knock *= -1;
		}

		ProcessHit(damage, stun, blocked, knock);
	}

	protected virtual void ProcessHit(int damage, int stun, bool blocked, float knockback)
	{
		if (blocked)
		{
			SetState(CharacterState.BLOCKSTUN);
		}
		else
		{
			SetState(CharacterState.HITSTUN);
			combo += 1;
		}

		this.health -= damage;
		this.hitstun = stun;
		this.knockback = knockback;
	}

	public virtual void Reset()
	{
		combo = 0;
		health = maxHealth;
		hitstun = 0;
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
		foreach (BaseBoxData box in frameData.hitboxes)
		{
			Vector3 usedPos = box.position;
			if (myVisuals.flipX)
			{
				usedPos.x *= -1;
			}

			Gizmos.color = Color.cyan;
			Gizmos.DrawWireCube(transform.position + usedPos, box.size);
		}

		foreach (HurtBoxData box in frameData.hurtboxes)
		{
			Vector3 usedPos = box.position;
			if (myVisuals.flipX)
			{
				usedPos.x *= -1;
			}

			Gizmos.color = Color.red;
			Gizmos.DrawWireCube(transform.position + usedPos, box.size);
		}
	}
}
