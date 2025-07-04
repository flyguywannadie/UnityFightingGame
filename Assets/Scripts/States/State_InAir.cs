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
		int usedSpeed = c.GetSpeed();
		if (c.AmIFacingBackward())
		{
			usedSpeed *= -1;
		}
		if (input.Back())
		{
			c.AddMotion(-usedSpeed, 0);
		}
		if (input.Forward())
		{
			c.AddMotion(usedSpeed, 0);
		}
	}

	public override void OnExitState(BaseCharacter c, BufferedInput input)
	{

	}

	public override void StateUpdate(BaseCharacter c, BufferedInput input)
	{
		
	}
}
