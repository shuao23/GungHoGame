using System;
using UnityEngine;

public class Follow : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private Bool3 following;
    [SerializeField]
    private UpdateMode updateMode;

    private Vector3 initialPosition;

    void Awake()
    {
        initialPosition = transform.position;
    }


    private void FixedUpdate()
    {
        if (ShouldUpdate(UpdateMode.FixedUpdate))
        {
            UpdateInternal();
        }
    }

    private void Update()
    {
        if(ShouldUpdate(UpdateMode.Update))
        {
            UpdateInternal();
        }
    }


    private bool ShouldUpdate(UpdateMode currentUpdate)
    {
        return (currentUpdate & updateMode) != 0;
    }

    private void UpdateInternal()
    {
        Vector3 newPosition = initialPosition;

        if (following.x)
        {
            newPosition.x = target.position.x;
        }

        if (following.y)
        {
            newPosition.y = target.position.y;
        }

        if (following.z)
        {
            newPosition.z = target.position.z;
        }

        transform.position = newPosition;
    }
}
