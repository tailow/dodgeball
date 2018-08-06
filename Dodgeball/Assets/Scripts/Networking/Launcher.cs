using UnityEngine;

public class Launcher : MonoBehaviour
{
    #region Variables

    string gameVersion = "0.5";

    #endregion

    void Awake()
    {
        // We don't join the lobby. There is no need to join a lobby to get the list of rooms.
        PhotonNetwork.autoJoinLobby = false;

        // This makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
        PhotonNetwork.automaticallySyncScene = true;
    }

    void Start()
    {
        Connect();
    }

    // Start the connection process.
    // - If already connected, we attempt joining a random room
    // - if not yet connected, Connect this application instance to Photon Cloud Network

    public void Connect()
    {
        // We check if we are connected or not, we join if we are , else we initiate the connection to the server.
        if (PhotonNetwork.connected)
        {
            // We need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnPhotonRandomJoinFailed() and we'll create one.
            PhotonNetwork.JoinRandomRoom();
        }

        else
        {
            // We must first and foremost connect to Photon Online Server.
            PhotonNetwork.ConnectUsingSettings(gameVersion);
        }
    }
}