using UnityEngine;

public class State_Blockstun : BaseState
{
	private bool onground;
	private CommonAnimations blockingAnim;

	public override bool WasAttackBlocked(BufferedInput input, HurtboxProperties property)
	{
		bool blocked = false;
		CommonAnimations newblock = blockingAnim;
		if (!onground)
		{
			blocked = input.Back();
			newblock = CommonAnimations.AIRBLOCK;
		}
		else if (input.Down())
		{
			blocked = input.Back() && !(property.attackHeight == AttackHeight.OVERHEAD);
			newblock = CommonAnimations.CROUCHBLOCK;
		}
		else
		{
			blocked = input.Back() && !(property.attackHeight == AttackHeight.LOW);
			newblock = CommonAnimations.BLOCKSTUN;
		}

		if (blocked)
		{
			blockingAnim = newblock;
		}

		return blocked;
	}

	public override void OnEnterState(BaseCharacter c, BufferedInput input)
	{
		onground = c.IsOnGround();
		if (!onground)
		{
			blockingAnim = CommonAnimations.AIRBLOCK;
		}
		else if (input.Down())
		{
			blockingAnim = CommonAnimations.CROUCHBLOCK;
		}
		else
		{
			blockingAnim = CommonAnimations.BLOCKSTUN;
		}
		c.SetAnimation(blockingAnim);
		c.LoseControl();
	}

	public override void OnExitState(BaseCharacter c, BufferedInput input)
	{
		c.GainControl();
	}

	public override void StateUpdate(BaseCharacter c, BufferedInput input)
	{
		onground = c.IsOnGround();
		c.SetAnimation(blockingAnim);
	}
}
