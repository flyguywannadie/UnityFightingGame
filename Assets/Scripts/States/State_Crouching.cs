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
		c.SetMotion(0, 0);
		if (!input.Down())
		{
			c.SetState(CharacterState.STANDING);
			if (input.Up())
			{
				c.SetState(CharacterState.INAIR);

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

				c.JumpAction();
			}
		}
	}
}
