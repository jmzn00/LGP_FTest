using UnityEngine;

public enum HitZone { Default, Head, Torso, Arms, Legs, Weakpoint, Armor }
public class Hitbox : MonoBehaviour
{
    public HitZone zone = HitZone.Default;
    public bool meleeImmune = false;
    public bool hitImmune = false;
}
