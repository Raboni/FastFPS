using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour
{
    Rigidbody rb;
    public Vector3 direction = Vector3.zero;
    public float speed = 50f;
    public Ray ray = new Ray();

	// Use this for initialization
	void Start ()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = direction * speed;
	}
    public void Init(Ray ray)
    {
        this.ray = ray;
        direction = ray.direction;
    }
	
	// Update is called once per frame
	void Update ()
    {
	    
	}
}
