using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    //A node for path finding containing, distance from start, distance from end, and the sum of START + END
    public int xPos; //x of node
    public int yPos; //y of node

    public bool isWall;//If node has wall object
    public Vector3 Position;//In world location of node

    public Node Parent;//Allows backtracking to return node it came from for path generation A*

    public int gCost;//Cost of moving to this square
    public int hCost;//Cost of this square until destination

    public int FCost { get { return gCost + hCost; } } //no set needed to edit this. Total weight of node

    public Node(bool wall, Vector3 in_Pos, int x, int y)//Class constructor
    {
        isWall = wall;
        Position = in_Pos;
        xPos = x;
        yPos = y;
    }

    public override string ToString()
    {
        return "World Position: X: " + Position.x + " Y: " + Position.y + " Z: " + Position.z + "\n";
    }
}

