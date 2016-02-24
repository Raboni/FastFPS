using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ServerScript : Photon.MonoBehaviour //by Quill18 modified by Robin
{
    public bool Online = true;
    GameObject player;
    int PlayerTeam = 0;
    public GameObject Camera2Disable;
    public static GameObject scriptManager;

	// Use this for initialization
	void Start ()
	{
        scriptManager = gameObject;
        //connect if online
        PhotonNetwork.offlineMode = !Online;
        if (Online)
            PhotonNetwork.ConnectUsingSettings("Pew 1.0.2");
    }
    void OnGUI()
    {
        //draw connection state
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }
	
    //photon connection
    void OnConnectedToMaster()
    {
        if(!PhotonNetwork.autoJoinLobby)
            PhotonNetwork.JoinLobby();
        if (!Online)
            PhotonNetwork.JoinOrCreateRoom("offline", new RoomOptions(), TypedLobby.Default);
    }
    void OnJoinedLobby()
    {
        PhotonNetwork.JoinRandomRoom();
    }
    void OnJoinedRoom()
    {
        //create clientPlayer on the network
        player = PhotonNetwork.Instantiate("PlayerObjects", GetComponent<SpawnScript>().Respawn(PlayerTeam), Quaternion.identity, 0);
        playerMovement.player = player;
        PlayerLook.player = player;
        CustomMouseLook.player = player;
        player.GetComponent<PlayerStats>().clientPlayer = PhotonNetwork.player;
        //MyThirdPersonController.clientPlayer = NetworkPlayerController.clientPlayer;
        //MyThirdPersonController.clientPlayer.GetComponent<PlayerInfo>().ID = PhotonNetwork.countOfPlayersInRooms + 1;

        //disable starting camera
        Camera2Disable.SetActive(false);
    }
    void OnPhotonRandomJoinFailed()
    {
        PhotonNetwork.CreateRoom(null);
    }
    void OnPhotonPlayerDisconnected(PhotonPlayer other)
    {
        //find other clientPlayer with id and destroy him
        PhotonNetwork.DestroyPlayerObjects(other);
    }
}
