using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Custom Tiles/Bush Rule Tile")]
public class BushRuleTile : RuleTile<BushRuleTile.Neighbor>
{
    public enum Neighbor
    {
        None,
        Bush
    }

    // 방향별 스프라이트
    public Sprite n, s, e, w;
    public Sprite nw, ne, sw, se;
    public Sprite centerPlain;
    public Sprite defaultSprite;

    public override bool RuleMatch(int neighbor, TileBase tile)
    {
        return neighbor == (int)Neighbor.Bush && tile is BushRuleTile;
    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);

        // 8방향 이웃 판별
        bool hasN = tilemap.GetTile(position + Vector3Int.up) is BushRuleTile;
        bool hasS = tilemap.GetTile(position + Vector3Int.down) is BushRuleTile;
        bool hasE = tilemap.GetTile(position + Vector3Int.right) is BushRuleTile;
        bool hasW = tilemap.GetTile(position + Vector3Int.left) is BushRuleTile;
        bool hasNE = tilemap.GetTile(position + new Vector3Int(1, 1, 0)) is BushRuleTile;
        bool hasNW = tilemap.GetTile(position + new Vector3Int(-1, 1, 0)) is BushRuleTile;
        bool hasSE = tilemap.GetTile(position + new Vector3Int(1, -1, 0)) is BushRuleTile;
        bool hasSW = tilemap.GetTile(position + new Vector3Int(-1, -1, 0)) is BushRuleTile;

        // 조건 우선순위대로 검사

        if (!hasN && !hasW && !hasNW)
        {
            tileData.sprite = nw;
        }
        else if (!hasN && !hasE && !hasNE)
        {
            tileData.sprite = ne;
        }
        else if (!hasS && !hasW && !hasSW)
        {
            tileData.sprite = sw;
        }
        else if (!hasS && !hasE && !hasSE)
        {
            tileData.sprite = se;
        }
        else if (!hasN)
        {
            tileData.sprite = n;
        }
        else if (!hasS)
        {
            tileData.sprite = s;
        }
        else if (!hasE)
        {
            tileData.sprite = e;
        }
        else if (!hasW)
        {
            tileData.sprite = w;
        }
        else if (hasN && hasS && hasE && hasW && hasNE && hasNW && hasSE && hasSW)
        {
            tileData.sprite = centerPlain;
        }
        else
        {
            tileData.sprite = defaultSprite;
        }

        tileData.colliderType = Tile.ColliderType.None;
    }
}
