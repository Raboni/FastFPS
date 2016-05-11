using UnityEngine;
using System.Collections;

public class MaterialApplier : MonoBehaviour
{
    public Material material;
	
	// Update is called once per frame
	void Update ()
    {
        if (material != null)
        {
            //Debug.Log("color not null");
            GetComponent<MeshRenderer>().material = material;

            int l = GetComponent<MeshRenderer>().materials.Length;
            for (int i = 0; i < l; i++)
            {
                GetComponent<MeshRenderer>().materials[i] = material;
            }
        }
	}

    /*void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(material);
            Debug.Log(name + " sent the material: " + material.ToString());
        }
        else
        {
            Material m = (Material)stream.ReceiveNext();
            Debug.Log("material of " + name + " changed to: " + m.ToString());
            material = m;
        }
    }*/
}
