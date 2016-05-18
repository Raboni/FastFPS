using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

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
            else if (GUILayout.Button("Quit"))
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                Application.Quit();
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
        GetComponent<ScoreManager>().RemovePlayer(other.name);
    }

    private void SpawnPlayer()
    {
        //lock and hide mouse
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        //create clientPlayer on the network
        player = PhotonNetwork.Instantiate("PlayerObjects", Vector3.zero, Quaternion.identity, 0);
        player.transform.FindChild("PlayerFeet").position = GetComponent<SpawnScript>().Respawn(PlayerTeam);
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
        //enable camera again
        Camera2Disable.SetActive(true);
        //disconnect
        PhotonNetwork.Disconnect();
        //reset
        doInit = false;
        teamSelected = false;
        GetComponent<ShopScript>().playerSpawned = false;
        GetComponent<ShopScript>().crosshair.SetActive(false);
        GetComponent<MatchScriptLocal>().MatchStarted = false;
        GetComponent<ScoreManager>().Reset();
        SceneManager.LoadScene("Map1");
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