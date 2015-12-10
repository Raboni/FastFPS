using UnityEngine;
using System.Collections;

public class PlayerLook : MonoBehaviour
{
    private bool playerInit = false;
    public static GameObject player;
    private GameObject playerBody;
    private GameObject camera;

    private Vector3 look;
    private Vector3 prevMousePos;

	// Use this for initialization
	void Init()
    {
        playerBody = player.transform.FindChild("PlayerBody").gameObject;
        camera = player.transform.FindChild("Main Camera").gameObject;
	}
	
	// Update is called once per frame
	void Update ()
    {
        //make sure the player has spawned
        if (player == null)
        {
            playerInit = false;
            return;
        }
        else if (!playerInit)
        {
            Init();
            playerInit = true;
        }

        look = Input.mousePosition - prevMousePos;
        camera.transform.Rotate(look.y, 0, look.x);
        if (prevMousePos != Input.mousePosition)
            Debug.Log("mouse change");

        prevMousePos = Input.mousePosition;
	}
}
