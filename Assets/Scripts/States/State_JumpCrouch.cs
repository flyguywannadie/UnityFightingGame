using UnityEngine;

public class State_JumpCrouch : BaseState
{
	private Vector2 storedMotion;

	public override bool HandleGettingHit(BufferedInput input, bool low)
	{
		return true;
	}

	public override void OnEnterState(BaseCharacter c, BufferedInput input)
	{
		storedMotion = c.motion;
		int usedSpeed = c.GetSpeed();
		if (c.AmIFacingBackward())
		{
			usedSpeed *= -1;
		}
		if (input.Back())
		{
			storedMotion.x = -usedSpeed;
		}
		if (input.Forward())
		{
			storedMotion.x = usedSpeed;
		}
		c.SetMotion(0, 0);
		c.SetAnimation(CommonAnimations.JUMP);
		c.LoseControl();
	}

	public override void OnExitState(BaseCharacter c, BufferedInput input)
	{
		c.AddMotion(storedMotion.x, storedMotion.y);
		c.GainControl();
	}

	public override void StateUpdate(BaseCharacter c, BufferedInput input)
	{
		
	}
}
