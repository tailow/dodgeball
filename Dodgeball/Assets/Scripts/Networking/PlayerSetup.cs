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
}
