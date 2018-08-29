using UnityEngine;

public class Enemy : MonoBehaviour {

    [SerializeField]
    private DamageVolume damageVolume;
    [SerializeField]
    private int maxHealth;
    [SerializeField]
    private GameObject DeathEffect;
    [SerializeField]
    private float effectDuration;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip onHurt;
    [SerializeField]
    private new MeshRenderer renderer;
    [SerializeField]
    private float lightFadeInSpeed;
    [SerializeField]
    private Color dimmedColor;
    [SerializeField]
    private Color onColor;


    private CharacterHealth health;
    private Material glowMat;

    private void Awake()
    {
        health = new CharacterHealth(maxHealth);
        glowMat = renderer.materials[1];
        glowMat.SetColor("Emission", onColor); 
    }

    private void Start () {
        damageVolume.OnDamaged += DamageVolume_OnDamaged;
        health.OnDamage += Health_OnDamage;
	}

    private void Update()
    {
        Color current = glowMat.GetColor("_EmissionColor");
        glowMat.SetColor("_EmissionColor", Color.Lerp(current, onColor, Time.deltaTime * lightFadeInSpeed));
    }

    private void OnDestroy () {
        damageVolume.OnDamaged -= DamageVolume_OnDamaged;
    }

    private void DamageVolume_OnDamaged(object sender, DamageVolume.DamageEventArgs e)
    {
        int overDamage;
        health.Damage(e.Damage, out overDamage);
    }

    private void Health_OnDamage(object sender, System.EventArgs e)
    {
        if (health.IsDead)
        {
            Destroy(gameObject);
            Destroy(Instantiate(DeathEffect, transform.position, transform.rotation), effectDuration);
        }
        else
        {
            audioSource.PlayOneShot(onHurt);
            glowMat.SetColor("_EmissionColor", dimmedColor);
        }
    }
}
