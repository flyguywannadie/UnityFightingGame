using UnityEngine;

[CreateAssetMenu(fileName = "New InAir State", menuName = "Scriptable Objects/States/InAir")]
public class State_InAir : BaseState
{
	public override void HandleGettingHit(BaseCharacter c, BufferedInput input)
	{
		if (input.UpBack())
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
		throw new System.NotImplementedException();
	}

	public override void StateUpdate(BaseCharacter c, BufferedInput input)
	{
		throw new System.NotImplementedException();
	}
}
