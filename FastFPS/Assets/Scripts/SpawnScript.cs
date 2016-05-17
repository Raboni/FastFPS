using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnScript : MonoBehaviour //by Linus
{
    public List<GameObject>[] SpawnArea = new List<GameObject>[2];
    public List<GameObject> SpawnAreaBlue = new List<GameObject>();
    public List<GameObject> SpawnAreaRed = new List<GameObject>();

    public Vector3 Respawn(int Team)
    {
        SpawnArea[0] = SpawnAreaBlue;
        SpawnArea[1] = SpawnAreaRed;
        float x;
        x = Random.Range(SpawnArea[Team][Random.Range(0, SpawnArea[Team].Count)].GetComponent<Collider>().bounds.min.x, SpawnArea[Team][Random.Range(0, SpawnArea[Team].Count)].GetComponent<Collider>().bounds.max.x);
        float z;
        z = Random.Range(SpawnArea[Team][Random.Range(0, SpawnArea[Team].Count)].GetComponent<Collider>().bounds.min.z, SpawnArea[Team][Random.Range(0, SpawnArea[Team].Count)].GetComponent<Collider>().bounds.max.z);
        Vector3 SpawnPosition = new Vector3(x, 0.5f, z);
        return SpawnPosition;
    }
}
