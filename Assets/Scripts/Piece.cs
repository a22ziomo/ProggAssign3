using System;
using System.Collections;
using System.Collections.Generic;
using Grid;
using UnityEngine;


[RequireComponent(typeof(GridTile))]
public class Piece : MonoBehaviour
{
    public GridTile state;
    public int G;
    public int H;
    public int F => G + H;
    public Vector2 Parent;
    public Vector2Int PortalExitCoord;
    
    private void OnDrawGizmos()
    {
        var tile = GetComponent<GridTile>();
        if (tile.GetProperty(TileProperty.Blocked))
        {
            Gizmos.color = Color.red;
        }
        else if (state.GetProperty(TileProperty.Hinder))
        {
            Gizmos.color = Color.yellow;
        }
        else
        {
            Gizmos.color = Color.green;
        }
        Gizmos.DrawCube(transform.position, new Vector3(1, 0.1f, 1));


        if (tile.GetProperty(TileProperty.Portal))
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position+Vector3.up, 0.5f);
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(new Vector3(PortalExitCoord.x, 1, PortalExitCoord.y), 0.5f);
            
        }

        if (tile.GetProperty(TileProperty.Goal))
        {
            Gizmos.color = Color.white;
            Gizmos.DrawSphere(transform.position+new Vector3(0,0.5f,0), 0.25f);
            
            
        }
        
        if (tile.GetNeighbourProperty(0, TileProperty.Blocked))
        {
            Gizmos.color = Color.yellow;
        }
    }
    
}
