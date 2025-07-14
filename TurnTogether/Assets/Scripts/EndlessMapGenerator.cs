using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EndlessMapGenerator : MonoBehaviour
{
    public GameObject tilePrefab;
    public GameObject startingTilePrefab;
    public int initialTiles = 10;
    public float tileLength = 1f;
    public Transform player;
    public Vector3 startPosition = Vector3.zero;

    public Queue<GameObject> tilePool = new Queue<GameObject>();
    public List<GameObject> activeTiles = new List<GameObject>();

    public Vector3 currentDirection = Vector3.forward;
    public Quaternion currentRotation = Quaternion.identity;
    public Vector3 currentPosition = Vector3.zero;

    public string lastTurn = "none"; // left / right / none
    private bool leftUsed = false;


    public GameObject coinPrefab;
    public GameObject starPrefab;

    [Range(0f, 1f)]
    public float spawnChance = 0.4f; // 40% tiles will get something
    [Range(0f, 1f)]
    public float starChance = 0.1f;  // Of those, only 10% will be stars
    private bool rightUsed = false;

    void Start()
    {
        // 1. Spawn starting tile at startPosition
        GameObject startTile = Instantiate(startingTilePrefab, startPosition, Quaternion.identity);
        activeTiles.Add(startTile);

        // 2. Pre-fill the object pool
        for (int i = 0; i < initialTiles; i++)
        {
            GameObject tile = Instantiate(tilePrefab);
            tile.SetActive(false);
            tilePool.Enqueue(tile);
        }

        // 3. Spawn normal tiles after the first one
        currentPosition = startPosition + Vector3.forward * tileLength;
        currentDirection = Vector3.forward;
        currentRotation = Quaternion.identity;
        lastTurn = "none";
        leftUsed = false;
        rightUsed = false;

        for (int i = 1; i < initialTiles; i++)
        {
            SpawnNextTile();
        }
    }

    public void SpawnNextTile()
    {
        GameObject tile;

        // Reuse from pool or expand
        if (tilePool.Count > 0)
        {
            tile = tilePool.Dequeue();
        }
        else
        {
            tile = Instantiate(tilePrefab);
        }

        tile.transform.position = currentPosition;
        tile.transform.rotation = currentRotation;
        tile.SetActive(true);
        tile.GetComponent<Tile>().generator = this;
        TrySpawnCollectible(tile);


        activeTiles.Add(tile);

        int nextTurn;

        // 🟩 First 4 tiles: straight, 5th = left, then smart logic
        if (activeTiles.Count <= 4)
        {
            nextTurn = 0; // straight
        }
        else if (activeTiles.Count == 5)
        {
            nextTurn = 1; // force first turn to be left
        }
        else
        {
            nextTurn = GetSmartTurn();
        }

        // 🌀 Apply turn
        if (nextTurn == 1) // left
        {
            currentDirection = Quaternion.Euler(0, -90, 0) * currentDirection;
            currentRotation = Quaternion.LookRotation(currentDirection);
            lastTurn = "left";
            leftUsed = true;

            if (rightUsed) rightUsed = false;
        }
        else if (nextTurn == 2) // right
        {
            currentDirection = Quaternion.Euler(0, 90, 0) * currentDirection;
            currentRotation = Quaternion.LookRotation(currentDirection);
            lastTurn = "right";
            rightUsed = true;

            if (leftUsed) leftUsed = false;
        }
        else
        {
            lastTurn = "none"; // straight
        }

        currentPosition += currentDirection * tileLength;
    }

    void TrySpawnCollectible(GameObject tile)
    {
        // Get all spawn points (child objects named "Spawn1", "Spawn2", etc.)
        List<Transform> spawnPoints = new List<Transform>();

        foreach (Transform child in tile.transform)
        {
            if (child.name.StartsWith("Spawn"))
            {
                spawnPoints.Add(child);
            }
        }

        if (spawnPoints.Count == 0) return;

        // Randomly decide whether to spawn anything
        if (Random.value > spawnChance) return;

        // Pick one random spawn point
        Transform chosenPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];

        // Decide what to spawn: Star (rare) or Coin (common)
        GameObject itemToSpawn = (Random.value < starChance) ? starPrefab : coinPrefab;

        // Instantiate it at that point and parent it under the tile
        Instantiate(itemToSpawn, chosenPoint.position, Quaternion.identity, tile.transform);
    }

    public void RecycleOldTile()
    {
        if (activeTiles.Count < 3) return;

        GameObject oldTile = activeTiles[0];
        activeTiles.RemoveAt(0);

        StartCoroutine(DropAndRecycle(oldTile, 1.25f));
    }

    IEnumerator DropAndRecycle(GameObject tile, float delay)
    {
        yield return new WaitForSeconds(delay);

        Vector3 startPos = tile.transform.position;
        Vector3 endPos = startPos + Vector3.down * 3f;

        float t = 0f;
        float duration = 0.5f;

        while (t < 1f)
        {
            tile.transform.position = Vector3.Lerp(startPos, endPos, t);
            t += Time.deltaTime / duration;
            yield return null;
        }

        tile.SetActive(false);
        tile.transform.position = Vector3.zero;
        tilePool.Enqueue(tile);

        SpawnNextTile();
    }

    int GetSmartTurn()
    {
        // 🔁 Smart logic to avoid same turn twice
        if (!leftUsed)
        {
            int[] options = { 0, 1 }; // straight or left
            return options[Random.Range(0, options.Length)];
        }
        else if (!rightUsed)
        {
            int[] options = { 0, 2 }; // straight or right
            return options[Random.Range(0, options.Length)];
        }
        else
        {
            // both used once → reset and allow any
            leftUsed = false;
            rightUsed = false;
            return Random.Range(0, 3);
        }
    }
}
