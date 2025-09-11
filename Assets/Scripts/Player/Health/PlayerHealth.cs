using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 100;
    private int _currentHealth;

    public HitResult ApplyHit(in HitInfo hitInfo)
    {
        Debug.Log($"{hitInfo.baseDamage} damage from {hitInfo.instigator.name}");
        TakeDamage((int)hitInfo.baseDamage);
        return new HitResult {
            outcome = HitOutcome.Normal            
        };
    }

    private void Start()
    {
        _currentHealth = maxHealth;

         
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            ApplyHit(new HitInfo { baseDamage = 10, instigator = this.gameObject });
        }
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
    private void Heal(int amount) 
    {
        _currentHealth += amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, maxHealth);
        OnHealthChanged(_currentHealth);
    }
    private void TakeDamage(int damage) 
    {
        _currentHealth -= damage;
        OnHealthChanged(_currentHealth);
    }

    private void OnHealthChanged(int newValue) 
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
