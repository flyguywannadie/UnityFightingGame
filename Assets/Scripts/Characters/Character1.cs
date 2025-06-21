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

		if (onGround && knocked <= 0 && inControl)
		{
			if (AmIFacingBackward())
			{
				input.FlipForwardBack();
			}
			Debug.Log(input.DownBack());

			if (input.NoDirection())
			{
				SetState(GenericStates.IDLE);
			}

			if (input.Walking())
			{
				int usedSpeed = speed;

				if (AmIFacingBackward())
				{
					usedSpeed *= -1;
				}

				if (input.Back())
				{
					motion.x += -usedSpeed;
					SetState(GenericStates.BACKWALKING);
				}

				if (input.Forward())
				{
					motion.x += usedSpeed;
					SetState(GenericStates.WALKING);
				}
			}

			if (input.Down())
			{
				motion.x = 0;
				SetState(GenericStates.CROUCH);
			}

			if (input.Up())
			{
				onGround = false;
				SetState(GenericStates.JUMP);
			}

			if (input.Light())
			{
				inControl = false;
				SetState((int)SpecificStates.LIGHT);
			}

			if (input.Heavy())
			{
				inControl = false;
				SetState((int)SpecificStates.HEAVY);
			}

			if (input.Special())
			{
				inControl = false;
				SetState((int)SpecificStates.SPECIAL);
			}


			blockingHigh = input.Back() && !input.Down();
			blockingLow = input.DownBack();
		} 
		else if (!onGround && knocked <= 0)
		{
			if (input.NoDirection())
			{
				SetState(GenericStates.INAIR);
			}
		}

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

	public void GetHit(int damage, bool islow, int stun, int blockstun, bool knockdown)
	{
		if (islow)
		{
			if (blockingLow)
			{
				hitstun = blockstun;
				SetState(GenericStates.BLOCKSTUN);
			}
			else
			{
				hitstun = stun;
				SetState(GenericStates.HITSTUN);
			}
		} 
		else
		{
			if (blockingHigh)
			{
				hitstun = blockstun;
				SetState(GenericStates.BLOCKSTUN);
			}
			else
			{
				hitstun = stun;
				SetState(GenericStates.HITSTUN);
			}
		}

		// health - damage
	}
}
