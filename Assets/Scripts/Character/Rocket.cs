using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Rocket : MonoBehaviour
{
    [SerializeField]
    private AudioSource rocketAudio;
    [SerializeField]
    private AudioClip rocketSound;
    [SerializeField]
    private new MeshRenderer renderer;
    [SerializeField]
    private ParticleSystem smokeParticle;
    [SerializeField]
    private TrailRenderer smokeTrails;
    [SerializeField]
    private GameObject explosionParticles;
    [SerializeField]
    private Rigidbody rigidBody;
    [SerializeField]
    private SphereCollider sphereCollider;
    [SerializeField]
    private float destroyDelay;
    [SerializeField]
    private float damageRadius;
    [SerializeField]
    private int damageAmount;


    private Collider[] buffer = new Collider[20];

    private void OnTriggerEnter(Collider other)
    {
        int count = Physics.OverlapSphereNonAlloc(transform.position, damageRadius, buffer);
        for(int i = 0; i < count; i++)
        {
            DamageVolume dv;
            if ((dv =buffer[i].GetComponent<DamageVolume>()) != null)
            {
                dv.Damage(damageAmount);
            }
        }

        Debug.Log(other.name);

        rocketAudio.Stop();
        rocketAudio.PlayOneShot(rocketSound);
        smokeParticle.Stop();
        smokeTrails.enabled = false;
        renderer.enabled = false;
        explosionParticles.SetActive(true);
        rigidBody.isKinematic = true;
        sphereCollider.enabled = false;
        Destroy(gameObject, destroyDelay);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
}
