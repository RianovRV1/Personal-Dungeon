using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : System.IEquatable<Node>
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

    public override string ToString() // for debug printing the node object
    {
        string returnString = "World Position: X: " + Position.x + " Y: " + Position.y + " Z: " + Position.z + "\n Is a wall: " + isWall + " NodeX: " + xPos + " NodeY: " + yPos;
        if (Parent != null)
            returnString += " Has Parent";
        return returnString;
    }

    //Equality code to ensure .Contains in lists actually works as intended

    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;
        var other = obj as Node;
        if ((System.Object)other == null) return false;

        return (other.xPos == this.xPos) && (other.yPos == this.yPos) && (other.isWall == this.isWall) && (other.Parent == this.Parent) && (other.Position == this.Position);
    }
    public bool Equals(Node other)
    {
        if ((object)other == null) return false;

        return (other.xPos == this.xPos) && (other.yPos == this.yPos) && (other.isWall == this.isWall) && (other.Parent == this.Parent) && (other.Position == this.Position);
    }
    public override int GetHashCode()
    {
        return xPos ^ yPos ^ (int)Position.x ^ (int)Position.y ^ (int)Position.z;
    }
    public static bool operator ==(Node a, Node b)
    {
        // If both are null, or both are same instance, return true.
        if (System.Object.ReferenceEquals(a, b))
        {
            return true;
        }

        // If one is null, but not both, return false.
        if (((object)a == null) || ((object)b == null))
        {
            return false;
        }

        // Return true if the fields match:
        return (a.xPos == b.xPos) && (a.yPos == b.yPos) && (a.isWall == b.isWall) && (a.Parent == b.Parent) && (a.Position == b.Position);
    }

    public static bool operator !=(Node a, Node b)
    {
        return !(a == b);
    }
}

