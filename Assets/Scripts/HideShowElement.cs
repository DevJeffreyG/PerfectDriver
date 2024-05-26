using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HideShowElement : MonoBehaviour
{
    /*// Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }*/
    public GameObject show;
    public GameObject hide1;
    public GameObject hide2;
    public void view (){
        show.SetActive(true);
        hide1.SetActive(false);
        hide2.SetActive(false);
    }
}
