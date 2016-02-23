using UnityEngine;
using System.Collections;

public class PlayerBodyScript : MonoBehaviour //by Robin
{
    public GameObject feetObject;
    private bool useExtFeet = false;
    public Vector3 bodyOffset = Vector3.zero;
    public GameObject camera;
    public PhotonPlayer localPlayer;

    public int HitPoints = 100;
    public int Armor = 100;
    public int Ammo = 0;

    private Ray raycast = new Ray();
    private RaycastHit rayHit;

    private int damage = 1;
    private float RoF = 0.3f;
    private float time = 0f;

    GameObject scriptManager;

	// Use this for initialization
	void Start ()
    {
        //ignore collision between body and feet
        if (feetObject != null)
        {
            useExtFeet = true;
            Physics.IgnoreCollision(feetObject.GetComponent<Collider>(), GetComponent<Collider>());
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        //transform.rotation.SetLookRotation(camera.transform.forward, transform.up);
        if (HitPoints <= 0)
        {
            transform.parent.position = scriptManager.GetComponent<SpawnScript>().Respawn(1);
            HitPoints = 100;
            Debug.Log("respawn");
        }

        //Rate of Fire
        RoF = GetComponent<PlayerStats>().RoF;
        time += Time.deltaTime;

        //body positioning (follow feet)
        transform.position = feetObject.transform.position + bodyOffset;
	}

    public void Shoot()
    {
        //shooting
        if (time >= RoF)
        {
            //Debug.Log("Pew!");
            damage = GetComponent<PlayerStats>().Damage;
            raycast = new Ray(camera.transform.position + camera.transform.forward * 2, camera.transform.forward);
            GameObject bullet = (GameObject)Instantiate(Resources.Load<Object>("Bullet"), camera.transform.position + camera.transform.forward * 2, camera.transform.rotation);
            /*RaycastHit hit;
            Physics.Raycast(new Ray(transform.position, -camera.transform.forward), out hit);
            Debug.Log(hit.distance);*/
            bullet.GetComponent<BulletScript>().Init(raycast);
            Physics.Raycast(raycast, out rayHit);
            if (rayHit.collider.gameObject.tag == "Player")
                rayHit.collider.SendMessage("Hit", new HitOptions(damage, rayHit.collider.GetComponent<PlayerBodyScript>().localPlayer));
            time = 0f;
        }
    }
    public void Hit(HitOptions hit)
    {
        if (hit.player != PhotonNetwork.player)
            HitPoints -= hit.damage;
    }
    public struct HitOptions
    {
        public HitOptions(int dmg, PhotonPlayer player)
        {
            this.damage = dmg;
            this.player = player;
        }
        public int damage;
        public PhotonPlayer player;
    }
}