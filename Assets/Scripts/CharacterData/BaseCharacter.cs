using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Windows;

public enum CharacterSubStates
{
	IDLE = 0,
	WALKING = 1,
	BLOCKSTUN = 2,
	HITSTUN = 3,
	JUMP = 4,
	INAIR = 5,
	INAIRKNOCKDOWNUP = 6,
	INAIRKNOCKDOWNMID = 7,
	INAIRKNOCKDOWNDOWN = 8,
	KNOCKDOWN = 9,
	ONGROUND = 10,
	GETUP = 11,
	CROUCH = 12,
	BACKWALKING = 13,
}

public enum CharacterState
{
	STANDING = 0,
	CROUCHING = 1,
	INAIR = 2,
}

public abstract class BaseCharacter : MonoBehaviour
{
	[SerializeField] protected int speed = 5;
	[SerializeField] protected int jumpPower = 5;
	[SerializeField] protected int myState = 0;
	[SerializeField] protected Vector2 motion;
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
	[SerializeField] private BufferedInput myLastInput;

	public bool changeState;

	public void Start()
	{
		inControl = true;
		if (whoIMove == null)
		{
			whoIMove = transform;
		}
	}

	public virtual void CharUpdate(BufferedInput input)
	{
		bool faceBack = AmIFacingBackward();
		if (faceBack)
		{
			input.FlipForwardBack();
		}

		bool newinput = (input.inputFlag != myLastInput.inputFlag);

		if (hitstun > 0)
		{
			hitstun--;
			return;
		}

		bool currentlyGrounded = IsOnGround();
		if (currentlyGrounded)
		{
			myVisuals.flipX = faceBack;
		} else
		{
			if (stateIndex != (int)CharacterState.INAIR)
			{
				SetState(CharacterState.INAIR);
			}

			AddMotion(0, -9.8f * Time.fixedDeltaTime);
		}

		animator.AnimatorUpdate(this);

		if (inControl && newinput)
		{
			states[stateIndex].StateUpdate(this, input);
		}
		
		//states[stateIndex].MovementOverride(this, input);

		whoIMove.Translate(motion * Time.fixedDeltaTime);
		if (!currentlyGrounded && IsOnGround())
		{
			if (input.Down())
			{
				SetState(CharacterState.CROUCHING);
				SetSubState(CharacterSubStates.CROUCH);
			}
			else
			{
				SetState(CharacterState.STANDING);
				SetSubState(CharacterSubStates.IDLE);

				int usedSpeed = GetSpeed();

				if (AmIFacingBackward())
				{
					usedSpeed *= -1;
				}

				if (myLastInput.Back())
				{
					AddMotion(-usedSpeed, 0);
					SetSubState(CharacterSubStates.BACKWALKING);
				}

				if (myLastInput.Forward())
				{
					AddMotion(usedSpeed, 0);
					SetSubState(CharacterSubStates.WALKING);
				}
			}

			LandFromAir();
		}

		myLastInput = input;
	}

	public virtual void LoseControl()
	{
		inControl = false;
	}

	public virtual void GainControl()
	{
		inControl = true;
	}

	public virtual void JumpAction()
	{
		AddMotion(0, jumpPower);
		int usedSpeed = GetSpeed();

		if (AmIFacingBackward())
		{
			usedSpeed *= -1;
		}

		if (myLastInput.Back())
		{
			AddMotion(-usedSpeed, 0);
		}

		if (myLastInput.Forward())
		{
			AddMotion(usedSpeed, 0);
		}
		whoIMove.Translate(motion * Time.fixedDeltaTime);
		SetState(CharacterState.INAIR);
	}

	public virtual void GoAir()
	{
		SetSubState(CharacterSubStates.INAIR);
	}

	public virtual void LandFromAir()
	{
		whoIMove.position.Set(whoIMove.position.x, 0, 0);
		motion = Vector2.zero;
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

	public void SetSubStateFromAnimator(CharacterSubStates state)
	{
		//SetSubState((int)state);
	}

	public void SetSubState(CharacterSubStates state)
	{
		SetSubState((int)state);
	}

	public void SetSubState(int state)
	{
		myState = state;
		animator.ChangeAnimationToID(state);
	}

	public void SetState(CharacterState state)
	{
		SetState((int)state);
	}

	public void SetState(int state)
	{
		stateIndex = state;
		myLastInput.Clear();
	}

	public virtual void ProcessGettingHit(bool low)
	{
		GetHit(0, 30, states[stateIndex].HandleGettingHit(myLastInput, low));
	}

	public virtual void GetHit(int damage, int stun, bool blocked)
	{
		//states[0].HandleGettingHit(this, input);

		if (blocked)
		{
			SetSubState(CharacterSubStates.BLOCKSTUN);
			stun = stun / 4;
		}
		else
		{
			SetSubState(CharacterSubStates.HITSTUN);
		}
		// health - damage
		this.hitstun = stun;
	}

	public int GetSpeed()
	{
		return speed;
	}

	public bool AmIFacingBackward()
	{
		return (otherPerson.position.x < whoIMove.position.x);
	}
}
