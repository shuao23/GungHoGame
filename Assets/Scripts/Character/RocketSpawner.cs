using UnityEngine;

public class RocketSpawner : MonoBehaviour {

    [SerializeField]
    private GameObject rocketPrefab;

    public void Spawn(bool isRightDirection)
    {
        GameObject go = Instantiate(rocketPrefab, transform.position, transform.rotation);
        InstantForce instant = go.GetComponent<InstantForce>();
        Vector3 force = instant.Force;
        instant.Force = new Vector3((isRightDirection ? -1 : 1) * Mathf.Abs(force.x), force.y, force.z);

        ConstantForce constant = go.GetComponent<ConstantForce>();
        force = constant.force;
        constant.force = new Vector3((isRightDirection ? -1 : 1) * Mathf.Abs(force.x), force.y, force.z);
    }
}
