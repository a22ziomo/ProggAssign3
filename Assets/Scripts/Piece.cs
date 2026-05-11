using System;
using System.Collections;
using System.Collections.Generic;
using Grid;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
[RequireComponent(typeof(GridTile))]
public class Piece : MonoBehaviour
{
    public GridTile state;
    public int extraMoveCost = 0;
    public int G;
    public int H;
    public int F => G + H;
    public Vector2 Parent;
    public Vector2Int PortalExitCoord;

    private Material material;
    private TextMeshPro textMesh;
    private int maxSteps;

    private void Start()
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        
        material = Instantiate(renderer.sharedMaterial);
        renderer.sharedMaterial = material;
        maxSteps = GetComponentInParent<Findpather>().maximumSteps;
        textMesh = gameObject.GetComponentInChildren<TextMeshPro>();
    }

    private void Update()
    {
        var tile = GetComponent<GridTile>();
        if (tile.GetProperty(TileProperty.Blocked))
        {
            material.color = Color.red;
        }
        else if (state.GetProperty(TileProperty.Hinder))
        {
            material.color = Color.yellow;
        }
        else
        {
            material.color = Color.green;
        }

        if (maxSteps >= G && G > 0)
        {

            textMesh.text = G.ToString();
        }
        else
        {
            textMesh.text = " ";
        }
        
    }

    private void OnDrawGizmos()
    {
        var tile = GetComponent<GridTile>();
        /*
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
        */
        

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
            Gizmos.DrawSphere(transform.position+new Vector3(0,1.5f,0), 0.25f);
            
            
        }
        
        if (tile.GetNeighbourProperty(0, TileProperty.Blocked))
        {
            Gizmos.color = Color.yellow;
        }
    }
    
}
