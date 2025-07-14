using System.Collections.Generic;
using UnityEngine;

public class HitboxBuilder : MonoBehaviour
{
	[SerializeField] private int boxCount = 6;
	[SerializeField] private Transform boxPrefab;
	[SerializeField] private List<Hitbox> boxPool;
	[SerializeField] private Transform character;

	private void Start()
	{
		// I don't know if it is necessary to do this so I am not right now
		
		// spawn in all the boxes
		// add them to box pool
	}

	public void BuildHitbox(CharacterAnimation.FrameData frameData)
	{
		ClearBoxes();

		if (frameData.boxes == null || frameData.boxes.Length <= 0)
		{
			return;
		}

		if (frameData.boxes.Length > boxPool.Count)
		{
			Debug.LogError("FrameData is too complex for this pool of boxes: " + frameData.boxes.Length + " > " + boxPool.Count);
			return;
		}

		CharacterAnimation.BoxData usedBox;

		for (int i = 0; i < frameData.boxes.Length; i++)
		{
			usedBox = frameData.boxes[i];

			boxPool[i].Build(character.position, usedBox);
		}
	}

	public void ClearBoxes()
	{
		foreach (Hitbox box in boxPool)
		{
			box.Clear();
		}
	}
}
