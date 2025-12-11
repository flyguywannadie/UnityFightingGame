using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputBuffer : MonoBehaviour
{
	public enum InputMode
	{
		NONE = 0,
		CPU = 1,
		PLAYER = 2,
	}

	[SerializeField] private InputMode inputMode;

    [SerializeField] private SpriteRenderer direction;
    [SerializeField] private Sprite[] directions;

    [SerializeField] private BaseCharacter myCharacter;

	[SerializeField] private List<BufferedInput> inputs;
	private BufferedInput sentInput;
	//[SerializeField] private int inputIndex = 0;

	private bool w;
	private bool a;
	private bool s;
	private bool d;
	private bool u;
	private bool i;
	private bool o;

	public void Start()
	{
		ClearBuffer();
		AddNewInput(new BufferedInput());
		sentInput = new BufferedInput();
	}

	//private void InputCheck()
	//{
	//	if (inputMode == InputMode.NONE)
	//	{
	//		if (!a && IsKeyPressed(KeyCode.LeftArrow)) { a = true; }
	//		if (!s && IsKeyPressed(KeyCode.DownArrow)) { s = true; }
	//		if (!d && IsKeyPressed(KeyCode.RightArrow)) { d = true; }
	//		if (!w && IsKeyPressed(KeyCode.UpArrow)) { w = true; }
	//		if (!u && Input.GetKeyDown(KeyCode.B)) { u = true; }
	//		if (!i && Input.GetKeyDown(KeyCode.N)) { i = true; }
	//		if (!o && Input.GetKeyDown(KeyCode.M)) { o = true; }
	//		return;
	//	}

	//	if (!a && IsKeyPressed(KeyCode.A)) { a = true; }
	//	if (!s && IsKeyPressed(KeyCode.S)) { s = true; }
	//	if (!d && IsKeyPressed(KeyCode.D)) { d = true; }
	//	if (!w && IsKeyPressed(KeyCode.Space)) { w = true; }
	//	if (!u && Input.GetKeyDown(KeyCode.U)) { u = true; }
	//	if (!i && Input.GetKeyDown(KeyCode.I)) { i = true; }
	//	if (!o && Input.GetKeyDown(KeyCode.O)) { o = true; }
	//}

	public void OnUp(InputAction.CallbackContext ctx)
	{
		var phas = ctx.action.phase;
		if (phas == InputActionPhase.Started)
		{
			w = true;
		}
		else if (phas == InputActionPhase.Canceled)
		{
			w = false;
		}
	}

	public void OnDown(InputAction.CallbackContext ctx)
	{
		var phas = ctx.action.phase;
		if (phas == InputActionPhase.Started)
		{
			s = true;
		}
		else if (phas == InputActionPhase.Canceled)
		{
			s = false;
		}
	}

	public void OnLeft(InputAction.CallbackContext ctx)
	{
		var phas = ctx.action.phase;
		if (phas == InputActionPhase.Started)
		{
			a = true;
		}
		else if (phas == InputActionPhase.Canceled)
		{
			a = false;
		}
	}

	public void OnRight(InputAction.CallbackContext ctx)
	{
		var phas = ctx.action.phase;
		if (phas == InputActionPhase.Started)
		{
			d = true;
		}
		else if (phas == InputActionPhase.Canceled)
		{
			d = false;
		}
	}

	public void OnLight(InputAction.CallbackContext ctx)
	{
		var phas = ctx.action.phase;
		if (phas == InputActionPhase.Started)
		{
			u = true;
		}
	}

	public void OnHeavy(InputAction.CallbackContext ctx)
	{
		var phas = ctx.action.phase;
		if (phas == InputActionPhase.Started)
		{
			i = true;
		}
	}

	public void OnSpecial(InputAction.CallbackContext ctx)
	{
		var phas = ctx.action.phase;
		if (phas == InputActionPhase.Started)
		{
			o = true;
		}
	}

	public void InputUpdate()
    {
		//InputCheck();

		UpdateBuffer();

		sentInput.CopyInput(inputs[0]);

		myCharacter.CharUpdate(sentInput);
	}

	private void UpdateBuffer()
	{
		int dir = 4;
		BufferedInput newInput = new BufferedInput();

		if (a)
		{
			dir -= 1;
			newInput.SetBack();
			//a = false;
		}
		if (s)
		{
			dir -= 3;
			newInput.SetDown();
			//s = false;
		}
		if (d)
		{
			dir += 1;
			newInput.SetForward();
			//d = false;
		}
		if (w)
		{
			dir += 3;
			newInput.SetUp();
			//w = false;
		}
		if (u)
		{
			newInput.SetLight();
			u = false;
		}
		if (i)
		{
			newInput.SetHeavy();
			i = false;
		}
		if (o)
		{
			newInput.SetSpecial();
			o = false;
		}

		direction.sprite = directions[dir];

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

	public bool ReadBufferForMotion(MotionDefinition motion, bool flip)
	{
		if (motion.motion.Length == 0)
		{
			return true;
		}

        if (motion.motion.Length > inputs.Count)
        {
            return false;
        }

        BufferedInput b = motion.motion[0].DirectionAsInputFlag();
        BufferedInput b2 = inputs[0];

		if (flip)
		{
			b2.FlipForwardBack();
		}

        if (motion.motion.Length == 1)
		{
            return b2.CompareDirectionLeniant(b);
		}

        /*
		 * 	var sequenceOrder = 0
			for x in checkedBuffer: #checking through the buffer
				if (sequenceOrder > 0) : #ignore time for the first sequence to enable command normals and not worrying about idling before doing a motion
					time += x.frames # add frames to time
					#print(time)
					if (time > timeGiven) :
						print(time, " > ", timeGiven)
						return false

				if (sequenceOrder >= sequence.length()) : # skip loop if the sequence has been run through
					continue

				print(x.input, " -o: ", sequence[sequenceOrder], " -t: ", time) # debug print buffer and sequence and time it has taken
				if (x.input[0] == sequence[sequenceOrder]) :
					sequenceOrder += 1
					checked += x.input[0]
		 */

        int motionIndex = motion.motion.Length - 1;
        int totalTime = 0;

        for (int i = 0; i < inputs.Count; i++)
		{
            b = motion.motion[motionIndex].DirectionAsInputFlag();
			b2 = inputs[i];

			if (flip)
            {
                b2.FlipForwardBack();
            }

            if (b2.CompareDirectionStrict(b))
			{
				motionIndex--;

				if (motionIndex < 0)
				{
					return true;
				}
			}

            totalTime += b2.frames;
            //Debug.Log(b2.inputFlag + " - " + b.inputFlag + " - " + totalTime);
            
			if (totalTime > 20)
            {
                return false;
            }
		}

		return false;
	}

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
