using UnityEngine;
using System.Collections;

public class playerMovement : MonoBehaviour //by Linus and Robin
{
    //public static GameObject player;
    public GameObject playerFeet;
    //private bool playerInit = false;

    Rigidbody rb;
    private GameObject playerBody;
    public Vector3 bodyOffset = Vector3.zero;
    private bool useExtBody = false;

    //private float speed = 20;
    //private float jumpPower = 20;

	// Use this for initialization
	void Start ()
    {
        //get the feet and body
        playerFeet = transform.FindChild("PlayerFeet").gameObject;
        playerBody = transform.FindChild("PlayerBody").gameObject;

        //set gravity and rigidbody
        //Debug.Log(Physics.gravity.ToString());
        Physics.gravity = new Vector3(0, -20, 0);
        rb = playerFeet.GetComponent<Rigidbody>();

        //ignore collision between feet and body
        if (playerBody != null)
        {
            useExtBody = true;
            Physics.IgnoreCollision(playerBody.GetComponent<Collider>(), playerFeet.GetComponent<Collider>());
        }

        //get speed and jump power
        //speed = player.GetComponent<PlayerStats>().Speed;
        //jumpPower = player.GetComponent<PlayerStats>().JumpPower;

        Debug.Log("Movement Initialized");
        Cursor.visible = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        //make sure the clientPlayer has spawned
        /*if (player == null)
            return;
        else if (!playerInit)
        {
            Init();
            playerInit = true;
        }*/
        PlayerStats stats = GetComponent<PlayerStats>();

        //movement
        /*Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (movement != Vector3.zero)
            GetComponent<CharacterController>().Move(movement);*/
        rb.velocity = (playerBody.transform.forward * Input.GetAxis("Vertical") * stats.Speed) + (playerBody.transform.right * Input.GetAxis("Horizontal") * stats.Speed) + (playerBody.transform.up * rb.velocity.y);
        //transform.position = new Vector3(transform.position.x, transform.FindChild("Sphere").position.y + 1.5f, transform.position.z);

        //jumping
        if (Input.GetButtonDown("Jump") && playerFeet.transform.FindChild("PlayerGroundCollider").GetComponent<GroundCollisionScript>().onGround)
        {
            rb.velocity = new Vector3(rb.velocity.x, stats.JumpPower, rb.velocity.z);
        }

        //body positioning
        //if (useExtBody)
            //playerBody.transform.position = playerFeet.transform.position + bodyOffset;
	}
}
