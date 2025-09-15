using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DebugConsole : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject consoleGo;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TMP_Text consoleLog;

    private List<string> prevCommands = new();

    private void Start()
    {
        inputField.onSubmit.AddListener(ParseInputCommand);
        SubscribeInputs();
    }

    private void SubscribeInputs() 
    {
        if(InputManager.Instance == null) 
        {
            Debug.LogError("InputManager is NULL");
            return;
        }
        var debug = InputManager.Instance.Actions.Debug;

        debug.Enable();
        debug.ToggleConsole.performed += OnToggleConsole;
        debug.UpDown.performed += ctx => ScrollPrevCommands(ctx.ReadValue<float>());
    }

    private void OnToggleConsole(InputAction.CallbackContext ctx) 
    {
        ToggleConsole();
    }
    int prevCommandIndex = 0;
    private void ScrollPrevCommands(float value) 
    {
        if (prevCommands.Count == 0) return;

        int val = (int)value;
        prevCommandIndex += val;
      
        if(prevCommandIndex > prevCommands.Count) 
            prevCommandIndex = 0;
        else if(prevCommandIndex < 0)
            prevCommandIndex = prevCommands.Count - 1;

            inputField.text = prevCommands[prevCommandIndex];               
    }
    private void ToggleConsole() 
    {
        bool show = !consoleGo.activeInHierarchy;

        consoleGo.SetActive(show);
        InputManager.Instance.TogglePlayerInputs(!show);
        InputManager.Instance.ToggleUiInputs(!show);

    }
    private void ParseInputCommand(string command)
    {
        if (string.IsNullOrEmpty(command))
        {
            LogMessage("Cmd Invalid");
            return;
        }

        prevCommands.Add(command);

        string[] parts = command.Split(' ');
        string cmd = parts[0].Trim();
        string[] args = parts.Length > 1 ? parts[1..] : new string[0];
      
        if (cmd.StartsWith("P_")) 
        {
            HandlePlayerCommand(cmd, args);
        }        
    }
    [Header("PlayerCommandReferences")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private ArmorLoadout playerArmor;
    [SerializeField] private MovementController playerMovement;

    [Header("Database")]
    [SerializeField] private ArmorDatabase armorDatabase;
    private void HandlePlayerCommand(string cmd, string[] args) 
    {
        if (string.IsNullOrEmpty(args[0])) 
        {
            LogMessage("Arg 0 Invalid");
            return;
        }

        switch (args[0].ToLower()) 
        {
            case "damage":
                if (string.IsNullOrEmpty(args[1])) 
                {
                    LogMessage("Arg 1 Invalid");
                    return;
                }
                Hitbox hitbox;

                switch (args[1].ToLower()) 
                {
                    case "head":
                        hitbox = playerHealth.GetHitbox(HitboxType.Head);
                        break;
                    case "torso":
                        hitbox = playerHealth.GetHitbox(HitboxType.Torso);       
                        break;
                    case "leftarm":
                        hitbox = playerHealth.GetHitbox(HitboxType.LeftArm);
                        break;
                    case "rightarm":
                        hitbox = playerHealth.GetHitbox(HitboxType.RightArm);
                        break;
                    case "leftleg":
                        hitbox = playerHealth.GetHitbox(HitboxType.LeftLeg);
                        break;
                    case "rightleg":
                        hitbox = playerHealth.GetHitbox(HitboxType.RightLeg);
                        break;
                    default: hitbox = null; break;
                }

                if (!hitbox || string.IsNullOrEmpty(args[2])) 
                {
                    LogMessage("Error");
                    return;
                }
                
                float.TryParse(args[2], out float dam);
                
                HitInfo hitInfo = new HitInfo { baseDamage = dam, Hitbox = hitbox};
                HitResult result = playerHealth.ApplyHit(hitInfo);
                
                LogMessage($"Hit {result.damageApplied} outcome {result.outcome}");

                break;
            case "armor":
                if (string.IsNullOrEmpty(args[1]) || string.IsNullOrEmpty(args[2]))
                    { LogMessage("Invalid Args"); return; }
                ArmorDefinition armor = null;
                if (!string.IsNullOrEmpty(args[3])) 
                {
                    armor = armorDatabase.GetArmorByName(args[3].ToLower());
                    if (!armor)
                    {
                        LogMessage("Invalid Armor");
                        return;
                    }
                }                

                ArmorSlot slot = ArmorSlot.None;
                switch (args[2].ToLower())
                {
                    case "head":
                        slot = ArmorSlot.Head;
                        break;
                    case "torso":
                        slot = ArmorSlot.Torso;
                        break;
                    case "leftarm":
                        slot = ArmorSlot.LeftArm;
                        break;
                    case "rightarm":
                        slot = ArmorSlot.RightArm;
                        break;
                    case "leftleg":
                        slot = ArmorSlot.LeftLeg;
                        break;
                    case "rightleg":
                        slot = ArmorSlot.RightLeg;
                        break;
                }

                if(slot == ArmorSlot.None) 
                {
                    LogMessage("Invalid Slot");
                    return;
                }

                switch (args[1].ToLower()) 
                {
                    case "set":
                        if(armor)
                            playerArmor.Set(slot, armor);
                        else 
                        {
                            LogMessage("Invalid Armor");
                            return;
                        }
                            break;
                    case "remove":
                        playerArmor.Remove(slot);
                        break;
                }
                break;
            case "teleport":
                if (string.IsNullOrEmpty(args[1]) || string.IsNullOrEmpty(args[2])
                    || string.IsNullOrEmpty(args[3])) { LogMessage("Invalid Args"); return; }

                float.TryParse(args[1], out float x);
                float.TryParse(args[2], out float y);
                float.TryParse(args[3], out float z);

                playerMovement.Teleport(new Vector3(x, y, z));                
                break;
            default: break;
        }
    }

    private void LogMessage(string message) 
    {
        consoleLog.text += "\n" + message;
        inputField.text = "";
    }
}
