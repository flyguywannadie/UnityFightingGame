using UnityEngine;

public class State_Knockdown : BaseState
{
	public override bool WasAttackBlocked(BufferedInput input, HurtboxProperties property)
	{
		return false;
	}

	public override void OnEnterState(BaseCharacter c, BufferedInput input)
	{
		CheckAnimation(c, input);
		c.LoseControl();
	}

	public override void OnExitState(BaseCharacter c, BufferedInput input)
	{

	}

	public override void StateUpdate(BaseCharacter c, BufferedInput input)
	{
		CheckAnimation(c, input);
	}

	private void CheckAnimation(BaseCharacter c, BufferedInput input)
	{
		if (c.IsOnGround())
		{
			c.SetAnimation(CommonAnimations.KNOCKDOWN);
		}
		else
		{
			var yvel = c.motion.y;
			var threshold = 5.0f;
			if (yvel > threshold)
			{
				c.SetAnimation(CommonAnimations.INAIRKNOCKDOWNUP);
			} 
			else if (yvel < -threshold)
			{
				c.SetAnimation(CommonAnimations.INAIRKNOCKDOWNDOWN);
			} 
			else
			{
				c.SetAnimation(CommonAnimations.INAIRKNOCKDOWNMID);
			}
		}
	}
}
