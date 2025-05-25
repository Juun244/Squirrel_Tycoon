using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator1 : MonoBehaviour
{
    public int seed = 1234;
    public TileBase[] groundTiles;
    public TileBase bushTile;
    public Tilemap groundTilemap;
    public Tilemap bushTilemap;

    void Start()
    {
        GenerateMap();
    }

    void Update()
    {

    }

    void GenerateBushes(System.Random rand)
    {
        int bushCount = 100;

        for (int i = 0; i < bushCount; i++)
        {
            int centerX = rand.Next(-450, 450);
            int centerY = rand.Next(-450, 450);

            int width = rand.Next(10, 101);
            int height = rand.Next(10, 101);
            int pattern = rand.Next(0, 3);

            switch (pattern)
            {
                case 0:
                    for (int x = -width / 2; x <= width / 2; x++)
                    {
                        for (int y = -height / 2; y <= height / 2; y++)
                        {
                            Vector3Int pos = new Vector3Int(centerX + x, centerY + y, 0);
                            bushTilemap.SetTile(pos, bushTile);
                        }
                    }
                    break;

                case 1:
                    for (int x = 0; x <= width / 2; x++)
                    {
                        for (int y = 0; y <= height / 2; y++)
                        {
                            if (x < 3 && y < 3) continue;
                            Vector3Int pos = new Vector3Int(centerX + x, centerY + y, 0);
                            bushTilemap.SetTile(pos, bushTile);
                        }
                    }
                    break;

                case 2:
                    for (int x = -width / 2; x <= 0; x++)
                    {
                        for (int y = 0; y <= height / 2; y++)
                        {
                            if (-x < 3 && y < 3) continue;
                            Vector3Int pos = new Vector3Int(centerX + x, centerY + y, 0);
                            bushTilemap.SetTile(pos, bushTile);
                        }
                    }
                    break;
            }
        }

        bushTilemap.RefreshAllTiles();
    }

    public void GenerateMap()
    {
        System.Random rand = new System.Random(seed);
        int halfSize = 500;

        for (int x = -halfSize; x < halfSize; x++)
        {
            for (int y = -halfSize; y < halfSize; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                float dist = Vector2.Distance(Vector2.zero, new Vector2(x, y));
                float noise = (float)rand.NextDouble();
                int groundType;

                if (dist <= 100)
                {
                    if (noise < 0.2f) groundType = 0;
                    else if (noise < 0.6f) groundType = 3;
                    else groundType = 4;
                }
                else if (dist <= 300)
                {
                    if (noise < 0.3f) groundType = 0;
                    else if (noise < 0.7f) groundType = 1;
                    else groundType = 2;
                }
                else
                {
                    if (noise < 0.9f) groundType = 0;
                    else groundType = 1;
                }

                groundTilemap.SetTile(pos, groundTiles[groundType]);
            }
        }

        GenerateBushes(rand);
    }
}