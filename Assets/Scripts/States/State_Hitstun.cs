using UnityEngine;

public class State_Hitstun : BaseState
{
	public override bool WasAttackBlocked(BufferedInput input, bool low, bool overhead)
	{
		return false;
	}

	public override void OnEnterState(BaseCharacter c, BufferedInput input)
	{
		if (!c.IsOnGround())
		{
			c.SetAnimation(CommonAnimations.HITSTUN);
		}
		else if (input.Down())
		{
			c.SetAnimation(CommonAnimations.HITSTUN);
		}
		else
		{
			c.SetAnimation(CommonAnimations.HITSTUN);
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
			c.SetAnimation(CommonAnimations.HITSTUN);
		}
		else if (input.Down())
		{
			c.SetAnimation(CommonAnimations.HITSTUN);
		}
		else
		{
			c.SetAnimation(CommonAnimations.HITSTUN);
		}
	}
}
