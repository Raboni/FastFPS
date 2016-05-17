using UnityEngine;
using System.Collections;

public class CustomMouseLook : MonoBehaviour //standard asset MouseLook script modified by Robin
{
    //public static GameObject player;
    private GameObject playerBody;
    private GameObject weapon;
    private GameObject camera;
    private bool playerInit = false;

    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityX = 15F;
    public float sensitivityY = 15F;
    public float minimumX = -360F;
    public float maximumX = 360F;
    public float minimumY = -60F;
    public float maximumY = 60F;
    float rotationY = 0F;

    void Update()
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

        if (axes == RotationAxes.MouseXAndY) //the one that matters most
        {
            //set rotation
            float rotationX = playerBody.transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            //apply the rotation
            //transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
            camera.transform.localEulerAngles = new Vector3(-rotationY, camera.transform.localEulerAngles.y, 0);
            playerBody.transform.localEulerAngles = new Vector3(playerBody.transform.localEulerAngles.x, rotationX, 0);
        }
        else if (axes == RotationAxes.MouseX)
        {
            camera.transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
        }
        else
        {
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            playerBody.transform.localEulerAngles = new Vector3(-rotationY, playerBody.transform.localEulerAngles.y, 0);
        }

        //shoot
        if (Input.GetMouseButton(0))
            playerBody.GetComponent<PlayerBodyScript>().Shoot();
    }

    void Init()
    {
        //set body and camera
        playerBody = ServerScript.player.transform.FindChild("PlayerBody").gameObject;
        weapon = playerBody.transform.FindChild("WeaponRight").gameObject;
        camera = ServerScript.player.transform.FindChild("Main Camera").gameObject;

        // Make the rigid body not change rotation
        if (GetComponent<Rigidbody>())
            GetComponent<Rigidbody>().freezeRotation = true;
    }
}
