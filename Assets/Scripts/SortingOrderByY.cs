using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SortingOrderByY : MonoBehaviour
{
    private SpriteRenderer sr;
    private PlayerController playerController;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        // "Player"라는 이름의 오브젝트에서 PlayerController 찾기
        GameObject playerObject = GameObject.Find("Player");
        if (playerObject != null)
        {
            playerController = playerObject.GetComponent<PlayerController>();
        }
        else
        {
            Debug.LogWarning("Player 오브젝트를 찾을 수 없습니다. 이름이 정확한지 확인하세요.");
        }
    }

    void LateUpdate()
    {
        // 플레이어 자신이고 Climb 중이라면 정렬 스킵
        if (playerController != null && playerController.isClimbing && gameObject == playerController.gameObject)
        {
            return;
        }

        sr.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);
    }
}
