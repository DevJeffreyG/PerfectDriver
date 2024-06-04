using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Points : MonoBehaviour
{
    public int points;
    public Text textPoints;

    private void Update()
    {
        textPoints.text = "Points: " + points.ToString(); 
    }
}
