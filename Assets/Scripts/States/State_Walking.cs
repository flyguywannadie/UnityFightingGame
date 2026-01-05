using UnityEngine;

public class State_Walking : BaseState
{
	int runTime = 0;
	BufferedInput prevInput = new BufferedInput();

	public override bool WasAttackBlocked(BufferedInput input, HurtboxProperties property)
	{
		return (input.Back() && !(property.attackHeight == AttackHeight.LOW));
	}

	public override void OnEnterState(BaseCharacter c, BufferedInput input)
	{
		//      Debug.Log("ON Enter - " + runTime + " -- " + GameManager.instance.GetTime() + " Compare: " + GameManager.instance.CompareTime(runTime));
		//      //Debug.Log("ON Enter - " + input.GetDirection() + " -- " + prevInput.GetDirection());
		if (input.GetDirection() == prevInput.GetDirection() && runTime < 8)
		{
			if (input.Forward())
			{
				c.SetState(CharacterState.RUNNING);
			}
			prevInput.Clear();
            return;
		}
		prevInput.CopyInput(input);
		runTime = 0;
		Walk(c, input);
		c.LoseCombo();
	}

	public override void OnExitState(BaseCharacter c, BufferedInput input)
	{
        //runTime = GameManager.instance.GetTime();
		//Debug.Log("ON Exit - " + runTime);
        //c.SetMotion(0, 0);
    }

	public override void StateUpdate(BaseCharacter c, BufferedInput input)
	{
		if (input.Up())
		{
			c.SetState(CharacterState.JUMP);
			return;
		}

		if (input.Down())
		{
			c.SetState(CharacterState.CROUCHING);
			return;
		}

		if (!input.Walking())
		{
			c.SetState(CharacterState.STANDING);
            return;
		}

		runTime++;

		Walk(c, input);
	}

	private void Walk(BaseCharacter c, BufferedInput input)
	{
		int usedSpeed = c.GetSpeed();
		if (c.AmIFacingBackward())
		{
			usedSpeed *= -1;
		}
		if (input.Back())
		{
			c.SetMotion(-usedSpeed, 0);
			c.SetAnimation(CommonAnimations.BACKWALKING);
		}
		if (input.Forward())
		{
			c.SetMotion(usedSpeed, 0);
			c.SetAnimation(CommonAnimations.WALKING);
		}
	}
}
