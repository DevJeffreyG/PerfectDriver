using TMPro;
using UnityEngine;

public class Points : MonoBehaviour
{
    public int points;
    public TMP_Text textPoints;

    private void Update()
    {
        textPoints.text = "Points: " + points.ToString(); 
    }
}
