using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkManager : Photon.MonoBehaviour
{
    public static List<PlayerKit> playerKitList = new List<PlayerKit>();

	// Use this for initialization
	void Start ()
    {
        UpdatePlayerList();
        PhotonNetwork.RPC(photonView, "UpdatePlayerList", PhotonTargets.All, false, null);
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    [PunRPC]
    public void UpdatePlayerList()
    {
        for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
        {
            playerKitList = new List<PlayerKit>();
            PlayerKit kit = new PlayerKit(PhotonNetwork.playerList[i], FindPlayerObject(PhotonNetwork.playerList[i]));
            playerKitList.Add(kit);
        }

    }
    public GameObject GetPlayerObject(PhotonPlayer clientPlayer)
    {
        foreach (PlayerKit kit in playerKitList)
        {
            if (kit.Photon == clientPlayer)
                return kit.Object;
        }
        return null;
    }
    private GameObject FindPlayerObject(PhotonPlayer clientPlayer)
    {
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("PlayerObjects");
        foreach (GameObject o in playerObjects)
        {
            if (o.GetComponent<PlayerStats>().clientPlayer == clientPlayer)
                return o;
        }
        return null;
    }

    public struct PlayerKit
    {
        public PlayerKit(PhotonPlayer player, GameObject gameObject)
        {
            this.Photon = player;
            this.Object = gameObject;
        }
        public PhotonPlayer Photon;
        public GameObject Object;
    }

    public void Hit(HitOptions hit)
    {
        GameObject playerBody = FindPlayerObject(hit.player).transform.FindChild("PlayerBody").gameObject;
        if (playerBody != null)
            playerBody.GetComponent<PlayerBodyScript>().HitPoints -= hit.damage;
        else
            Debug.Log("Unknown Target");
    }
    public struct HitOptions
    {
        public HitOptions(int dmg, PhotonPlayer player)
        {
            this.damage = dmg;
            this.player = player;
        }
        public int damage;
        public PhotonPlayer player;
    }
}