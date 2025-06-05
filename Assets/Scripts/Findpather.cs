using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Grid;
using Vectors;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(VectorRenderer))]

public class Findpather : MonoBehaviour
{
    [SerializeField] private GameObject gamer;
    [SerializeField] private Vector2 startPosition;
    [SerializeField] public int maximumSteps;
    private Vector2 size;
    //private Dictionary<Vector2, Tile> Tiles;
    [SerializeField] private List<Vector2> goals;
    [SerializeField] private List<Vector2> portalen;
    
    private VectorRenderer vectors;

    private void OnEnable()
    {
        vectors.GetComponent<VectorRenderer>();
    }
    /*
    // Update is called once per frame
    void Update()
    {
        Tiles = new Dictionary<Vector2, Tile>();
        size = GetComponent<Grid.Grid>().Size;
        Vector3 temporaryStart = new Vector3(startPosition.x, .25f, startPosition.y);
        
        if (gamer.transform.position != temporaryStart)
        {
            gamer.transform.position = temporaryStart;      
        }

        goals = new List<Vector2>();
        portalen = new List<Vector2>();
        foreach (Transform offspring in this.transform)
        {
            if (offspring == gamer.transform)
            {
                continue;
            }

            Vector2 pos = new Vector2(offspring.position.x, offspring.position.z);
            Tile tile = offspring.GetComponent<Tile>();
            Tiles.Add(pos,tile);

            if (tile.state.GetProperty(TileTypes.Goal))
            {
                goals.Add(pos);
            }

            if (tile.state.GetProperty(TileTypes.Portal))
            {
                portalen.Add(pos);
            }
        }

        foreach (Vector2 goal in goals)
        {
            List<Vector2> path = FindPath(startPosition, goal);
        }
    }

    private void FindG(Vector2 start)
    {
        List<Vector2> openList = new List<Vector2>();
        List<Vector2> closedList = new List<Vector2>();
        if (!Tiles[start].state.GetProperty(TilesTypes.Blocked))
        {
            Tiles[start].G = 0;
            openList.Add(start);
        }

        while (openList.Count > 0)
        {
            Vector2 current = openList[0];
            openList.RemoveAt(0);
            closedList.Add(current);
            if (Tiles[current].G < maximumSteps)
            {
                foreach (Vector2 neighbour in GetNeighbours(current))
                {
                    Tile tile = Tiles[neighbour];
                    int tempG = Tiles[current].G + 1;
                    if (tile.state.GetProperty(TilesTypes.Obstacle))
                    {
                        tempG += tile.ExtraMoveCost;
                    }

                    if (closedList.Contains(neighbour))
                    {
                        if (tempG < tile.G)
                        {
                            tile.G = tempG;
                        }
                        continue;
                    }

                    if (!openList.Contains(neighbour))
                    {
                        openList.Add(neighbour);
                    }

                    if (tempG < tile.G || tile.G == -1)
                    {
                        tile.G = tempG;
                    }
                }
            }
        }
        
    }

    private List<Vector2> FindPath(Vector2 start, Vector2 end)
    {
        List<Vector2> openList = new List<Vector2>();
        List<Vector2> closedList = new List<Vector2>();
        if (!Tiles[start].state.GetProperty(TileTypes.Blocked))
        {
            Tiles[start].G = 0;
            openList.Add(start);
        }

        while (openList.Count > 0)
        {
            Vector2 current_ = GetLowestF(openList);

            if (current_ == end)
            {
                return ReconstructPath(current);
            }
            
            openList.Remove(current_);
            closedList.Remove(current_);

            foreach (Vector2 neighbour in GetNeighbours(current_))
            {
                if (closedList.Contains(neighbour))
                {
                    continue;
                }

                Tile tile = Tiles[neighbour];
                int tempG = Tiles[current_].G + 1;
                if (tile.state.GetProperty(TileTypes.Obstacle))
                {
                    tempG += tile.ExtraMoveCost;
                }

                if (!openList.Contains(neighbour))
                {
                    openList.Add(neighbour);
                }

                if (tempG < tile.G || tile.G == -1)
                {
                    tile.G = tempG;
                }

            }
        }
    }*/
}
