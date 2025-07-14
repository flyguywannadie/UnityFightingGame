using System;
using System.Collections.Generic;
using UnityEngine;

public enum BoxType
{
    HITBOX,
    HURTBOX
}

[CreateAssetMenu(fileName = "New Char Animation", menuName = "Scriptable Objects/CharacterAnimation")]
public class CharacterAnimation : ScriptableObject
{
    [Serializable]
    public struct AnimFrames
    {
        public int duration;
        public Sprite sprite;
    }

    [Serializable]
    public struct FrameData
    {
        public int duration;
        public BoxData[] boxes;
    }

    [Serializable]
    public struct BoxData
    {
        public Vector2 position;
        public Vector2 size;
        public BoxType boxType;
    }

    [Serializable]
    public struct AnimEvent
    {
        public int callFrame;
        public string name;
    }

    public int ID;
    public AnimFrames[] Data;
    public List<FrameData> HitboxData;
	public bool loop = true;
    public bool changeStateOnFinish = false;
    public CharacterState endState;
    public List<AnimEvent> Events;

    public Sprite GetCurrentSprite(int frame)
    {
        for (int i = 0; i < Data.Length; i++)
        {
            if (Data[i].duration > frame)
            {
                return Data[i].sprite;
            }
            else
            {
                frame -= Data[i].duration;
            }
        }

        return Data[Data.Length - 1].sprite;
    }

    public int GetAnimationDuration()
    {
        int total = 0;

		foreach (AnimFrames frame in Data)
		{
            total += frame.duration;
		}

		return total;
    }

    public int GetHitboxDataIndex(int frame)
    {
        int duration = 0;
        for (int i = 0; i < HitboxData.Count; i++)
        {
            duration += HitboxData[i].duration;
            if (frame < duration)
            {
                return i;
            }
        }

        return HitboxData.Count - 1;
    }

    public FrameData GetHitboxData(int index)
    {
        index = Mathf.Clamp(index, 0, HitboxData.Count - 1);
		//for (int i = 0; i < HitboxData.Count; i++)
		//{
		//	if (HitboxData[i].duration > frame)
		//	{
		//		return HitboxData[i];
		//	}
		//	else
		//	{
		//		frame -= HitboxData[i].duration;
		//	}
		//}
		return HitboxData[index];
    }
}
