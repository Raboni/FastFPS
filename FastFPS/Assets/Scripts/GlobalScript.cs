using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GlobalScript : Photon.MonoBehaviour
{
    //players info
    public static int PlayerAmount = 0;
    public List<PhotonPlayer> PlayerList = new List<PhotonPlayer>();

    //match info
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
        PlayerAmount = PlayerList.Count;
	}

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(PlayerList);
        }
        else
        {
            PlayerList = (List<PhotonPlayer>)stream.ReceiveNext();
        }
    }

    /// <summary>
    /// Returns player id or 255 for non-existing players
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public int GetPlayerId(PhotonPlayer player)
    {
        //PhotonPlayer[] playerList = PhotonNetwork.playerList;
        List<PhotonPlayer> pList = PlayerList;
        for (int i = 0; i < pList.Count; i++)
        {
            if (pList[i] == player)
                return i;
        }
        return 255;
    }
}
