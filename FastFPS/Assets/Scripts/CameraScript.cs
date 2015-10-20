using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{
    public GameObject player;
    public Vector3 offset;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.position = player.transform.position + offset;
        transform.rotation = new Quaternion(0, player.transform.rotation.y, 0, 0);
	}
}
