using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour
{
    Rigidbody rb;
    public Vector3 direction = Vector3.zero;
    public float speed = 50f;
    public Ray ray = new Ray();
    private RaycastHit rayHit;
    private float time = 0f;

	// Use this for initialization
	void Start ()
    {
        rb = GetComponent<Rigidbody>();
	}
    public void Init(Ray ray)
    {
        //initialize the ray for raycasting and bullet direction
        this.ray = ray;
        direction = ray.direction;

        //set speed and time acording to distance
        speed = 50 * Time.deltaTime;
        Physics.Raycast(ray, out rayHit);
        time = rayHit.distance / speed;

        //set velocity
        rb.velocity = direction * speed;
    }
	
	// Update is called once per frame
	void Update ()
    {
        //make time pass
        time -= Time.deltaTime;
	    //destroy when time's up
        if (time <= 0)
            Destroy((Object)gameObject);
	}
}
