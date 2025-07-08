using Unity.VisualScripting;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class State_InAir : BaseState
{
	public override bool HandleGettingHit(BufferedInput input, bool low)
	{
		return (input.Back());
	}

	public override void OnEnterState(BaseCharacter c, BufferedInput input)
	{
		c.SetAnimation(CommonAnimations.INAIR);
	}

	public override void OnExitState(BaseCharacter c, BufferedInput input)
	{

	}

	public override void StateUpdate(BaseCharacter c, BufferedInput input)
	{
		
	}
}
