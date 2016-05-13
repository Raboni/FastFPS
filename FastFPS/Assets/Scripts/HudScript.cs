using UnityEngine;
using System.Collections;

public class HudScript : MonoBehaviour { //by Oskar and Robin
	GUIText myTextHP;
	public int HitPoints = 100;
    public int Armor = 100;
    public int Ammo = 0;
	// Use this for initialization
	void Start () {
		//get GUIText
        //myTextHP = ServerScript.scriptManager.GetComponent<GUIText>();
	}
	
	// Update is called once per frame
	void Update () {
        //update values
        HitPoints = GetComponent<PlayerStats>().HitPoints;
        Ammo = GetComponent<PlayerStats>().Ammo;

        //set text
        //myTextHP.text = GetComponent<PlayerStats>().HitPoints.ToString();

        // V PLZ fix V
        //myTextHP.text = Armor.ToString();
        //myTextHP.text = Ammo.ToString();
	}

    void OnGUI()
    {
        PlayerStats stats = GetComponent<PlayerStats>();
        bool reloading = GetComponentInChildren <PlayerBodyScript>().Reloading;

        GUILayout.BeginArea(new Rect(Vector2.zero, new Vector2(Screen.width, Screen.height)));
        GUILayout.BeginVertical();
        GUILayout.FlexibleSpace();
        GUILayout.Label("HP: " + stats.HitPoints);
        GUILayout.EndVertical();
        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect(Vector2.zero, new Vector2(Screen.width, Screen.height)));
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical();
        GUILayout.FlexibleSpace();
        if (!reloading)
            GUILayout.Label("Ammo: " + stats.Ammo);
        else
            GUILayout.Label("Reloading");
        GUILayout.Label("Weapon: " + stats.EquipedRanged.Name);
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }
}
