using UnityEngine;

public abstract class BaseCharacter : MonoBehaviour
{
	public enum GenericStates
	{
		IDLE = 0,
		WALKING = 1,
		BLOCKSTUN = 2,
		HITSTUN = 3,
		JUMP = 4,
		INAIR = 5,
		INAIRBLOCKSTUN = 6,
		INAIRHITSTUN = 7,
		INAIRKNOCKDOWNUP = 8,
		INAIRKNOCKDOWNMID = 9,
		INAIRKNOCKDOWNDOWN = 10,
		KNOCKDOWN = 11,
		ONGROUND = 12,
		GETUP = 13,
		CROUCH = 14,
		BACKWALKING = 15,
	}

	[SerializeField] protected int speed = 5;
	[SerializeField] protected int jumpPower = 5;
	[SerializeField] protected int myState = 0;
	[SerializeField] protected Vector2 motion;
	[SerializeField] protected Transform whoIMove;
	[SerializeField] protected SpriteRenderer myVisuals;
	[SerializeField] protected Animator anims;
	[SerializeField] protected bool inControl = true;
	[SerializeField] protected bool onGround = true;
	[SerializeField] protected int knocked = 0;
	[SerializeField] private Transform otherPerson;

	public void Start()
	{
		if (whoIMove == null)
		{
			whoIMove = transform;
		}
	}

	public virtual void CharUpdate(BufferedInput input)
	{
		whoIMove.Translate(motion * Time.fixedDeltaTime);

		if (onGround)
		{
			motion = Vector2.zero;
			if (knocked > 0)
			{
				knocked -= 1;
				if (knocked <= 5)
				{
					SetState(GenericStates.GETUP);
				}
			}
		}
		else if (!inControl)
		{
			motion.y -= 9.8f * Time.fixedDeltaTime;

			if (knocked > 0)
			{
				if (motion.y > 1.0f)
				{
					SetState(GenericStates.INAIRKNOCKDOWNUP);
				}
				else if (motion.y < -1.0f)
				{
					SetState(GenericStates.INAIRKNOCKDOWNDOWN);
				} else
				{
					SetState(GenericStates.INAIRKNOCKDOWNMID);
				}

				if (whoIMove.position.y <= 0)
				{
					whoIMove.position.Set(whoIMove.position.x, 0, 0);
					motion = Vector2.zero;
					onGround = true;
					SetState(GenericStates.KNOCKDOWN);
				}
			} else
			{
				if (whoIMove.position.y <= 0)
				{
					whoIMove.position.Set(whoIMove.position.x, 0, 0);
					motion = Vector2.zero;
					onGround = true;
					inControl = true;
					SetState(GenericStates.IDLE);
				}
			}
		}

		anims.SetInteger("STATE", myState);

		myVisuals.flipX = AmIFacingBackward();
	}

	public virtual void JumpAction()
	{
		motion += new Vector2(0,jumpPower);
		inControl = false;
	}

	public void SetStateFromAnimator(GenericStates state)
	{
		SetState((int)state);
	}

	public void SetState(GenericStates state)
	{
		SetState((int)state);
	}

	public void SetState(int state)
	{
		myState = state;
		if (state == (int)GenericStates.IDLE)
		{
			inControl = true;
		}
	}

	public bool AmIFacingBackward()
	{
		return (otherPerson.position.x < whoIMove.position.x);
	}
}
