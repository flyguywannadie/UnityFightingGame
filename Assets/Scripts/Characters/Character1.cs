using UnityEngine;

public class Character1 : BaseCharacter
{
    protected enum SpecificStates
    {
        LIGHT = 100,
        HEAVY = 101,
        SPECIAL = 102
    }

	public override void CharUpdate(BufferedInput input)
	{
		if (onGround && knocked <= 0 && inControl)
		{
			if (AmIFacingBackward())
			{
				input.FlipForwardBack();
			}
			Debug.Log(input.DownBack());

			if (input.DoingNothing())
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
				SetState(GenericStates.BLOCKSTUN);
			}

			if (input.Special())
			{
				SetState(GenericStates.HITSTUN);
			}
		} 
		else if (!onGround)
		{
			if (input.Light())
			{
				motion.y = 3;
				knocked = 30;
				SetState(GenericStates.INAIRKNOCKDOWNUP);
			}

			if (input.Heavy())
			{
				SetState(GenericStates.INAIRBLOCKSTUN);
			}

			if (input.Special())
			{
				SetState(GenericStates.INAIRHITSTUN);
			}
		}

		base.CharUpdate(input);
	}
}
