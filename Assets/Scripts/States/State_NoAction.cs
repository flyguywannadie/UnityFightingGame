using UnityEngine;

public class State_NoAction : BaseState
{
	public override bool HandleGettingHit(BufferedInput input, bool low)
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
