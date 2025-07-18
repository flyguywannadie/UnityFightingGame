using System;
using UnityEngine;

public enum AttackHeight
{
    NORMAL,
    LOW,
    OVERHEAD,
    UNBLOCKABLE
}

public enum AttackTags
{
    KNOCKDOWN,
    OTG,
}

[Serializable]
public class HurtboxProperties
{
    public int damage = 5;
    public int hitstun = 15;
    public int blockstun = 10;
    public AttackHeight attackHeight = AttackHeight.NORMAL;
    public Vector2 knockback = Vector2.right;
    public AttackTags[] AttackTags = new AttackTags[0];
}
