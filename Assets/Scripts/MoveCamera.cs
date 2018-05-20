using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Camera))]
public class MoveCamera : MonoBehaviour {

    private Vector3 mousePos;
    public float speed = 6f;
    // Use this for initialization
    public Vector3 destination;
	void Start () {
        destination = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            mousePos = Input.mousePosition;
            Vector2 worldPoint  = Camera.main.ScreenToWorldPoint(mousePos);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
            destination = new Vector3(hit.point.x, hit.point.y, transform.position.z);
            return;
        }
        if (transform.position != destination)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
        }
    }
}
