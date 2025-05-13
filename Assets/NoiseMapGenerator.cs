using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase groundTile;
    public TileBase forestTile;
    public TileBase mountainTile;
    public TileBase lakeTile;

    public int width = 1000;   // �� ũ��: 1000x1000
    public int height = 1000;  // �� ũ��: 1000x1000
    public float scale = 0.01f; // ���� ������ ������
    public float lakeNoiseScale = 0.02f; // �� ������ ������ (1000x1000�� �°� ���)
    public float lakeNoiseMin = 0.5f; // �� ���� ���� ���� (Ȯ��)
    public float lakeNoiseMax = 0.8f; // �� ���� ���� ���� (Ȯ��)
    public int safeZoneRadius = 50; // �������� ���� ���� (5m �ݰ�)
    public int chunkSize = 100; // ûũ ũ��

    private int seed;
    private int lakeTileCount = 0; // ������: �� Ÿ�� �� ī��Ʈ

    private void Start()
    {
        GenerateSeed();
        GenerateMap(); // �� ��ü�� ûũ ������ �� �� ����
    }

    void GenerateSeed()
    {
        seed = Random.Range(0, 100000);
    }

    void GenerateMap()
    {
        lakeTileCount = 0; // ī���� �ʱ�ȭ

        // ��ü ���� ûũ ������ ����
        for (int chunkX = -width / 2; chunkX < width / 2; chunkX += chunkSize)
        {
            for (int chunkY = -height / 2; chunkY < height / 2; chunkY += chunkSize)
            {
                GenerateChunk(chunkX, chunkY);
            }
        }

        Debug.Log($"Total lake tiles generated: {lakeTileCount}");
        if (lakeTileCount == 0)
        {
            Debug.LogWarning("No lake tiles were generated! Check lakeNoiseScale, lakeNoiseMin, lakeNoiseMax, or safe zone settings.");
        }
    }

    void GenerateChunk(int startX, int startY)
    {
        int endX = Mathf.Min(startX + chunkSize, width / 2);
        int endY = Mathf.Min(startY + chunkSize, height / 2);

        Vector3Int[] tilePositions = new Vector3Int[(endX - startX) * (endY - startY)];
        TileBase[] tiles = new TileBase[tilePositions.Length];
        int index = 0;

        for (int x = startX; x < endX; x++)
        {
            for (int y = startY; y < endY; y++)
            {
                float xCoord = (x + width / 2 + seed) * scale;
                float yCoord = (y + height / 2 + seed) * scale;
                float noiseValue = Mathf.PerlinNoise(xCoord, yCoord);

                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                tilePositions[index] = tilePosition;

                if (IsInsideSafeZone(x, y, safeZoneRadius))
                {
                    tiles[index] = groundTile;
                }
                else
                {
                    tiles[index] = SelectTileBasedOnNoise(noiseValue, x, y);
                    if (tiles[index] == lakeTile)
                    {
                        lakeTileCount++;
                        if (lakeTileCount <= 20) // ó�� 20�� Ÿ�ϸ� �α� ���
                        {
                            Debug.Log($"Lake tile placed at position ({x}, {y})");
                        }
                    }
                }

                index++;
            }
        }

        tilemap.SetTiles(tilePositions, tiles);
    }

    bool IsInsideSafeZone(int x, int y, int radius)
    {
        return Mathf.Sqrt(x * x + y * y) <= radius;
    }

    TileBase SelectTileBasedOnNoise(float noiseValue, int x, int y)
    {
        if (IsLakeZone(x, y))
        {
            return lakeTile;
        }

        if (noiseValue < 0.4f)
        {
            return groundTile;
        }
        else if (noiseValue < 0.65f)
        {
            return forestTile;
        }
        else
        {
            return mountainTile;
        }
    }

    bool IsLakeZone(int x, int y)
    {
        // �߾� �������� �ݰ� 50ĭ �̳��� �� ���� �� ��
        if (Mathf.Abs(x) < 50 && Mathf.Abs(y) < 50)
            return false;

        float lakeNoise = Mathf.PerlinNoise((x + width / 2 + seed) * lakeNoiseScale, (y + height / 2 + seed) * lakeNoiseScale);
        bool isLake = lakeNoise >= lakeNoiseMin && lakeNoise <= lakeNoiseMax;

        return isLake;
    }
}