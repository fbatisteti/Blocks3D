using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class GameFlow : MonoBehaviour
{
    public GameObject grid;
    public GameObject blocksSpawned;
    public List<GameObject> blocks;

    public GameObject outerWallBack;
    public GameObject outerWallFront;
    public GameObject outerWallRight;
    public GameObject outerWallLeft;

    private Vector3 spawnPoint;
    private List<GameObject> blocksPool;
    private GameObject nextBlock;
    private GameObject currentBlock;
    private GameObject movingBlock;
    private int dangerCounter = 0;
    private float moveSpeed = 8f;

    private bool moveDone = true;
    private bool allowMove = true;

    void Start()
    {
        ResetSpawnPoint();
        ResetOuterBorders();

        blocksPool = new List<GameObject>(blocks);

        SpawnFirstBlock();
    }

    void Update()
    {
        // drop
        if (Input.GetKeyDown(KeyCode.X) && moveDone == true && allowMove == true)
        {
            allowMove = false;
            StartCoroutine(MoveBlockDown(movingBlock));
        }

        // move
        if (Input.GetKeyDown(KeyCode.UpArrow) && moveDone == true && allowMove == true)
        {
            MoveBlockAround(movingBlock, "back");
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && moveDone == true && allowMove == true)
        {
            MoveBlockAround(movingBlock, "front");
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) && moveDone == true && allowMove == true)
        {
            MoveBlockAround(movingBlock, "right");
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) && moveDone == true && allowMove == true)
        {
            MoveBlockAround(movingBlock, "left");
        }

        // rotate
        if (Input.GetKeyDown(KeyCode.W) && moveDone == true && allowMove == true)
        {
            RotateBlock(movingBlock, "back");
        }
        if (Input.GetKeyDown(KeyCode.S) && moveDone == true && allowMove == true)
        {
            RotateBlock(movingBlock, "front");
        }
        if (Input.GetKeyDown(KeyCode.A) && moveDone == true && allowMove == true)
        {
            RotateBlock(movingBlock, "right");
        }
        if (Input.GetKeyDown(KeyCode.D) && moveDone == true && allowMove == true)
        {
            RotateBlock(movingBlock, "left");
        }
        if (Input.GetKeyDown(KeyCode.Q) && moveDone == true && allowMove == true)
        {
            RotateBlock(movingBlock, "anticlock");
        }
        if (Input.GetKeyDown(KeyCode.E) && moveDone == true && allowMove == true)
        {
             RotateBlock(movingBlock, "clock");
        }
    }

    private void ResetSpawnPoint()
    {
        spawnPoint = grid.GetComponent<TetrisGrid>().GetSize();
        spawnPoint = new Vector3(spawnPoint.x/2f - 0.5f, spawnPoint.y + 3f, spawnPoint.z/2f - 0.5f);
    }

    private void ResetOuterBorders()
    {
        Vector3 gridSize = grid.GetComponent<TetrisGrid>().GetSize();
        outerWallBack.transform.position = new Vector3(-0.5f, gridSize.y/2f, gridSize.z/2f);
        outerWallFront.transform.position = new Vector3(gridSize.x - 0.5f, gridSize.y/2f, gridSize.z/2f);
        outerWallRight.transform.position = new Vector3(gridSize.x/2f, gridSize.y/2f, -0.5f);
        outerWallLeft.transform.position = new Vector3(gridSize.x/2f, gridSize.y/2f, gridSize.z - 0.5f);
    }

    private void RotateBlock (GameObject block, string direction)
    {
        // first go to child (BlockPrefab), then get the children
        GameObject blockPrefab = block.transform.Find("BlockPrefab").gameObject;
        Transform[] children = blockPrefab.GetComponentsInChildren<Transform>();

        int centerX = (int)spawnPoint.x;
        int centerY = (int)spawnPoint.y;
        int centerZ = (int)spawnPoint.z;

        float rotationAngle = 90.0f;
        float angleRad = rotationAngle * Mathf.Deg2Rad;

        foreach (var cube in children)
        {
            if (cube.name != "BlockPrefab")
            {
                int positionX = (int)cube.position.x;
                int positionY = (int)cube.position.y;
                int positionZ = (int)cube.position.z;

                int newX = (int)cube.position.x;
                int newY = (int)cube.position.y;
                int newZ = (int)cube.position.z;

                switch (direction)
                {
                    case "back":
                        newX = Mathf.RoundToInt((positionX - centerX) * Mathf.Cos(angleRad) - (positionY - centerY) * Mathf.Sin(angleRad)) + centerX;
                        newY = Mathf.RoundToInt((positionX - centerX) * Mathf.Sin(angleRad) + (positionY - centerY) * Mathf.Cos(angleRad)) + centerY;
                        newZ = positionZ;
                        break;

                    case "front":
                        newX = Mathf.RoundToInt((positionX - centerX) * Mathf.Cos(-angleRad) - (positionY - centerY) * Mathf.Sin(-angleRad)) + centerX;
                        newY = Mathf.RoundToInt((positionX - centerX) * Mathf.Sin(-angleRad) + (positionY - centerY) * Mathf.Cos(-angleRad)) + centerY;
                        newZ = positionZ;
                        break;

                    case "right":
                        newX = positionX;
                        newY = Mathf.RoundToInt((positionY - centerY) * Mathf.Cos(angleRad) - (positionZ - centerZ) * Mathf.Sin(angleRad)) + centerY;
                        newZ = Mathf.RoundToInt((positionY - centerY) * Mathf.Sin(angleRad) + (positionZ - centerZ) * Mathf.Cos(angleRad)) + centerZ;
                        break;

                    case "left":
                        newX = positionX;
                        newY = Mathf.RoundToInt((positionY - centerY) * Mathf.Cos(-angleRad) - (positionZ - centerZ) * Mathf.Sin(-angleRad)) + centerY;
                        newZ = Mathf.RoundToInt((positionY - centerY) * Mathf.Sin(-angleRad) + (positionZ - centerZ) * Mathf.Cos(-angleRad)) + centerZ;
                        break;

                    case "clock":
                        newX = Mathf.RoundToInt((positionX - centerX) * Mathf.Cos(angleRad) - (positionZ - centerZ) * Mathf.Sin(angleRad)) + centerX;
                        newY = positionY;
                        newZ = Mathf.RoundToInt((positionX - centerX) * Mathf.Sin(angleRad) + (positionZ - centerZ) * Mathf.Cos(angleRad)) + centerZ;
                        break;

                    case "anticlock":
                        newX = Mathf.RoundToInt((positionX - centerX) * Mathf.Cos(-angleRad) - (positionZ - centerZ) * Mathf.Sin(-angleRad)) + centerX;
                        newY = positionY;
                        newZ = Mathf.RoundToInt((positionX - centerX) * Mathf.Sin(-angleRad) + (positionZ - centerZ) * Mathf.Cos(-angleRad)) + centerZ;
                        break;

                    default:
                        break;
                }

                cube.position = new Vector3(newX, newY, newZ);
            }
        }
    }

    private void SpawnFirstBlock()
    {
        int i;
        currentBlock = RandomizeBlock(blocksPool, out i);
        blocksPool.RemoveAt(i);
        nextBlock = RandomizeBlock(blocksPool, out i);
        blocksPool.RemoveAt(i);

        movingBlock = Instantiate(currentBlock, spawnPoint, Quaternion.identity, blocksSpawned.transform);

        currentBlock = nextBlock;
        nextBlock = RandomizeBlock(blocksPool, out i);
        blocksPool.RemoveAt(i);
    }

    private void SpawnBlock()
    {
        int i;
        if (blocksPool.Count <= 2)
        {
            blocksPool = new List<GameObject>(blocks);
            dangerCounter++;
        }

        movingBlock = Instantiate(currentBlock, spawnPoint, Quaternion.identity, blocksSpawned.transform);
        currentBlock = nextBlock;
        nextBlock = RandomizeBlock(blocksPool, out i);
        blocksPool.RemoveAt(i);
    }

    private IEnumerator MoveBlockDown(GameObject block)
    {
        while (GetLowestCube(block).transform.position.y > 0 && !ThereAreBlocksBelow(block))
        {
            moveDone = false;
            block.transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
            yield return null;
        }
        moveDone = true;

        // end turn
        CheckPlane();
        ResetSpawnPoint();
        allowMove = true;

        SpawnBlock();
    }

    private void MoveBlockAround(GameObject block, string direction)
    {
        Vector3 movement = Vector3.zero;

        switch (direction)
        {
            case "back":
                movement += new Vector3(-1, 0, 0);
                break;

            case "front":
                movement += new Vector3(1, 0, 0);
                break;

            case "left":
                movement += new Vector3(0, 0, 1);
                break;

            case "right":
                movement += new Vector3(0, 0, -1);
                break;

            default:
                break;
        }

        if (AllowMove(block, direction))
        {
            spawnPoint += movement;
            block.transform.position += movement;
        }
    }

    private void CheckPlane()
    {
        Transform[] spawnedBlocks = blocksSpawned.GetComponentsInChildren<Transform>();

        int size = (int)grid.GetComponent<TetrisGrid>().GetSize().y;
        int limit = size * size;
        int blocks = 0;

        for (int plane = 0; plane < size; plane++)
        {
            // check plane
            foreach (var cube in spawnedBlocks)
            {
                if (cube.childCount == 0 && Mathf.RoundToInt(cube.position.y) == plane) blocks++;
            }

            if (blocks == limit)
            {
                // delete on plane
                foreach (var cube in spawnedBlocks)
                {
                    if (cube.childCount == 0 && Mathf.RoundToInt(cube.position.y) == plane)
                    {
                        Destroy(cube.gameObject);
                    }
                }

                // move down pieces
                for (int plane2 = plane + 1; plane2 < size; plane2++)
                {
                    foreach (var cube in spawnedBlocks)
                    {
                        if (cube.childCount == 0 && Mathf.RoundToInt(cube.position.y) == plane2)
                        {
                            cube.transform.position += Vector3.down;
                        }
                    }
                }

                // check again
                plane = 0;
            }
        }
    }

    private GameObject RandomizeBlock(List<GameObject> blocksPool, out int i)
    {
        i = Random.Range(0, blocksPool.Count);
        return blocksPool[i];
    }

    private GameObject GetLowestCube(GameObject block)
    {
        GameObject lowestCube = block;
        float y = 100;

        Transform[] cubes = block.GetComponentsInChildren<Transform>();
        foreach (var cube in cubes)
        {
            if (cube.position.y <= y)
            {
                y = cube.position.y;
                lowestCube = cube.gameObject;
            }
        }
        
        return lowestCube;
    }

    private bool ThereAreBlocksBelow(GameObject block)
    {
        // first go to child (BlockPrefab), then get the children
        GameObject blockPrefab = block.transform.Find("BlockPrefab").gameObject;
        Transform[] children = blockPrefab.GetComponentsInChildren<Transform>();

        foreach (var child in children)
        {
            if (child.name != "BlockPrefab" && child.GetComponent<CubeCollider>().hasCollided) return true;
        }

        return false;
    }

    private bool AllowMove(GameObject block, string direction)
    {
        // first go to child (BlockPrefab), then get the children
        GameObject blockPrefab = block.transform.Find("BlockPrefab").gameObject;
        Transform[] children = blockPrefab.GetComponentsInChildren<Transform>();

        foreach (var child in children)
        {
            if (child.name != "BlockPrefab" && !child.GetComponent<CubeCollider>().AllowMove(direction))
            {
                return false;
            }
        }

        return true;
    }
}
