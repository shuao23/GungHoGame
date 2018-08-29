using UnityEngine;

public class SimpleRotate : MonoBehaviour {

    [SerializeField]
    private Vector3  rotateSpeed;
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(rotateSpeed * Time.deltaTime);
	}
}
