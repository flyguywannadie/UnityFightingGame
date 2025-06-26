using UnityEngine;

public abstract class BaseState : ScriptableObject
{
	//void SetState(BaseCharacter character, int val = 0);
	[SerializeField] private MoveDefinition[] moves;

	public abstract void StateUpdate(BaseCharacter c, BufferedInput input);
	public abstract void HandleGettingHit(BaseCharacter c, BufferedInput input, bool low);
	public abstract void HandleMovement(BaseCharacter c, BufferedInput input);

	public MoveDefinition[] GetMoves()
	{
		return moves;
	}
}
