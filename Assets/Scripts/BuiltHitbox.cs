using UnityEngine;

public class BuiltHitbox : BaseBuildableBox
{
	protected override void Start()
	{
		base.Start();
		gameObject.layer = LayerMask.NameToLayer(gameObject.tag + "Hit");
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		//Debug.Log("BuiltHitbox: " + tag + " " + name + " has been hit by: " + collision.name);
	}
}
