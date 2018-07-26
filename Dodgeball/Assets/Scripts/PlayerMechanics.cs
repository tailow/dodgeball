using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMechanics : MonoBehaviour
{

    #region Variables

    public float playerStrength;
    public float ballMaxSpeed;

    public Transform desiredBallPos;

    public GameObject ball;
    public GameObject ballPickupIndicator;

    Vector3 currentRotation;
    Vector3 previousRotation;

    Vector3 currentBallPos;
    Vector3 previousBallPos;

    float ballThrowSpeed;
    float angularVelocity;

    bool ballGrabbed;
    bool ballReleased = true;
    bool ballInRange;

    #endregion

    void Update()
    {
        currentRotation = new Vector3(Camera.main.transform.eulerAngles.x, transform.eulerAngles.y, 0);
        currentBallPos = ball.transform.localPosition;

        Collider[] colliders = Physics.OverlapSphere(new Vector3(desiredBallPos.position.x, desiredBallPos.position.y + 0.3f, desiredBallPos.position.z + 0.5f), 1);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.name == "Ball")
            {
                ballInRange = true;
            }
        }

        if (ballInRange && !ballGrabbed)
        {
            ballPickupIndicator.SetActive(true);
        }

        else
        {
            ballPickupIndicator.SetActive(false);
        }

        // Ball grab
        if (Input.GetMouseButtonDown(0) && ballInRange)
        {
            ballGrabbed = true;
        }

        else if (Input.GetMouseButtonUp(0))
        {
            ballGrabbed = false;
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

        ballInRange = false;

        previousRotation = currentRotation;
        previousBallPos = currentBallPos;
    }

    void GrabBall()
    {
        ball.GetComponent<Rigidbody>().velocity = Vector3.zero;

        ball.transform.position = Vector3.Lerp(ball.transform.position, desiredBallPos.position, Time.deltaTime * 20);

        ball.GetComponent<Rigidbody>().useGravity = false;
    }

    void ReleaseBall()
    {
        Vector3 deltaBallPosition = currentBallPos - previousBallPos;
        Vector3 deltaRotation = new Vector3(Mathf.DeltaAngle(currentRotation.x, previousRotation.x), Mathf.DeltaAngle(currentRotation.y, previousRotation.y));

        angularVelocity = deltaRotation.magnitude / Time.deltaTime;

        ball.GetComponent<Rigidbody>().AddForce(deltaBallPosition * playerStrength * 10, ForceMode.Impulse);

        ball.GetComponent<Rigidbody>().useGravity = true;
    }
}
