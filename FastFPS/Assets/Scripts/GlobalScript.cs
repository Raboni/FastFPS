using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GlobalScript : Photon.MonoBehaviour
{
    public static int PlayerAmount = 0;
    public int[] TeamScore = new int[2];
    public List<int> PlayerKills = new List<int>();
    public List<int> PlayerDeaths = new List<int>();

	// Use this for initialization
	void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    /// <summary>
    /// Returns player id or 255 for non-existing players
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public int GetPlayerId(PhotonPlayer player)
    {
        PhotonPlayer[] playerList = PhotonNetwork.playerList;
        for (int i = 0; i < playerList.Length; i++)
        {
            if (playerList[i] == player)
                return i;
        }
        return 255;
    }
}
