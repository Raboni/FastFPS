using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ServerScript : Photon.MonoBehaviour
{
	
	// Use this for initialization
	void Start ()
	{
        PhotonNetwork.ConnectUsingSettings("Test 1.2");
    }

    void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }
	
    //photon connection
    void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }
    void OnJoinedLobby()
    {
        PhotonNetwork.JoinRandomRoom();
    }
    void OnJoinedRoom()
    {
        playerMovement.player = PhotonNetwork.Instantiate("PlayerObjects", Vector2.zero, Quaternion.identity, 0);
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
