using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HealthSystem : MonoBehaviour {

    public event EventHandler OnHealthAmountMaxChanged;
    public event EventHandler OnDamaged;
    public event EventHandler OnDied;
    public event EventHandler OnHealed;

    [SerializeField] private int maxHealthAmount;
    private int currentHealthAmount;

    private void Awake() {
        currentHealthAmount = maxHealthAmount;
    }

    public void Damage(int damageAmount) {
        currentHealthAmount -= damageAmount;
        currentHealthAmount = Mathf.Clamp(currentHealthAmount, 0, maxHealthAmount);

        OnDamaged?.Invoke(this, EventArgs.Empty);

        if (IsDead()) {
            OnDied?.Invoke(this, EventArgs.Empty);
        }
    }

    public void Heal(int healAmount) {
        currentHealthAmount += healAmount;
        currentHealthAmount = Mathf.Clamp(currentHealthAmount, 0, maxHealthAmount);

        OnHealed?.Invoke(this, EventArgs.Empty);
    }

    public void HealFull() {
        currentHealthAmount = maxHealthAmount;

        OnHealed?.Invoke(this, EventArgs.Empty);
    }

    public bool IsDead() {
        return currentHealthAmount == 0;
    }

    public bool IsFullHealth() {
        return currentHealthAmount == maxHealthAmount;
    }

    public int GetCurrentHealthAmount() {
        return currentHealthAmount;
    }

    public int GetMaxHealthAmount() { 
        return maxHealthAmount;
    
    }

    public float GetHealthAmountNormalized() {
        return (float)currentHealthAmount / maxHealthAmount;
    }

    public void SetHealthAmountMax(int maxHealthAmount, bool updateHealthAmount) {
        this.maxHealthAmount = maxHealthAmount;

        if (updateHealthAmount ) {
            currentHealthAmount = maxHealthAmount;
        }

        OnHealthAmountMaxChanged?.Invoke(this, EventArgs.Empty);
    }

}
