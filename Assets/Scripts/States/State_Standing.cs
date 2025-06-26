using UnityEngine;

[CreateAssetMenu(fileName = "New Grounded State", menuName = "Scriptable Objects/States/Grounded")]
public class State_Standing : BaseState
{

	public override bool HandleGettingHit(BufferedInput input, bool low)
	{
		return (input.Back() && !input.Down() && low);
	}

	public override void StateUpdate(BaseCharacter c, BufferedInput input)
	{
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
			c.SetSubState(CharacterSubStates.IDLE);
			c.SetMotion(0, 0);
		}

		if (input.Down())
		{
			c.SetState(CharacterState.CROUCHING);
		}
		else if (input.Up())
		{
			//c.SetState(CharacterState.INAIR);
			//c.JumpAction();
			c.SetSubState(CharacterSubStates.JUMP);
			c.LoseControl();
		}
	}
}
