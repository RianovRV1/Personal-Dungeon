using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Camera))]
public class MouseCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    float mDelta = 10f; // Pixels. The width border at the edge in which the movement work
    float mSpeed = 3.0f; // Scale. Speed of the movement

    private Vector3 mRightDirection = Vector3.right; // Direction the camera should move when on the right edge
    private Vector3 mUp = Vector3.up;
    void Update()
    {
        // Check if on the right edge
        if (Input.mousePosition.x >= Screen.width - mDelta)
        {
            // Move the camera
            transform.position += mRightDirection * Time.deltaTime * mSpeed;
        }
        if(Input.mousePosition.x <= 0 + mDelta)
        {
            // Move the camera
            transform.position -= mRightDirection * Time.deltaTime * mSpeed;
        }
        if (Input.mousePosition.y >= Screen.height - mDelta)
        {
            transform.position += mUp * Time.deltaTime * mSpeed;
        }
        if (Input.mousePosition.y <= 0 + mDelta)
        {
            transform.position -= mUp * Time.deltaTime * mSpeed;
        }
    }
}
