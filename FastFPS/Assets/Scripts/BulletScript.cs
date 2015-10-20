using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour
{
    Rigidbody rb;
    public Vector3 direction = Vector3.zero;

	// Use this for initialization
	void Start ()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = direction;
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}
}
