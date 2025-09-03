using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int _currentHealth;

    private void Start()
    {
        _currentHealth = maxHealth;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(10);
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
