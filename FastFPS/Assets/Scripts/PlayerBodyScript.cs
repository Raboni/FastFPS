using UnityEngine;
using System.Collections;

public class PlayerBodyScript : MonoBehaviour
{
    public GameObject feetObject;
    private bool useExtFeet = false;
    public GameObject camera;

    private Ray raycast = new Ray();
    private RaycastHit rayHit;

    private float hitPoints = 100;
    public byte weaponEquiped = 1;
    public float damageBoost = 0f;
    public float damageTotal = 0f;

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

        if (Input.GetMouseButtonDown(1))
        {
            damageTotal = (float)weaponEquiped + damageBoost;
            raycast = new Ray(camera.transform.position, camera.transform.forward);
            GameObject bullet = (GameObject)Instantiate(Resources.Load<Object>("Bullet"), camera.transform.position + camera.transform.forward, camera.transform.rotation);
            bullet.GetComponent<BulletScript>().Init(raycast);
            Physics.Raycast(raycast, out rayHit);
            if (rayHit.collider.gameObject.tag == "Enemy")
                rayHit.collider.SendMessage("Hit", damageTotal);
        }
	}

    public void Hit(float dmg)
    {
        hitPoints -= dmg;
    }
}