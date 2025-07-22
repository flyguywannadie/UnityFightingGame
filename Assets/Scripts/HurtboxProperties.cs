using System;
using System.Collections.Generic;
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
    LAUNCH,
    KNOCKDOWN,
    AIRKNOCK,
    OTG,
    ONLYPUSH,
    IGNOREPUSHBACK,
}

[Serializable]
public class HurtboxProperties
{
    public int damage = 5;
	[Min(1)] public int hitstun = 15;
	[Min(1)] public int blockstun = 10;
    public AttackHeight attackHeight = AttackHeight.NORMAL;
    public Vector2 knockback = Vector2.right;
    public List<AttackTags> AttackTags = new List<AttackTags>();

    public HurtboxProperties() { }

    public HurtboxProperties(Vector2 knockback, List<AttackTags> attackTags)
    {
        this.knockback = knockback;
        this.AttackTags = attackTags;
    }

    public bool HasTag(AttackTags tag)
    {
        return AttackTags.Contains(tag);
    }
}
