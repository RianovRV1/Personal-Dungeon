using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour {
    internal List<Node> followPath;
    internal Node goToNode;
    private int index = 0;
    public float speed = 10f;
    internal bool canMove = false;
    // Use this for initialization
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
        A_Star2D.AddEntity(this);
    }
    internal void SetNull()
    {
        canMove = false;
        goToNode = null;
        followPath = null;
        index = 0;
    }
}
