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
			int usedSpeed = c.GetSpeed();

			if (c.AmIFacingBackward())
			{
				usedSpeed *= -1;
			}

			if (input.Back())
			{
				c.SetMotion(-usedSpeed, 0);
				c.SetSubState(CharacterSubStates.BACKWALKING);
				Debug.Log("Back");
			}

			if (input.Forward())
			{
				c.SetMotion(usedSpeed, 0);
				c.SetSubState(CharacterSubStates.WALKING);
				Debug.Log("Forward");
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
			c.SetSubState(CharacterSubStates.CROUCH);
			c.SetMotion(0, 0);
		}
		else if (input.Up())
		{
			c.SetSubState(CharacterSubStates.JUMP);
			c.LoseControl();
			c.SetMotion(0, 0);
		}
	}
}
