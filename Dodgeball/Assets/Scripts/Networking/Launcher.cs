using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class Launcher : Photon.PunBehaviour
{
    #region Variables

    [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
    public byte MaxPlayersPerRoom = 4;

    public PhotonLogLevel Loglevel = PhotonLogLevel.Informational;

    public TMP_Text connectingText;

    string gameVersion = "0.5";

    #endregion

    void Awake()
    {
        PhotonNetwork.autoJoinLobby = false;

        PhotonNetwork.automaticallySyncScene = true;

        PhotonNetwork.logLevel = Loglevel;
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
        Debug.Log("DemoAnimator/Launcher: OnConnectedToMaster() was called by PUN");

        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnDisconnectedFromPhoton()
    {
        Debug.LogWarning("DemoAnimator/Launcher: OnDisconnectedFromPhoton() was called by PUN");
    }

    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        Debug.Log("DemoAnimator/Launcher:OnPhotonRandomJoinFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom(null, new RoomOptions() {maxPlayers = 4}, null);");

        PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = MaxPlayersPerRoom }, null);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("DemoAnimator/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");

        PhotonNetwork.LoadLevel("scene_map1");
    }
}