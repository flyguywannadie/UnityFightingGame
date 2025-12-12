using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

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
        //public BoxData[] boxes;
		public BaseBoxData[] hitboxes;
		public HurtBoxData[] hurtboxes;
	}

    [Serializable]
    public struct BoxData
    {
        public Vector2 position;
        public Vector2 size;
	}

    [Serializable]
    public struct AnimEvent
    {
        public int callFrame;
        public string name;
    }

    public CommonAnimations ID;
    public int customID;
    public AnimFrames[] Data;
    [FormerlySerializedAs("HitboxData")] public List<FrameData> hitboxDatas;
	public bool loop = true;
    public bool changeStateOnFinish = false;
    public CharacterState endState;
    public List<AnimEvent> Events;
    public int cancelWindowStart = -1;
    public int cancelWindowEnd = -1;
    public List<int> cancelIDs;

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
        for (int i = 0; i < hitboxDatas.Count; i++)
        {
            duration += hitboxDatas[i].duration;
            if (frame < duration)
            {
                return i;
            }
        }

        return hitboxDatas.Count - 1;
    }

    public FrameData GetHitboxData(int index)
    {
        index = Mathf.Clamp(index, 0, hitboxDatas.Count - 1);
		//for (int i = 0; i < hitboxDatas.Count; i++)
		//{
		//	if (hitboxDatas[i].duration > frame)
		//	{
		//		return hitboxDatas[i];
		//	}
		//	else
		//	{
		//		frame -= hitboxDatas[i].duration;
		//	}
		//}
		return hitboxDatas[index];
    }

    public int GetAnimID()
    {
        if (ID == CommonAnimations.CUSTOM)
        {
            return customID;
        }
        return (int)ID;
    }
}
