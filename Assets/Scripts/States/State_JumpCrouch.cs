using UnityEngine;

public class State_JumpCrouch : BaseState
{
	public override bool HandleGettingHit(BufferedInput input, bool low)
	{
		throw new System.NotImplementedException();
	}

	public override void OnEnterState(BaseCharacter c, BufferedInput input)
	{
		c.SetSubState(CharacterSubStates.JUMP);
	}

	public override void OnExitState(BaseCharacter c, BufferedInput input)
	{
		throw new System.NotImplementedException();
	}

	public override void StateUpdate(BaseCharacter c, BufferedInput input)
	{
		throw new System.NotImplementedException();
	}
}
