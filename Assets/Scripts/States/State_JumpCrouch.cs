using UnityEngine;

public class State_JumpCrouch : BaseState
{
	public override bool HandleGettingHit(BufferedInput input, bool low)
	{
		return true;
	}

	public override void OnEnterState(BaseCharacter c, BufferedInput input)
	{
		c.SetMotion(0, 0);
		c.SetAnimation(CommonAnimations.JUMP);
		c.LoseControl();
	}

	public override void OnExitState(BaseCharacter c, BufferedInput input)
	{
		c.GainControl();
	}

	public override void StateUpdate(BaseCharacter c, BufferedInput input)
	{
		
	}
}
