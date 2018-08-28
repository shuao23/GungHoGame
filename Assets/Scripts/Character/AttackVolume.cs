using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AttackVolume : MonoBehaviour
{
    private Collider collider;


    public bool Enabled { get; set; }


    private void Awake()
    {
        collider = GetComponent<Collider>();
        collider.enabled = Enabled;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
    }
}