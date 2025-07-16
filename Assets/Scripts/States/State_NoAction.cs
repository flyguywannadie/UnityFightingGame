using UnityEngine;

public class State_NoAction : BaseState
{
	public override bool WasAttackBlocked(BufferedInput input, bool low, bool overhead)
	{
		return true;
	}

	public override void OnEnterState(BaseCharacter c, BufferedInput input)
	{

	}

	public override void OnExitState(BaseCharacter c, BufferedInput input)
	{

	}

	public override void StateUpdate(BaseCharacter c, BufferedInput input)
	{

	}
}
