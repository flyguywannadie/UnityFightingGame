using UnityEngine;

public class State_OnGround : BaseState
{
	public override bool WasAttackBlocked(BufferedInput input, HurtboxProperties property)
	{
		return false;
	}

	public override void OnEnterState(BaseCharacter c, BufferedInput input)
	{
		c.SetAnimation(CommonAnimations.ONGROUND);
	}

	public override void OnExitState(BaseCharacter c, BufferedInput input)
	{
		c.GainControl();
	}

	public override void StateUpdate(BaseCharacter c, BufferedInput input)
	{

	}
}
