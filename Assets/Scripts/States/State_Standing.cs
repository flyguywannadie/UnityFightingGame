using UnityEngine;

public class State_Standing : BaseState
{
	public override bool WasAttackBlocked(BufferedInput input, HurtboxProperties property)
	{
		return (input.Back() && !(property.attackHeight == AttackHeight.LOW));
	}

	public override void OnEnterState(BaseCharacter c, BufferedInput input)
	{
		c.SetMotion(0,0);
		c.SetAnimation(CommonAnimations.IDLE);
	}

	public override void OnExitState(BaseCharacter c, BufferedInput input)
	{

	}

	public override void StateUpdate(BaseCharacter c, BufferedInput input)
	{
		if (input.Up())
		{
			c.SetState(CharacterState.JUMP);
			return;
		}

		if (input.Down())
		{
			c.SetState(CharacterState.CROUCHING);
			return;
		}

		if (input.Walking())
		{
			c.SetState(CharacterState.WALKING);
			return;
		}

		//if (input.Walking())
		//{
		//	int usedSpeed = c.GetSpeed();

		//	if (c.AmIFacingBackward())
		//	{
		//		usedSpeed *= -1;
		//	}

		//	if (input.Back())
		//	{
		//		c.SetMotion(-usedSpeed, 0);
		//		c.SetSubState(CharacterSubStates.BACKWALKING);
		//	}

		//	if (input.Forward())
		//	{
		//		c.SetMotion(usedSpeed, 0);
		//		c.SetSubState(CharacterSubStates.WALKING);
		//	}
		//}
		//else
		//{
		//	c.SetSubState(CharacterSubStates.IDLE);
		//	c.SetMotion(0, 0);
		//}

		//if (input.Down())
		//{
		//	c.SetState(CharacterState.CROUCHING);
		//	c.SetSubState(CharacterSubStates.CROUCH);
		//	c.SetMotion(0, 0);
		//}
		//else if (input.Up())
		//{
		//	c.SetSubState(CharacterSubStates.JUMP);
		//	c.LoseControl();
		//	c.SetMotion(0, 0);
		//}
	}
}
