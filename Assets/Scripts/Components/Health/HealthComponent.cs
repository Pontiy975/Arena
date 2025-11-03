using System;
using UnityEngine;

namespace Arena.Components.Health
{
    public class HealthComponent : MonoBehaviour
    {
        public event Action OnDeath;

        public bool IsDead { get; private set; }
        public bool IsFull => Health >= MaxHealth;
        public float Health { get; private set; }
        public float MaxHealth { get; private set; }

        public virtual void Init(int maxHealth)
        {
            MaxHealth = maxHealth;
            Health = MaxHealth;
            IsDead = false;
        }

        public virtual void ChangeHealth(int delta)
        {
            if (IsDead) return;

            Health += delta;

            if (Health > MaxHealth)
                Health = MaxHealth;

            if (Health <= 0)
            {
                IsDead = true;
                OnDeath?.Invoke();
            }
        }

        public float GetLostHealthPercentage()
        {
            if (MaxHealth == 0) return 0f;
            return 1f - (Health / MaxHealth);
        }

        public virtual void SetMaxHealth(int maxHealth)
        {
            if (MaxHealth >= maxHealth)
                return;

            float delta = maxHealth - MaxHealth;
            MaxHealth = maxHealth;
            Health += delta;
        }
    }
}
