using UnityEngine;
using System.Collections;

public class MatchScript : MonoBehaviour
{
    GameModeScript[] gamemodes;
    GameObject scriptmanager;
    GameObject globalObject;
    bool initialized = false;

    int currentMode = 1;
    public float time = 0f;

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
	public void Init ()
    {
        scriptmanager = GameObject.FindGameObjectWithTag("ScriptManager");
        globalObject = scriptmanager.GetComponent<ServerScript>().GlobalObject;
        gamemodes = scriptmanager.transform.FindChild("GameModes").GetComponents<GameModeScript>();
        initialized = true;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (initialized)
        {
            GlobalScript global = globalObject.GetComponent<GlobalScript>();
            if (MatchStarted)
                time += Time.deltaTime;
            if (gamemodes[currentMode].UseTeams)
            {
                if (global.TeamScore[0] >= gamemodes[currentMode].ScoreLimit)
                {
                    //blue team wins
                    EndMatch(0);
                }
                else if (global.TeamScore[1] >= gamemodes[currentMode].ScoreLimit)
                {
                    //red team wins
                    EndMatch(1);
                }
                else if (global.TeamScore[2] >= gamemodes[currentMode].ScoreLimit)
                {
                    //white team wins
                    EndMatch(2);
                }
                else if (time >= gamemodes[currentMode].TimeLimit)
                {
                    //times up
                    EndMatch(3);
                }
            }
            else
            {
                int hScore = 0;
                int hId = 0;
                //find highest score
                for (int i = 0; i < global.PlayerAmount; i++)
                {
                    if (global.PlayerKills[i] > hScore)
                    {
                        hScore = global.PlayerKills[i];
                        hId = i;
                    }
                }
                //check if anyone has won
                if (hScore >= gamemodes[currentMode].ScoreLimit)
                {
                    EndMatch(hId);
                }
                else if (time >= gamemodes[currentMode].TimeLimit)
                {
                    EndMatch(255);
                }
            }

            if (showWinScreen && Input.GetKeyDown(KeyCode.Return))
            {
                //return to lobby
                showWinScreen = false;
                time = 0f;
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
            GUILayout.Label("Press ENTER to return to lobby");
            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }
        else if (MatchStarted)
        {
            GUILayout.BeginArea(new Rect(Vector2.zero, new Vector2(Screen.width, Screen.height)));
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Time left: " + (gamemodes[currentMode].TimeLimit - (int)time));
            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();
            GUILayout.FlexibleSpace();
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
    private void EndMatch(int winner)
    {
        MatchStarted = false;
        if (gamemodes[currentMode].UseTeams)
        {
            showWinScreen = true;
            if (winner == 0)
                winnerName = "Blue Team";
            else if (winner == 1)
                winnerName = "Red Team";
            else if (winner == 2)
                winnerName = "White Team";
            else
                winnerName = "Time";
        }
        else
        {
            showWinScreen = true;
            if (winner != 255)
                winnerName = globalObject.GetComponent<GlobalScript>().PlayerList[winner].name;
            else
                winnerName = "Time";
        }
    }
}
