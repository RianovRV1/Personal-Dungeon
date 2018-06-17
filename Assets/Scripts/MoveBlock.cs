using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBlock : MonoBehaviour {

    public LayerMask hitLayers;
    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0)) //click mouse
        {
            Vector3 mouse = Input.mousePosition; //get mouse position
            Ray castPoint = Camera.main.ScreenPointToRay(mouse); //cast ray where mouse is clicking
            RaycastHit hit;
            if(Physics.Raycast(castPoint, out hit, Mathf.Infinity, hitLayers)) //set the hit point of the raycast based on hitlayers
            {
                this.transform.position = hit.point;  //update the position of the object with THIS SCRIPT
            }
        }
	}
}
