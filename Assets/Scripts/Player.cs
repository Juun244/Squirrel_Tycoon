using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // 이동 속도
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Vector2 moveInput;

    public TextMeshProUGUI climbHintText;
    private bool isNearTree = false;

    private Transform nearbyTree;
    public bool isClimbing { get; private set; }

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        climbHintText.enabled = false;
    }

    IEnumerator ClimbTree(Vector3 startPos, float climbHeight, float duration)
    {
        isClimbing = true;

        float elapsed = 0f;
        Vector3 endPos = startPos + Vector3.up * climbHeight;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPos, endPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;

        // 애니메이션 상태 초기화
        animator.SetBool("isMoving", false);
        isClimbing = false;

        UnityEngine.SceneManagement.SceneManager.LoadScene("TreeScene");
    }

    void Update()
    {
        if (isClimbing) return;

        // 상하 좌우 입력처리
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        // 애니메이션 상태 업데이트
        bool isMoving = moveInput.magnitude > 0;
        animator.SetBool("isMoving", isMoving);
        animator.SetFloat("moveX", moveInput.x);
        animator.SetFloat("moveY", moveInput.y);

        if (isNearTree && Input.GetKeyDown(KeyCode.F))
        {
            if (nearbyTree != null)
            {
                Collider2D treeCollider = nearbyTree.GetComponent<Collider2D>();
                float treeHeight = treeCollider.bounds.size.y;

                // 시작 위치 계산 (나무 아래 중앙)
                Vector3 climbStartPos = new Vector3(
                    nearbyTree.position.x,
                    treeCollider.bounds.center.y - treeHeight / 2f + 0.2f,
                    transform.position.z
                );

                transform.position = climbStartPos;

                // 위로 걷는 애니메이션 재생 조건
                animator.SetBool("isMoving", true);
                animator.SetFloat("moveX", 0f);
                animator.SetFloat("moveY", 1f);

                StartCoroutine(ClimbTree(transform.position, 2.5f, 1.0f)); // 높이, 시간 조절 가능
            }
        }
    }
   
    void FixedUpdate()
    {
        if (isClimbing) return;
        Move();
    }

    void Move()
    {
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Tree"))
        {
            isNearTree = true;
            climbHintText.enabled = true;
            nearbyTree = collider.transform;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Tree"))
        {
            isNearTree = false;
            climbHintText.enabled = false;
            nearbyTree = null;
        }
    }


}