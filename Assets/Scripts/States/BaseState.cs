using UnityEngine;

public abstract class BaseState : ScriptableObject
{
	//void SetState(BaseCharacter character, int val = 0);
	[SerializeField] private MoveDefinition[] moves;

	public abstract void StateUpdate(BaseCharacter c, BufferedInput input);
	public abstract bool HandleGettingHit(BufferedInput input, bool low);

	public MoveDefinition[] GetMoves()
	{
		return moves;
	}
}
