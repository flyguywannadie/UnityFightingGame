using UnityEngine;

public class Hitbox : MonoBehaviour
{
	[SerializeField] private BoxCollider2D box;
	[SerializeField] private BoxType myBoxType;
	[SerializeField] private HurtboxProperties myHurtboxProperty;

	private void Start()
	{
		box = GetComponent<BoxCollider2D>();
		box.isTrigger = true;
	}

	public void Build(Vector3 newpos, CharacterAnimation.BoxData usedBox)
	{
		box.enabled = true;
		transform.position = newpos;
		box.offset = usedBox.position;
		box.size = usedBox.size;

		myBoxType = usedBox.boxType;
		switch (myBoxType)
		{
			case BoxType.HITBOX:
				gameObject.layer = LayerMask.NameToLayer(gameObject.tag + "Hit");
				break;
			case BoxType.HURTBOX:
				gameObject.layer = LayerMask.NameToLayer(gameObject.tag + "Hurt");
				break;
		}

		//myHurtboxProperty = property;
	}

	public void Clear()
	{
		box.enabled = false;
		transform.position = Vector3.down * 10;
		box.offset = Vector2.zero;
		box.size = Vector2.one;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		switch (myBoxType)
		{
			case BoxType.HITBOX:
				Debug.Log("Hitbox: " + tag + " " + name + " has been hit by: " + collision.name);
				break;
			case BoxType.HURTBOX:
				box.enabled = false;
				GameManager.instance.QueueCollision(collision.attachedRigidbody.GetComponent<BaseCharacter>(), new HurtboxProperties());
				Debug.Log("Hurtbox: " + tag + " " + name + " has hit with thing: " + collision.name);
				break;
		}

	}
}
