using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnScript : MonoBehaviour {
    public List<GameObject> SpawnArea = new List<GameObject>();
    public void Respawn(int Team)
    {
        float x;
         x = Random.Range(SpawnArea[Team].GetComponent<Collider>().bounds.min.x, SpawnArea[Team].GetComponent<Collider>().bounds.max.x);
        //x = //slumpa mellan SpawnArea[0].transform.position.x Max
         float z;
         z = Random.Range(SpawnArea[Team].GetComponent<Collider>().bounds.min.z, SpawnArea[Team].GetComponent<Collider>().bounds.max.z);
         Vector3 SpawnPosition = new Vector3(x, 0, z);
    }
}
