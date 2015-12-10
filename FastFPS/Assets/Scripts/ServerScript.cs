using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ServerScript : Photon.MonoBehaviour
{
    public bool Online = true;
    GameObject player;

	// Use this for initialization
	void Start ()
	{
        PhotonNetwork.offlineMode = !Online;
        if (Online)
            PhotonNetwork.ConnectUsingSettings("Test 1.2");
    }

    void OnGUI()
    {
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
        player = PhotonNetwork.Instantiate("PlayerObjects", Vector2.zero, Quaternion.identity, 0);
        playerMovement.player = player;
        PlayerLook.player = player;
        //MyThirdPersonController.player = NetworkPlayerController.player;
        //MyThirdPersonController.player.GetComponent<PlayerInfo>().ID = PhotonNetwork.countOfPlayersInRooms + 1;
    }
    void OnPhotonRandomJoinFailed()
    {
        PhotonNetwork.CreateRoom(null);
    }
    void OnPhotonPlayerDisconnected(PhotonPlayer other)
    {
        //find other player with id and destroy him
        PhotonNetwork.DestroyPlayerObjects(other);
    }
}
