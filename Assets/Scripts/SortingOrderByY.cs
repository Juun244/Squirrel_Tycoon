/*Y 값을 기반으로 레이어 정렬을 하는 스크립트
* ex)   나무보다 캐릭터의 Y값이 높음 -> 나무가 캐릭터 앞에 위치하도록 정렬
*       캐릭터가 나무보다 Y값이 높음 -> 캐릭터가 나무 앞에 위치하도록 정렬
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
