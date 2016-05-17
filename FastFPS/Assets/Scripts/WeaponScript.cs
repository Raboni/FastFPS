using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class WeaponScript : MonoBehaviour
{
    //stats
    public bool Ranged = true;
    public int Damage = 10;
    public byte ClipAmount = 3;
    public byte ClipSize = 10;
    public float RoF = 0.3f;
    public float ReloadSpeed = 2f;
    public int Range = 100;

    //shop
    public string Name = "New Weapon";
    public bool Bought = false;
    public int Cost = 100;

    //game
    public Mesh Model;
    public Vector3 Offset = Vector3.zero;
    public AudioClip sound;

    public WeaponScript(int dmg, byte clipSize, byte clipAmt, float RoF, float reloadSpeed, int range)
    {
        this.Damage = dmg;
        this.ClipAmount = clipAmt;
        this.ClipSize = clipSize;
        this.RoF = RoF;
        this.ReloadSpeed = reloadSpeed;
        this.Range = range;
        Bought = false;
    }
}
