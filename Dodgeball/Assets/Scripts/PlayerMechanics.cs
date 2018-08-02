using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class PlayerMechanics : NetworkBehaviour
{
    #region Variables

    public float playerStrength;

    public Transform desiredBallPos;
    public GameObject ballPickupIndicator;

    GameObject ball;

    Vector3 currentBallPos;
    Vector3 previousBallPos;

    float ballThrowSpeed;

    bool ballGrabbed;
    bool ballReleased = true;
    bool ballInRange;

    #endregion

    void Start()
    {
        ballPickupIndicator = gameObject.transform.GetChild(1).GetChild(0).gameObject;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        CheckPlayerInput();
    }

    void FixedUpdate()
    {
        if (ball != null)
        {
            currentBallPos = ball.GetComponent<Rigidbody>().position;
        }

        if (ballGrabbed)
        {
            GrabBall();

            ballReleased = false;
        }

        else if (!ballGrabbed && !ballReleased)
        {
            ReleaseBall();

            ballReleased = true;
        }

        previousBallPos = currentBallPos;
    }

    void CheckPlayerInput()
    {
        Collider[] colliders = Physics.OverlapSphere(new Vector3(desiredBallPos.position.x, desiredBallPos.position.y, desiredBallPos.position.z + 0.5f), 1);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].tag == "Ball" && !ballGrabbed)
            {
                ballInRange = true;

                ball = colliders[i].gameObject;
            }
        }

        if (Input.GetMouseButtonDown(0) && ballInRange && !ballGrabbed)
        {
            ballGrabbed = true;
        }

        else if (Input.GetMouseButtonUp(0))
        {
            ballGrabbed = false;
        }

        if (ballInRange && !ballGrabbed)
        {
            ballPickupIndicator.SetActive(true);
        }

        else
        {
            ballPickupIndicator.SetActive(false);
        }

        ballInRange = false;
    }

    void GrabBall()
    {
        ball.GetComponent<Rigidbody>().velocity = Vector3.zero;

        ball.GetComponent<Rigidbody>().MovePosition(Vector3.Lerp(ball.GetComponent<Rigidbody>().position, desiredBallPos.position, Time.deltaTime * 20));

        ball.GetComponent<Rigidbody>().useGravity = false;
    }

    void ReleaseBall()
    {
        Vector3 deltaBallPosition = currentBallPos - previousBallPos;

        ball.GetComponent<Rigidbody>().AddForce(deltaBallPosition * playerStrength * 10, ForceMode.Impulse);

        ball.GetComponent<Rigidbody>().useGravity = true;
    }
}