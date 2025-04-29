using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public Tilemap tilemap;

    public TileBase groundTile;
    public TileBase forestTile;
    public TileBase mountainTile;
    public TileBase lakeTile;

    public int width = 200;   // ���� ũ��
    public int height = 200;  // ���� ũ��
    public float scale = 0.1f; // ������ ������

    public int safeZoneRadius = 5; // �������� ���� ���� (5ĭ ������)

    private int seed;

    private void Start()
    {
        GenerateSeed();
        GenerateMap();
    }

    void GenerateSeed()
    {
        seed = Random.Range(0, 100000);
    }

    void GenerateMap()
    {
        tilemap.ClearAllTiles();

        for (int x = -width / 2; x < width / 2; x++)
        {
            for (int y = -height / 2; y < height / 2; y++)
            {
                float xCoord = (x + width / 2 + seed) * scale;
                float yCoord = (y + height / 2 + seed) * scale;

                float noiseValue = Mathf.PerlinNoise(xCoord, yCoord);

                Vector3Int tilePosition = new Vector3Int(x, y, 0);

                // ���� ���� �ݰ� safeZoneRadius �̳��� ������ Ground
                if (IsInsideSafeZone(x, y, safeZoneRadius))
                {
                    tilemap.SetTile(tilePosition, groundTile);
                }
                else
                {
                    TileBase selectedTile = SelectTileBasedOnNoise(noiseValue, x, y);
                    tilemap.SetTile(tilePosition, selectedTile);
                }
            }
        }
    }

    bool IsInsideSafeZone(int x, int y, int radius)
    {
        return Mathf.Sqrt(x * x + y * y) <= radius;
    }

    TileBase SelectTileBasedOnNoise(float noiseValue, int x, int y)
    {
        // ��(Lake) ���� ���� ��ġ
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
        // ���� x�� �������� �����ϵ�, �߾� �������� �ݰ� 10ĭ �̳��� ���Ѵ�
        if (Mathf.Abs(x) < 10 && Mathf.Abs(y) < 10)
            return false;

        float lakeNoise = Mathf.PerlinNoise((x + width / 2 + seed) * 0.05f, (y + height / 2 + seed) * 0.05f);
        return lakeNoise > 0.55f && lakeNoise < 0.65f; // Ư�� ���븸 ������ ���� ��� �̾�����
    }
}
