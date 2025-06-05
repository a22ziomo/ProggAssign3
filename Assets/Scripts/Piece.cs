using System;
using System.Collections;
using System.Collections.Generic;
using Grid;
using UnityEngine;


[RequireComponent(typeof(GridTile))]
public class Piece : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        var tile = GetComponent<GridTile>();
        if (tile.GetProperty(GridTileProperty.Solid))
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.green;
        }

        Gizmos.DrawCube(transform.position, new Vector3(1, 0.1f, 1));
        if (tile.GetNeighbourProperty(0, GridTileProperty.Solid))
        {
            Gizmos.color = Color.yellow;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
