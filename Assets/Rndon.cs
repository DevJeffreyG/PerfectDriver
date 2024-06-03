using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{

    public Transform pos;

    public GameObject[] objectsToInstantiate;

    // Start is called before the first frame update
    void Start()
    {
        int n = Random.Range(0,objectsToInstantiate.Length);

        GameObject r = Instantiate(objectsToInstantiate[n],pos.position,objectsToInstantiate[n].transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
