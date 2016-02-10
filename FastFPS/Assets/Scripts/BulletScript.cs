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
    private Vector3 start = Vector3.zero;

	// Use this for initialization
	void Start ()
    {
        
	}
    public void Init(Ray ray)
    {
        rb = GetComponent<Rigidbody>();

        start = transform.position;

        //initialize the ray for raycasting and bullet direction
        this.ray = new Ray(transform.position + ray.direction, ray.direction);
        direction = ray.direction;
        transform.rotation = Quaternion.LookRotation(direction);

        //raycast for distance
        Physics.Raycast(ray, out rayHit);

        /*//set speed and time acording to distance (need fix)
        speed *= 1000;
        time = rayHit.distance / (speed * 0.00055f);
        Debug.Log(rayHit.distance);*/

        //set velocity
        rb.velocity = direction * speed;
    }
	
	// Update is called once per frame
	void Update ()
    {
        //destroy when hit
        if (rayHit.distance <= Vector3.Distance(start, transform.position))
            Destroy(gameObject);
        //Debug.Log(rayHit.collider.transform.name);
        /*//make time pass
        time -= Time.deltaTime;
	    //destroy when time's up
        if (time <= 0)
            Destroy(gameObject);*/
	}
}
