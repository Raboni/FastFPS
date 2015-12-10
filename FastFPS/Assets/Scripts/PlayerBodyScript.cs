using UnityEngine;
using System.Collections;

public class PlayerBodyScript : MonoBehaviour
{
    public GameObject feetObject;
    private bool useExtFeet = false;
    public Vector3 bodyOffset = Vector3.zero;
    public GameObject camera;

    private Ray raycast = new Ray();
    private RaycastHit rayHit;

    private int damage = 0;
    private float RoF = 0.3f;
    private float time = 0f;

	// Use this for initialization
	void Start ()
    {
        if (feetObject != null)
        {
            useExtFeet = true;
            Physics.IgnoreCollision(feetObject.GetComponent<Collider>(), GetComponent<Collider>());
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        transform.rotation.SetLookRotation(camera.transform.forward, transform.up);
        RoF = GetComponent<PlayerStats>().RoF;
        time += Time.deltaTime;

        //body positioning
        if (useExtFeet)
            transform.position = feetObject.transform.position + bodyOffset;
	}

    public void Shoot()
    {
        //shooting
        if (time >= RoF)
        {
            Debug.Log("Pew!");
            damage = GetComponent<PlayerStats>().Damage;
            raycast = new Ray(camera.transform.position, camera.transform.forward);
            GameObject bullet = (GameObject)Instantiate(Resources.Load<Object>("Bullet"), camera.transform.position + camera.transform.forward, camera.transform.rotation);
            bullet.GetComponent<BulletScript>().Init(raycast);
            Physics.Raycast(raycast, out rayHit);
            if (rayHit.collider.gameObject.tag == "Player")
                rayHit.collider.SendMessage("Hit", damage);
            time = 0f;
        }
    }
    public void Hit(int dmg)
    {
        GetComponent<PlayerStats>().MaxHitPoints -= dmg;
    }
}