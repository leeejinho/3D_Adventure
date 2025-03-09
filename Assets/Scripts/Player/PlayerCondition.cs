using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour, IDamagable
{
    public UICondition uiCondition;
    Condition health => uiCondition.health;
    Condition stamina => uiCondition.stamina;

    public float staminaRecoveryRate;
    private float lastUseStamina;
    public event Action OnTakeDamageIndicator;

    private void Update()
    {
        // 스테미나 사용 후 일정 시간 후 회복
        if (Time.time - lastUseStamina > staminaRecoveryRate)
        {
            stamina.SetValue(stamina.passiveValue * Time.deltaTime);
        }
    }

    public void Heal(float value)
    {
        health.SetValue(value);
    }

    public void Die()
    {

    }

    public void TakeDamage(float value)
    {
        health.SetValue(-value);
        OnTakeDamageIndicator?.Invoke();

        if (health.curValue <= 0f)
        {
            Die();
        }
    }

    public bool UseStamina(float value)
    {
        if (stamina.curValue - value <= 0f)
            return false;

        stamina.SetValue(-value);
        lastUseStamina = Time.time;
        return true;
    }
}
