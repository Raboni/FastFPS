using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnScript : MonoBehaviour //by Linus
{
    //public static List<GameObject> SpawnAreaStatic = new List<GameObject>();
    public List<GameObject> SpawnArea = new List<GameObject>();
    public Vector3 Respawn(int Team)
    {
        float x;
        x = Random.Range(SpawnArea[Team].GetComponent<Collider>().bounds.min.x, SpawnArea[Team].GetComponent<Collider>().bounds.max.x);
        float z;
        z = Random.Range(SpawnArea[Team].GetComponent<Collider>().bounds.min.z, SpawnArea[Team].GetComponent<Collider>().bounds.max.z);
        Vector3 SpawnPosition = new Vector3(x, 0, z);
        return SpawnPosition;
    }

    void Update()
    {
        
    }
}
