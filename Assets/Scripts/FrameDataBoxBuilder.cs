using System.Collections.Generic;
using UnityEngine;

public class FrameDataBoxBuilder : MonoBehaviour
{
	[SerializeField] private int boxCount = 6;
	[SerializeField] private Transform boxPrefab;
	[SerializeField] private List<BuiltHitbox> hitboxPool;
	[SerializeField] private List<BuiltHurtbox> hurtboxPool;
	[SerializeField] private Transform character;
	[SerializeField] private BaseCharacter instigator;

	public void Start()
	{
        // I don't know if it is necessary to do this so I am not right now

        // spawn in all the boxes
        // add them to box pool

        foreach (BuiltHurtbox box in hurtboxPool)
        {
			box.SetInstigator(instigator);
        }
    }

	public void BuildHitbox(CharacterAnimation.FrameData frameData)
	{
		ClearBoxes();

		// replace this with hitboxes
		if (frameData.hitboxes.Length > 0)
		{
			if (frameData.hitboxes.Length > hitboxPool.Count)
			{
				Debug.LogError("FrameData is too complex for this pool of boxes: " + frameData.hitboxes.Length + " > " + hitboxPool.Count);
				return;
			}

			BaseBoxData usedBox;

			for (int i = 0; i < frameData.hitboxes.Length; i++)
			{
				usedBox = frameData.hitboxes[i];

				hitboxPool[i].Build(character.position, usedBox);
			}
		}

		// replace this with hurtboxes
		if (frameData.hurtboxes.Length > 0)
		{
			if (frameData.hurtboxes.Length > hurtboxPool.Count)
			{
				Debug.LogError("FrameData is too complex for this pool of boxes: " + frameData.hurtboxes.Length + " > " + hurtboxPool.Count);
				return;
			}

			HurtBoxData usedBox;

			for (int i = 0; i < frameData.hurtboxes.Length; i++)
			{
				usedBox = frameData.hurtboxes[i];

				hurtboxPool[i].Build(character.position, usedBox);
			}
		}
	}

	public void ClearBoxes()
	{
		foreach (BuiltHitbox box in hitboxPool)
		{
			box.Clear();
		}

		foreach (BuiltHurtbox box in hurtboxPool)
		{
			box.Clear();
		}
	}
}
