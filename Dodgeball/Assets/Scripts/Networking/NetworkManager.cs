using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : Photon.PunBehaviour
{
    public GameObject playerPrefab;
    public GameObject ballPrefab;

    public bool gameStarted;

    void Start()
    {
        PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 10f, 0f), Quaternion.identity, 0);

        PhotonNetwork.Instantiate(ballPrefab.name, new Vector3(0f, 10f, 5f), Quaternion.identity, 0);
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        gameStarted = true;
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.countOfPlayers == 2)
        {
            gameStarted = true;
        }
    }
}