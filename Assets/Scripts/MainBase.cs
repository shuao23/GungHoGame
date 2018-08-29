using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MainBase : MonoBehaviour {

	[SerializeField]
    private int maxHealth = 3;
    [SerializeField]
    private Character character;

    public CharacterHealth Health { get; private set; }

    private void Awake()
    {
        Health = new CharacterHealth(maxHealth);
        Health.OnDeath += Health_OnDeath;
    }

    private void Health_OnDeath(object sender, System.EventArgs e)
    {
        int overDamage;
        character.Health.Damage(int.MaxValue, out overDamage);
    }

    private void OnDestroy()
    {
        Health.OnDeath -= Health_OnDeath;
    }

    private void OnTriggerEnter(Collider other)
    {
        int overDamage;
        Health.Damage(1, out overDamage);
    }
}
