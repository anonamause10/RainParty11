using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    private Color origColor;
    private Color targetColor;
    private Material mat;
    private RaycastHit hit;
    private bool glowing;
    
    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<Renderer>().material;
        origColor = mat.color;
        targetColor = mat.GetColor("_EmissionColor");
    }

    // Update is called once per frame
    void Update()
    {
        if(glowing){
            targetColor = mat.color!=Color.black?mat.color*2.5f:Color.gray*2.5f;
        }
        else{
            targetColor =  mat.color*1.5f;
        }

        
        if(Input.GetKeyDown(KeyCode.Space)){
            mat.color = Random.ColorHSV() * Random.Range(0,3f);
        }



        Color col = Color.Lerp(mat.GetColor("_EmissionColor"),targetColor, 10*Time.deltaTime);
        mat.SetColor("_EmissionColor", col);

        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)){
            if(hit.collider.gameObject == gameObject){
                glowing = true;
            }else{
                glowing = false;
            }
        }else{
            glowing = false;
        }
    }
}
