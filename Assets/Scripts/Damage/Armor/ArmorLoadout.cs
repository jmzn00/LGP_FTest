using System;
using UnityEngine;

public class ArmorLoadout : MonoBehaviour
{
    [SerializeField] private ArmorDefinition head;
    [SerializeField] private ArmorDefinition torso;

    [SerializeField] private ArmorDefinition leftArm;
    [SerializeField] private ArmorDefinition rightArm;

    [SerializeField] private ArmorDefinition leftLeg;
    [SerializeField] private ArmorDefinition rightLeg;

    public event Action<ArmorSlot, ArmorDefinition> OnArmorChanged;

    public ArmorDefinition Get(ArmorSlot slot) 
    {
        switch (slot) 
        {
            case ArmorSlot.Head: return head;
            case ArmorSlot.Torso: return torso;
            case ArmorSlot.LeftArm: return leftArm;
            case ArmorSlot.RightArm: return rightArm;
            case ArmorSlot.LeftLeg: return leftLeg;
            case ArmorSlot.RightLeg: return rightLeg;
            default: return null;
        }
    }
    public bool Set(ArmorSlot slot, ArmorDefinition def) 
    {
        if(def == null) 
        {
            Debug.LogWarning("Def is null", this);
            return false;
        }
        if (def.rule.slot != slot) 
        {
            Debug.Log("Incorrect Armor for Slot");
            return false;
        }

        switch (slot) 
        {
            case ArmorSlot.Head: head = def; break;
            case ArmorSlot.Torso: torso = def; break;
            case ArmorSlot.LeftArm: leftArm = def; break;
            case ArmorSlot.RightArm: rightArm = def; break;
            case ArmorSlot.LeftLeg: leftLeg = def; break;
            case ArmorSlot.RightLeg: rightLeg = def; break;
            default: return false;
        }
        OnArmorChanged?.Invoke(slot, def);
        return true;
    }
    public bool Remove(ArmorSlot slot) 
    {
        ArmorDefinition old = Get(slot);
        switch (slot) 
        {
            case ArmorSlot.Head: head = null; break;
            case ArmorSlot.Torso: torso = null; break;
            case ArmorSlot.LeftArm: leftArm = null; break;
            case ArmorSlot.RightArm: rightArm = null; break;
            case ArmorSlot.LeftLeg:  leftLeg = null; break;
            case ArmorSlot.RightLeg: rightLeg = null; break;
            default: return false;
        }

        OnArmorChanged?.Invoke(slot, null);
        return old != null;
    }
}
