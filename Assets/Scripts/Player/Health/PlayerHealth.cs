using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    private ArmorLoadout armorLoadout;
    [SerializeField] private float maxHealth = 100;
    private float _currentHealth;

    [SerializeField] private Hitbox[] hitboxes;
    public Hitbox GetHitbox(HitboxType type) 
    {
        for (int i = 0; i < hitboxes.Length; i++) 
        {
            if (hitboxes[i].type == type) 
            {
                return hitboxes[i];
            }
        }
        return null;
    }
    public HitResult ApplyHit(in HitInfo hitInfo)
    {
        ArmorDefinition armor = armorLoadout.Get(hitInfo.Hitbox.slot);
        if (!armor) 
        {
            Debug.Log($"No Armor On {hitInfo.Hitbox.slot}");
            HitResult result = new HitResult
            {
                damageApplied = hitInfo.baseDamage,
                outcome = HitOutcome.Normal
            };
            return result;
        }
        else 
        {
            if(armor.rule.immuneToAll || armor.rule.immuneToMelee && hitInfo.isMelee) 
            {
                HitResult r = new HitResult
                {
                    damageApplied = 0f,
                    outcome = HitOutcome.ArmorImmune
                };
                return r;
            }

            HitResult result = new HitResult
            {
                damageApplied = hitInfo.baseDamage * armor.rule.damageMultiplier,
                outcome = HitOutcome.Resisted
            };
            return result;
        }
    }

    private void Awake()
    {
        armorLoadout = GetComponent<ArmorLoadout>();
        _currentHealth = maxHealth;
    }
    public bool CanHeal(int amount) 
    {
        if(_currentHealth >= maxHealth) 
        {
            return false;
        }
        Heal(amount);
        return true;
    }
    private void Heal(float amount) 
    {
        _currentHealth += amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, maxHealth);
        OnHealthChanged(_currentHealth);
    }
    private void TakeDamage(float damage) 
    {
        _currentHealth -= damage;
        OnHealthChanged(_currentHealth);
    }

    private void OnHealthChanged(float newValue) 
    {
        Debug.Log("Player Health changed to: " + newValue);
        if(newValue <= 0) 
        {
            OnDeath();
        }
    }

    private void OnDeath() 
    {
        Debug.Log("Player has died.");
    }

    
}
