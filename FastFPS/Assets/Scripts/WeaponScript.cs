using UnityEngine;
using System.Collections;

public class WeaponScript : MonoBehaviour
{
    //stats
    public int Damage = 10;
    public byte MaxClipAmount = 3;
    public byte MaxClipSize = 10;
    public float RoF = 0.3f;
    public float ReloadSpeed = 2f;
    public int Range = 100;

    //shop
    public string Name = "New Weapon";
    public bool Bought = false;

    //game
    public Mesh Model;
    public Vector3 Offset = Vector3.zero;

    public WeaponScript(int dmg, byte maxClipSize, byte maxClipAmt, float RoF, float reloadSpeed, int range)
    {
        this.Damage = dmg;
        this.MaxClipAmount = maxClipAmt;
        this.MaxClipSize = maxClipSize;
        this.RoF = RoF;
        this.ReloadSpeed = reloadSpeed;
        this.Range = range;
        Bought = false;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
