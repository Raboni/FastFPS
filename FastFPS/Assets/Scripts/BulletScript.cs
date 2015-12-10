using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour
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
        this.ray = ray;
        direction = ray.direction;
        transform.rotation = Quaternion.Euler(direction);

        //set speed and time acording to distance
        speed *= 1000;
        Physics.Raycast(ray, out rayHit);
        time = rayHit.distance / (speed * 0.00055f);

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
