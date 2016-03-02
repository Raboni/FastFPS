using UnityEngine;
using System.Collections;

public class TargetScript : MonoBehaviour
{
    public int MaxHitPoints = 100;
    int HitPoints;

	// Use this for initialization
	void Start ()
    {
        HitPoints = MaxHitPoints;
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    [PunRPC]
    void Hit(int amt)
    {
        //HitPoints -= hit.damage;
        HitPoints -= amt;
        Debug.Log("Hit");

        if (HitPoints <= 0)
        {
            Destroy(gameObject);
        }
    }
    /*public struct HitOptions
    {
        public HitOptions(int dmg)//, PhotonPlayer player)
        {
            this.damage = dmg;
            //this.player = player;
        }
        public int damage;
        //public PhotonPlayer player;
    }*/
}
