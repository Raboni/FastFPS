﻿using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour //by Robin and Kevin
{
    //health
    public int MaxHitPoints = 100;
    public int MaxArmor = 100;
    //movement
    public float MaxSpeed = 20;
    public float JumpPower = 20;
    //weapon
    public int Damage = 10;
    public byte MaxClipSize = 10;
    public byte MaxClipAmount = 3;
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
	    
	}
}