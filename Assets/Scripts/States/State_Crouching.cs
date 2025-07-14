using UnityEngine;

public class State_Crouching : BaseState
{
	public override bool HandleGettingHit(BufferedInput input, bool low, bool overhead)
	{
		return (input.DownBack() && !overhead);
	}

	public override void OnEnterState(BaseCharacter c, BufferedInput input)
	{
		c.SetMotion(0, 0);
		c.SetAnimation(CommonAnimations.CROUCH);
	}

	public override void OnExitState(BaseCharacter c, BufferedInput input)
	{

	}

	public override void StateUpdate(BaseCharacter c, BufferedInput input)
	{
		if (!input.Down())
		{
			if (input.Up())
			{
				c.SetState(CharacterState.JUMP);
				return;
			}

			if (input.Walking())
			{
				c.SetState(CharacterState.WALKING);
				return;
			}

			c.SetState(CharacterState.STANDING);
			return;
		}

		//if (!input.Down())
		//{
		//	c.SetState(CharacterState.STANDING);
		//	c.SetAnimation(CommonAnimations.IDLE);

		//	int usedSpeed = c.GetSpeed();

		//	if (c.AmIFacingBackward())
		//	{
		//		usedSpeed *= -1;
		//	}

		//	if (input.Back())
		//	{
		//		c.SetMotion(-usedSpeed, 0);
		//		c.SetAnimation(CommonAnimations.BACKWALKING);
		//	}

		//	if (input.Forward())
		//	{
		//		c.SetMotion(usedSpeed, 0);
		//		c.SetAnimation(CommonAnimations.WALKING);
		//	}

		//	if (input.Up())
		//	{
		//		c.SetAnimation(CommonAnimations.JUMP);
		//		c.LoseControl();
		//		c.SetMotion(0, 0);
		//	}
		//}
		//else
		//{
		//	c.SetAnimation(CommonAnimations.CROUCH);
		//}
	}
}
