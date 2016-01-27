using UnityEngine;
using System.Collections;

public class HudScript : MonoBehaviour {
	GUIText myText;
	int HitPoints = 100;
    int Armor = 100;
	// Use this for initialization
	void Start () {
		myText.text = HitPoints.ToString ();
        myText.text = Armor.ToString();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
