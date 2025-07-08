/*Y ���� ������� ���̾� ������ �ϴ� ��ũ��Ʈ
* ex)   �������� ĳ������ Y���� ���� -> ������ ĳ���� �տ� ��ġ�ϵ��� ����
*       ĳ���Ͱ� �������� Y���� ���� -> ĳ���Ͱ� ���� �տ� ��ġ�ϵ��� ����
*/
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SortingOrderByY : MonoBehaviour
{
    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void LateUpdate()
    {
        sr.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);
    }
}
