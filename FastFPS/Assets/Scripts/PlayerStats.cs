using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour //by Robin and Kevin
{
    public PhotonPlayer clientPlayer;

    //health
    private int defaultMaxHitPoints = 100;
    private int defaultMaxArmor = 100;

    public int MaxHitPoints = 100;
    public int MaxArmor = 100;

    public int HitPoints = 100;
    public int Armor = 100;

    //movement
    private float defaultMaxSpeed = 20;
    private float defaultJumpPower = 20;

    public float MaxSpeed = 20;
    public float JumpPower = 20;

    public float Speed = 20;

    //weapon stats
    private int defaultDamage = 10;
    private byte defaultMaxClipAmount = 3;
    private byte defaultClipSize = 3;
    private float defaultRoF = 0.3f;
    private float defaultReloadSpeed = 2f;
    private int defaultRange = 100;

    public int Damage = 10;
    public byte MaxClipAmount = 3;
    public byte ClipSize = 10;
    public float RoF = 0.3f;
    public float ReloadSpeed = 2f;
    public int Range = 100;

    public byte ClipAmount = 3;
    public byte Ammo = 10;

    //weapon equiped
    WeaponScript equipedRanged;
    public WeaponScript primaryRanged;
    public WeaponScript secondaryRanged;

    MeleeScript equipedMelee;
    public MeleeScript secondaryMelee;
    public MeleeScript primaryMelee;

    //shop
    bool[] perksBought = new bool[2];
    public int credits = 100;

	// Use this for initialization
	void Start ()
    {
        ResetMax();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Armor < 0)
            Armor = 0;
        if (HitPoints <= 0)
        {
            transform.position = SpawnScript.Respawn(1);
            HitPoints = MaxHitPoints;
            Debug.Log("respawn");
        }
	}

    private void ResetMax()
    {
        MaxHitPoints = defaultMaxHitPoints;
        MaxArmor = defaultMaxArmor;

        MaxSpeed = defaultMaxSpeed;
        JumpPower = defaultJumpPower;

        Damage = defaultDamage;
        MaxClipAmount = defaultMaxClipAmount;
        ClipSize = defaultClipSize;
        RoF = defaultRoF;
        ReloadSpeed = defaultReloadSpeed;
        Range = defaultRange;
    }
    public void UpdateMax()
    {
        ResetMax();
        
        //set perks & weapon stats
    }
}
