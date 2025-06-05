using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Grid;
using Unity.VisualScripting;
using Vectors;
using UnityEngine;
using Color = UnityEngine.Color;

[ExecuteAlways]
[RequireComponent(typeof(VectorRenderer))]

public class Findpather : MonoBehaviour
{
    [SerializeField] private int ExtraMoveCost;
    [SerializeField] private Vector2 startPosition;
    [SerializeField] public int maximumSteps;
    private Vector2 size;
    private Dictionary<Vector2, Piece> Tiles;
    [SerializeField] private List<Vector2> goals;
    [SerializeField] private List<Vector2> portalen;
    
    private VectorRenderer vectors;

    private void OnEnable()
    {
        vectors = GetComponent<VectorRenderer>();
    }
    
    // Update is called once per frame
    void Update()
    {
        Tiles = new Dictionary<Vector2, Piece>();
        size = GetComponent<Grid.Grid>().Size;

        goals = new List<Vector2>();
        portalen = new List<Vector2>();
        foreach (Transform offspring in this.transform)
        {
            Vector2 pos = new Vector2(offspring.position.x, offspring.position.z);
            Piece tile = offspring.GetComponent<Piece>();
            Tiles.Add(pos,tile);

            if (tile.state.GetProperty(TileProperty.Goal))
            {
                goals.Add(pos);
            }

            if (tile.state.GetProperty(TileProperty.Portal))
            {
                portalen.Add(pos);
            }
        }

        foreach (Piece tile in Tiles.Values)
        {
            tile.G = -1;
        }
        
        FindG(startPosition);
        

        foreach (Vector2 goal in goals)
        {
            List<Vector2> path = FindPath(startPosition, goal);
            if (path != null)
            {
                using (vectors.Begin())
                {
                    for (int i = 0; i < path.Count; i++)
                    {
                        if (!((int)path[i].x == (int)goal.x && (int)path[i].y == (int)goal.y))
                        {
                            Vector3 from = new Vector3(path[i].x, 0.1f, path[i].y);
                            Vector3 to = new Vector3(path[i + 1].x, 0.1f, path[i + 1].y);
                            vectors.Draw(from,to,Color.grey);
                        }
                    }
                }
            }
        }
    }

    private void FindG(Vector2 start)
    {
        List<Vector2> openList = new List<Vector2>();
        List<Vector2> closedList = new List<Vector2>();
        if (!Tiles[start].state.GetProperty(TileProperty.Blocked))
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
                    Piece tile = Tiles[neighbour];
                    int tempG = Tiles[current].G + 1;
                    if (tile.state.GetProperty(TileProperty.Hinder))
                    {
                        tempG += ExtraMoveCost;
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
        if (!Tiles[start].state.GetProperty(TileProperty.Blocked))
        {
            Tiles[start].G = 0;
            openList.Add(start);
        }

        while (openList.Count > 0)
        {
            Vector2 current_ = GetLowestF(openList);

            if (current_ == end)
            {
                return ReconstructPath(current_);
            }
            
            openList.Remove(current_);
            closedList.Add(current_);

            foreach (Vector2 neighbour in GetNeighbours(current_))
            {
                if (closedList.Contains(neighbour))
                {
                    continue;
                }

                Piece tile = Tiles[neighbour];
                int tempG = Tiles[current_].G + 1;
                if (tile.state.GetProperty(TileProperty.Hinder))
                {
                    tempG += ExtraMoveCost;
                }

                if (!openList.Contains(neighbour))
                {
                    openList.Add(neighbour);
                }

                if (tempG < tile.G || tile.G == -1)
                {
                    tile.G = tempG;
                }

                if (tempG <= tile.G)
                {
                    tile.Parent = current_;
                }

                tile.H = calcH(current_, end);

            }
        }

        return null;
    }

    private List<Vector2> GetNeighbours(Vector2 origin)
    {
        List<Vector2> neighbours = new List<Vector2>();
        Piece originTile = Tiles[origin];
        Vector2[] directions = new Vector2[]
            { new Vector2(0, 1), new Vector2(1, 0), new Vector2(-1, 0), new Vector2(0, -1) };

        foreach (Vector2 direction in directions)
        {
            float x = origin.x + direction.x;
            float y = origin.y + direction.y;

            if (x >= 0 && x < size.x && y >= 0 && y < size.y && !Tiles[new Vector2(x,y)].state.GetProperty(TileProperty.Blocked))
            {
                neighbours.Add(new Vector2(x,y));
            }
            
        }

        if (originTile.state.GetProperty(TileProperty.Portal))
        {
            neighbours.Add(originTile.PortalExitCoord);
        }

        return neighbours;
    }

    private int calcH(Vector2 start, Vector2 end, bool checkPortals = true)
    {
        List<int> hList = new List<int>();
        hList.Add((int)(Mathf.Abs(start.x - end.x) + Mathf.Abs(start.y - end.y)));

        if (checkPortals)
        {
            foreach (Vector2 portal in portalen)
            {
                hList.Add((int)(calcH(start, end, false) + 1 + calcH(Tiles[portal].PortalExitCoord, end, false)));
            }
        }

        int lowest = hList[0];
        foreach (int h in hList)
        {
            if (h < lowest)
            {
                lowest = h;
            }
        }

        return lowest;
    }

    private List<Vector2> ReconstructPath(Vector2 goal)
    {
        List<Vector2> path = new List<Vector2>();
        while (!((int)goal.x == (int)startPosition.x && (int)goal.y == (int)startPosition.y))
        {
            path.Add(goal);
            goal = Tiles[goal].Parent;
        }
        path.Add(startPosition);
        path.Reverse();
        return path;
    }

    private Vector2 GetLowestF(List<Vector2> list)
    {
        Vector2 lowest = list[0];
        foreach (Vector2 tile in list)
        {
            if (Tiles[tile].F < Tiles[lowest].F)
            {
                lowest = tile;
            }
        }

        return lowest;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawCube(new Vector3(startPosition.x, 1, startPosition.y),new Vector3(0.5f, 0.5f, 0.5f) );
    }
}
