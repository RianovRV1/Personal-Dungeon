using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Camera))]
public class CameraWithBoundry : MonoBehaviour {

    private Vector3 mouseStart;
    private Vector3 mouseMove;

    private GameObject map;

    private float dist;
    private float minX;
    private float maxX;
    private float minY;
    private float maxY;

    private void Start()
    {
        // Get map size and position
        map = GameObject.FindWithTag("Map"); ;
        float mapX = map.GetComponent<Renderer>().bounds.size.x;
        float mapY = map.GetComponent<Renderer>().bounds.size.y;
        Vector3 mapPos = map.transform.position;

        // Get Camera info
        dist = transform.position.z;
        float limitY = Camera.main.GetComponent<Camera>().orthographicSize;
        float limitX = limitY * Screen.width / Screen.height;

        // Calculate camera limits
        minX = limitX + mapPos.x - mapX / 2;
        maxX = mapX / 2 - limitX + mapPos.x;
        minY = limitY + mapPos.y - mapY / 2;
        maxY = mapY / 2 - limitY + mapPos.y;
    }

    private void Update()
    {
        // Move Camera
        if (Input.GetMouseButtonDown(0))
        {
            mouseStart = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dist);
        }
        else if (Input.GetMouseButton(0))
        {
            mouseMove = new Vector3(Input.mousePosition.x - mouseStart.x, Input.mousePosition.y - mouseStart.y, dist);
            mouseStart = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dist);
            transform.position = new Vector3(transform.position.x - mouseMove.x * Time.deltaTime, transform.position.y - mouseMove.y * Time.deltaTime, dist);
        }

        // Clamp camera
        Vector3 camClamp = transform.position;
        camClamp.x = Mathf.Clamp(camClamp.x, minX, maxX);
        camClamp.y = Mathf.Clamp(camClamp.y, minY, maxY);
        transform.position = camClamp;
    }
}
