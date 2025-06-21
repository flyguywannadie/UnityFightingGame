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

    public class MotionPiece
    {
        public Direction direction = Direction.FORWARD;
        public int frameCount = -1;
    }

    public string name = "New Move";
    public Direction[] motion;
}
