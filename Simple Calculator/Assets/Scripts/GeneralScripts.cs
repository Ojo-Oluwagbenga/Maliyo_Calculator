using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System.Data;
using  System;

public class GeneralScripts : MonoBehaviour
{
    public ParticleSystem particleSystem;
    public ParticleSystem particleSystem2;

    public TextMeshPro textMeshPro;
    // Start is called before the first frame update
    void Start(){
        Debug.Log(evaluate("2+2"));
    }


    // void OnMouseDown(){
    //     Debug.Log("Sprite Clicked");
    // }
    // Update is called once per frame
    string evstring = "";  
    string lastclick = "";
    int counter = 0;
    bool start_clear_countdown = false;    

    void Update(){     
        if (start_clear_countdown){
            counter += 1;
            Debug.Log("Counting" + counter);
            if (counter == 100){
                textMeshPro.text = evstring;
                counter = 0;
                start_clear_countdown = false;
            }
        }   

        if (Input.GetMouseButtonDown(0)) {
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null) {
                GameObject clicked = hit.collider.gameObject;
                particleSystem.transform.parent = hit.collider.gameObject.transform;
                Vector3 ip = hit.collider.gameObject.transform.localPosition;
                hit.collider.gameObject.transform.localPosition = new Vector3(ip.x, ip.y, -3.5f);
                particleSystem.transform.localPosition = new Vector3(0, 0, -5);
                particleSystem.transform.localScale = new Vector3(1, 1, 1);
                particleSystem.Play();
                string ed = "-";
                
                if (clicked.name == "BC"){
                    evstring = evstring.Substring(0,evstring.Length-1);
                    if (lastclick == "*" || lastclick == "/" || lastclick == "+"){ 
                        evstring = evstring.Substring(0,evstring.Length-3);
                    }else{
                        evstring = evstring.Substring(0,evstring.Length-1);
                    } 
                    ed = "";
                }
                if (clicked.name == "C"){
                    evstring = "";
                    ed = "";
                }
                string prev_evstring = evstring;

                
                if (clicked.name == "Evaluate"){
                    string ret = evaluate(evstring);
                    evstring = ret;                
                    ed = "";
                }
                if (ed == "-"){
                    if (clicked.name == "*" || clicked.name == "/" || clicked.name == "+"){ 
                        evstring += " " + clicked.name + " ";
                    }else{
                        evstring += clicked.name;
                    }                    
                }
                lastclick = clicked.name;

                textMeshPro.text = evstring;
                if (evstring == "Incorrect syntax!"){
                    evstring = prev_evstring;
                }else{
                    if (clicked.name == "Evaluate"){
                        particleSystem2.Play();
                    }
                }
            }else{
                Debug.Log("nothing was clicked!");
            }
		}        
    }
    string evaluate(string s){
        DataTable dt = new DataTable();
        string ret = "";
        try { 
            ret = "" + dt.Compute(s,"");            
        }catch (Exception e){
            start_clear_countdown = true;
            ret = "Incorrect syntax!";
        }
        return ret;
    }
}
