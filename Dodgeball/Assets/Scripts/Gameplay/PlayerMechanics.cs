using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMechanics : Photon.MonoBehaviour
{
    #region Variables

    public float playerStrength;

    public Transform desiredBallPos;

    public GameObject ballPickupIndicator;
    public GameObject exitButton;

    public bool isInPauseMenu;

    GameObject ball;

    Rigidbody ballRigidBody;


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
        if (photonView.isMine == false && PhotonNetwork.connected == true)
        {
            return;
        }

        if (Input.GetButtonDown("Cancel"))
        {
            Cursor.visible = !Cursor.visible;

            if (Cursor.lockState == CursorLockMode.None)
            {
                Cursor.lockState = CursorLockMode.Locked;

                exitButton.SetActive(false);

                isInPauseMenu = false;
            }

            else if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;

                exitButton.SetActive(true);

                isInPauseMenu = true;
            }
        }

        CheckPlayerInput();
    }

    void FixedUpdate()
    {
        if (ball != null)
        {
            currentBallPos = ballRigidBody.position;
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
                ballRigidBody = ball.GetComponent<Rigidbody>();
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
        if (PhotonNetwork.inRoom)
        {
            ball.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.player);
        }

        ballRigidBody.velocity = Vector3.zero;

        ballRigidBody.MovePosition(Vector3.Lerp(ballRigidBody.position, desiredBallPos.position, Time.deltaTime * 20));

        ballRigidBody.useGravity = false;
    }

    void ReleaseBall()
    {
        Vector3 deltaBallPosition = currentBallPos - previousBallPos;

        ballRigidBody.AddForce(deltaBallPosition * playerStrength * 10, ForceMode.Impulse);

        ballRigidBody.useGravity = true;
    }
}