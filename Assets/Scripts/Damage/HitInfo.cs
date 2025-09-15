using UnityEngine;
public enum  HitOutcome { Normal, Weakpoint, ArmorImmune, Blocked, Resisted, Killed}
public enum ArmorSlot { None, Head, Torso, LeftArm, RightArm, LeftLeg, RightLeg }

public struct HitInfo 
{
    public Vector3 point;
    public Vector3 normal;
    public Vector3 direction;
    public float distance;

    public bool isMelee;

    public float baseDamage;
    public Hitbox Hitbox;
    public GameObject instigator;
}
public struct HitResult 
{
    public HitOutcome outcome;
    public float damageApplied;
    public Vector3 impactPoint;
    public Vector3 impactNormal;

    public string vfxTag;
    public string sfxTag;
}
public interface IDamageable 
{
    HitResult ApplyHit(in HitInfo hitInfo);
}

