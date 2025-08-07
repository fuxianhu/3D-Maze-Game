using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private float moveSpeed = 5f;
    private float jumpForce = 7f;

    private Rigidbody rb;
    private BoxCollider childCollider;

    [Header("Raycast Settings")]
    [SerializeField] private float raycastDistance = 0.4f; // 射线检测距离
    [SerializeField] private LayerMask groundLayer; // 地面层级

    [Header("Foot Positions")]
    [SerializeField] private Transform leftFoot; // 左脚位置
    [SerializeField] private Transform rightFoot; // 右脚位置

    [Header("Debug")]
    [SerializeField] private bool showDebugRays = true; // 是否显示调试射线
    [SerializeField] private Color rayColor = Color.green; // 射线颜色

    [Header("Camera Settings")]
    public Transform playerCamera;

    private float xRotation = 0f;
    private float yRotation = 90f;

    [Header("Other")]
    public MazeRenderer mazeRenderer;

    // 是否在地面上（属性，供外部访问）
    public bool IsGrounded { get; private set; }

    private void CheckGround()
    {
        // 检查两只脚是否至少有一只在地面上
        bool leftFootGrounded = CheckSingleFoot(leftFoot);
        bool rightFootGrounded = CheckSingleFoot(rightFoot);

        IsGrounded = leftFootGrounded || rightFootGrounded;

        // 调试信息
        if (showDebugRays)
        {
            Debug.DrawRay(leftFoot.position, Vector3.down * raycastDistance, rayColor);
            Debug.DrawRay(rightFoot.position, Vector3.down * raycastDistance, rayColor);
        }
    }

    private bool CheckSingleFoot(Transform foot)
    {
        // 使用 SphereCast 替代 Raycast，提升地面检测的鲁棒性
        RaycastHit hit;
        float sphereRadius = 0.15f;
        if (Physics.SphereCast(foot.position, sphereRadius, Vector3.down, out hit, raycastDistance, groundLayer))
        {
            return true;
        }
        return false;
    }

    // 在编辑器中可视化射线
    private void OnDrawGizmos()
    {
        if (showDebugRays && leftFoot != null && rightFoot != null)
        {
            Gizmos.color = rayColor;
            Gizmos.DrawLine(leftFoot.position, leftFoot.position + Vector3.down * raycastDistance);
            Gizmos.DrawLine(rightFoot.position, rightFoot.position + Vector3.down * raycastDistance);
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        childCollider = GetComponentInChildren<BoxCollider>();

        if (childCollider == null)
            Debug.LogError("No child BoxCollider found!");
    }

    private void Start()
    {
        // 锁定光标到游戏窗口并隐藏
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        CheckGround();
        if (Input.GetKeyDown(KeyCode.Space) && (mazeRenderer.reachedEnd ? true : IsGrounded))
        {
            Jump();
        }
        CHandleMouseLook();
        CHandleMovement();
    }

    void CHandleMouseLook()
    {
        // 获取鼠标输入
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // 垂直旋转(上下看)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // 限制上下旋转角度

        // 水平旋转(左右看)
        yRotation += mouseX;

        // 应用旋转
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }

    void CHandleMovement()
    {
        // 获取键盘输入
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // 计算移动方向(基于玩家当前朝向)
        Vector3 move = transform.right * x + transform.forward * z;

        // 应用移动
        transform.position += move * moveSpeed * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontal, 0, vertical).normalized;
        moveDirection = transform.TransformDirection(moveDirection) * moveSpeed;
        moveDirection.y = rb.velocity.y;

        rb.velocity = moveDirection;
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}