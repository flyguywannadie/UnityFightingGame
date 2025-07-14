using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField] private InputBuffer[] characterControllers;

	private class CharCollision
	{
		private BaseCharacter taker;

		public CharCollision(BaseCharacter taker)
		{
			this.taker = taker;
		}

		public void DoCollision()
		{
			taker.ProcessGettingHit(true, false);
		}
	}
	[SerializeField] private List<CharCollision> collisions = new List<CharCollision>();

	private void FixedUpdate()
	{
		foreach (InputBuffer control in characterControllers)
		{
			control.InputUpdate();
		}

		foreach (CharCollision collision in collisions)
		{
			collision.DoCollision();
		}
	}

	public static void QueueCollision(BaseCharacter taker)
	{
		
	}
}
