﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkManager : Photon.MonoBehaviour
{
    public static List<PlayerKit> playerKitList = new List<PlayerKit>();
    public static PhotonView view;

	// Use this for initialization
	void Start ()
    {
        //UpdatePlayerList();
        //PhotonNetwork.RPC(photonView, "UpdatePlayerList", PhotonTargets.All, false, null);
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
        view = photonView;
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
        Debug.Log("Could Not Find Player");
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

    [PunRPC]
    public void Hit(HitOptions hit)
    {
        Debug.Log("Hit");
        GameObject playerBody = FindPlayerObject(PhotonNetwork.player).transform.FindChild("PlayerBody").gameObject;
        if (playerBody != null)
        {
            playerBody.GetComponent<PlayerBodyScript>().HitPoints -= hit.damage;
            Debug.Log("Ouch!");
        }
        else
            Debug.Log("Unknown Target");
    }
    public struct HitOptions
    {
        public HitOptions(int dmg)//, PhotonPlayer player)
        {
            this.damage = dmg;
            //this.player = player;
        }
        public int damage;
        //public PhotonPlayer player;
    }
}