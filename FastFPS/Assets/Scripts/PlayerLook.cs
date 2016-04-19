using UnityEngine;
using System.Collections;

public class PlayerLook : MonoBehaviour //20% working script by Robin
{
    private bool playerInit = false;
    //public static GameObject player;
    private GameObject playerBody;
    public GameObject camera;

    private Vector3 look;
    private Vector3 prevMousePos;

	// Use this for initialization
	void Init()
    {
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
        playerBody = ServerScript.player.transform.FindChild("PlayerBody").gameObject;
        camera = ServerScript.player.transform.FindChild("Main Camera").gameObject;
	}
	
	// Update is called once per frame
	void Update ()
    {
        //make sure the player has spawned
        if (ServerScript.player == null)
        {
            playerInit = false;
            return;
        }
        else if (!playerInit)
        {
            Init();
            playerInit = true;
        }

        look = new Vector3(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height, 0) * 400;

        Vector3 camEuler = camera.transform.rotation.eulerAngles;
        camera.transform.rotation = Quaternion.Euler(-look.y, camEuler.y, 0);
        playerBody.transform.rotation = Quaternion.Euler(0, look.x, 0);

        if (Input.GetMouseButtonDown(0))
            playerBody.GetComponent<PlayerBodyScript>().SendMessage("Shoot");
	}
}
