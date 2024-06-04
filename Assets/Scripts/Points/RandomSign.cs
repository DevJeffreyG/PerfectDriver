using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSign : MonoBehaviour
{

    public Transform pos;

    public GameObject[] objectsToInstantiate;

    // Start is called before the first frame update
    void Start()
    {
        int n = Random.Range(0,objectsToInstantiate.Length);
        GameObject selected = objectsToInstantiate[n];
        if(selected != null) Instantiate(objectsToInstantiate[n],pos.position,objectsToInstantiate[n].transform.rotation);
    }
}
