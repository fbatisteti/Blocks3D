// UNUSED

using System;
using UnityEngine;

public class SpecialBlock : MonoBehaviour
{
    public GameObject specialBlockPrefab;

    private int[,,] spawnGrid;

    void Start()
    {
        spawnGrid = new int[3, 3, 3];
        Array.Clear(spawnGrid, 0, spawnGrid.Length);
        spawnGrid[1,1,1] = 1;

        GenerateForm();
        InstantiateBlock();
    }

    void Update() {}

    private void GenerateForm()
    {
        int newBlocks = 4;
        while (newBlocks > 0)
        {
            int randomX = UnityEngine.Random.Range(0, 3);
            int randomY = UnityEngine.Random.Range(0, 3);
            int randomZ = UnityEngine.Random.Range(0, 3);

            if (spawnGrid[randomX, randomY, randomZ] != 1)
            {
                spawnGrid[randomX, randomY, randomZ] = 1;
                newBlocks--;
            }
        }
    }

    private void InstantiateBlock()
    {
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                for (int z = 0; z < 3; z++)
                {
                    if (spawnGrid[x,y,z] == 1)
                    {
                        Instantiate(specialBlockPrefab, transform.position + new Vector3(x-1, y-1, z-1), Quaternion.identity, this.gameObject.transform);
                    }
                }
            }
        }
    }
}
