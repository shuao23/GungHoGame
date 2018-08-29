using UnityEngine;

public class Enemy : MonoBehaviour {

    [SerializeField]
    private DamageVolume damageVolume;


    private void Start () {
        damageVolume.OnDamaged += DamageVolume_OnDamaged;
	}

    private void OnDestroy () {
        damageVolume.OnDamaged -= DamageVolume_OnDamaged;
    }

    private void DamageVolume_OnDamaged(object sender, DamageVolume.DamageEventArgs e)
    {
        Destroy(gameObject);
    }
}
