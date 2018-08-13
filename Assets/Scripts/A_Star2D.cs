using System.Collections;
using System;
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
    PathRequestManager requestManager;
    Grid2D grid;
    public bool weightGraph = false;
    private void Awake()
    {
        grid = GetComponent<Grid2D>();
        requestManager = GetComponent<PathRequestManager>();
        //works with only one, tags might work better
    }

    // Update is called once per frame
  
    internal void StartFindPath(Vector3 startPos, Vector3 endPos)
    {
        StartCoroutine(FindPath(startPos, endPos));
    }
    IEnumerator FindPath(Vector3 start, Vector3 end) // start Vec3 is extracted from startEntity
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        Vector3[] waypoints = new Vector3[0];
        bool pathFound = false;

        Node startNode = grid.NodeFromWorldPosition(start);
        Node targetNode = grid.NodeFromWorldPosition(end);

        List<Node> CheckedNeighbors = new List<Node>();
        if (!targetNode.isWall)
        {
            Heap<Node> OpenList = new Heap<Node>(grid.totalNodeCount);
            HashSet<Node> ClosedList = new HashSet<Node>();
            CheckedNeighbors = new List<Node>();
            OpenList.Add(startNode);

            while (OpenList.Count > 0)
            {
                Node CurrentNode = OpenList.RemoveFirst();
                ClosedList.Add(CurrentNode);

                if (CurrentNode == targetNode)
                {
                    sw.Stop();
                    UnityEngine.Debug.Log("Path was found in " + sw.ElapsedMilliseconds + " Milliseconds");
                    pathFound = true;
                    break;
                }

                foreach (Node Neighbor in grid.GetNeighboringNodes(CurrentNode))
                {
                    if (!CheckedNeighbors.Contains(Neighbor))
                        CheckedNeighbors.Add(Neighbor);
                    if (Neighbor.isWall || ClosedList.Contains(Neighbor))
                        continue;
                    int moveCost = CurrentNode.gCost + GetManHattenDistance(CurrentNode, Neighbor) + (weightGraph ? Neighbor.weightPenalty : 0);

                    if (moveCost < Neighbor.gCost || !OpenList.Contains(Neighbor))
                    {
                        Neighbor.gCost = moveCost;
                        Neighbor.hCost = GetManHattenDistance(Neighbor, targetNode);
                        Neighbor.Parent = CurrentNode;
                        if (!OpenList.Contains(Neighbor))
                            OpenList.Add(Neighbor);
                        else
                            OpenList.UpdateItem(Neighbor);
                    }
                }
            }
        }
        yield return null;
        if (pathFound)
        {
            waypoints = GetFinalPath(startNode, targetNode, CheckedNeighbors);
        }
        requestManager.FinishedProcessingPath(waypoints, pathFound);
    }

    Vector3[] GetFinalPath(Node start, Node end, List<Node> visitedNodes)
    {
        List<Node> FinalPath = new List<Node>();
        Node CurrentNode = end;
        while (CurrentNode != start)
        {
            FinalPath.Add(CurrentNode);
            CurrentNode = CurrentNode.Parent;
        }
        Vector3[] waypoints = SimplifyPath(FinalPath);
        Array.Reverse(waypoints);
        //grid.FinalPath = FinalPath; //consider removing all these things
        //grid.visitedNodes = visitedNodes;
        UnityEngine.Debug.Log(string.Format("Calculated a path. Path Node Count {0}, Visited Node Count: {1}, grid size: {2}, grid floor tile count: {3}", FinalPath.Count, visitedNodes.Count, grid.totalNodeCount, grid.pathableNodes));
        return waypoints;
    }

    Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 oldDirection = Vector2.zero;
        for(int i = 1; i < path.Count; i++)
        {
            Vector2 newDirection = new Vector2(path[i - 1].xPos - path[i].xPos, path[i - 1].yPos - path[i].yPos);
            if(newDirection != oldDirection)
            {
                waypoints.Add(path[i].Position);
            }
            oldDirection = newDirection;
        }
        return waypoints.ToArray();
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
