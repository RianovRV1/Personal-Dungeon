using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Star2D : MonoBehaviour {

    //algorithm A* for pathfinding
    // Use this for initialization
    Grid2D grid;
    public Transform endPosition;
    private Vector3 previousEndPosition;
    public static List<FollowPath> movingEntitys = new List<FollowPath>();
    private bool calculated = false;
    private void Awake()
    {
        grid = GetComponent<Grid2D>();
        //works with only one, tags might work better
    }

    void Start()
    {
        previousEndPosition = endPosition.position;
    }

    // Update is called once per frame
    private void Update()
    {
        if (previousEndPosition != endPosition.position)
        {
            foreach (FollowPath entity in movingEntitys)
            {
                entity.SetNull();

            }
            calculated = false;
        }
        if (!calculated)
        {
            foreach (FollowPath entity in movingEntitys)
                FindPath(entity, endPosition.position);
        }

    }

    void FindPath(FollowPath startEntity, Vector3 end)
    {
        Vector3 start = startEntity.transform.position;
        Node startNode = grid.NodeFromWorldPosition(start);
        Node targetNode = grid.NodeFromWorldPosition(end);

        List<Node> OpenList = new List<Node>();
        HashSet<Node> ClosedList = new HashSet<Node>();

        OpenList.Add(startNode);

        while (OpenList.Count > 0)
        {
            Node CurrentNode = OpenList[0];
            for (int i = 1; i < OpenList.Count; i++)
            {
                if (OpenList[i].FCost < CurrentNode.FCost || OpenList[i].FCost == CurrentNode.FCost && OpenList[i].hCost < CurrentNode.hCost)
                {
                    CurrentNode = OpenList[i];
                }
            }
            OpenList.Remove(CurrentNode);
            ClosedList.Add(CurrentNode);

            if (CurrentNode == targetNode)
            {
                GetFinalPath(startNode, targetNode, startEntity);
            }

            foreach (Node Neighbor in grid.GetNeighboringNodes(CurrentNode))
            {
                if (Neighbor.isWall || ClosedList.Contains(Neighbor))
                    continue;
                int moveCost = CurrentNode.gCost + GetManHattenDistance(CurrentNode, Neighbor);

                if (moveCost < Neighbor.gCost || !OpenList.Contains(Neighbor))
                {
                    Neighbor.gCost = moveCost;
                    Neighbor.hCost = GetManHattenDistance(Neighbor, targetNode);
                    Neighbor.Parent = CurrentNode;
                    if (!OpenList.Contains(Neighbor))
                        OpenList.Add(Neighbor);
                }
            }
        }
    }

    public static void AddEntity(FollowPath entity)
    {
        movingEntitys.Add(entity);
    }

    void GetFinalPath(Node start, Node end, FollowPath entity)
    {
        List<Node> FinalPath = new List<Node>();
        Node CurrentNode = end;
        while (CurrentNode != start)
        {
            FinalPath.Add(CurrentNode);
            CurrentNode = CurrentNode.Parent;
        }

        FinalPath.Reverse();
        previousEndPosition = endPosition.position;
        grid.FinalPath = FinalPath;
        entity.canMove = true;
        entity.followPath = FinalPath;
        calculated = true;
    }

    int GetManHattenDistance(Node nodeA, Node nodeB)
    {
        int x = Mathf.Abs(nodeA.xPos - nodeB.xPos);
        int y = Mathf.Abs(nodeA.yPos - nodeB.yPos);
        return x + y;
    }
}
