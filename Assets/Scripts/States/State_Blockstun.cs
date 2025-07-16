using UnityEngine;

public class State_Blockstun : BaseState
{
	public override bool WasAttackBlocked(BufferedInput input, bool low, bool overhead)
	{
		return true;
	}

	public override void OnEnterState(BaseCharacter c, BufferedInput input)
	{
		if (!c.IsOnGround())
		{
			c.SetAnimation(CommonAnimations.BLOCKSTUN);
		}
		else if (input.Down())
		{
			c.SetAnimation(CommonAnimations.BLOCKSTUN);
		}
		else
		{
			c.SetAnimation(CommonAnimations.BLOCKSTUN);
		}
		c.LoseControl();
	}

	public override void OnExitState(BaseCharacter c, BufferedInput input)
	{
		c.GainControl();
	}

	public override void StateUpdate(BaseCharacter c, BufferedInput input)
	{
		if (!c.IsOnGround())
		{
			c.SetAnimation(CommonAnimations.BLOCKSTUN);
		}
		else if (input.Down())
		{
			c.SetAnimation(CommonAnimations.BLOCKSTUN);
		}
		else
		{
			c.SetAnimation(CommonAnimations.BLOCKSTUN);
		}
	}
}
