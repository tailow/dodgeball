using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetup : Photon.MonoBehaviour
{
    public Behaviour[] componentsToDisable;

    void Start()
    {
        if (!photonView.isMine)
        {
            for (int i = 0; i < componentsToDisable.Length; i++)
            {
                componentsToDisable[i].enabled = false;
            }
        }
    }

    [PunRPC]
    void TeleportPlayer(Vector3 position)
    {
        transform.position = position;

        transform.GetChild(0).localRotation = Quaternion.Euler(Vector3.zero);

        Vector3 targetPosition = new Vector3(0, transform.position.y, 0);

        transform.LookAt(targetPosition);
    }
}
