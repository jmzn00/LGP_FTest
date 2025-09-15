using UnityEngine;

public enum HitboxType { Default, Head, Torso, LeftArm, RightArm, LeftLeg, RightLeg, WeakPoint}
public enum HitZone { Default, Head, Torso, Arms, Legs, Weakpoint, Armor }
public class Hitbox : MonoBehaviour
{
    [Header("Classification")]
    public HitZone zone = HitZone.Default;
    public HitboxType type = HitboxType.Default;
    public ArmorSlot slot = ArmorSlot.None;

    [Header("Flags")]
    public bool meleeImmune = false;
    public bool hitImmune = false;

    IDamageable damageable = null;
    private void Awake() => damageable = GetComponentInParent<IDamageable>();

    public HitResult ForwardHit(HitInfo info) 
    {
        if (hitImmune) return new HitResult { outcome = HitOutcome.Blocked, impactPoint = info.point, impactNormal = info.normal };
        if (meleeImmune && info.isMelee) return new HitResult { outcome = HitOutcome.ArmorImmune, impactPoint = info.point, impactNormal = info.normal };
        return damageable.ApplyHit(info);
    }    
}
