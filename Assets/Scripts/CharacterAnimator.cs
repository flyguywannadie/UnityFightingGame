using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
	public int currentFrame = 0;

	public SpriteRenderer visuals;

	public List<CharacterAnimation> animations;
	public int currentAnimation;

	public void AnimatorUpdate(BaseCharacter c)
	{
		CharacterAnimation current = animations[currentAnimation];

		currentFrame++;
		
		if (currentFrame > current.GetAnimationDuration())
		{
			if (current.loop)
			{
				currentFrame = 0;
			}
			else if (current.changeStateOnFinish)
			{
				currentFrame--;
				c.SetState(current.endState);
			}
		}
		
		visuals.sprite = current.GetCurrentSprite(currentFrame);

		List<CharacterAnimation.AnimEvent> es = current.Events.FindAll(x => x.callFrame == currentFrame);
		foreach (CharacterAnimation.AnimEvent e in es)
		{
			c.Invoke(e.name, 0f);
		}
	}

	public void ChangeAnimationToID(int id)
	{
		CharacterAnimation anim = animations.Find(x => x.ID == id);
		if (anim == null)
		{
			Debug.LogError("There is no animation of ID: " + id);
			return;
		}
		//Debug.Log("Test");

		currentAnimation = animations.IndexOf(anim);
		currentFrame = 0;
	}
}
