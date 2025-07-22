using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager instance { get; private set; }
	[SerializeField] private InputBuffer[] characterControllers;
	[SerializeField] private BaseCharacter[] characters;

	[SerializeField] private bool frameAdvanceMode = false;
	[SerializeField] private bool advanceFrame = false;

	[Serializable]
	private class CharInteraction
	{
		private BaseCharacter taker;
		private HurtboxProperties property;

		public CharInteraction(BaseCharacter taker, HurtboxProperties property)
		{
			this.taker = taker;
			this.property = property;
		}

		public void DoInteraction()
		{
			//Debug.Log(taker.name + " Has Been Hit");
			taker.GetHit(property);
		}
	}
	[SerializeField] private List<CharInteraction> interactions = new List<CharInteraction>();

	private void Start()
	{
		instance = this;
		RestartGame();
	}

	private void RestartGame()
	{
		characters[0].transform.parent.position = new Vector3(-4,0,0);
		characters[1].transform.parent.position = new Vector3(4,0,0);

		characters[0].ResetChar();
		characters[1].ResetChar();
	}

	private void FixedUpdate()
	{
		if (!frameAdvanceMode || (frameAdvanceMode && advanceFrame))
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

			advanceFrame = false;
		}
	}

	public void Update()
	{
		var property = new HurtboxProperties();
		property.damage = 0;
		property.hitstun = 30;
		property.blockstun = 45;
		property.attackHeight = AttackHeight.NORMAL;

		if (Input.GetKeyDown(KeyCode.H))
		{
			QueueCollision(characters[0],property);
			QueueCollision(characters[1],property);
		}
		else if (Input.GetKeyDown(KeyCode.J))
		{
			property.attackHeight = AttackHeight.LOW;
			QueueCollision(characters[0], property);
			QueueCollision(characters[1], property);
		}
		else if (Input.GetKeyDown(KeyCode.K))
		{
			property.attackHeight = AttackHeight.OVERHEAD;
			QueueCollision(characters[0], property);
			QueueCollision(characters[1], property);
		}
		else if (Input.GetKeyDown(KeyCode.L))
		{
			property.attackHeight = AttackHeight.UNBLOCKABLE;
			QueueCollision(characters[0],property);
			QueueCollision(characters[1],property);
		}

		if (Input.GetKeyDown(KeyCode.Backspace))
		{
			RestartGame();
		}

		if (Input.GetKeyDown(KeyCode.G))
		{
			frameAdvanceMode = !frameAdvanceMode;
		}

		if (Input.GetKeyDown(KeyCode.T))
		{
			advanceFrame = true;
		}
	}

	public void QueueCollision(BaseCharacter taker, HurtboxProperties property)
	{
		interactions.Add(new CharInteraction(taker, property));
	}
}
