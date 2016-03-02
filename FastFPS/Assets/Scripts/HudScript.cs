using UnityEngine;
using System.Collections;

public class HudScript : MonoBehaviour {
	GUIText myTextHP;
	public int HitPoints = 100;
    public int Armor = 100;
    public int Ammo = 0;
	// Use this for initialization
	void Start () {
		//get GUIText
        myTextHP = ServerScript.scriptManager.GetComponent<GUIText>();
	}
	
	// Update is called once per frame
	void Update () {
        //update values
        HitPoints = GetComponent<PlayerStats>().HitPoints;

        //set text
        myTextHP.text = GetComponent<PlayerStats>().HitPoints.ToString();

        // V PLZ fix V
        //myTextHP.text = Armor.ToString();
        //myTextHP.text = Ammo.ToString();
	}
}
