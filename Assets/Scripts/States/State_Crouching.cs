using UnityEngine;

[CreateAssetMenu(fileName = "New Crouching State", menuName = "Scriptable Objects/States/Crouching")]
public class State_Crouching : BaseState
{
	public override void HandleGettingHit(BaseCharacter c, BufferedInput input)
	{
		if (input.DownBack())
		{
			//hitstun = blockstun;
			c.SetSubState(CharacterSubStates.BLOCKSTUN);
		}
		else
		{
			//hitstun = stun;
			c.SetSubState(CharacterSubStates.HITSTUN);
		}
	}

	public override void HandleMovement(BaseCharacter c, BufferedInput input)
	{
		c.SetMotion(0, 0);
	}

	public override void StateUpdate(BaseCharacter c, BufferedInput input)
	{
		if (!input.Down())
		{
			c.SetState(CharacterState.STANDING);
		}
	}
}
