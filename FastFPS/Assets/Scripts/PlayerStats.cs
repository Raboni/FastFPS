using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour //by Robin and Kevin
{
    public PhotonPlayer clientPlayer;
    //health
    public int MaxHitPoints = 100;
    public int MaxArmor = 100;
    public int HitPoints = 100;
    public int Armor = 100;
    //movement
    public float MaxSpeed = 20;
    public float Speed = 20;
    public float JumpPower = 20;
    //weapon
    public int Damage = 10;
    public byte MaxClipAmount = 3;
    public byte ClipAmount = 3;
    public byte ClipSize = 10;
    public byte Ammo = 10;
    public float RoF = 0.3f;
    public float ReloadSpeed = 2f;
    public int Range = 100;

	// Use this for initialization
	void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (HitPoints <= 0)
        {
            transform.position = SpawnScript.Respawn(1);
            HitPoints = MaxHitPoints;
            Debug.Log("respawn");
        }
	}
}
