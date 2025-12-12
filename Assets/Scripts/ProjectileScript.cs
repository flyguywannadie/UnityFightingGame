using UnityEngine;

public class ProjectileScript : BuiltHurtbox
{
    [SerializeField] private float LifeTime = 2;
    [SerializeField] private int speed = 5;
    [SerializeField] private Vector2 direction = Vector2.right;

    public bool ProjectileUpdate()
    {
        if (LifeTime <= 0 || !box.enabled)
        {
            return true;
        }

        transform.position += ((Vector3)direction * speed * transform.localScale.x * Time.fixedDeltaTime);

        LifeTime -= Time.fixedDeltaTime;

        return false;
    }

    public void Expire()
    {
        Destroy(gameObject);
    }
}
