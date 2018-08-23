using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotController : MonoBehaviour {

    [SerializeField]
    private Robot robot;


    private void Awake()
    {
        if (!TryFindRobot())
        {
            Debug.LogWarning("Robot not assigned nor found. Disabling");
            enabled = false;
        }
    }

    private void FixedUpdate()
    {
        float unitDeltaX = 0;
        if (Input.GetKey(KeyCode.D))
        {
            unitDeltaX += 1;
        }

        if (Input.GetKey(KeyCode.A))
        {
            unitDeltaX -= 1;
        }

        robot.Move(unitDeltaX);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            robot.Jump();
        }
    }

    private void Reset()
    {
        TryFindRobot();
    }


    private bool TryFindRobot()
    {
        if(robot == null)
        {
            robot = GetComponent<Robot>();
        }
        return robot != null;
    }
}
