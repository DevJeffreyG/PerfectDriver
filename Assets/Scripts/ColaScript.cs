using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ColaScript : MonoBehaviour
{
    public static List<GameObject[]> history = new List<GameObject[]>();
    public static GameObject[] current = {null,null,null,null}; // la posicion 0 se muestra, las demas se ocultan
    // Start is called before the first frame update
    void Start()
    {
        current[0] = GameObject.Find("Canvas/PanelControles");
        //current[1] = GameObject.Find("Canvas/PanelVideo"); // no se pueden obtener los elementos al estar desactivados
        //current[2] = GameObject.Find("Canvas/PanelAudio");
        //current[3] = GameObject.Find("Canvas/PanelAccesibilidad");
        

        // Recorre todos los hijos del Canvas // los unicos elementos desactivados hijos directos de Canvas son los paneles de video audio... 
        int i = 1;
        foreach (Transform hijo in GameObject.Find("Canvas").transform)
        {
            // Verifica si el hijo está desactivado
            if (!hijo.gameObject.activeSelf) {
                current[i++] = hijo.gameObject; // agrega los elementos a la lista de elementos en las pocisiones a ocultar
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && Input.GetKeyDown(KeyCode.Escape)){
            if (history.Count > 0){
                var lt = history.Last();
                if (lt != null && lt[0] != null){
                    lt[0].SetActive(true);
                    lt[1].SetActive(false);
                    lt[2].SetActive(false);
                    lt[3].SetActive(false);

                    current = lt;
                    history.RemoveAt(history.Count - 1);
                } 
            } else {
                SceneManager.LoadScene("MainMenu");
            }
        }
    }
    public void add (GameObject[] elements){
        if (current[0] != elements[0]){ // evitar que se regrese a la misma pagina (cuando se da clic mas de una vez en la misma seccion)
            history.Add(current);
            current = elements;
        }
    }
}
