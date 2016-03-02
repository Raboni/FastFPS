using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnScript : MonoBehaviour {
    public static List<GameObject> SpawnAreaStatic = new List<GameObject>();
    public List<GameObject> SpawnArea = new List<GameObject>();
    public static Vector3 Respawn(int Team)
    {
        float x;
        x = Random.Range(SpawnAreaStatic[Team].GetComponent<Collider>().bounds.min.x, SpawnAreaStatic[Team].GetComponent<Collider>().bounds.max.x);
        float z;
        z = Random.Range(SpawnAreaStatic[Team].GetComponent<Collider>().bounds.min.z, SpawnAreaStatic[Team].GetComponent<Collider>().bounds.max.z);
        Vector3 SpawnPosition = new Vector3(x, 0, z);
        return SpawnPosition;
    }

    void Update()
    {
        SpawnAreaStatic = SpawnArea;
    }
    void UpdateSpawns()
    {
        SpawnAreaStatic = SpawnArea;
    }
}
