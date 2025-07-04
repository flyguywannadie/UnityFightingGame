using UnityEngine;
using UnityEngine.Windows;

public class State_Walking : BaseState
{
	public override bool HandleGettingHit(BufferedInput input, bool low)
	{
		throw new System.NotImplementedException();
	}

	public override void OnEnterState(BaseCharacter c, BufferedInput input)
	{
		Walk(c, input);
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
