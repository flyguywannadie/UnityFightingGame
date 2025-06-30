using UnityEngine;

[CreateAssetMenu(fileName = "New Crouching State", menuName = "Scriptable Objects/States/Crouching")]
public class State_Crouching : BaseState
{
	public override bool HandleGettingHit(BufferedInput input, bool low)
	{
		return (input.DownBack() && low);
	}

	public override void StateUpdate(BaseCharacter c, BufferedInput input)
	{
		if (!input.Down())
		{
			c.SetState(CharacterState.STANDING);
			c.SetSubState(CharacterSubStates.IDLE);

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

			if (input.Up())
			{
				c.SetSubState(CharacterSubStates.JUMP);
				c.LoseControl();
				c.SetMotion(0, 0);
			}
		}
		else
		{
			c.SetSubState(CharacterSubStates.CROUCH);
		}
	}
}
