using UnityEngine;

public abstract class BaseState
{
	//void SetState(BaseCharacter character, int val = 0);
	public abstract void OnEnterState(BaseCharacter c, BufferedInput input);
	public abstract void StateUpdate(BaseCharacter c, BufferedInput input);
	public abstract void OnExitState(BaseCharacter c, BufferedInput input);
	public abstract bool HandleGettingHit(BufferedInput input, bool low, bool overhead);
	//public virtual void MovementOverride(BaseCharacter c, BufferedInput input) {}
}
