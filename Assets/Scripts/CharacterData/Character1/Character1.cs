using UnityEngine;

public class Character1 : BaseCharacter
{
    protected enum SpecificStates
    {
        LIGHT = 100,
        HEAVY = 101,
        SPECIAL = 102
    }

	bool blockingHigh = false;
	bool blockingLow = false;

	public override void CharUpdate(BufferedInput input)
	{
		//if (onGround && knocked <= 0 && inControl)
		//{
		//	if (input.NoDirection())
		//	{
		//		SetSubState(CharacterSubStates.IDLE);
		//	}



		//	if (input.Down())
		//	{
		//		motion.x = 0;
		//		SetSubState(CharacterSubStates.CROUCH);
		//	}

		//	if (input.Up())
		//	{
		//		onGround = false;
		//		SetSubState(CharacterSubStates.JUMP);
		//	}

		//	if (input.Light())
		//	{
		//		inControl = false;
		//		SetSubState((int)SpecificStates.LIGHT);
		//	}

		//	if (input.Heavy())
		//	{
		//		inControl = false;
		//		SetSubState((int)SpecificStates.HEAVY);
		//	}

		//	if (input.Special())
		//	{
		//		inControl = false;
		//		SetSubState((int)SpecificStates.SPECIAL);
		//	}


		//	blockingHigh = input.Back() && !input.Down();
		//	blockingLow = input.DownBack();
		//} 
		//else if (!onGround && knocked <= 0)
		//{
		//	if (input.NoDirection())
		//	{
		//		SetSubState(CharacterSubStates.INAIR);
		//	}
		//}

		if (Input.GetKeyDown(KeyCode.H))
		{
			GetHit(0, false, 30, 10, false);
		}

		if (Input.GetKeyDown(KeyCode.J))
		{
			GetHit(0, true, 30, 10, false);
		}

		base.CharUpdate(input);
	}
}
