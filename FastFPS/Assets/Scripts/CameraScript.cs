using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour //by Robin
{
    public GameObject Object2Follow;
    public bool disableOtherCameras = true;
    public Vector3 Offset = Vector3.zero;

    public static PhotonPlayer photonPlayer;

	// Use this for initialization
	void Awake ()
    {
        if (playerMovement.player != null)
        {
            if (this == playerMovement.player.transform.FindChild("Main Camera").GetComponent<CameraScript>())
                photonPlayer = PhotonNetwork.player;
            else
                gameObject.SetActive(false);
        }
        else if (!PhotonNetwork.offlineMode)
            gameObject.SetActive(false);

        //GameObject[] cameraArray = GameObject.FindGameObjectsWithTag("MainCamera");
        /*if (cameraArray.Length > 1) //does not work (second player camera gets disabled)
        {
            gameObject.SetActive(false);
        }*/
        /*if (playerMovement.player != null) //no idea why it doesn't work
        {
            if (disableOtherCameras && gameObject == playerMovement.player.transform.FindChild("Main Camera"))
            {
                GameObject[] cameraArray = GameObject.FindGameObjectsWithTag("MainCamera");
                foreach (GameObject c in cameraArray)
                {
                    if (c != gameObject)
                        c.SetActive(false);
                }
            }
        }*/
    }
	
	// Update is called once per frame
	void Update ()
    {
        transform.position = Object2Follow.transform.position + Offset;
        //transform.rotation = new Quaternion(0, Object2Follow.transform.rotation.y, 0, 0); //DOES NOT WORK!!!!!!!!!!!!!!!!!!
        Vector3 temp = new Vector3(transform.rotation.eulerAngles.x, Object2Follow.transform.rotation.eulerAngles.y, 0);
        transform.rotation = Quaternion.Euler(temp);
	}
}
