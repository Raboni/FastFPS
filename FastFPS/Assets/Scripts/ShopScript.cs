using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShopScript : MonoBehaviour
{
    public PlayerStats playerStats;

    // Ranged Weapons
    public WeaponScript Pistol;
    WeaponScript Assault;
    WeaponScript Sniper;
    WeaponScript Smg;
    
    //Melee Weapons
    MeleeScript Slowrd;
    MeleeScript Fagger;

	// Use this for initialization
	public void Init ()
    {
        playerStats = playerMovement.player.transform.FindChild("PlayerBody").GetComponent<PlayerStats>();
        Pistol = new WeaponScript(0, 0, 0, 0, 0, 0);
        Sniper = new WeaponScript(0, 0, 0, 0, 0, 0);
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown("b"))
        {

        }
	}

    //simple buy menu
    void OnGUI()
    {
        if (false)
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();
            //pistol
            if (!Pistol.bought)
            {
                if (GUILayout.Button("Pistol (10$)") && playerStats.credits >= 10)
                {
                    playerStats.primaryRanged = Pistol;
                    playerStats.credits -= 10;
                    Pistol.bought = true;
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
            if (!Sniper.bought)
            {
                if (GUILayout.Button("Sniper (50$)") && playerStats.credits >= 50)
                {
                    playerStats.primaryRanged = Sniper;
                    playerStats.credits -= 50;
                    Sniper.bought = true;
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
