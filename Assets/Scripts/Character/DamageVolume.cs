using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DamageVolume : MonoBehaviour
{
    [SerializeField]
    private bool isEnabled = true;
    [SerializeField]
    private float restTime = 1f;

    private float timeSinceLastHit;

    public event EventHandler<DamageEventArgs> OnDamaged;

    public bool IsEnabled {
        get { return isEnabled; }
        set { isEnabled = value; }
    }

    public void Damage(int amount)
    {
        if (OnDamaged != null && IsEnabled && timeSinceLastHit >= restTime)
        {
            OnDamaged(this, new DamageEventArgs(amount));
            timeSinceLastHit = 0;
        }
    }


    private void Update()
    {
        timeSinceLastHit += Time.deltaTime;
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
