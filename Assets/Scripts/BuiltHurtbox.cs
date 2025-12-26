using System.Collections.Generic;
using UnityEngine;

public class BuiltHurtbox : BaseBuildableBox
{
	[SerializeField] protected HurtboxProperties myHurtboxProperty;
    [SerializeField] private BaseCharacter instigator;

    protected override void Start()
	{
		base.Start();
		gameObject.layer = LayerMask.NameToLayer(gameObject.tag + "Hurt");
	}

	public void SetInstigator(BaseCharacter character)
	{
		instigator = character;
	}

	public override void Build(Vector3 newpos, BaseBoxData usedBox)
	{
		HurtBoxData box = (HurtBoxData)usedBox;

		myHurtboxProperty = box.hurtboxProperty;

		base.Build(newpos, usedBox);

		//myBoxType = usedBox.boxType;
		//switch (myBoxType)
		//{
		//	case BoxType.HITBOX:

		//		break;
		//	case BoxType.HURTBOX:
		//		gameObject.layer = LayerMask.NameToLayer(gameObject.tag + "Hurt");
		//		break;
		//}

		//myHurtboxProperty = property;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		GameManager.instance.QueueCollision(collision.attachedRigidbody.GetComponent<BaseCharacter>(), myHurtboxProperty);
		if (!myHurtboxProperty.HasTag(AttackTags.IGNOREPUSHBACK))
		{
			var pushback = new HurtboxProperties(Vector2.right * 2, new List<AttackTags>() { AttackTags.ONLYPUSH });
			GameManager.instance.QueueCollision(instigator, pushback);
		}
		//Debug.Log("BuiltHurtbox: " + tag + " " + name + " has hit with thing: " + collision.name);
		box.enabled = false;
		instigator.SetCancelable(true);
		//GameManager.instance.AddHitstop(30);
	}
}
