using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Move", menuName = "Scriptable Objects/Move Definition")]
public class MoveDefinition : ScriptableObject
{
    public enum Direction
    {
        FORWARD,
        DOWNFORWARD,
        DOWN,
        DOWNBACK,
        BACK,
        UPBACK,
        UP,
        UPFORWARD
    }

    public enum Button
    {
        NONE,
        LIGHT,
        HEAVY,
        SPECIAL
    }

    [Serializable]
    public class MotionPiece
    {
        public Direction direction = Direction.FORWARD;
        public Button button = Button.NONE;
        public int frameRequirement = 0;
    }

    public string moveName = "New Move";
    public Button action = Button.LIGHT;
    public MotionPiece[] motion;
    public int moveState;
}
