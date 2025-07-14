using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Windows;

public enum CommonAnimations
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
	WALKING = 1,
	CROUCHING = 2,
	JUMP = 3,
	INAIR = 4,
	ATTACK = 5,
	NOACTION = 6,
}

public abstract class BaseCharacter : MonoBehaviour
{
	[SerializeField] protected int speed = 5;
	[SerializeField] protected int jumpPower = 5;
	[SerializeField] protected int myState = 0;
	[SerializeField] public Vector2 motion { get; protected set; }
	[SerializeField] protected Transform whoIMove;
	[SerializeField] protected SpriteRenderer myVisuals;
	[SerializeField] protected CharacterAnimator animator;
	//[SerializeField] protected Animation anims;
	[SerializeField] protected bool inControl;
	//[SerializeField] protected bool onGround = true;
	//[SerializeField] protected int hitstun = 0;
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

		InitializeStates();
		editorHitboxes = false;
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
			new State_NoAction(),
		};
	}

	public virtual void CharUpdate(BufferedInput input)
	{
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
				if (faceBack)
				{
					whoIMove.localScale = new Vector3(-1, 1, 1);
				}
				else
				{
					whoIMove.localScale = new Vector3(1, 1, 1);
				}
					
				//myVisuals.flipX = faceBack;
			}
		}
		else
		{
			AddMotion(0, -9.8f * Time.fixedDeltaTime);
			//if (!(stateIndex == (int)CharacterState.INAIR))
			//{
			//	SetState(CharacterState.INAIR);
			//	ChangeState();
			//}
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
		whoIMove.Translate(motion * Time.fixedDeltaTime);
		if (!currentlyGrounded && IsOnGround())
		{
			if (myLastInput.Down())
			{
				SetState(CharacterState.CROUCHING);
			}
			else
			{
				SetState(CharacterState.STANDING);
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

	public virtual void ProcessGettingHit(bool low, bool overhead)
	{
		GetHit(0, 30, states[stateIndex].HandleGettingHit(myLastInput, true, false));
	}

	protected virtual void GetHit(int damage, int stun, bool blocked)
	{
		//states[0].HandleGettingHit(this, input);

		//if (blocked)
		//{
		//	SetAnimation(CommonAnimations.BLOCKSTUN);
		//	stun = stun / 4;
		//}
		//else
		//{
		//	SetAnimation(CommonAnimations.HITSTUN);
		//}
		//// health - damage
		//this.hitstun = stun;
	}

	public int GetSpeed()
	{
		return speed;
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
		if (frameData.boxes == null)
		{
			return;
		}

		foreach (CharacterAnimation.BoxData box in frameData.boxes)
		{
			Vector3 usedPos = box.position;
			if (myVisuals.flipX)
			{
				usedPos.x *= -1;
			}

			switch (box.boxType)
			{
				case BoxType.HITBOX:
					Gizmos.color = Color.cyan;
					break;
				case BoxType.HURTBOX:
					Gizmos.color = Color.red;
					break;
			}
			Gizmos.DrawWireCube(transform.position + usedPos, box.size);
		}
	}
}
