using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainController : MonoBehaviour
{
    private Terrain Terrain;
    void Start()
    {
        Terrain = GetComponent<Terrain>();

        Terrain.treeLODBiasMultiplier = 0;
    }
}
