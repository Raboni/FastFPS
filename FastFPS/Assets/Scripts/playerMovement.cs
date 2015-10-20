using UnityEngine;
using System.Collections;

public class playerMovement : MonoBehaviour
{
    Rigidbody rb;
    public GameObject fullBody;
    public Vector3 bodyOffset = Vector3.zero;
    private bool useExtBody = false;

	// Use this for initialization
	void Start ()
    {
        Debug.Log(Physics.gravity.ToString());
        Physics.gravity = new Vector3(0, -20, 0);
        rb = GetComponent<Rigidbody>();
        if (fullBody != null)
        {
            useExtBody = true;
            Physics.IgnoreCollision(fullBody.GetComponent<Collider>(), GetComponent<Collider>());
        }
            
	}
	
	// Update is called once per frame
	void Update ()
    {
        //movement
        /*Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (movement != Vector3.zero)
            GetComponent<CharacterController>().Move(movement);*/
        rb.velocity = new Vector3(Input.GetAxis("Horizontal") * 20, rb.velocity.y, Input.GetAxis("Vertical") * 20);
        //transform.position = new Vector3(transform.position.x, transform.FindChild("Sphere").position.y + 1.5f, transform.position.z);

        //jumping
        if (Input.GetButton("Jump") && transform.FindChild("PlayerGroundCollider").GetComponent<GroundCollisionScript>().onGround)
            rb.velocity = new Vector3(rb.velocity.x, 20, rb.velocity.z);

        //

        //body
        if (useExtBody)
            fullBody.transform.position = transform.position + bodyOffset;
	}
}
