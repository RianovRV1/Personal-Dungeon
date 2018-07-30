using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
/// <summary>
/// Monobehavior that calculates weights on nodes in a 2D grid in Unity 2D setting
/// Key bit for weight calculation
/// </summary>
public class A_Star2D : MonoBehaviour {

    //algorithm A* for pathfinding
    // Use this for initialization
    Grid2D grid;
    
    public Transform endPosition;
    private Vector3 previousEndPosition;
    public static List<FollowPath> movingEntitys = new List<FollowPath>();
    private bool calculated = false;
    public int nodeCheck = 0;
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

    void FindPath(FollowPath startEntity, Vector3 end) // start Vec3 is extracted from startEntity
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();
        Vector3 start = startEntity.transform.position;
        Node startNode = grid.NodeFromWorldPosition(start);
        Node targetNode = grid.NodeFromWorldPosition(end);



        Heap<Node> OpenList = new Heap<Node>(grid.totalNodeCount);
        HashSet<Node> ClosedList = new HashSet<Node>();
        List<Node> CheckedNeighbors = new List<Node>();
        OpenList.Add(startNode);
        
        while (OpenList.Count > 0 && startEntity.followPath == null)
        {
            Node CurrentNode = OpenList.RemoveFirst();
            ClosedList.Add(CurrentNode);

            if (CurrentNode == targetNode)
            {
                sw.Stop();
                UnityEngine.Debug.Log("Path was found in " + sw.ElapsedMilliseconds + " Milliseconds");
                GetFinalPath(startNode, targetNode, startEntity, CheckedNeighbors);
                return;
            }

            foreach (Node Neighbor in grid.GetNeighboringNodes(CurrentNode))
            {
                if (!CheckedNeighbors.Contains(Neighbor))
                    CheckedNeighbors.Add(Neighbor);
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
                    OpenList.UpdateItem(Neighbor);
                }
            }
        }
    }

    public static void AddEntity(FollowPath entity)
    {
        movingEntitys.Add(entity);
    }

    void GetFinalPath(Node start, Node end, FollowPath entity, List<Node> visitedNodes)
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
        grid.visitedNodes = visitedNodes;
        nodeCheck = visitedNodes.Count;
        entity.canMove = true;
        entity.followPath = FinalPath;
        calculated = true;
        UnityEngine.Debug.Log(string.Format("Calculated a path. Path Node Count {0}, Visited Node Count: {1}, grid size: {2}, grid floor tile count: {3}", FinalPath.Count, visitedNodes.Count, grid.totalNodeCount, grid.pathableNodes));
    }

    int GetManHattenDistance(Node nodeA, Node nodeB)
    {
        int x = Mathf.Abs(nodeA.xPos - nodeB.xPos);
        int y = Mathf.Abs(nodeA.yPos - nodeB.yPos);
        if (x > y)
            return 14 * y + 10 * (x - y);
        return 14 * x + 10 * (y - x);
    }
}
