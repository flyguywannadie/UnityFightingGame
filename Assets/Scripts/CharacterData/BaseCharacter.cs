using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Rendering;

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
	[SerializeField] protected Animator anims;
	[SerializeField] protected bool inControl;
	//[SerializeField] protected bool onGround = true;
	[SerializeField] protected int hitstun = 0;
	[SerializeField] protected int knocked = 0;
	[SerializeField] private Transform otherPerson;
	[SerializeField] private BaseState[] states;
	[SerializeField] private int stateIndex = 0;
	[SerializeField] private BufferedInput myLastInput;

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

		if (hitstun > 0)
		{
			hitstun--;
			SetAnimatorValues();
			return;
		}

		if (IsOnGround())
		{
			myVisuals.flipX = faceBack;
		}

		if (inControl)
		{
			states[stateIndex].StateUpdate(this, input);
		}
		
		//states[stateIndex].HandleMovement(this, input);

		whoIMove.Translate(motion * Time.fixedDeltaTime);

		SetAnimatorValues();

		myLastInput = input;

		//motion.y -= 9.8f * Time.fixedDeltaTime;
		//if (motion.y <= 0.0f)
		//{
		//	whoIMove.position.Set(whoIMove.position.x, 0, 0);
		//	motion = Vector2.zero;
		//	onGround = true;
		//}

		//if (onGround && inControl)
		//{
		//	motion = Vector2.zero;
		//	if (knocked > 0)
		//	{
		//		knocked -= 1;
		//		if (knocked <= 5)
		//		{
		//			SetState(GenericStates.GETUP);
		//		}
		//	}
		//}
		//else if (!inControl)
		//{
		//	motion.y -= 9.8f * Time.fixedDeltaTime;

		//	if (knocked > 0)
		//	{
		//		if (motion.y > 1.0f)
		//		{
		//			SetState(GenericStates.INAIRKNOCKDOWNUP);
		//		}
		//		else if (motion.y < -1.0f)
		//		{
		//			SetState(GenericStates.INAIRKNOCKDOWNDOWN);
		//		} else
		//		{
		//			SetState(GenericStates.INAIRKNOCKDOWNMID);
		//		}

		//		if (whoIMove.position.y <= 0)
		//		{
		//			whoIMove.position.Set(whoIMove.position.x, 0, 0);
		//			motion = Vector2.zero;
		//			onGround = true;
		//			SetState(GenericStates.KNOCKDOWN);
		//		}
		//	} else
		//	{
		//		if (whoIMove.position.y <= 0)
		//		{
		//			whoIMove.position.Set(whoIMove.position.x, 0, 0);
		//			motion = Vector2.zero;
		//			onGround = true;
		//			inControl = true;
		//			SetState(GenericStates.IDLE);
		//		}
		//	}
		//}
	}

	public virtual void LoseControl()
	{
		inControl = false;
	}

	public virtual void RegainControl()
	{
		inControl = true;
	}

	public virtual void JumpAction()
	{
		motion += new Vector2(0,jumpPower);
		whoIMove.Translate(motion * Time.fixedDeltaTime);
		SetSubState(CharacterSubStates.IDLE);
		SetState(CharacterState.INAIR);
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
		SetSubState((int)state);
	}

	public void SetSubState(CharacterSubStates state)
	{
		SetSubState((int)state);
	}

	public void SetSubState(int state)
	{
		myState = state;
		if (state == (int)CharacterSubStates.IDLE)
		{
			inControl = true;
		}
	}

	public void SetState(CharacterState state)
	{
		SetState((int)state);
	}

	public void SetState(int state)
	{
		RegainControl();
		stateIndex = state;
	}

	public virtual void SetAnimatorValues()
	{
		anims.SetInteger("State", stateIndex);
		anims.SetInteger("SubState", myState);
		if (AmIFacingBackward())
		{
			anims.SetFloat("XMotion", -motion.x);
		} else
		{
			anims.SetFloat("XMotion", motion.x);
		}
		anims.SetFloat("YMotion", motion.y);
		anims.SetInteger("Stun", hitstun);
	}

	public virtual void ProcessGettingHit(bool low)
	{
		GetHit(0, 30, states[stateIndex].HandleGettingHit(myLastInput, low));
		SetAnimatorValues();
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
