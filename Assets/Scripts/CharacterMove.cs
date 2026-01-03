using UnityEngine;

[CreateAssetMenu(fileName = "CharacterMove", menuName = "Scriptable Objects/CharacterMove")]
public class CharacterMove : ScriptableObject
{
    [SerializeField] public int cancelPriority;
    //[SerializeField] public bool specialCancel;
    [SerializeField] public bool inAir;
    [SerializeField] public MotionDefinition motion;
    [SerializeField] public CharacterAnimation anim;

    [Header("Required Button Input"), SerializeField] public bool Light;
    [SerializeField] public bool Heavy;
    [SerializeField] public bool Special;
    [SerializeField] public bool AnyOfTheRequiredInputs;
}
