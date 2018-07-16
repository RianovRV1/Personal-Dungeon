using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid2D : MonoBehaviour {

    public LayerMask wallMask;
    public Vector2 gridWorldSize;
    public float nodeRadius; //node size
    public float Distance; //distance between 
    public int searchRadius = 1;
    internal List<Node> visitedNodes;
    Node[,] grid;
    public List<Node> FinalPath;
    public bool drawPath = false;
    public bool drawVisited = false;
    float nodeDiameter;
    int gridSizeX, gridSizeY, wallCount;
    internal int pathableNodes, totalNodeCount;
    // Use this for initialization
    private void Start()
    {
        Distance = Mathf.Clamp(Distance, 0f, 100f);
        nodeRadius =  Mathf.Clamp(nodeRadius, 0.001f, 100f);
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        GenerateGrid();
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    void GenerateGrid()//user made function to initialize grid
    {
        grid = new Node[gridSizeX, gridSizeY];
        totalNodeCount = (gridSizeX * gridSizeY);
        Vector3 bottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2; //Math to take the center position then find the bottomleft location of the grid
        for (int y = 0; y < gridSizeY; y++)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius); //math that starts at the bottomleft then adds vectors to get the nodes current location
                bool Wall = false;
                bool wallColCheck = Physics2D.OverlapCircle(worldPoint, nodeRadius, wallMask);
                if (wallColCheck)
                {
                    Wall = true;
                    wallCount += 1;
                }
                grid[x, y] = new Node(Wall, worldPoint, x, y);
            }
        }
        pathableNodes = totalNodeCount - wallCount;
    }
    internal Node NodeFromWorldPosition(Vector3 worldPosition)
    {
        float xpoint = ((worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x); // gets x position of grid node
        float ypoint = ((worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y); // gets y position of grid node
        xpoint = Mathf.Clamp01(xpoint); // clamps it to prevent out of range errors
        ypoint = Mathf.Clamp01(ypoint); // clamps it to prevent out of range errors
        int x = Mathf.RoundToInt((gridSizeX - 1) * xpoint); // offsets it by 1 then uses X point poisition to get correct ARRAY INDEX
        int y = Mathf.RoundToInt((gridSizeY - 1) * ypoint); // offsets it by 1 then uses Y point poisition to get correct ARRAY INDEX

        return grid[x, y];
    }

    private void AddValidNeighbor(List<Node> inList, int inX, int inY) //helper function to reduce a few lines
    {
        if (inX >= 0 && inX < gridSizeX)
        {
            if (inY >= 0 && inY < gridSizeY)
            {
                inList.Add(grid[inX, inY]);
            }
        }
    }

    internal List<Node> GetNeighboringNodes(Node startNode)
    {
        if (searchRadius < 1) //prevent for loop from not running at least once.
            searchRadius = 1;

        List<Node> neighboringNodes = new List<Node>();//return list
        int xCheck, yCheck;// variables to hold offsets.
        
        for(int i = 1; i <= searchRadius; i++)
        {
            for (int j = 1; j <= searchRadius; j++)
            {
                xCheck = startNode.xPos + i;
                yCheck = startNode.yPos;
                AddValidNeighbor(neighboringNodes, xCheck, yCheck);
               
                xCheck = startNode.xPos + i;
                yCheck = startNode.yPos + j;
                AddValidNeighbor(neighboringNodes, xCheck, yCheck);
                

                xCheck = startNode.xPos + i;
                yCheck = startNode.yPos - j;
                AddValidNeighbor(neighboringNodes, xCheck, yCheck);

                xCheck = startNode.xPos - i;
                yCheck = startNode.yPos;
                AddValidNeighbor(neighboringNodes, xCheck, yCheck);

                xCheck = startNode.xPos - i;
                yCheck = startNode.yPos - j;
                AddValidNeighbor(neighboringNodes, xCheck, yCheck);

                xCheck = startNode.xPos - i;
                yCheck = startNode.yPos + j;
                AddValidNeighbor(neighboringNodes, xCheck, yCheck);

                xCheck = startNode.xPos;
                yCheck = startNode.yPos + j;
                AddValidNeighbor(neighboringNodes, xCheck, yCheck);

                xCheck = startNode.xPos;
                yCheck = startNode.yPos - j;
                AddValidNeighbor(neighboringNodes, xCheck, yCheck);
            }
        }
        return neighboringNodes;
    }
    private void OnDrawGizmos()//this is for scene debugging to make sure our grid comes out the way we expect.
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 1));

        if (grid != null)
        {

            foreach (Node node in grid)
            {
                if (node.isWall)
                {
                    Gizmos.color = Color.black; // color our wall white
                }
                else
                {
                    Gizmos.color = Color.white; // color the floor yellow.
                }

                if (visitedNodes != null && drawVisited)
                {
                    if(visitedNodes.Contains(node))
                        Gizmos.color = Color.red;
                }

                if (FinalPath != null && drawPath)
                {
                    if (FinalPath.Contains(node))
                        Gizmos.color = Color.green; //color our node path
                }

                Gizmos.DrawCube(node.Position, Vector3.one * (nodeDiameter / 2 - Distance)); //Draw our node
            }
        }
    }
}
