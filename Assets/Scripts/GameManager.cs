using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField] private InputBuffer[] characterControllers;

	private void FixedUpdate()
	{
		foreach (InputBuffer control in characterControllers)
		{
			control.InputUpdate();
		}
	}
}
