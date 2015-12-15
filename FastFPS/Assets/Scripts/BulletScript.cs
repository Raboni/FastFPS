using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour //by Robin
{
    Rigidbody rb;
    public Vector3 direction = Vector3.zero;
    public float speed = 50f;
    public Ray ray = new Ray();
    public RaycastHit rayHit;
    private float time = 10f;

	// Use this for initialization
	void Start ()
    {
        
	}
    public void Init(Ray ray)
    {
        rb = GetComponent<Rigidbody>();

        //initialize the ray for raycasting and bullet direction
        this.ray = new Ray(transform.position + ray.direction, ray.direction);
        direction = ray.direction;
        transform.rotation = Quaternion.LookRotation(direction);

        //set speed and time acording to distance (need fix)
        speed *= 1000;
        Physics.Raycast(ray, out rayHit);
        time = rayHit.distance / (speed * 0.00055f);
        Debug.Log(rayHit.distance);

        //set velocity
        rb.AddForce(direction * speed);
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
