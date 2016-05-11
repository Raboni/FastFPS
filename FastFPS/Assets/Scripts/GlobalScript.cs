using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GlobalScript : Photon.MonoBehaviour
{
    //players info
    public int PlayerAmount = 0;
    public int[] TeamPlayerAmount = new int[3];
    public List<PhotonPlayer> PlayerList = new List<PhotonPlayer>();
    bool playerAdded = false;

    //match info
    public int[] TeamScore = new int[3];
    public List<int> PlayerKills = new List<int>();
    public List<int> PlayerDeaths = new List<int>();
	
	// Update is called once per frame
	void Update ()
    {
        //update player amount
        PlayerAmount = PlayerList.Count;

        //update team player amount
        UpdateTeamPlayerAmount();

        //update team score
        UpdateTeamScore();
	}

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            //send players
            stream.SendNext(PlayerList.Count);
            for (int i = 0; i < PlayerList.Count; i++)
            {
                stream.SendNext(PlayerList[i]);
            }
            playerAdded = false;

            //send kills
            stream.SendNext(PlayerKills.Count);
            for (int i = 0; i < PlayerKills.Count; i++)
            {
                stream.SendNext(PlayerKills[i]);
            }

            //send deaths
            stream.SendNext(PlayerDeaths.Count);
            for (int i = 0; i < PlayerDeaths.Count; i++)
            {
                stream.SendNext(PlayerDeaths[i]);
            }
        }
        else
        {
            //get players
            int plLength = (int)stream.ReceiveNext();
            List<PhotonPlayer> plTemp = new List<PhotonPlayer>();
            for (int i = 0; i < plLength; i++)
            {
                plTemp.Add((PhotonPlayer)stream.ReceiveNext());
            }
            if (!playerAdded)
                PlayerList = plTemp;

            //get kills
            int pkLength = (int)stream.ReceiveNext();
            List<int> pkTemp = new List<int>();
            for (int i = 0; i < pkLength; i++)
            {
                pkTemp.Add((int)stream.ReceiveNext());
            }
            PlayerKills = pkTemp;

            //get deaths
            int pdLength = (int)stream.ReceiveNext();
            List<int> pdTemp = new List<int>();
            for (int i = 0; i < pdLength; i++)
            {
                pdTemp.Add((int)stream.ReceiveNext());
            }
            PlayerDeaths = pdTemp;
        }
    }
    /// <summary>
    /// Updates the amount of players in each team
    /// </summary>
    public void UpdateTeamPlayerAmount()
    {
        TeamPlayerAmount = GetTeamPlayerAmount();
    }
    /// <summary>
    /// Updates the scores of all teams
    /// </summary>
    public void UpdateTeamScore()
    {
        TeamScore = GetTeamKills();
        /* old code
        int temp = 0;
        for (int i = 0; i < PlayerList.Count; i++)
        {
            if (PlayerList[i].GetTeam() == PunTeams.Team.blue)
                temp += PlayerKills[i];
        }
        TeamScore[0] = temp;
        temp = 0;
        for (int i = 0; i < PlayerList.Count; i++)
        {
            if (PlayerList[i].GetTeam() == PunTeams.Team.red)
                temp += PlayerKills[i];
        }
        TeamScore[1] = temp;*/
    }
    /// <summary>
    /// Gets the amount of players in each team (0:Blue 1:Red 2:Other)
    /// </summary>
    /// <returns>Player Amount</returns>
    private int[] GetTeamPlayerAmount()
    {
        int[] tpa = new int[3];
        foreach(PhotonPlayer p in PlayerList)
        {
            switch (p.GetTeam())
            {
                case PunTeams.Team.none:
                    tpa[2]++;
                    break;
                case PunTeams.Team.red:
                    tpa[1]++;
                    break;
                case PunTeams.Team.blue:
                    tpa[0]++;
                    break;
                default:
                    tpa[2]++;
                    break;
            }
        }
        return tpa;
    }
    /// <summary>
    /// Gets all the kills by players in each team (0:Blue 1:Red 2:Other)
    /// </summary>
    /// <returns>Kills</returns>
    private int[] GetTeamKills()
    {
        int[] tk = new int[3];
        for (int i = 0; i < PlayerList.Count; i++)
        {
            switch (PlayerList[i].GetTeam())
            {
                case PunTeams.Team.none:
                    tk[2] += PlayerKills[i];
                    break;
                case PunTeams.Team.red:
                    tk[1] += PlayerKills[i];
                    break;
                case PunTeams.Team.blue:
                    tk[0] += PlayerKills[i];
                    break;
                default:
                    tk[2] += PlayerKills[i];
                    break;
            }
        }
        return tk;
    }

    /// <summary>
    /// Returns player id or 255 for non-existing players
    /// </summary>
    /// <param name="player">Player to find</param>
    /// <returns>Player id</returns>
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
    /// <summary>
    /// Add player to global tracking
    /// </summary>
    /// <param name="player">Player to add</param>
    public void AddPlayer(PhotonPlayer player)
    {
        playerAdded = true;
        PlayerList.Add(player);
        PlayerKills.Add(0);
        PlayerDeaths.Add(0);
        Debug.Log("Player Added");
    }
    /// <summary>
    /// Remove player from global tracking
    /// </summary>
    /// <param name="player">Player to remove</param>
    public void RemovePlayer(PhotonPlayer player)
    {
        int id = GetPlayerId(player);
        if (id != 255)
        {
            PlayerList.RemoveAt(id);
            PlayerKills.RemoveAt(id);
            PlayerDeaths.RemoveAt(id);
        }
        else
        {
            Debug.Log("Unknown player to remove");
        }
        Debug.Log("Player Removed");
    }
    public void Reset()
    {
        TeamScore = new int[3];
        for (int i = 0; i < PlayerList.Count; i++)
        {
            PlayerKills[i] = 0;
            PlayerDeaths[i] = 0;
        }
    }
}
