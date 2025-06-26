using UnityEngine;
using static Unity.Collections.AllocatorManager;

[CreateAssetMenu(fileName = "New InAir State", menuName = "Scriptable Objects/States/InAir")]
public class State_InAir : BaseState
{
	public override void HandleGettingHit(BaseCharacter c, BufferedInput input, bool low)
	{
		bool blocked = false;
		if (input.Back())
		{
			//hitstun = blockstun;
			blocked = true;
		}

		c.GetHit(0, 30, blocked);
	}

	public override void HandleMovement(BaseCharacter c, BufferedInput input)
	{
		//throw new System.NotImplementedException();
		c.AddMotion(0, -9.8f * Time.fixedDeltaTime);
	}

	public override void StateUpdate(BaseCharacter c, BufferedInput input)
	{
		//throw new System.NotImplementedException();
		if (c.IsOnGround())
		{
			if (input.Down())
			{
				c.SetState(CharacterState.CROUCHING);
			}
			else
			{
				c.SetState(CharacterState.STANDING);
			}

			c.LandFromJump();
		}
	}
}
