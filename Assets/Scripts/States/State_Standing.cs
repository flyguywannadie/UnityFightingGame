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
		if (input.Down())
		{
			c.SetState(CharacterState.CROUCHING);
		}
		if (input.Up())
		{
			//c.SetState(CharacterState.INAIR);
			//c.JumpAction();
			c.SetSubState(CharacterSubStates.JUMP);
		}
	}

	public override void HandleMovement(BaseCharacter c, BufferedInput input)
	{
		//throw new System.NotImplementedException();
		if (input.Walking())
		{
			c.SetSubState(CharacterSubStates.WALKING);
			int usedSpeed = c.GetSpeed();

			if (c.AmIFacingBackward())
			{
				usedSpeed *= -1;
			}

			if (input.Back())
			{
				c.SetMotion(-usedSpeed, 0);
			}

			if (input.Forward())
			{
				c.SetMotion(usedSpeed, 0);
			}
		}
		else
		{
			c.SetMotion(0, 0);
			c.SetSubState(CharacterSubStates.IDLE);
		}
	}

	public override void HandleGettingHit(BaseCharacter c, BufferedInput input, bool low)
	{
		bool blocked = false;
		if (input.Back() && !input.Down() && low)
		{
			//hitstun = blockstun;
			blocked = true;
		}

		c.GetHit(0, 30, blocked);
	}
}
