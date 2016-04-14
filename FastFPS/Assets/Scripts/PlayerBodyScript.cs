using UnityEngine;
using System.Collections;

public class PlayerBodyScript : Photon.MonoBehaviour //by Robin
{
    public GameObject feetObject;
    private bool useExtFeet = false;
    public Vector3 bodyOffset = Vector3.zero;
    public GameObject camera;
    public PhotonPlayer localPlayer;
    public bool local = false;

    private Ray raycast = new Ray();
    private RaycastHit rayHit;

    //private int damage = 1;
    //private float RoF = 0.3f;
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
        PlayerStats stats = transform.parent.GetComponent<PlayerStats>();
        localPlayer = stats.clientPlayer;
        //transform.rotation.SetLookRotation(camera.transform.forward, transform.up);

        //update weapon model
        //transform.FindChild("WeaponRight").GetComponent<MeshFilter>().mesh = stats.primaryRanged.Model;
        //transform.FindChild("WeaponLeft").GetComponent<MeshFilter>().mesh = stats.primaryMelee.Model;

        //make time tick
        time += Time.deltaTime;

        //body positioning (follow feet)
        transform.position = feetObject.transform.position + bodyOffset;
	}

    public void SetLocal()
    {
        local = true;
        camera.SetActive(true);
        transform.parent.GetComponent<HudScript>().enabled = true;
    }

    public void Shoot()
    {
        PlayerStats stats = transform.parent.GetComponent<PlayerStats>();
        GlobalScript global = scriptManager.GetComponent<ServerScript>().GlobalObject.GetComponent<GlobalScript>();

        //shooting
        if (time >= stats.RoF)
        {
            Debug.Log("Pew!");
            stats.Ammo--;
            //damage = stats.Damage;
            //bool kill = false;
            object[] hitParam = new object[2];
            hitParam[0] = stats.Damage;
            hitParam[1] = global.GetPlayerId(PhotonNetwork.player);
            //hitParam[1] = transform;

            raycast = new Ray(camera.transform.position + camera.transform.forward * 2, camera.transform.forward);
            //GameObject bullet = (GameObject)Instantiate(Resources.Load<Object>("Bullet"), camera.transform.position + camera.transform.forward * 2, camera.transform.rotation);
            GameObject bullet = PhotonNetwork.Instantiate("Bullet", camera.transform.position + camera.transform.forward * 2, camera.transform.rotation, 0);
            /*RaycastHit hit;
            Physics.Raycast(new Ray(transform.position, -camera.transform.forward), out hit);
            Debug.Log(hit.distance);*/
            bullet.GetComponent<BulletScript>().Init(raycast);
            Physics.Raycast(raycast, out rayHit);
            if (rayHit.transform == transform)
                Debug.Log("STOP HITTING YOURSELF!");
            if (rayHit.transform.tag == "Player")
                rayHit.transform.GetComponent<PlayerBodyScript>().GetComponent<PhotonView>().RPC("HitPlayer", PhotonTargets.AllBufferedViaServer, hitParam);
            else if (rayHit.transform.tag == "Target")
                rayHit.transform.GetComponent<TargetScript>().GetComponent<PhotonView>().RPC("Hit", PhotonTargets.All, stats.Damage);
            else
                Debug.Log("Unknown target: " + rayHit.transform.name);
            /*PhotonNetwork.RPC(NetworkManager.view, "Hit", localPlayer,
                    false, new NetworkManager.HitOptions(damage));*/

            //score check
            /*if (kill)
            {
                PhotonNetwork.player.AddScore(1);
                kill = false;
            }*/

            time = 0f;
        }
        if (stats.Ammo <= 0)
        {
            Reload();
        }
    }
    private void Reload()
    {
        PlayerStats stats = transform.parent.GetComponent<PlayerStats>();
        stats.Ammo = stats.ClipSize;
        stats.ClipAmount--;
        time = -stats.ReloadSpeed;
    }
    [PunRPC]
    public void HitPlayer(int amt, int playerSentId)//, PhotonPlayer playerSent, Transform playerSentBody)
    {
        Debug.Log("Hit");
        PlayerStats stats = transform.parent.GetComponent<PlayerStats>();
        GlobalScript global = scriptManager.GetComponent<ServerScript>().GlobalObject.GetComponent<GlobalScript>();
        int playerRecievedId = global.GetPlayerId(PhotonNetwork.player);
        if (stats.Armor > 0 && !stats.ArmorPenetration)
        {
            stats.HitPoints -= amt / 2;
            stats.Armor -= amt;
        }
        else
        {
            stats.HitPoints -= amt;
        }
        if (stats.HitPoints <= 0)
        {
            Debug.Log("player:" + playerSentId + " Killed player:" + playerRecievedId);
            //playerSentBody.GetComponent<PhotonView>().RPC("KilledPlayer", playerSent, null);
            if (playerSentId != 255)
            {
                //PhotonNetwork.playerList[playerSentId].AddScore(100);
                
                global.PlayerKills[playerSentId]++;
                if (playerRecievedId != 255)
                    global.PlayerDeaths[playerRecievedId]++;
                else
                    Debug.Log("Rage Quit?");
            }
            else
                Debug.Log("Unknown PlayerSent");
        }
        /*GameObject player = playerMovement.player;
        if (player != null)
        {
            player.GetComponent<PlayerStats>().HitPoints -= hit.damage;
            Debug.Log("Ouch!");
        }
        else
            Debug.Log("Unknown Target");*/
    }
    /*[PunRPC]
    public void KilledPlayer()
    {
        PhotonNetwork.player.AddScore(1);
    }
    public struct HitOptions
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