using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ServerScript : Photon.MonoBehaviour //by Quill18 modified (a lot) by Robin
{
    //public bool Online = true;
    public static GameObject player;
    int PlayerTeam = 0;
    public GameObject Camera2Disable;
    public static GameObject scriptManager;
    //public GameObject GlobalObject = null;
    bool connecting = false;
    bool doInit = false;
    bool teamSelected = false;
    public Material MaterialBlue;
    public Material MaterialRed;
    public bool Mute = false;

	// Use this for initialization
	void Start ()
	{
        scriptManager = gameObject;
        //GetComponent<ServerScript>().SendMessage("UpdateSpawns");
        PhotonNetwork.player.name = PlayerPrefs.GetString("Username", "Player");
    }
    void Update ()
    {
        if (doInit && teamSelected)
        {
            SpawnPlayer();
            //InitGlobal();
            //selectTeam();
            //initialize shop
            GetComponent<ShopScript>().init();
            //start match
            scriptManager.GetComponent<MatchScriptLocal>().Init();
            scriptManager.GetComponent<MatchScriptLocal>().MatchStarted = true;
            
            doInit = false;
        }
        UpdateTeamColor();
    }

    void OnDestroy()
    {
        PlayerPrefs.SetString("Username", PhotonNetwork.player.name);
    }
    void OnGUI()
    {
        //draw connection state
        //GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());

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
            Mute = GUILayout.Toggle(Mute, "Mute");
        }
        else if (!teamSelected)
        {
            GUILayout.BeginArea(new Rect(Vector2.zero, new Vector2(Screen.width, Screen.height)));
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Blue Team"))
            {
                PlayerTeam = 0;
                teamSelected = true;
            }
            if (GUILayout.Button("Red Team"))
            {
                PlayerTeam = 1;
                teamSelected = true;
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }
    }
    void Connect()
    {
        //connect if online
        //PhotonNetwork.offlineMode = !Online;
        //if (Online)
            PhotonNetwork.ConnectUsingSettings("Pew 2.0");
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
        //GlobalObject.GetComponent<GlobalScript>().RemovePlayer(other);
    }

    /*private void InitGlobal()
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
    }*/
    /*private void selectTeam()
    {
        GlobalScript global = GlobalObject.GetComponent<GlobalScript>();
        global.UpdateTeamPlayerAmount();
        //select team
        if (global.TeamPlayerAmount[0] > global.TeamPlayerAmount[1])
        {
            PlayerTeam = 1;
            PhotonNetwork.player.SetTeam(PunTeams.Team.red);
            player.transform.FindChild("PlayerBody").FindChild("Body").GetComponent<MaterialApplier>().material = MaterialRed;
            player.transform.FindChild("PlayerFeet").GetComponent<MaterialApplier>().material = MaterialRed;
        }
        else if (global.TeamPlayerAmount[1] >= global.TeamPlayerAmount[0])
        {
            PlayerTeam = 0;
            PhotonNetwork.player.SetTeam(PunTeams.Team.blue);
            player.transform.FindChild("PlayerBody").FindChild("Body").GetComponent<MaterialApplier>().material = MaterialBlue;
            player.transform.FindChild("PlayerFeet").GetComponent<MaterialApplier>().material = MaterialBlue;
        }
    }*/
    private void SpawnPlayer()
    {
        //create clientPlayer on the network
        player = PhotonNetwork.Instantiate("PlayerObjects", GetComponent<SpawnScript>().Respawn(PlayerTeam), Quaternion.identity, 0);
        player.GetComponent<playerMovement>().enabled = true;
        player.GetComponent<PlayerStats>().clientPlayer = PhotonNetwork.player;
        player.transform.FindChild("PlayerBody").GetComponent<PlayerBodyScript>().SetLocal();
        player.GetComponent<TeamMember>().Name = PhotonNetwork.playerName;
        player.GetComponent<TeamMember>().Team = PlayerTeam;
        //disable starting camera
        Camera2Disable.SetActive(false);
        //hide your player from your camera
        player.transform.FindChild("PlayerBody").FindChild("Body").gameObject.layer = 8;
    }
    public void ReturnToLobby()
    {
        //GlobalObject.GetComponent<GlobalScript>().Reset();
    }
    public void UpdateTeamColor()
    {
        //update team color
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<TeamMember>().Team != 2)
            {
                Material m = MaterialRed;
                if (players[i].GetComponent<TeamMember>().Team == 0)
                    m = MaterialBlue;
                else if (players[i].GetComponent<TeamMember>().Team == 1)
                    m = MaterialRed;
                players[i].transform.FindChild("PlayerBody").FindChild("Body").GetComponent<MaterialApplier>().material = m;
                players[i].transform.FindChild("PlayerBody").FindChild("WeaponRight").GetComponent<MaterialApplier>().material = m;
                players[i].transform.FindChild("PlayerFeet").GetComponent<MaterialApplier>().material = m;
                //Debug.Log("updated color to " + m.ToString());
            }
        }
    }
}