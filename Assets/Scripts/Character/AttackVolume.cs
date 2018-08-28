using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AttackVolume : MonoBehaviour
{
    [SerializeField]
    private int damage;
    [SerializeField]
    private bool isEnabled = true;

    private new Collider collider;


    public int Damage {
        get { return damage; }
        set { damage = value; }
    }

    public bool IsEnabled {
        get { return isEnabled; }
        set { isEnabled = value; }
    }


    private void Awake()
    {
        collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsEnabled)
        {
            DamageVolume damageVolume = other.GetComponent<DamageVolume>();
            if(damageVolume != null)
            {
                damageVolume.Damage(damage);
            }
        }
    }
}