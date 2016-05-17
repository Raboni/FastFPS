using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShopScript : MonoBehaviour //by Kevin and Robin
{
    public PlayerStats playerStats;
    public GameObject scriptmanager;

    public WeaponScript[] weaponArray;

    bool playerSpawned;
    bool Open = false;

	// Use this for initialization
	public void init ()
    {
        playerSpawned = true;

        playerStats = ServerScript.player.GetComponent<PlayerStats>();
        scriptmanager = GameObject.FindGameObjectWithTag("ScriptManager");
        weaponArray = scriptmanager.transform.FindChild("Weapons").GetComponents<WeaponScript>();

        Debug.Log("shop init");
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown("b") && playerSpawned)
        {
            Open = !Open;
            Cursor.visible = Open;
            Debug.Log("open: " + Open);
        }
	}

    //simple buy menu
    void OnGUI()
    {
        if (Open && playerSpawned)
        {
            scriptmanager.GetComponent<CustomMouseLook>().enabled = false;
            GUILayout.BeginArea(new Rect(Vector2.zero, new Vector2(Screen.width, Screen.height)));
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Credits: " + playerStats.credits);
            foreach (WeaponScript weapon in weaponArray)
            {
                if (!weapon.Bought && weapon.Ranged)
                {
                    if (GUILayout.Button(weapon.Name + " (" + weapon.Cost + ")") && playerStats.credits >= weapon.Cost)
                    {
                        playerStats.EquipedRanged = weapon;
                        playerStats.credits -= weapon.Cost;
                        weapon.Bought = true;
                        playerStats.UpdateStats();
                    }
                }
                else if (weapon.Ranged)
                {
                    if (GUILayout.Button(weapon.Name))
                    {
                        playerStats.EquipedRanged = weapon;
                        playerStats.UpdateStats();
                    }
                }
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }
        else if (playerSpawned)
            scriptmanager.GetComponent<CustomMouseLook>().enabled = true;
    }
}
