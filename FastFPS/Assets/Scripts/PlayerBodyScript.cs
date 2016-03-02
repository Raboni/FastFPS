using UnityEngine;
using System.Collections;

public class PlayerBodyScript : Photon.MonoBehaviour //by Robin
{
    public GameObject feetObject;
    private bool useExtFeet = false;
    public Vector3 bodyOffset = Vector3.zero;
    public GameObject camera;
    public PhotonPlayer localPlayer;

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
        scriptManager = GameObject.FindGameObjectWithTag("ScriptManager");
    }
	
	// Update is called once per frame
	void Update ()
    {
        localPlayer = transform.parent.GetComponent<PlayerStats>().clientPlayer;
        //transform.rotation.SetLookRotation(camera.transform.forward, transform.up);

        //Rate of Fire
        RoF = transform.parent.GetComponent<PlayerStats>().RoF;
        time += Time.deltaTime;

        //body positioning (follow feet)
        transform.position = feetObject.transform.position + bodyOffset;
	}

    public void Shoot()
    {
        //shooting
        if (time >= RoF)
        {
            Debug.Log("Pew!");
            damage = transform.parent.GetComponent<PlayerStats>().Damage;
            raycast = new Ray(camera.transform.position + camera.transform.forward * 2, camera.transform.forward);
            GameObject bullet = (GameObject)Instantiate(Resources.Load<Object>("Bullet"), camera.transform.position + camera.transform.forward * 2, camera.transform.rotation);
            /*RaycastHit hit;
            Physics.Raycast(new Ray(transform.position, -camera.transform.forward), out hit);
            Debug.Log(hit.distance);*/
            bullet.GetComponent<BulletScript>().Init(raycast);
            Physics.Raycast(raycast, out rayHit);
            if (rayHit.transform.tag == "Player")
                rayHit.transform.GetComponent<PlayerBodyScript>().GetComponent<PhotonView>().RPC("HitPlayer", PhotonTargets.All, damage);
            else if (rayHit.transform.tag == "Target")
                rayHit.transform.GetComponent<TargetScript>().GetComponent<PhotonView>().RPC("Hit", PhotonTargets.All, damage);
            else
                Debug.Log("Unknown target: " + rayHit.transform.name);
            /*PhotonNetwork.RPC(NetworkManager.view, "Hit", localPlayer,
                    false, new NetworkManager.HitOptions(damage));*/
            time = 0f;
        }
    }
    [PunRPC]
    public void HitPlayer(int amt)
    {
        Debug.Log("Hit");
        transform.parent.GetComponent<PlayerStats>().HitPoints -= amt;
        /*GameObject player = playerMovement.player;
        if (player != null)
        {
            player.GetComponent<PlayerStats>().HitPoints -= hit.damage;
            Debug.Log("Ouch!");
        }
        else
            Debug.Log("Unknown Target");*/
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