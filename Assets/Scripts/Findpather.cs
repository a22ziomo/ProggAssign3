using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Grid;
using Vectors;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteAlways]
[RequireComponent(typeof(VectorRenderer))]

public class Findpather : MonoBehaviour
{
    [SerializeField] private GameObject gamer;
    [SerializeField] private Vector2 startPosition;
    [SerializeField] public int maximumSteps;
    private Vector2 size;

    private Dictionary<Vector2, Tile> Tiles;
    [SerializeField] private List<Vector2> goals;
    [SerializeField] private List<Vector2> portalen;
    private VectorRenderer vectors;

    private void OnEnable()
    {
        vectors.GetComponent<VectorRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Tiles = new Dictionary<Vector2, Tile>();
        size = GetComponent<Grid.Grid>().Size;
        Vector3 temporaryStart = new Vector3(startPosition.x, .25f, startPosition.y);
    }
}
