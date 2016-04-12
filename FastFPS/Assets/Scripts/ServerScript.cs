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
    public GameObject GlobalObject = null;
    bool connecting = false;
    bool initGlobal = false;

	// Use this for initialization
	void Start ()
	{
        scriptManager = gameObject;
        //GetComponent<ServerScript>().SendMessage("UpdateSpawns");
        PhotonNetwork.player.name = PlayerPrefs.GetString("Username", "Player");
    }
    void Update ()
    {
        if (initGlobal)
            InitGlobal();
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
        else
        {
            GUILayout.Label("global:" + GameObject.FindGameObjectsWithTag("Global").Length);
        }
    }
    void Connect()
    {
        //connect if online
        //PhotonNetwork.offlineMode = !Online;
        //if (Online)
            PhotonNetwork.ConnectUsingSettings("Pew 1.1");
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
        player = PhotonNetwork.Instantiate("PlayerObjects", GetComponent<SpawnScript>().Respawn(PlayerTeam), Quaternion.identity, 0);
        playerMovement.player = player;
        PlayerLook.player = player;
        CustomMouseLook.player = player;
        player.GetComponent<PlayerStats>().clientPlayer = PhotonNetwork.player;
        player.transform.FindChild("PlayerBody").GetComponent<PlayerBodyScript>().SetLocal();
        //disable starting camera
        Camera2Disable.SetActive(false);

        //initialize shop
        GetComponent<ShopScript>().init();

        GameObject[] g = GameObject.FindGameObjectsWithTag("Global");
        //GameObject g = GameObject.FindGameObjectWithTag("Global");
        if (g.Length > 0)
        {
            GlobalObject = g[0];
            Debug.Log("Found global: " + g[0].name + "(" + g.Length + ")");
        }
        else
        {
            initGlobal = true;
        }

        //MyThirdPersonController.clientPlayer = NetworkPlayerController.clientPlayer;
        //MyThirdPersonController.clientPlayer.GetComponent<PlayerInfo>().ID = PhotonNetwork.countOfPlayersInRooms + 1;
        //PhotonNetwork.RPC(photonView, "UpdatePlayerList", PhotonTargets.All, false, null);
    }
    void OnPhotonRandomJoinFailed()
    {
        PhotonNetwork.CreateRoom(null);
    }
    void OnPhotonPlayerDisconnected(PhotonPlayer other)
    {
        //find other clientPlayer with id and destroy him
        PhotonNetwork.DestroyPlayerObjects(other);
        GlobalObject.GetComponent<GlobalScript>().RemovePlayer(other);
    }
    private void InitGlobal()
    {
        if (GlobalObject == null)
        {
            GameObject[] g = GameObject.FindGameObjectsWithTag("Global");
            //GameObject g = GameObject.FindGameObjectWithTag("Global");
            if (g.Length > 0)
            {
                GlobalObject = g[0];
                Debug.Log("Found global later: " + g[0].name + "(" + g.Length + ")");
            }
            else
            {
                GlobalObject = PhotonNetwork.Instantiate("GlobalObject", Vector3.zero, Quaternion.identity, 0);
                Debug.Log("Created global");
            }
        }
        //add player
        GlobalObject.GetComponent<GlobalScript>().AddPlayer(PhotonNetwork.player);

        initGlobal = false;
        Debug.Log("Global Initialized");
    }
}
