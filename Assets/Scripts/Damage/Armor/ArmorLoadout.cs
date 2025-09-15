using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.iOS;

public class ArmorLoadout : MonoBehaviour
{
    [SerializeField] private ArmorDefinition head;
    [SerializeField] private ArmorDefinition torso;

    [SerializeField] private ArmorDefinition leftArm;
    [SerializeField] private ArmorDefinition rightArm;

    [SerializeField] private ArmorDefinition leftLeg;
    [SerializeField] private ArmorDefinition rightLeg;

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
    public void Set(ArmorSlot slot, ArmorDefinition def) 
    {
        if (def.rule.slot != slot) 
        {
            Debug.Log("Incorrect Armor for Slot");
            return;
        }

        switch (slot) 
        {
            case ArmorSlot.Head: head = def; break;
            case ArmorSlot.Torso: torso = def; break;
            case ArmorSlot.LeftArm: leftArm = def; break;
            case ArmorSlot.RightArm: rightArm = def; break;
            case ArmorSlot.LeftLeg: leftLeg = def; break;
            case ArmorSlot.RightLeg: rightLeg = def; break;
        }
    }
    public void Remove(ArmorSlot slot) 
    {
        switch (slot) 
        {
            case ArmorSlot.Head: head = null; break;
            case ArmorSlot.Torso: torso = null; break;
            case ArmorSlot.LeftArm: leftArm = null; break;
            case ArmorSlot.RightArm: rightArm = null; break;
            case ArmorSlot.LeftLeg:  leftArm = null; break;
            case ArmorSlot.RightLeg: rightLeg = null; break;
        }
    }
}
