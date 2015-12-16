using UnityEngine;
using System.Collections;

public class playerMovement : MonoBehaviour //by Linus and Robin
{
    public static GameObject player;
    public GameObject playerFeet;
    private bool playerInit = false;

    Rigidbody rb;
    private GameObject playerBody;
    public Vector3 bodyOffset = Vector3.zero;
    private bool useExtBody = false;

    private float speed = 20;
    private float jumpPower = 20;

	// Use this for initialization
	void Init ()
    {
        //get the feet and body
        playerFeet = player.transform.FindChild("PlayerFeet").gameObject;
        playerBody = player.transform.FindChild("PlayerBody").gameObject;

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
        speed = playerBody.GetComponent<PlayerStats>().MaxSpeed;
        jumpPower = playerBody.GetComponent<PlayerStats>().JumpPower;

        Cursor.visible = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = !Cursor.visible;
            Application.Quit();
        }

        //make sure the player has spawned
        if (player == null)
            return;
        else if (!playerInit)
        {
            Init();
            playerInit = true;
        }


        //movement
        /*Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (movement != Vector3.zero)
            GetComponent<CharacterController>().Move(movement);*/
        rb.velocity = (playerBody.transform.forward * Input.GetAxis("Vertical") * speed) + (playerBody.transform.right * Input.GetAxis("Horizontal") * speed) + (playerBody.transform.up * rb.velocity.y);
        //transform.position = new Vector3(transform.position.x, transform.FindChild("Sphere").position.y + 1.5f, transform.position.z);

        //jumping
        if (Input.GetKeyDown("space") && playerFeet.transform.FindChild("PlayerGroundCollider").GetComponent<GroundCollisionScript>().onGround)
            rb.velocity = new Vector3(rb.velocity.x, jumpPower, rb.velocity.z);

        //body positioning
        //if (useExtBody)
            //playerBody.transform.position = playerFeet.transform.position + bodyOffset;
	}
}
