using UnityEngine;

public class BaseBuildableBox : MonoBehaviour
{
	[SerializeField] protected BoxCollider2D box;

    protected virtual void Start()
	{
		box = GetComponent<BoxCollider2D>();
		box.isTrigger = true;
	}

	public virtual void Build(Vector3 newpos, BaseBoxData usedBox)
	{
		box.enabled = true;
		transform.position = newpos;
		box.offset = usedBox.position;
		box.size = usedBox.size;
	}

	public virtual void Clear()
	{
		box.enabled = false;
		transform.position = Vector3.down * 10;
		box.offset = Vector2.zero;
		box.size = Vector2.one;
	}

}
