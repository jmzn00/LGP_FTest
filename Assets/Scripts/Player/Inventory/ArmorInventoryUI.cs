using UnityEngine;
using UnityEngine.UI;

public class ArmorInventoryUI : MonoBehaviour
{
    private ArmorLoadout ArmorLoadout;

    [SerializeField] private Image headImage;
    [SerializeField] private Image torsoImage;
    [SerializeField] private Image leftArmImage;
    [SerializeField] private Image rightArmImage;
    [SerializeField] private Image leftLegImage;
    [SerializeField] private Image rightLegImage;

    private void Awake()
    {
        ArmorLoadout = GetComponent<ArmorLoadout>();
        ArmorLoadout.OnArmorChanged += UpdateArmorSlot;
    }

    public void UpdateArmorSlot(ArmorSlot slot, ArmorDefinition def) 
    {
        Color color = def == null ? Color.white : def.rule.tempColor;
        switch (slot) 
        {
            case ArmorSlot.Head:
                headImage.color = color;
                break;
            case ArmorSlot.Torso:
                torsoImage.color = color;
                break;
            case ArmorSlot.LeftArm:
                leftArmImage.color = color;
                break;
            case ArmorSlot.RightArm:
                rightArmImage.color = color;
                break;
            case ArmorSlot.LeftLeg:
                leftLegImage.color = color;
                break;
            case ArmorSlot.RightLeg:
                rightArmImage.color= color;
                break;
        }
    }
}
