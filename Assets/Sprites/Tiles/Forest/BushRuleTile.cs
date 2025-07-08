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

    // ���⺰ ��������Ʈ
    public Sprite n, s, e, w;
    public Sprite nw, ne, sw, se;
    public Sprite cnw, cne, csw, cse;
    public Sprite cnwse, cnesw;
    public Sprite centerPlain;
    public Sprite defaultSprite;

    public override bool RuleMatch(int neighbor, TileBase tile)
    {
        return neighbor == (int)Neighbor.Bush && tile is BushRuleTile;
    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);

        // 8���� �̿� �Ǻ�
        bool hasN = tilemap.GetTile(position + Vector3Int.up) is BushRuleTile;
        bool hasS = tilemap.GetTile(position + Vector3Int.down) is BushRuleTile;
        bool hasE = tilemap.GetTile(position + Vector3Int.right) is BushRuleTile;
        bool hasW = tilemap.GetTile(position + Vector3Int.left) is BushRuleTile;
        bool hasNE = tilemap.GetTile(position + new Vector3Int(1, 1, 0)) is BushRuleTile;
        bool hasNW = tilemap.GetTile(position + new Vector3Int(-1, 1, 0)) is BushRuleTile;
        bool hasSE = tilemap.GetTile(position + new Vector3Int(1, -1, 0)) is BushRuleTile;
        bool hasSW = tilemap.GetTile(position + new Vector3Int(-1, -1, 0)) is BushRuleTile;

        //��� ����
        if (!hasN && !hasW && !hasNW)
        {
            tileData.sprite = nw;
        }
        //��� ������
        else if (!hasN && !hasE && !hasNE)
        {
            tileData.sprite = ne;
        }
        //�ϴ� ����
        else if (!hasS && !hasW && !hasSW)
        {
            tileData.sprite = sw;
        }
        //�ϴ� ������
        else if (!hasS && !hasE && !hasSE)
        {
            tileData.sprite = se;
        }
        //���
        else if (!hasN)
        {
            tileData.sprite = n;
        }
        //�ϴ�
        else if (!hasS)
        {
            tileData.sprite = s;
        }
        //������
        else if (!hasE)
        {
            tileData.sprite = e;
        }
        //����
        else if (!hasW)
        {
            tileData.sprite = w;
        }
        //���
        else if (hasN && hasS && hasE && hasW && hasNE && hasNW && hasSE && hasSW)
        {
            tileData.sprite = centerPlain;
        }
        //Ŀ��
        else if (hasN && hasS && hasE && hasW && hasNE && !hasNW && hasSE && hasSW)
        {
            tileData.sprite = cnw;
        }
        else if (hasN && hasS && hasE && hasW && !hasNE && hasNW && hasSE && hasSW)
        {
            tileData.sprite = cne;
        }
        else if (hasN && hasS && hasE && hasW && hasNE && hasNW && hasSE && !hasSW)
        {
            tileData.sprite = csw;
        }
        else if (hasN && hasS && hasE && hasW && hasNE && hasNW && !hasSE && hasSW)
        {
            tileData.sprite = cse;
        }
        else if (hasN && hasS && hasE && hasW && hasNE && !hasNW && !hasSE && hasSW)
        {
            tileData.sprite = cnwse;
        }
        else if (hasN && hasS && hasE && hasW && !hasNE && hasNW && hasSE && !hasSW)
        {
            tileData.sprite = cnesw;
        }
        else
        {
            tileData.sprite = defaultSprite;
        }

        tileData.colliderType = Tile.ColliderType.None;
    }
}
