using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public Tilemap tilemap;

    public TileBase groundTile;
    public TileBase forestTile;
    public TileBase mountainTile;
    public TileBase lakeTile;

    public int width = 200;   // 가로 크기
    public int height = 200;  // 세로 크기
    public float scale = 0.1f; // 노이즈 스케일

    public int safeZoneRadius = 5; // 시작지점 안전 구역 (5칸 반지름)

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

                // 시작 지점 반경 safeZoneRadius 이내는 무조건 Ground
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
        // 강(Lake) 먼저 강제 배치
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
        // 강은 x축 기준으로 생성하되, 중앙 안전지대 반경 10칸 이내는 피한다
        if (Mathf.Abs(x) < 10 && Mathf.Abs(y) < 10)
            return false;

        float lakeNoise = Mathf.PerlinNoise((x + width / 2 + seed) * 0.05f, (y + height / 2 + seed) * 0.05f);
        return lakeNoise > 0.55f && lakeNoise < 0.65f; // 특정 값대만 강으로 만들어서 길게 이어지게
    }
}
