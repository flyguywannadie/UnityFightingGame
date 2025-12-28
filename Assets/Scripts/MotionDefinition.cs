using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[CreateAssetMenu(fileName = "New Motion", menuName = "Scriptable Objects/Motion Definition")]
public class MotionDefinition : ScriptableObject
{
    public enum Direction
    {
        FORWARD = 0,
        DOWN = 1,
        BACK = 2,
        UP = 3,
        DOWNFORWARD = 4,
        DOWNBACK = 5,
        UPBACK = 6,
        UPFORWARD = 7
    }

    [Serializable]
    public class MotionPiece
    {
        public Direction direction = Direction.FORWARD;
        public int frameRequirement = 0;

        public BufferedInput DirectionAsInputFlag()
        {
            BufferedInput b = new BufferedInput();

            switch (direction)
            {
                case MotionDefinition.Direction.FORWARD:
                    b.SetForward();
                    break;
                case MotionDefinition.Direction.DOWNFORWARD:
                    b.SetForward();
                    b.SetDown();
                    break;
                case MotionDefinition.Direction.DOWN:
                    b.SetDown();
                    break;
                case MotionDefinition.Direction.DOWNBACK:
                    b.SetDown();
                    b.SetBack();
                    break;
                case MotionDefinition.Direction.BACK:
                    b.SetBack();
                    break;
                case MotionDefinition.Direction.UPBACK:
                    b.SetBack();
                    b.SetUp();
                    break;
                case MotionDefinition.Direction.UP:
                    b.SetUp();
                    break;
                case MotionDefinition.Direction.UPFORWARD:
                    b.SetUp();
                    b.SetForward();
                    break;
            }

            return b;
        }
    }

    public MotionPiece[] motion;
}
