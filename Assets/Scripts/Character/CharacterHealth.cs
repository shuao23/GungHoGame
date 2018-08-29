using System;
using UnityEngine;

public class CharacterHealth
{
    private readonly int maximumHealth;


    public int CurrentHealth { get; private set; }
    public int MaximumHealth { get { return maximumHealth; } }
    public bool IsDead { get { return CurrentHealth == 0; } }


    public event EventHandler OnDeath;
    public event EventHandler OnDamage;
    public event EventHandler OnHeal;


    public CharacterHealth(int maximumHealth)
    {
        if(maximumHealth < 0)
        {
            throw new ArgumentException("maximumHealth is negative");
        }

        this.maximumHealth = maximumHealth;
        CurrentHealth = maximumHealth;
    }


    public void Heal(int amount, out int overHeal)
    {
        if(amount < 0)
        {
            throw new ArgumentException("ammount is negative");
        }

        CurrentHealth += amount;
        overHeal = Mathf.Max(0, CurrentHealth - MaximumHealth);
        CurrentHealth = Mathf.Min(CurrentHealth, MaximumHealth);

        if (OnHeal != null)
        {
            OnHeal(this, EventArgs.Empty);
        }
    }


    public void Damage(int amount, out int overDammage)
    {
        if (amount < 0)
        {
            throw new ArgumentException("ammount is negative");
        }

        CurrentHealth -= amount;
        overDammage = Mathf.Max(0, -CurrentHealth);
        CurrentHealth = Mathf.Max(0, CurrentHealth);


        if (OnDamage != null)
        {
            OnDamage(this, EventArgs.Empty);
        }
        if (CurrentHealth == 0 && OnDeath != null)
        {
            OnDeath(this, EventArgs.Empty);
        }
    }

    public bool TrySetCurrentHealth(int health)
    {
        if(health > MaximumHealth)
        {
            return false;
        }

        CurrentHealth = health;
        return true;
    }

    public void FullHeal() {
        CurrentHealth = MaximumHealth;
    }

}
