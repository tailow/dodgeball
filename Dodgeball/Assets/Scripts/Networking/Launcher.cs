using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class Launcher : Photon.PunBehaviour
{
    #region Variables

    public byte maxPlayersPerRoom;

    public PhotonLogLevel loglevel = PhotonLogLevel.Informational;

    public TMP_Text connectingText;

    string gameVersion = "0.5";

    #endregion

    void Awake()
    {
        PhotonNetwork.autoJoinLobby = false;

        PhotonNetwork.automaticallySyncScene = true;

        PhotonNetwork.logLevel = loglevel;
    }

    void Start()
    {
        Connect();

        StartCoroutine("TextColorCoroutine");
    }

    IEnumerator TextColorCoroutine()
    {
        while (true)
        {
            connectingText.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1);

            yield return new WaitForSeconds(0.5f);
        }
    }

    public void Connect()
    {
        if (PhotonNetwork.connected)
        {
            PhotonNetwork.JoinRandomRoom();
        }

        else
        {
            PhotonNetwork.ConnectUsingSettings(gameVersion);
        }
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = maxPlayersPerRoom }, null);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("scene_map1");
    }
}