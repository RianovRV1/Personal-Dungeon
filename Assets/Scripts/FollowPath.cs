using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour {
    internal List<Node> followPath;
    internal Node goToNode;
    private int index = 0;
    public float speed = 10f;
    public bool canMove = false;
    // Use this for initialization
    private void Awake()
    {
        
    }
    
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        if (goToNode != null && canMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, goToNode.Position, speed * Time.deltaTime);
        }
    }
    private void LateUpdate()
    {
        
        if (followPath != null && canMove)
        { 
            if (goToNode == null && followPath.Count > 0) 
            {
                goToNode = followPath[index];
                Debug.Log(goToNode);
            }
            if(goToNode != null)
                if (transform.position == goToNode.Position)
                {
                    if (index < followPath.Count - 1)
                    {
                        index += 1;
                        goToNode = followPath[index];
                    }
                }
        }
    }

    private void OnEnable()
    {
        A_Star.AddEntity(this);
    }
    internal void SetNull()
    {
        canMove = false;
        goToNode = null;
        followPath = null;
        index = 0;
    }
}
