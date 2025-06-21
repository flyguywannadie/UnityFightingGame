using UnityEngine;

public interface IState
{
	void SetState(BaseCharacter character, int val = 0);

	int StateUpdate(BufferedInput input);
}
