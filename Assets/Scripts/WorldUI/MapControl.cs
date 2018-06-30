using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapControl : MonoBehaviour
{
    private Vector3 mouseStart;
    private Vector3 mouseMove;
    private Vector3 mapPos;

    [SerializeField] private GameObject map;
    private Camera cam;

    private float minX;
    private float maxX;
    private float minY;
    private float maxY;
    private float mapX;
    private float mapY;
    private float limitX;
    private float limitY;
    private float camSize;

    void Awake()
    {
        // Get map size and position
        //map = GameObject.FindWithTag("Map");
        mapX = map.GetComponent<Renderer>().bounds.size.x;
        mapY = map.GetComponent<Renderer>().bounds.size.y;
        mapPos = map.transform.position;

        // Get Camera info
        cam = Camera.main;
        camSize = cam.orthographicSize;
        NewCamLimit();
    }

    void Update()
    {
        // Move Camera
        if (Input.GetMouseButtonDown(0))
        {
            mouseStart = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            mouseMove = Input.mousePosition;
            Vector3 camMove = cam.transform.position + cam.ScreenToWorldPoint(mouseStart) - cam.ScreenToWorldPoint(mouseMove);
            cam.transform.position = new Vector3(camMove.x, camMove.y, cam.transform.position.z);
            mouseStart = mouseMove;
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            camSize--;
            camSize = Mathf.Clamp(camSize, 2, 11);
            cam.orthographicSize = camSize;
            NewCamLimit();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            camSize++;
            camSize = Mathf.Clamp(camSize, 2, 11);
            cam.orthographicSize = camSize;
            NewCamLimit();
        }

        // Clamp camera
        Vector3 camClamp = cam.transform.position;
        camClamp.x = Mathf.Clamp(camClamp.x, minX, maxX);
        camClamp.y = Mathf.Clamp(camClamp.y, minY, maxY);
        cam.transform.position = camClamp;
    }

    void NewCamLimit()
    {
        // Calculate camera limits
        limitY = cam.GetComponent<Camera>().orthographicSize;
        limitX = limitY * Screen.width / Screen.height;
                
        minX = limitX + mapPos.x - mapX / 2;
        maxX = mapX / 2 - limitX + mapPos.x;
        minY = limitY + mapPos.y - mapY / 2;
        maxY = mapY / 2 - limitY + mapPos.y;
    }

    //void OnGUI()
    //{
    //    Vector3 p = new Vector3();
    //    Vector3 cx = cam.transform.position;
    //    Vector2 mousePos = new Vector2(Input.mousePosition.x, cam.pixelHeight - Input.mousePosition.y);

    //    p = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane));

    //    GUILayout.BeginArea(new Rect(20, 20, 250, 120));
    //    GUILayout.Label("Screen pixels: " + cam.pixelWidth + ":" + cam.pixelHeight);
    //    GUILayout.Label("Mouse position: " + mousePos);
    //    GUILayout.Label("World position: " + p.ToString("F3"));
    //    GUILayout.Label("Camera position: " + cx.ToString("F3"));
    //    GUILayout.EndArea();
    //}
}


