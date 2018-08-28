using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DamageVolume : MonoBehaviour
{
    [SerializeField]
    public bool isEnabled = true;

    public event EventHandler<DamageEventArgs> OnDamaged;

    public bool IsEnabled {
        get { return isEnabled; }
        set { isEnabled = value; }
    }

    public void Damage(int amount)
    {
        Debug.Log(gameObject.name + " is damaged");
        if (OnDamaged != null && IsEnabled)
        {
            OnDamaged(this, new DamageEventArgs(amount));
        }
    }

    [Serializable]
    public class DamageEventArgs : EventArgs
    {
        public int Damage { get; set; }

        public DamageEventArgs(int damage)
        {
            Damage = damage;
        }
    }
}
