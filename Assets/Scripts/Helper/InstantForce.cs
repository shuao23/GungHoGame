using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class InstantForce : MonoBehaviour {

    [SerializeField]
    private Vector3 force;
    [SerializeField]
    private float delay;

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

    public Vector3 Force {
        get { return force; }
        set { force = value; }
    }

    public float Delay {
        get { return delay; }
        set { delay = value; }
    }


	void Start () {
        if(delay == 0)
        {
            AddForce();
        }
        else
        {
            StartCoroutine(AddForceCoroutine());
        }
	}

    private IEnumerator AddForceCoroutine()
    {
        yield return new WaitForSeconds(delay);
        AddForce();
    }

    private void AddForce()
    {
        Rigidbody.AddForce(force, ForceMode.Impulse);
    }

}
