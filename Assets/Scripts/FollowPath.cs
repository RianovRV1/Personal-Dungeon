using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour {
    public List<Node> followPath;
    public Node goToNode;
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
            if (goToNode == null) //problematic on recalculation 
            {
                goToNode = followPath[index];
            }
            
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
}
