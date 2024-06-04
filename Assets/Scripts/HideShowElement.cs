using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class HideShowElement : MonoBehaviour
{
    // Start is called before the first frame update
    /*void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }*/
    private static ColaScript cola = new ColaScript();
    public GameObject show;
    public GameObject hide1;
    public GameObject hide2;
    public GameObject hide3;
    public void view (){
        show.SetActive(true);
        hide1.SetActive(false);
        hide2.SetActive(false);
        hide3.SetActive(false);
        
        // cola

        GameObject[] elements = {show, hide1, hide2, hide3};
        cola.add(elements);
    }
}
