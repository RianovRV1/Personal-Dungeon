using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Star : MonoBehaviour {
    //algorithm A* for pathfinding
    // Use this for initialization
    Grid grid;
    public Transform startPosition;
    public Transform endPosition;
    private Vector3 previousStartPosition;
    private Vector3 previousEndPosition;
    public FollowPath movingEntity;
    private void Awake()
    {
        grid = GetComponent<Grid>();
        movingEntity = FindObjectsOfType<FollowPath>()[0];
    }

    void Start () {
        
        previousStartPosition = startPosition.position;
        previousEndPosition = endPosition.position;
    }
	
	// Update is called once per frame
	private void Update ()
    {
        FindPath(startPosition.position, endPosition.position);
        if (previousStartPosition != startPosition.position || previousEndPosition != endPosition.position)
        {
            movingEntity.canMove = false;
        }
	}

    void FindPath(Vector3 start, Vector3 end)
    {
        Node startNode = grid.NodeFromWorldPosition(start);
        Node targetNode = grid.NodeFromWorldPosition(end);

        List<Node> OpenList = new List<Node>();
        HashSet<Node> ClosedList = new HashSet<Node>();

        OpenList.Add(startNode);

        while (OpenList.Count > 0)
        {
            Node CurrentNode = OpenList[0];
            for(int i = 1; i < OpenList.Count; i++)
            {
                if (OpenList[i].FCost < CurrentNode.FCost || OpenList[i].FCost == CurrentNode.FCost && OpenList[i].hCost < CurrentNode.hCost)
                {
                    CurrentNode = OpenList[i];
                }
            }
            OpenList.Remove(CurrentNode);
            ClosedList.Add(CurrentNode);

            if(CurrentNode == targetNode)
            {
                GetFinalPath(startNode, targetNode);
            }

            foreach(Node Neighbor in grid.GetNeighboringNodes(CurrentNode))
            {
                if (Neighbor.isWall || ClosedList.Contains(Neighbor))
                    continue;
                int moveCost = CurrentNode.gCost + GetManHattenDistance(CurrentNode,Neighbor);

                if (moveCost < Neighbor.gCost || !OpenList.Contains(Neighbor))
                {
                    Neighbor.gCost = moveCost;
                    Neighbor.hCost = GetManHattenDistance(Neighbor, targetNode);
                    Neighbor.Parent = CurrentNode;
                    if(!OpenList.Contains(Neighbor))
                        OpenList.Add(Neighbor);
                }
            }
        }
    }

    void GetFinalPath(Node start, Node end)
    {
        List<Node> FinalPath = new List<Node>();
        Node CurrentNode = end;
        while (CurrentNode != start)
        {
            FinalPath.Add(CurrentNode);
            CurrentNode = CurrentNode.Parent;
        }

        FinalPath.Reverse();
        previousStartPosition = startPosition.position;
        previousEndPosition = endPosition.position;
        grid.FinalPath = FinalPath;
        movingEntity.canMove = true;
        movingEntity.followPath = FinalPath;
    }

    int GetManHattenDistance(Node nodeA, Node nodeB)
    {
        int x = Mathf.Abs(nodeA.xPos - nodeB.xPos);
        int y = Mathf.Abs(nodeA.yPos - nodeB.yPos);
        return x + y;
    }
}
