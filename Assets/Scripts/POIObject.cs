using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BoxCollider2D))]
public class POIObject : MonoBehaviour {

    private Vector3 destination;
    bool traveling;
    // Use this for initialization
	void Start () {
        traveling = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (traveling)
        {
            Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, destination, Time.deltaTime * 6);
            if(Camera.main.transform.position == destination)
            {
                traveling = false;
            }
        }
    }
    void OnMouseDown()
    {
        destination = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
        traveling = true;
    }
}
