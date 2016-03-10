﻿using UnityEngine;
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

    public void SetLocal()
    {
        local = true;
        camera.SetActive(true);
        transform.parent.GetComponent<HudScript>().enabled = true;
    }

    public void Shoot()
    {
        PlayerStats stats = transform.parent.GetComponent<PlayerStats>();

        //shooting
        if (time >= RoF)
        {
            Debug.Log("Pew!");
            stats.Ammo--;
            damage = stats.Damage;
            bool kill = false;
            object[] hitParam = new object[2];
            hitParam[0] = damage;
            hitParam[1] = kill;

            raycast = new Ray(camera.transform.position + camera.transform.forward * 2, camera.transform.forward);
            GameObject bullet = (GameObject)Instantiate(Resources.Load<Object>("Bullet"), camera.transform.position + camera.transform.forward * 2, camera.transform.rotation);
            /*RaycastHit hit;
            Physics.Raycast(new Ray(transform.position, -camera.transform.forward), out hit);
            Debug.Log(hit.distance);*/
            bullet.GetComponent<BulletScript>().Init(raycast);
            Physics.Raycast(raycast, out rayHit);
            if (rayHit.transform == transform)
                Debug.Log("STOP HITTING YOURSELF!");
            if (rayHit.transform.tag == "Player")
                rayHit.transform.GetComponent<PlayerBodyScript>().GetComponent<PhotonView>().RPC("HitPlayer", PhotonTargets.All, hitParam);
            else if (rayHit.transform.tag == "Target")
                rayHit.transform.GetComponent<TargetScript>().GetComponent<PhotonView>().RPC("Hit", PhotonTargets.All, damage);
            else
                Debug.Log("Unknown target: " + rayHit.transform.name);
            /*PhotonNetwork.RPC(NetworkManager.view, "Hit", localPlayer,
                    false, new NetworkManager.HitOptions(damage));*/

            //score check
            if (kill)
            {
                PhotonNetwork.player.AddScore(1);
                kill = false;
            }

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
        time = -10f;
    }
    [PunRPC]
    public void HitPlayer(int amt, out bool died)
    {
        //bool died;
        Debug.Log("Hit");
        PlayerStats stats = transform.parent.GetComponent<PlayerStats>();
        if (stats.Armor > 0)
        {
            stats.HitPoints -= amt / 2;
            stats.Armor -= amt;
        }
        else
        {
            stats.HitPoints -= amt;
        }
        if (stats.HitPoints <= 0)
            died = true;
        else
            died = false;
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