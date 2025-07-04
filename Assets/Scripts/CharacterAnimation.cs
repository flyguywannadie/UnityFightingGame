using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Char Animation", menuName = "Scriptable Objects/CharacterAnimation")]
public class CharacterAnimation : ScriptableObject
{
    [Serializable]
    public struct FrameData
    {
        public int duration;
        public Sprite sprite;
    }

    [Serializable]
    public struct AnimEvent
    {
        public int callFrame;
        public string name;
    }

    public int ID;
    public FrameData[] Data;
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

        return Data[0].sprite;
    }

    public int GetAnimationDuration()
    {
        int total = 0;

		foreach (FrameData frame in Data)
		{
            total += frame.duration;
		}

		return total;
    }
}
