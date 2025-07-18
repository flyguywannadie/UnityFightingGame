using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
	public string animPath = "CharAnimations/Char1";

	public int currentFrame = 0;

	public SpriteRenderer visuals;

	public List<CharacterAnimation> animations;
	public int currentAnimation;

	public int currentAnimFrame;
	public int currentHitboxFrame;

	public HitboxBuilder hitboxBuilder;

	private void Start()
	{
		currentFrame = 0;
		currentHitboxFrame = -1;
		animations = new List<CharacterAnimation>(Resources.LoadAll<CharacterAnimation>(animPath));
		ChangeAnimationToID((int)CommonAnimations.IDLE);
	}

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

		int hitboxIndex = current.GetHitboxDataIndex(currentFrame);
		if (hitboxIndex > currentHitboxFrame)
		{
			CharacterAnimation.FrameData hitboxdata = current.GetHitboxData(hitboxIndex);
			hitboxBuilder.BuildHitbox(hitboxdata);
			currentHitboxFrame = hitboxIndex;
		}

		List<CharacterAnimation.AnimEvent> es = current.Events.FindAll(x => x.callFrame == currentFrame);
		foreach (CharacterAnimation.AnimEvent e in es)
		{
			c.Invoke(e.name, 0f);
		}
	}

	public void ChangeAnimationToID(int id)
	{
		CharacterAnimation anim = animations.Find(x => x.GetAnimID() == id);
		if (anim == null)
		{
			Debug.LogError("There is no animation of ID: " + id + " (" + ((CommonAnimations)id).ToString() + ")");
			currentAnimation = 0;
			currentFrame = 0;
			return;
		}
		//Debug.Log("Test");

		currentAnimation = animations.IndexOf(anim);
		currentFrame = 0;
		currentHitboxFrame = -1;
	}

	public int GetCurrentAnimationID()
	{
		var current = animations[currentAnimation];

		if (current.ID == CommonAnimations.CUSTOM)
		{
			return current.customID;
		}

		return (int)current.ID;
	}
	
	public CharacterAnimation.FrameData GetCurrentFrameData(int currentFrame)
	{
		return animations[currentAnimation].GetHitboxData(currentFrame);
	}
}
