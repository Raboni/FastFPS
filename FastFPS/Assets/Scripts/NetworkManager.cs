using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkManager : Photon.MonoBehaviour
{
    List<PhotonPlayer> playerList = new List<PhotonPlayer>();

	// Use this for initialization
	void Start ()
    {
        for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
		{
            playerList.Add(PhotonNetwork.playerList[i]);
		}
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}
}