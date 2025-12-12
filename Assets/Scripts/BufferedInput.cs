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

	public void Clear()
	{
		inputFlag = 0;
	}

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
		int inputcopy = inputFlag;

		bool f = ((inputcopy & FORWARD) == FORWARD);
		bool b = ((inputcopy & BACK) == BACK);

		if (f^b)
		{
			inputFlag ^= (FORWARD | BACK);
		}
	}

	public void CopyInput(BufferedInput input)
	{
		inputFlag = input.inputFlag;
		frames = input.frames;
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
		return F() & !B();
	}

	private bool F()
	{
		return (inputFlag & FORWARD) == FORWARD;
	}

	public bool DownForward()
	{
		return Forward() & Down();
	}

	public bool Down()
	{
		return D() & !U();
	}

	private bool D()
	{
		return (inputFlag & DOWN) == DOWN;
	}

	public bool DownBack()
	{
		return Back() & Down();
	}

	public bool Back()
	{
		return B() & !F();
	}

	private bool B()
	{
		return (inputFlag & BACK) == BACK;
	}

	public bool UpBack()
	{
		return Back() & Up();
	}

	public bool Up()
	{
		return U() & !D();
	}

	private bool U()
	{
		return (inputFlag & UP) == UP;
	}

	public bool UpForward()
	{
		return Forward() & Up();
	}

	public bool CompareDirectionLeniant(BufferedInput b)
	{
        return (b.Up() && Up()) || (b.Forward() && Forward()) || (b.Down() && Down()) || (b.Back() && Back());
    }

	public bool CompareDirectionStrict(BufferedInput b)
	{
        return !(b.Up() ^ Up()) && !(b.Forward() ^ Forward()) && !(b.Down() ^ Down()) && !(b.Back() ^ Back());
    }

	#endregion

	#region Input Attack Boolean Logic

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

    public override string ToString()
    {
		string toret = "";
		int temp = 0b0001111 & inputFlag;

		switch (temp)
		{
			case 0b0000:
				toret += ".*.";
				break;
            case FORWARD:
                toret += "..>";
                break;
            case (FORWARD | DOWN):
                toret += ".v>";
                break;
            case DOWN:
                toret += ".v.";
                break;
            case (DOWN | BACK):
                toret += "<v.";
                break;
            case BACK:
                toret += "<..";
                break;
            case (BACK | UP):
                toret += "<^.";
                break;
            case UP:
                toret += ".^.";
                break;
            case (UP | FORWARD):
                toret += ".^>";
                break;
        }

		return toret + "(" + frames + ")";
    }
}
