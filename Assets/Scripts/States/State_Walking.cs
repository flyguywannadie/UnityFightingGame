using UnityEngine;

public class State_Walking : BaseState
{
	public override bool WasAttackBlocked(BufferedInput input, HurtboxProperties property)
	{
		return (input.Back() && !(property.attackHeight == AttackHeight.LOW));
	}

	public override void OnEnterState(BaseCharacter c, BufferedInput input)
	{
		Walk(c, input);
	}

	public override void OnExitState(BaseCharacter c, BufferedInput input)
	{
		c.SetMotion(0, 0);
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

		if (!input.Walking())
		{
			c.SetState(CharacterState.STANDING);
			return;
		}

		Walk(c, input);
	}

	private void Walk(BaseCharacter c, BufferedInput input)
	{
		int usedSpeed = c.GetSpeed();
		if (c.AmIFacingBackward())
		{
			usedSpeed *= -1;
		}
		if (input.Back())
		{
			c.SetMotion(-usedSpeed, 0);
			c.SetAnimation(CommonAnimations.BACKWALKING);
		}
		if (input.Forward())
		{
			c.SetMotion(usedSpeed, 0);
			c.SetAnimation(CommonAnimations.WALKING);
		}
	}
}
