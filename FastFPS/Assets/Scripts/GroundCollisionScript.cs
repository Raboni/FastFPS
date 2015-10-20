using UnityEngine;
using System.Collections;

public class GroundCollisionScript : MonoBehaviour
{
    public bool onGround = false;

	// Use this for initialization
	void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground")
            onGround = true;
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ground")
            onGround = false;
    }
}
