using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterSounds : MonoBehaviour {

    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip footStep;
    [SerializeField]
    private AudioClip jump;
    [SerializeField]
    private AudioClip land;
    [SerializeField]
    private AudioClip damaged;
    [SerializeField]
    private AudioClip death;
    [SerializeField]
    private AudioClip punch;
    [SerializeField]
    private AudioClip rocketPunch;


    private void OnFootStep()
    {
        audioSource.PlayOneShot(footStep);
    }

    private void OnJump()
    {
        audioSource.PlayOneShot(jump);
    }

    private void OnLand()
    {
        audioSource.PlayOneShot(land);
    }

    private void OnPunch()
    {
        audioSource.PlayOneShot(punch);
    }

    private void OnRocketPunch(string message)
    {
        if (string.IsNullOrEmpty(message))
        {
            audioSource.PlayOneShot(rocketPunch);
        }
    }

    private void OnDamage()
    {
        audioSource.PlayOneShot(damaged);
    }

    private void OnDeath()
    {
        audioSource.PlayOneShot(death);
    }
}
