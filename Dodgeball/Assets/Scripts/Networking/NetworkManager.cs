using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : Photon.PunBehaviour
{
    public GameObject playerPrefab;
    public GameObject ballPrefab;

    public Transform[] spawnList;
    public Color[] colorList;

    GameObject[] playerArray;

    GameObject target;

    public bool gameStarted;
    public int roomSize;

    void Start()
    {
        PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0f, 10f, 0f), Quaternion.identity, 0);

        PhotonNetwork.Instantiate(ballPrefab.name, new Vector3(0f, 10f, 0f), Quaternion.identity, 0);
    }

    void Update()
    {
        if (!gameStarted && PhotonNetwork.playerList.Length == roomSize)
        {
            playerArray = GameObject.FindGameObjectsWithTag("Player");

            if (playerArray.Length == roomSize)
            {
                StartGame();
            }
        }
    }

    void StartGame()
    {
        gameStarted = true;

        if (PhotonNetwork.isMasterClient)
        {
            for (int i = 0; i < playerArray.Length; i++)
            {
                target = playerArray[i];

                target.GetComponent<PhotonView>().RPC("TeleportPlayer", playerArray[i].GetComponent<PhotonView>().owner, spawnList[i].position);

                GetComponent<PhotonView>().RPC("ChangePlayerColor", PhotonTargets.All, i);
            }
        }
    }

    [PunRPC]
    void ChangePlayerColor(int index)
    {
        target.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", colorList[index]);
    }
}