using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisGrid : MonoBehaviour
{
    public int width = 7; // z
    public int length = 7; // x
    public int height = 7; // y
    public GridSpot gridSpotPrefab;
    public GameObject outerPerimeter;
    public GameObject mainCamera;
    public GridSpot[,,] grid;

    void Start()
    {
        outerPerimeter.transform.position = new Vector3(width/2f - 0.5f, length/2f - 0.5f, height/2f - 0.5f);
        outerPerimeter.transform.localScale = new Vector3(width + 0.1f, length + 0.1f, height + 0.1f);
        
        grid = new GridSpot[width, length, height];
        
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < length; j++)
            {
                for (int k = 0; k < height; k++)
                {
                    grid[i,j,k] = Instantiate<GridSpot>(gridSpotPrefab, new Vector3(i, j, k), Quaternion.identity, GameObject.Find("GridSpots").transform);
                }
            }
        }
    }

    void Update() {}

    public Vector3 GetSize() => new Vector3(width, length, height);
}
