using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableCamera : MonoBehaviour {

    private Vector3 curMousePoint;
    private Vector3 mouseDelta;
    private Vector3 prevMousePoint;

    public float speed = 0.04f;
    // Use this for initialization
    void Start ()
    { }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            prevMousePoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
            return;
        }


        if (Input.GetMouseButton(0))
        {
            prevMousePoint = curMousePoint;
            curMousePoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
            mouseDelta = new Vector3(prevMousePoint.x - curMousePoint.x, prevMousePoint.y - curMousePoint.y, transform.position.z);
            if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            {
                Vector3 moveDirection = new Vector3(speed * Input.GetAxis("Mouse X"), speed * Input.GetAxis("Mouse Y"), 0);
                transform.position -= moveDirection ;
            }
        }
        
    }
    
}
