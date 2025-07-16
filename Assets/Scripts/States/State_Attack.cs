using UnityEngine;

public class State_Attack : BaseState
{
	public override bool WasAttackBlocked(BufferedInput input, bool low, bool overhead)
	{
		return false;
	}

	public override void OnEnterState(BaseCharacter c, BufferedInput input)
	{
		if (c.IsOnGround())
		{
			c.SetMotion(0, 0);
		}
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
