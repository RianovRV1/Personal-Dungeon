using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid2D : MonoBehaviour {

    public LayerMask wallMask;
    public Vector2 gridWorldSize;
    public float nodeRadius; //node size
    public float Distance; //distance between 


    Node[,] grid;
    public List<Node> FinalPath;

    float nodeDiameter;
    int gridSizeX, gridSizeY;
    // Use this for initialization
    private void Start()
    {
        Distance = Mathf.Clamp(Distance, 0.1f, 100f);
        nodeRadius =  Mathf.Clamp(nodeRadius, 0.5f, 100f);
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
        Vector3 bottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2; //Math to take the center position then find the bottomleft location of the grid
        for (int y = 0; y < gridSizeY; y++)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius); //math that starts at the bottomleft then adds vectors to get the nodes current location
                bool Wall = false;
                bool wallColCheck = Physics.CheckSphere(worldPoint, nodeRadius, wallMask);
                if (wallColCheck)
                {
                    Wall = true;
                }
                Debug.Log("X: " + x + " Y: " + y);
                grid[x, y] = new Node(Wall, worldPoint, x, y);
            }
        }
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

    internal List<Node> GetNeighboringNodes(Node startNode)
    {
        List<Node> neighboringNodes = new List<Node>();
        int xCheck, yCheck;

        //Right Side Checking
        xCheck = startNode.xPos + 1;
        yCheck = startNode.yPos;
        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                neighboringNodes.Add(grid[xCheck, yCheck]);
            }
        }

        // Diagonal Right up 
        xCheck = startNode.xPos + 1;
        yCheck = startNode.yPos + 1;
        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                neighboringNodes.Add(grid[xCheck, yCheck]);
            }
        }
        // Diagonal Right down 
        xCheck = startNode.xPos + 1;
        yCheck = startNode.yPos - 1;
        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                neighboringNodes.Add(grid[xCheck, yCheck]);
            }
        }
        //left Side Checking
        xCheck = startNode.xPos - 1;
        yCheck = startNode.yPos;
        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                neighboringNodes.Add(grid[xCheck, yCheck]);
            }
        }
        // Diagonal left down Side Checking
        xCheck = startNode.xPos - 1;
        yCheck = startNode.yPos - 1;
        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                neighboringNodes.Add(grid[xCheck, yCheck]);
            }
        }
        // Diagonal left up Side Checking
        xCheck = startNode.xPos - 1;
        yCheck = startNode.yPos + 1;
        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                neighboringNodes.Add(grid[xCheck, yCheck]);
            }
        }
        //top Side Checking
        xCheck = startNode.xPos;
        yCheck = startNode.yPos + 1;
        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                neighboringNodes.Add(grid[xCheck, yCheck]);
            }
        }

        //Bottom Side Checking
        xCheck = startNode.xPos;
        yCheck = startNode.yPos - 1;
        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                neighboringNodes.Add(grid[xCheck, yCheck]);
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
                    Gizmos.color = Color.white; // color our wall white
                }
                else
                {
                    Gizmos.color = Color.yellow; // color the floor yellow.
                }
                if (FinalPath != null)
                {
                    if (FinalPath.Contains(node))
                        Gizmos.color = Color.red; //color our node path
                }

                Gizmos.DrawCube(node.Position, Vector3.one * (nodeDiameter / 2 - Distance)); //Draw our node
            }
        }
    }
}
