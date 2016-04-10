using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ServerScript : Photon.MonoBehaviour //by Quill18 modified by Robin
{
    //public bool Online = true;
    GameObject player;
    int PlayerTeam = 0;
    public GameObject Camera2Disable;
    public static GameObject scriptManager;
    bool connecting = false;

	// Use this for initialization
	void Start ()
	{
        scriptManager = gameObject;
        GetComponent<ServerScript>().SendMessage("UpdateSpawns");
        PhotonNetwork.player.name = PlayerPrefs.GetString("Username", "Player");
    }
    void OnDestroy()
    {
        PlayerPrefs.SetString("Username", PhotonNetwork.player.name);
    }
    void OnGUI()
    {
        //draw connection state
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());

        //draw menu
        if (!PhotonNetwork.connected && !connecting)
        {
            //GUI
            GUILayout.BeginHorizontal();
            GUILayout.Label("Username: ");
            PhotonNetwork.player.name = GUILayout.TextField(PhotonNetwork.player.name);
            GUILayout.EndHorizontal();

            if (GUILayout.Button("Single Player"))
            {
                connecting = true;
                PhotonNetwork.offlineMode = true;
                OnJoinedLobby();
            }
            else if (GUILayout.Button("Multi Player"))
            {
                connecting = true;
                PhotonNetwork.offlineMode = false;
                Connect();
            }
        }
    }
    void Connect()
    {
        //connect if online
        //PhotonNetwork.offlineMode = !Online;
        //if (Online)
            PhotonNetwork.ConnectUsingSettings("Pew 1.0.2");
    }
	
    //photon connection
    void OnConnectedToMaster()
    {
        if(!PhotonNetwork.autoJoinLobby)
            PhotonNetwork.JoinLobby();
        if (PhotonNetwork.offlineMode)
            PhotonNetwork.JoinOrCreateRoom("offline", new RoomOptions(), TypedLobby.Default);
    }
    void OnJoinedLobby()
    {
        PhotonNetwork.JoinRandomRoom();
    }
    void OnJoinedRoom()
    {
        //create clientPlayer on the network
        player = PhotonNetwork.Instantiate("PlayerObjects", SpawnScript.Respawn(PlayerTeam), Quaternion.identity, 0);
        playerMovement.player = player;
        PlayerLook.player = player;
        CustomMouseLook.player = player;
        player.GetComponent<PlayerStats>().clientPlayer = PhotonNetwork.player;
        player.transform.FindChild("PlayerBody").GetComponent<PlayerBodyScript>().SetLocal();
        //MyThirdPersonController.clientPlayer = NetworkPlayerController.clientPlayer;
        //MyThirdPersonController.clientPlayer.GetComponent<PlayerInfo>().ID = PhotonNetwork.countOfPlayersInRooms + 1;
        PhotonNetwork.RPC(photonView, "UpdatePlayerList", PhotonTargets.All, false, null);

        //disable starting camera
        Camera2Disable.SetActive(false);

        GetComponent<ShopScript>().init();
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
