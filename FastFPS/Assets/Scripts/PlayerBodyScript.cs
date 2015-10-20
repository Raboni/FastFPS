using UnityEngine;
using System.Collections;

public class PlayerBodyScript : MonoBehaviour
{
    public GameObject feetObject;
    private bool useExtFeet = false;

	// Use this for initialization
	void Start ()
    {
        if (feetObject != null)
        {
            useExtFeet = true;
            //Physics.IgnoreCollision(feetObject.GetComponent<Collider>(), GetComponent<Collider>());
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetMouseButtonDown(1))
        {
            GameObject bullet = (GameObject)Instantiate(Resources.Load<Object>("Bullet"), transform.position + transform.forward, transform.rotation);
            bullet.GetComponent<BulletScript>().direction = transform.forward * 50;
        }
	}
}