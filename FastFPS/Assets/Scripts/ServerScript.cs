using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ServerScript : Photon.MonoBehaviour //by Quill18 modified by Robin
{
    //public bool Online = true;
    public static GameObject player;
    int PlayerTeam = 0;
    public GameObject Camera2Disable;
    public static GameObject scriptManager;
    public GameObject GlobalObject = null;
    bool connecting = false;
    bool doInit = false;
    public Material MaterialBlue;
    public Material MaterialRed;

	// Use this for initialization
	void Start ()
	{
        scriptManager = gameObject;
        //GetComponent<ServerScript>().SendMessage("UpdateSpawns");
        PhotonNetwork.player.name = PlayerPrefs.GetString("Username", "Player");
    }
    void Update ()
    {
        if (doInit)
        {
            InitGlobal();
            selectTeam();
            //initialize shop
            GetComponent<ShopScript>().init();

            doInit = false;
        }
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
        else if (!doInit)
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
        {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.JoinOrCreateRoom("offline", new RoomOptions(), TypedLobby.Default);
        }
    }
    void OnJoinedLobby()
    {
        PhotonNetwork.JoinRandomRoom();
    }
    void OnJoinedRoom()
    {
        //create clientPlayer on the network
        player = PhotonNetwork.Instantiate("PlayerObjects", GetComponent<SpawnScript>().Respawn(PlayerTeam), Quaternion.identity, 0);
        //playerMovement.player = player;
        //PlayerLook.player = player;
        //CustomMouseLook.player = player;
        player.GetComponent<playerMovement>().enabled = true;
        player.GetComponent<PlayerStats>().clientPlayer = PhotonNetwork.player;
        player.transform.FindChild("PlayerBody").GetComponent<PlayerBodyScript>().SetLocal();
        //disable starting camera
        Camera2Disable.SetActive(false);

        //initialize
        doInit = true;

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

        Debug.Log("Global Initialized");
    }
    private void selectTeam()
    {
        GlobalScript global = GlobalObject.GetComponent<GlobalScript>();
        global.UpdateTeamPlayerAmount();
        //select team
        if (global.TeamPlayerAmount[1] > global.TeamPlayerAmount[2])
        {
            PlayerTeam = 2;
            PhotonNetwork.player.SetTeam(PunTeams.Team.red);
            player.transform.FindChild("PlayerBody").FindChild("Body").GetComponent<MeshRenderer>().material = MaterialRed;
            player.transform.FindChild("PlayerFeet").GetComponent<MeshRenderer>().material = MaterialRed;
        }
        else if (global.TeamPlayerAmount[2] >= global.TeamPlayerAmount[1])
        {
            PlayerTeam = 1;
            PhotonNetwork.player.SetTeam(PunTeams.Team.blue);
            player.transform.FindChild("PlayerBody").FindChild("Body").GetComponent<MeshRenderer>().material = MaterialBlue;
            player.transform.FindChild("PlayerFeet").GetComponent<MeshRenderer>().material = MaterialBlue;
        }
    }
}
