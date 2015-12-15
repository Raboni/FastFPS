using UnityEngine;
using System.Collections;

public class NetworkPlayer : Photon.MonoBehaviour //by Quill18 modified by Robin
{
    Vector3 realPos = Vector3.zero;
    Vector3 realVel = Vector3.zero;
    Quaternion realRot = Quaternion.identity;
    Ping ping = new Ping(MasterServer.ipAddress);

    // Use this for initialization
    void Start ()
    {
        Debug.Log(ping.time.ToString());
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (photonView.isMine)
        {
            //do nothing
        }
        else
        {
            //smooth movement
            transform.position = Vector3.Lerp(transform.position, realPos, 0.1f);
            transform.rotation = Quaternion.Lerp(transform.rotation, realRot, 0.1f);
        }
	}

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(GetComponent<Rigidbody>().velocity);
            stream.SendNext(transform.rotation);
        }
        else
        {
            realPos = (Vector3)stream.ReceiveNext();
            realVel = (Vector3)stream.ReceiveNext();
            realRot = (Quaternion)stream.ReceiveNext();
        }
    }
}
