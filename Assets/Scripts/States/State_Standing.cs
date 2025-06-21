using UnityEngine;

public class State_Standing : IState
{
	public void SetState(BaseCharacter character, int val = 0)
	{
		throw new System.NotImplementedException();
	}

	public int StateUpdate(BufferedInput input)
	{
		return (int)GenericStates.IDLE;
	}
}
