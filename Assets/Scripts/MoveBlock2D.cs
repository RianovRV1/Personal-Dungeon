using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBlock2D : MonoBehaviour {

    //int layer;
    //public LayerMask ignoreLayers;
    //int layer; 
    // Use this for initialization
    public enum HitTag { Floor, Wall};
    public HitTag hitTag;
    void Start()
    {
        //layer = 1 << LayerMask.NameToLayer("floor");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) //click mouse
        {
            Debug.Log("casting ray");
            
            Vector3 mouse = Input.mousePosition; //get mouse position
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(mouse); //convert to vector2 worldpoint
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero); //cast the ray you have to include distance to filter hit layers
            //RaycastHit2D hitWall = Physics2D.Raycast(worldPoint, Vector2.zero, Mathf.Infinity, ignoreLayers);
            //Debug.Log("RayHit " + hit.collider.gameObject.layer +" "+ hitLayers.value);
            if (hit)
            {
                if (hit.collider.tag == hitTag.ToString()) //set the hit point of the raycast based on hitlayers
                {
                    Debug.Log("RayHit " + hit.collider.name + hitTag);
                    //if(hit.collider.name == "floor")
                    this.transform.position = hit.point;  //update the position of the object with THIS SCRIPT
                }
            }
        }
    }
}
