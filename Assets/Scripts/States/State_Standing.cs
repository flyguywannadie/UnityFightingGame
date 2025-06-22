using UnityEngine;

[CreateAssetMenu(fileName = "New Grounded State", menuName = "Scriptable Objects/States/Grounded")]
public class State_Standing : BaseState
{

	//public void SetState(BaseCharacter character, int val = 0)
	//{
	//	throw new System.NotImplementedException();
	//}

	public override void StateUpdate(BaseCharacter c, BufferedInput input)
	{
		if (input.Walking())
		{
			int usedSpeed = c.GetSpeed();

			if (c.AmIFacingBackward())
			{
				usedSpeed *= -1;
			}

			if (input.Back())
			{
				c.SetMotion(-usedSpeed, 0);
				c.SetSubState(CharacterSubStates.BACKWALKING);
			}

			if (input.Forward())
			{
				c.SetMotion(usedSpeed, 0);
				c.SetSubState(CharacterSubStates.WALKING);
			}
		}
		else
		{
			c.SetMotion(0, 0);
		}
	}

	public override void HandleGettingHit(BaseCharacter c, BufferedInput input)
	{
		//if (islow)
		//{
		//	if (blockingLow)
		//	{
		//		hitstun = blockstun;
		//		SetSubState(CharacterSubStates.BLOCKSTUN);
		//	}
		//	else
		//	{
		//		hitstun = stun;
		//		SetSubState(CharacterSubStates.HITSTUN);
		//	}
		//}
		//else
		//{
		//	if (blockingHigh)
		//	{
		//		hitstun = blockstun;
		//		SetSubState(CharacterSubStates.BLOCKSTUN);
		//	}
		//	else
		//	{
		//		hitstun = stun;
		//		SetSubState(CharacterSubStates.HITSTUN);
		//	}
		//}
	}

	public override void HandleMovement(BaseCharacter c, BufferedInput input)
	{
		throw new System.NotImplementedException();
	}
}
