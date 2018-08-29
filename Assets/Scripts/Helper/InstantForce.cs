using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class InstantForce : MonoBehaviour {

    [SerializeField]
    private Vector3 force;

    private new Rigidbody rigidbody;

    private Rigidbody Rigidbody {
        get {
            if(rigidbody == null)
            {
                rigidbody = GetComponent<Rigidbody>();
            }
            return rigidbody;
        }
    }

	void Start () {
        Rigidbody.AddForce(force, ForceMode.Impulse);
	}

}
