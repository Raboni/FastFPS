using UnityEngine;
using System.Collections;

public class HudScript : PlayerBodyScript {
	GUIText myText;
	public int HitPoints = 100;
    public int Armor = 100;
    public int Ammo = 0;
	// Use this for initialization
	void Start () {
		myText.text = HitPoints.ToString ();
        myText.text = Armor.ToString();
        myText.text = Ammo.ToString();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
