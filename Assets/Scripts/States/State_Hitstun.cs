using UnityEngine;

public class State_Hitstun : BaseState
{
	public override bool WasAttackBlocked(BufferedInput input, HurtboxProperties property)
	{
		return false;
	}

	public override void OnEnterState(BaseCharacter c, BufferedInput input)
	{
		if (!c.IsOnGround())
		{
			c.SetAnimation(CommonAnimations.AIRHIT);
		}
		else if (input.Down())
		{
			c.SetAnimation(CommonAnimations.CROUCHHIT);
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
			c.SetAnimation(CommonAnimations.AIRHIT);
		}
		else if (input.Down())
		{
			c.SetAnimation(CommonAnimations.CROUCHHIT);
		}
		else
		{
			c.SetAnimation(CommonAnimations.HITSTUN);
		}
	}
}
