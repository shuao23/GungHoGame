using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSpanner : MonoBehaviour {

    [SerializeField]
    private float minSpawnWait = 1;
    [SerializeField]
    private float maxSpawnWait = 5;

    [SerializeField]
    private GameObject[] prefabs = new GameObject[0];

    // Use this for initialization
    void Start () {
        StartCoroutine(RandomSpawn());
	}

    private IEnumerator RandomSpawn()
    {
        while (true)
        {
            if(prefabs == null || prefabs.Length == 0)
            {
                break;
            }

            Instantiate(prefabs[Random.Range(0, prefabs.Length)], transform.position, transform.rotation);
            yield return new WaitForSeconds(Random.Range(minSpawnWait, maxSpawnWait));
        }
    }
}
