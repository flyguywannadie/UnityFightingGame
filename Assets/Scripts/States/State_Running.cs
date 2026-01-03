using UnityEngine;

public class State_Running : BaseState
{
    public override bool WasAttackBlocked(BufferedInput input, HurtboxProperties property)
    {
        return false;
    }

    public override void OnEnterState(BaseCharacter c, BufferedInput input)
    {
        Run(c, input);
        c.LoseCombo();
    }

    public override void OnExitState(BaseCharacter c, BufferedInput input)
    {
        //c.SetMotion(0, 0);
    }

    public override void StateUpdate(BaseCharacter c, BufferedInput input)
    {
        if (input.Up())
        {
            c.SetState(CharacterState.JUMP);
            return;
        }

        if (input.Down())
        {
            c.SetState(CharacterState.CROUCHING);
            return;
        }

        if (!input.Forward())
        {
            c.SetState(CharacterState.STANDING);
            return;
        }

        Run(c,input);
    }

    private void Run(BaseCharacter c, BufferedInput input)
    {
        float usedSpeed = c.GetSpeed() * 2;

        if (c.AmIFacingBackward())
        {
            usedSpeed *= -1;
        }

        c.SetMotion(usedSpeed, 0);
        c.SetAnimation(CommonAnimations.RUN);
    }
}
