using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MatchScriptLocal : MonoBehaviour
{
    GameModeScript[] gamemodes;
    GameObject scriptmanager;
    bool initialized = false;

    int currentMode = 1;

    bool showWinScreen = false;
    string winnerName = "winner";

    public bool MatchStarted = false;
    /// <summary>
    /// [Read only outside script] Checks if the match has started
    /// </summary>
    /*bool MatchStarted
    {
        public get {return MatchStarted;}
        private set {value = MatchStarted;}
    }*/

    // Use this for initialization
    public void Init()
    {
        scriptmanager = GameObject.FindGameObjectWithTag("ScriptManager");
        gamemodes = scriptmanager.transform.FindChild("GameModes").GetComponents<GameModeScript>();
        initialized = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (initialized)
        {
            int playerAmount = GameObject.FindGameObjectsWithTag("Player").Length;
            if (gamemodes[currentMode].UseTeams)
            {
                if (TeamScore(0) >= gamemodes[currentMode].ScoreLimit)
                {
                    //blue team wins
                    EndMatch();
                }
                else if (TeamScore(1) >= gamemodes[currentMode].ScoreLimit)
                {
                    //red team wins
                    EndMatch();
                }
            }
            else
            {

                int hScore = 0;
                int hId = 0;
                //find highest score
                /*for (int i = 0; i < playerAmount; i++)
                {
                    if (global.PlayerKills[i] > hScore)
                    {
                        hScore = global.PlayerKills[i];
                        hId = i;
                    }
                }*/
                //check if anyone has won
                if (hScore >= gamemodes[currentMode].ScoreLimit)
                {
                    EndMatch();
                }
            }

            if (showWinScreen && Input.GetKeyDown(KeyCode.Return))
            {
                //return to lobby
                showWinScreen = false;
                scriptmanager.GetComponent<ServerScript>().ReturnToLobby();
            }
        }
    }

    void OnGUI()
    {
        if (showWinScreen)
        {
            GUILayout.BeginArea(new Rect(Vector2.zero, new Vector2(Screen.width, Screen.height)));
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();
            GUILayout.Label(winnerName + " Wins!");
            GUILayout.FlexibleSpace();
            GUILayout.Label("Press ENTER to restart");
            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }
    }

    /// <summary>
    /// End the match
    /// </summary>
    /// <param name="winner">The winning team (0:blue 1:red 2:white) or player</param>
    private void EndMatch()
    {
        GameObject mvp = null;
        int mvpScore = 0;
        int teamWinner = TeamWinner(out mvp, out mvpScore);
        MatchStarted = false;
        if (gamemodes[currentMode].UseTeams)
        {
            showWinScreen = true;
            if (teamWinner == 0)
                winnerName = "Blue Team";
            else if (teamWinner == 1)
                winnerName = "Red Team";
            else
                winnerName = "Someone";
        }
        else
        {
            showWinScreen = true;
            winnerName = mvp.name;
        }
    }

    private int TeamScore(int team)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        List<GameObject> playersBlue = new List<GameObject>();
        List<GameObject> playersRed = new List<GameObject>();
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<TeamMember>().Team == 0)
                playersBlue.Add(players[i]);
            if (players[i].GetComponent<TeamMember>().Team == 1)
                playersRed.Add(players[i]);
        }

        int scoreBlue = 0;
        foreach (GameObject p in playersBlue)
            scoreBlue += p.GetComponent<TeamMember>().Kills;
        int scoreRed = 0;
        foreach (GameObject p in playersRed)
            scoreRed += p.GetComponent<TeamMember>().Kills;

        if (team == 0)
            return scoreBlue;
        else if (team == 1)
            return scoreRed;
        else
            return 0;
    }
    private int TeamWinner(out GameObject mvp, out int mvpKills)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        List<GameObject> playersBlue = new List<GameObject>();
        List<GameObject> playersRed = new List<GameObject>();
        mvp = null;
        mvpKills = 0;
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<TeamMember>().Team == 0)
                playersBlue.Add(players[i]);
            if (players[i].GetComponent<TeamMember>().Team == 1)
                playersRed.Add(players[i]);

            if (mvp == null || players[i].GetComponent<TeamMember>().Kills > mvpKills)
            {
                mvp = players[i];
                mvpKills = players[i].GetComponent<TeamMember>().Kills;
            }
        }

        int scoreBlue = 0;
        foreach (GameObject p in playersBlue)
            scoreBlue += p.GetComponent<TeamMember>().Kills;
        int scoreRed = 0;
        foreach (GameObject p in playersRed)
            scoreRed += p.GetComponent<TeamMember>().Kills;

        if (scoreBlue > scoreRed)
            return 0;
        else if (scoreRed > scoreBlue)
            return 1;
        else
            return 2;
    }
}
