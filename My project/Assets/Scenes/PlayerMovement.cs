using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private float moveSpeed = 5f;
    private float jumpForce = 7f;

    private Rigidbody rb;
    private BoxCollider childCollider;

    [Header("Raycast Settings")]
    [SerializeField] private float raycastDistance = 0.4f; // ���߼�����
    [SerializeField] private LayerMask groundLayer; // ����㼶

    [Header("Foot Positions")]
    [SerializeField] private Transform leftFoot; // ���λ��
    [SerializeField] private Transform rightFoot; // �ҽ�λ��

    [Header("Debug")]
    [SerializeField] private bool showDebugRays = true; // �Ƿ���ʾ��������
    [SerializeField] private Color rayColor = Color.green; // ������ɫ

    [Header("Camera Settings")]
    public Transform playerCamera;

    private float xRotation = 0f;
    private float yRotation = 90f;

    [Header("Other")]
    public MazeRenderer mazeRenderer;

    // �Ƿ��ڵ����ϣ����ԣ����ⲿ���ʣ�
    public bool IsGrounded { get; private set; }

    private void CheckGround()
    {
        // �����ֻ���Ƿ�������һֻ�ڵ�����
        bool leftFootGrounded = CheckSingleFoot(leftFoot);
        bool rightFootGrounded = CheckSingleFoot(rightFoot);

        IsGrounded = leftFootGrounded || rightFootGrounded;

        // ������Ϣ
        if (showDebugRays)
        {
            Debug.DrawRay(leftFoot.position, Vector3.down * raycastDistance, rayColor);
            Debug.DrawRay(rightFoot.position, Vector3.down * raycastDistance, rayColor);
        }
    }

    private bool CheckSingleFoot(Transform foot)
    {
        // ʹ�� SphereCast ��� Raycast�������������³����
        RaycastHit hit;
        float sphereRadius = 0.15f;
        if (Physics.SphereCast(foot.position, sphereRadius, Vector3.down, out hit, raycastDistance, groundLayer))
        {
            return true;
        }
        return false;
    }

    // �ڱ༭���п��ӻ�����
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
        // ������굽��Ϸ���ڲ�����
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
        // ��ȡ�������
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // ��ֱ��ת(���¿�)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // ����������ת�Ƕ�

        // ˮƽ��ת(���ҿ�)
        yRotation += mouseX;

        // Ӧ����ת
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }

    void CHandleMovement()
    {
        // ��ȡ��������
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // �����ƶ�����(������ҵ�ǰ����)
        Vector3 move = transform.right * x + transform.forward * z;

        // Ӧ���ƶ�
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