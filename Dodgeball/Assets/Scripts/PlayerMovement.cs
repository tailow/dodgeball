using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMovement : NetworkBehaviour
{
    #region Variables

    public int walkSpeed;
    public int sprintSpeed;
    public int walkFOV;

    float currentSpeed;
    float targetSpeed;
    float t;
    float lastJump;
    float xRot;

    int targetFOV;
    int sprintFOV;

    bool isCrouching;

    public float sensitivity;
    public float jumpHeight;
    public float movementAcceleration;
    public float stopAcceleration;
    public float cameraFOVAcceleration;
    public float maxSpeed;
    public float crouchHeight;
    public float standingHeight;

    Vector3 dir;
    Vector3 movement;

    Rigidbody rigid;
    Camera playerCamera;

    #endregion

    void Start()
    {
        rigid = GetComponent<Rigidbody>();

        sprintFOV = walkFOV + 10;

        playerCamera = transform.GetChild(0).GetComponent<Camera>();
        playerCamera.fieldOfView = walkFOV;
    }

    void FixedUpdate()
    {
        rigid.MovePosition(transform.position + transform.TransformDirection(movement) * Time.deltaTime);
    }

    void Update()
    {
        // MOUSE INPUT
        transform.Rotate(0, Input.GetAxisRaw("Mouse X") * sensitivity, 0);

        xRot += Input.GetAxisRaw("Mouse Y") * sensitivity;
        xRot = Mathf.Clamp(xRot, -90.0f, 90.0f);

        playerCamera.transform.localEulerAngles = new Vector3(-xRot, playerCamera.transform.localEulerAngles.y, playerCamera.transform.localEulerAngles.z);

        // CROUCHING
        if (Input.GetButtonDown("Crouch"))
        {
            StartCoroutine("ChangeStance");
        }

        // JUMPING
        if (Input.GetButtonDown("Jump") && IsGrounded() && (Time.time - lastJump) > 0.4f)
        {
            rigid.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);

            lastJump = Time.time;
        }

        if (rigid.velocity.sqrMagnitude > maxSpeed)
        {
            rigid.velocity *= 0.99f;
        }

        // MOVEMENT
        targetSpeed = walkSpeed;
        targetFOV = walkFOV;

        if (Input.GetButton("Sprint") && Input.GetAxisRaw("Vertical") > 0)
        {
            targetSpeed = sprintSpeed;

            targetFOV = sprintFOV;

            t = 0f;

            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, t += Time.deltaTime * cameraFOVAcceleration);

            if (Mathf.Abs(playerCamera.fieldOfView - targetFOV) < 0.01f)
            {
                playerCamera.fieldOfView = targetFOV;
            }
        }

        else
        {
            targetSpeed = walkSpeed;

            targetFOV = walkFOV;

            t = 0f;

            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, t += Time.deltaTime * cameraFOVAcceleration);

            if (Mathf.Abs(playerCamera.fieldOfView - targetFOV) < 0.01f)
            {
                playerCamera.fieldOfView = targetFOV;
            }
        }

        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            t = 0f;

            currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, t += Time.deltaTime * movementAcceleration);

            if (Mathf.Abs(currentSpeed - targetSpeed) < 0.01f)
            {
                currentSpeed = targetSpeed;
            }

            dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        }

        else
        {
            t = 0f;

            targetSpeed = 0f;

            currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, t += Time.deltaTime * stopAcceleration);

            if (Mathf.Abs(currentSpeed - targetSpeed) < 0.01f)
            {
                currentSpeed = targetSpeed;
            }
        }

        movement = dir.normalized * currentSpeed;
    }

    bool IsGrounded()
    {
        Ray ray1 = new Ray(new Vector3(rigid.position.x, rigid.position.y, rigid.position.z), Vector3.down);
        Ray ray2 = new Ray(new Vector3(rigid.position.x - 0.2f, rigid.position.y, rigid.position.z - 0.2f), Vector3.down);
        Ray ray3 = new Ray(new Vector3(rigid.position.x - 0.2f, rigid.position.y, rigid.position.z + 0.2f), Vector3.down);
        Ray ray4 = new Ray(new Vector3(rigid.position.x + 0.2f, rigid.position.y, rigid.position.z - 0.2f), Vector3.down);
        Ray ray5 = new Ray(new Vector3(rigid.position.x + 0.2f, rigid.position.y, rigid.position.z + 0.2f), Vector3.down);

        if (Physics.Raycast(ray1, transform.localScale.y) || Physics.Raycast(ray2, transform.localScale.y) || Physics.Raycast(ray3, transform.localScale.y)
            || Physics.Raycast(ray4, transform.localScale.y) || Physics.Raycast(ray5, transform.localScale.y))
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    IEnumerator ChangeStance()
    {
        if (!isCrouching)
        {
            while (playerCamera.transform.localPosition.y - crouchHeight > 0.1f)
            {
                playerCamera.transform.localPosition = Vector3.Lerp(playerCamera.transform.localPosition, new Vector3(playerCamera.transform.localPosition.x,
                crouchHeight, playerCamera.transform.localPosition.z), Time.deltaTime * 10f);

                yield return new WaitForEndOfFrame();
            }

            isCrouching = true;

            StopCoroutine("ChangeStance");
        }

        else if (isCrouching)
        {
            while (Mathf.Abs(playerCamera.transform.localPosition.y - standingHeight) > 0.1f)
            {
                playerCamera.transform.localPosition = Vector3.Lerp(playerCamera.transform.localPosition, new Vector3(playerCamera.transform.localPosition.x,
                standingHeight, playerCamera.transform.localPosition.z), Time.deltaTime * 10f);

                yield return new WaitForEndOfFrame();
            }

            isCrouching = false;

            StopCoroutine("ChangeStance");
        }
    }
}
