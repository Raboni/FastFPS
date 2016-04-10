using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShopScript : MonoBehaviour
{
    public PlayerStats playerStats;

    //Ranged Weapons
    public WeaponScript Pistol;//note from Robin: en weapon list kanske?
    WeaponScript Assault;
    WeaponScript Sniper;
    WeaponScript Smg;
    
    //Melee Weapons
    MeleeScript Slowrd;
    MeleeScript Fagger;

    bool playerSpawned;
    bool Open = false;

	// Use this for initialization
	public void init ()
    {
        playerSpawned = true;

        playerStats = playerMovement.player.transform.FindChild("PlayerBody").GetComponent<PlayerStats>();
        Pistol = new WeaponScript(0, 0, 0, 0, 0, 0);
        Sniper = new WeaponScript(0, 0, 0, 0, 0, 0);

        Debug.Log("hej");
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown("b") && playerSpawned==true)
        {
            Open = !Open;

            Debug.Log("open");
        }
	}

    //simple buy menu
    void OnGUI()
    {
        if (Open)
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();
            //pistol
            if (!Pistol.Bought)
            {
                if (GUILayout.Button("Pistol (10$)") && playerStats.credits >= 10)
                {
                    playerStats.primaryRanged = Pistol;
                    playerStats.credits -= 10;
                    Pistol.Bought = true;
                }
            }
            else
            {
                if (GUILayout.Button("Pistol"))
                {
                    playerStats.primaryRanged = Pistol;
                }
            }
            //sniper
            if (!Sniper.Bought)
            {
                if (GUILayout.Button("Sniper (50$)") && playerStats.credits >= 50)
                {
                    playerStats.primaryRanged = Sniper;
                    playerStats.credits -= 50;
                    Sniper.Bought = true;
                }
            }
            else
            {
                if (GUILayout.Button("Sniper"))
                {
                    playerStats.primaryRanged = Sniper;
                }
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
    }
}
