using System;
using UnityEngine;

[Serializable]
public class BufferedInput
{
	[SerializeField] public int inputFlag;
	[SerializeField] public int frames;

	private const int FORWARD = 0b0001;
	private const int DOWN = 0b0010;
	private const int BACK = 0b0100;
	private const int UP = 0b1000;
	private const int LIGHT = 0b10000;
	private const int HEAVY = 0b100000;
	private const int SPECIAL = 0b1000000;

	#region Input Setters

	public void SetForward()
	{
		inputFlag |= FORWARD;
	}

	public void SetDown()
	{
		inputFlag |= DOWN;
	}

	public void SetBack()
	{
		inputFlag |= BACK;
	}

	public void SetUp()
	{
		inputFlag |= UP;
	}

	public void SetLight()
	{
		inputFlag |= LIGHT;
	}

	public void SetHeavy()
	{
		inputFlag |= HEAVY;
	}

	public void SetSpecial()
	{
		inputFlag |= SPECIAL;
	}

	public void FlipForwardBack()
	{
		bool f = (inputFlag & FORWARD) == FORWARD;
		bool b = (inputFlag & BACK) == BACK;

		if (f^b)
		{
			inputFlag ^= (FORWARD | BACK);
		}
	}

	#endregion

	#region Input Movement Boolean Logic

	public bool NoDirection()
	{
		return inputFlag == 0;
	}

	public bool Walking()
	{
		return Forward() ^ Back();
	}

	public bool Forward()
	{
		return (inputFlag & FORWARD) == FORWARD;
	}

	public bool DownForward()
	{
		return Forward() & Down();
	}

	public bool Down()
	{
		return (inputFlag & DOWN) == DOWN;
	}

	public bool DownBack()
	{
		return Back() & Down();
	}

	public bool Back()
	{
		return (inputFlag & BACK) == BACK;
	}

	public bool UpBack()
	{
		return Back() & Up();
	}

	public bool Up()
	{
		return (inputFlag & UP) == UP;
	}

	public bool UpForward()
	{
		return Forward() & Up();
	}

	public bool PressingAttacks()
	{
		return Light() | Heavy() | Special();
	}

	public bool Light()
	{
		return (inputFlag & LIGHT) == LIGHT;
	}

	public bool Heavy()
	{
		return (inputFlag & HEAVY) == HEAVY;
	}

	public bool Special()
	{
		return (inputFlag & SPECIAL) == SPECIAL;
	}

	#endregion
}
