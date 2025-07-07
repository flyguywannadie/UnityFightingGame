using UnityEngine;

public class State_Attack : BaseState
{
	public override bool HandleGettingHit(BufferedInput input, bool low)
	{
		return true;
	}

	public override void OnEnterState(BaseCharacter c, BufferedInput input)
	{
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
