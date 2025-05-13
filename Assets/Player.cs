using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movePower = 5f; // 이동 속도
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Vector2 moveInput;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 입력 처리
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        // 애니메이션 상태 업데이트
        bool isMoving = moveInput.magnitude > 0;
        animator.SetBool("isMoving", isMoving);

        // 이동 방향에 따라 스프라이트 방향 및 애니메이션 파라미터 설정
        if (isMoving)
        {
            // 이동 방향 계산 (8방향)
            float angle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg;

            // 8방향에 따라 애니메이션 파라미터 설정
            // 애니메이터에 Direction 파라미터를 추가해 방향별 애니메이션 설정 필요
            //int direction = Mathf.RoundToInt(angle / 45f) % 8;
            //animator.SetFloat("Direction", direction);

            // 스프라이트 좌우 반전 (예: 왼쪽 방향일 때)
            if(Input.GetAxisRaw("Horizontal") != 0)
                spriteRenderer.flipX = moveInput.x > 0;
        }
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        // Rigidbody2D를 사용한 이동
        rb.linearVelocity = moveInput * movePower;
    }
}