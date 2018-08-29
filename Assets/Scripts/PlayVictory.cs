using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayVictory : MonoBehaviour {

    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip victory;

    private void OnVictory()
    {
        audioSource.PlayOneShot(victory);
    }
}
