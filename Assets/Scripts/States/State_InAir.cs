using Unity.VisualScripting;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

[CreateAssetMenu(fileName = "New InAir State", menuName = "Scriptable Objects/States/InAir")]
public class State_InAir : BaseState
{
	public override bool HandleGettingHit(BufferedInput input, bool low)
	{
		return (input.Back());
	}

	public override void StateUpdate(BaseCharacter c, BufferedInput input)
	{
		c.SetSubState(CharacterSubStates.INAIR);
	}
}
