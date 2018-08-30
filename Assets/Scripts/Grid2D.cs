using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 2D grid for node generation accross a 2D plane, specfically implemented for Unity 2D accounting for X and Y 
/// </summary>
public class Grid2D : MonoBehaviour {

    public LayerMask wallMask;
    public Vector2 gridWorldSize;
    public float nodeRadius; //node size
    public float Distance; //distance between 
    int searchRadius = 1;
    public TerrianType[] walkableRegions;
    LayerMask walkableMask;
    Dictionary<int, int> walkableDict = new Dictionary<int, int>();
    public bool blurredGraph = true;
    public int blurRadius = 3;
    Node[,] grid;
    public bool drawGizmos = true;
    float nodeDiameter;
    int minPenalty = int.MaxValue;
    int maxPenalty = int.MinValue;
    int gridSizeX, gridSizeY, wallCount;
    internal int pathableNodes, totalNodeCount;
    // Use this for initialization
    private void Awake()
    {
        Distance = Mathf.Clamp(Distance, 0f, 100f);
        nodeRadius =  Mathf.Clamp(nodeRadius, 0.001f, 100f);
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        foreach (TerrianType region in walkableRegions)
        {
            walkableMask.value += region.terrianMask.value;
            walkableDict.Add((int)Mathf.Log(region.terrianMask.value, 2f), region.terrainPenalty);
        }

        GenerateGrid();
        
    }

    // Update is called once per frame
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

                int weightPenalty = 0;

                // raycast for weight
                if (!wallColCheck || blurredGraph)
                {
                    RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero, Mathf.Infinity, walkableMask);
                    if(hit)
                    {
                        walkableDict.TryGetValue(hit.collider.gameObject.layer, out weightPenalty);
                    }
                    
                }

                grid[x, y] = new Node(Wall, worldPoint, x, y, weightPenalty);
            }
        }
        pathableNodes = totalNodeCount - wallCount;
        if(blurredGraph)
            BlurGridWeights(blurRadius);
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

    void BlurGridWeights(int blursize)
    {
        int kernelSize = (blursize*2) + 1;
        //int kernelExtents = (kernelSize - 1 ) / 2;
        int kernelExtents = blursize;

        int[,] horizontalBlur = new int[gridSizeX, gridSizeY];
        int[,] veritcalBlur = new int[gridSizeX, gridSizeY];

        for(int y = 0; y < gridSizeY; y++)
        {
            for(int x = -kernelExtents; x <= kernelExtents; x++)
            {
                int sampleX = Mathf.Clamp(x, 0, kernelExtents);
                horizontalBlur[0, y] += grid[sampleX, y].weightPenalty;
            }
            for(int x = 1; x < gridSizeX; x++)
            {
                int removeIndex = Mathf.Clamp(x - kernelExtents - 1, 0, gridSizeX);
                int addIndex = Mathf.Clamp(x + kernelExtents, 0, gridSizeX - 1);

                horizontalBlur[x, y] = horizontalBlur[x - 1, y] - grid[removeIndex, y].weightPenalty + grid[addIndex, y].weightPenalty;
            }
        }
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = -kernelExtents; y <= kernelExtents; y++)
            {
                int sampleY = Mathf.Clamp(y, 0, kernelExtents);
                veritcalBlur[x, 0] += horizontalBlur[x, sampleY];
            }
            for (int y = 1; y < gridSizeY; y++)
            {
                int removeIndex = Mathf.Clamp(y - kernelExtents - 1, 0, gridSizeY);
                int addIndex = Mathf.Clamp(y + kernelExtents, 0, gridSizeY - 1);

                veritcalBlur[x, y] = veritcalBlur[x, y - 1] - horizontalBlur[x, removeIndex] + horizontalBlur[x, addIndex];
                int blurredResult = Mathf.RoundToInt((float)veritcalBlur[x, y] / (kernelSize * kernelSize));
                grid[x, y].weightPenalty = blurredResult;

                if (blurredResult > maxPenalty)
                    maxPenalty = blurredResult;
                if (blurredResult < minPenalty)
                    minPenalty = blurredResult;
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
        if (drawGizmos)
        {
            if (grid != null)
            {
                foreach (Node node in grid)
                {
                    Gizmos.color = Color.Lerp(Color.white, Color.black, Mathf.InverseLerp(minPenalty, maxPenalty, node.weightPenalty));
                    if (node.isWall)
                    {
                        Gizmos.color = Color.red; // color our wall white
                    }
                    else
                    {
                        Gizmos.color = Gizmos.color; // color the floor yellow.
                    }
                    
                    Gizmos.DrawCube(node.Position, Vector3.one * (nodeDiameter / 2 - Distance)); //Draw our node
                }
            }
        }
    }

    [System.Serializable]
    public class TerrianType
    {
        public LayerMask terrianMask;
        public int terrainPenalty;
    }
}

