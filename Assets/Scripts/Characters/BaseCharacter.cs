using UnityEngine;

public abstract class BaseCharacter : MonoBehaviour
{
	protected enum GenericStates
	{
		IDLE = 0,
		WALKING = 1,
		BLOCKSTUN = 2,
		HITSUN = 3,
		JUMP = 4,
		INAIR = 5,
		INAIRBLOCKSTUN = 6,
		INAIRHITSTUN = 7,
		INAIRKNOCKDOWNUP = 8,
		INAIRKNOCKDOWNMID = 9,
		INAIRKNOCKDOWNDOWN = 10,
		KNOCKDOWN = 11,
		ONGROUND = 12,
		GETUP = 13,
	}

	[SerializeField] protected int speed = 5;
	[SerializeField] protected int myState;

	public abstract void CharUpdate();
}
