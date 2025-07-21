using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase groundTile;
    public TileBase forestTile;
    public TileBase mountainTile;
    public TileBase lakeTile;

    public int width = 1000;   // 맵 크기: 1000x1000
    public int height = 1000;  // 맵 크기: 1000x1000
    public float scale = 0.01f; // 지형 노이즈 스케일
    public float lakeNoiseScale = 0.02f; // 강 노이즈 스케일 (1000x1000에 맞게 축소)
    public float lakeNoiseMin = 0.5f; // 강 생성 범위 하한 (확대)
    public float lakeNoiseMax = 0.8f; // 강 생성 범위 상한 (확대)
    public int safeZoneRadius = 50; // 시작지점 안전 구역 (5m 반경)
    public int chunkSize = 100; // 청크 크기

    private int seed;
    private int lakeTileCount = 0; // 디버깅용: 강 타일 수 카운트

    private void Start()
    {
        GenerateSeed();
        GenerateMap(); // 맵 전체를 청크 단위로 한 번 생성
    }

    void GenerateSeed()
    {
        seed = Random.Range(0, 100000);
    }

    void GenerateMap()
    {
        lakeTileCount = 0; // 카운터 초기화

        // 전체 맵을 청크 단위로 생성
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
                        if (lakeTileCount <= 20) // 처음 20개 타일만 로그 출력
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
        // 중앙 안전지대 반경 50칸 이내는 강 생성 안 함
        if (Mathf.Abs(x) < 50 && Mathf.Abs(y) < 50)
            return false;

        float lakeNoise = Mathf.PerlinNoise((x + width / 2 + seed) * lakeNoiseScale, (y + height / 2 + seed) * lakeNoiseScale);
        bool isLake = lakeNoise >= lakeNoiseMin && lakeNoise <= lakeNoiseMax;

        return isLake;
    }
}