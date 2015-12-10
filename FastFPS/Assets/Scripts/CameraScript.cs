using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{
    public GameObject Object2Follow;
    public bool disableOtherCameras = true;
    public Vector3 Offset = Vector3.zero;

	// Use this for initialization
	void Start ()
    {
        if (disableOtherCameras)
        {
            GameObject[] cameraArray = GameObject.FindGameObjectsWithTag("MainCamera");
            foreach (GameObject c in cameraArray)
            {
                if (c != gameObject)
                    c.SetActive(false);
            }
        }
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
