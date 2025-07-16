using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager instance { get; private set; }
	[SerializeField] private InputBuffer[] characterControllers;

	private class CharInteraction
	{
		private BaseCharacter taker;
		private bool low;
		private bool overhead;

		public CharInteraction(BaseCharacter taker, bool low, bool overhead)
		{
			this.taker = taker;
			this.low = low;
			this.overhead = overhead;
		}

		public void DoInteraction()
		{
			taker.GetHit(low, overhead);
		}
	}
	[SerializeField] private List<CharInteraction> interactions = new List<CharInteraction>();

	private void Start()
	{
		instance = this;
	}

	private void FixedUpdate()
	{
		foreach (InputBuffer control in characterControllers)
		{
			control.InputUpdate();
		}

		if (interactions.Count > 0)
		{
			foreach (CharInteraction collision in interactions)
			{
				collision.DoInteraction();
			}
			interactions.Clear();
		}
	}

	public void QueueCollision(BaseCharacter taker, bool low, bool overhead)
	{
		interactions.Add(new CharInteraction(taker, low, overhead));
	}
}
