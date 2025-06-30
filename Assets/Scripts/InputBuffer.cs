using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputBuffer : MonoBehaviour
{
    [SerializeField] private SpriteRenderer direction;
    [SerializeField] private Sprite[] directions;

    [SerializeField] private BaseCharacter myCharacter;

	[SerializeField] private List<BufferedInput> inputs;
	private BufferedInput sentInput;
	//[SerializeField] private int inputIndex = 0;

	public void Start()
	{
		ClearBuffer();
		AddNewInput(new BufferedInput());
		sentInput = new BufferedInput();
	}

	public void InputUpdate()
    {
		UpdateBuffer();

		sentInput.CopyInput(inputs[0]);
		myCharacter.CharUpdate(sentInput);
	}

	private void UpdateBuffer()
	{
		int d = 4;
		BufferedInput newInput = new BufferedInput();

		if (IsKeyPressed(KeyCode.A))
		{
			d -= 1;
			newInput.SetBack();
		}
		if (IsKeyPressed(KeyCode.S))
		{
			d -= 3;
			newInput.SetDown();
		}
		if (IsKeyPressed(KeyCode.D))
		{
			d += 1;
			newInput.SetForward();
		}
		if (IsKeyPressed(KeyCode.Space))
		{
			d += 3;
			newInput.SetUp();
		}
		if (Input.GetKeyDown(KeyCode.U))
		{
			newInput.SetLight();
		}
		if (Input.GetKeyDown(KeyCode.I))
		{
			newInput.SetHeavy();
		}
		if (Input.GetKeyDown(KeyCode.O))
		{
			newInput.SetSpecial();
		}

		direction.sprite = directions[d];

		if (newInput.inputFlag != inputs[0].inputFlag)
		{
			AddNewInput(newInput);
			//SelectNextInput(newinputflag);
		}

		inputs[0].frames++;
	}

	//private void SelectNextInput(int newinputflag)
	//{
	//	inputIndex++;
	//	if (inputIndex >= inputs.Length)
	//	{
	//		inputIndex = 0;
	//	}

	//	inputs[inputIndex].frames = 0;
	//	inputs[inputIndex].inputFlag = newinputflag;
	//}

	private void AddNewInput(BufferedInput input)
	{
		inputs.Insert(0, input);
		
		if (inputs.Count >= 21)
		{
			inputs.RemoveAt(20);
		}
	}

	private bool IsKeyPressed(KeyCode key)
	{
		return (Input.GetKey(key) || Input.GetKeyDown(key));
	}

	private void ClearBuffer()
	{
		inputs.Clear();
	}
}
