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
        // Climb 중 player의 sortingOrder를 변경하지 않도록
        if(gameObject.tag == "Player" && Player.instance.isClimbing)
        {
            return;
        }
        sr.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);
    }
}
