using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class MapGenerator1 : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private int seed = 1234;

    [Header("Ground")]
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private TileBase[] groundTiles; // 1~5번 타일

    [Header("Bush")]
    [SerializeField] private Tilemap bushTilemap;
    [SerializeField] private RuleTile bushTile;
    [SerializeField] private int bushCount = 300;
    [SerializeField] private Vector2Int bushMinSize = new Vector2Int(3, 2);
    [SerializeField] private Vector2Int bushMaxSize = new Vector2Int(8, 6);

    private const int MAP_MIN = -500;
    private const int MAP_MAX = 500;

    private HashSet<Vector2Int> occupied = new HashSet<Vector2Int>();

    void Start()
    {
        Random.InitState(seed);
        GenerateGround();
        GenerateBushes();
    }

    void GenerateGround()
    {
        for (int x = MAP_MIN; x <= MAP_MAX; x++)
        {
            for (int y = MAP_MIN; y <= MAP_MAX; y++)
            {
                int index = Random.Range(0, groundTiles.Length);
                groundTilemap.SetTile(new Vector3Int(x, y, 0), groundTiles[index]);
            }
        }
    }

    void GenerateBushes()
    {
        int placed = 0;
        int maxTry = bushCount * 10;

        for (int tryCount = 0; tryCount < maxTry && placed < bushCount; tryCount++)
        {
            int x = Random.Range(MAP_MIN, MAP_MAX);
            int y = Random.Range(MAP_MIN, MAP_MAX);

            int width = Random.Range(bushMinSize.x, bushMaxSize.x + 1);
            int height = Random.Range(bushMinSize.y, bushMaxSize.y + 1);

            Vector2Int start = new Vector2Int(x, y);
            List<Vector2Int> area = new List<Vector2Int>();

            bool overlaps = false;
            for (int dx = 0; dx < width; dx++)
            {
                for (int dy = 0; dy < height; dy++)
                {
                    Vector2Int pos = new Vector2Int(x + dx, y + dy);
                    if (occupied.Contains(pos))
                    {
                        overlaps = true;
                        break;
                    }
                    area.Add(pos);
                }
                if (overlaps) break;
            }

            if (!overlaps)
            {
                foreach (var pos in area)
                {
                    bushTilemap.SetTile((Vector3Int)pos, bushTile);
                    occupied.Add(pos);
                }
                placed++;
            }
        }
        bushTilemap.RefreshAllTiles();

        Debug.Log($"Placed {placed} bushes (requested: {bushCount})");
    }
}
