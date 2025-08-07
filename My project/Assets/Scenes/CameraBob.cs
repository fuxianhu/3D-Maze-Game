using UnityEngine;


public class PhysicalCameraSway : MonoBehaviour
{
    [Header("�ƶ��ζ�")]
    public float positionSmoothTime = 0.1f;
    public Vector3 moveAmplitude = new Vector3(0.05f, 0.1f, 0.03f);

    [Header("�ӽǻζ�")]
    public float rotationSmoothTime = 0.2f;
    public Vector3 rotationAmplitude = new Vector3(1f, 0.5f, 0.8f);

    [Header("��½���")]
    public float landShakeAmount = 0.3f;
    public float landShakeDuration = 0.5f;

    private Vector3 posVelocity;
    private Vector3 rotVelocity;
    private Vector3 originalPos;
    private Quaternion originalRot;
    private float landTimer;

    void Start()
    {
        originalPos = transform.localPosition;
        originalRot = transform.localRotation;
    }

    void Update()
    {
        // �ƶ�������
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");
        bool isMoving = Mathf.Abs(inputX) > 0.1f || Mathf.Abs(inputY) > 0.1f;

        // �ƶ��ζ�
        Vector3 targetPos = originalPos;
        if (isMoving)
        {
            targetPos += new Vector3(
                Mathf.Sin(Time.time * 10) * moveAmplitude.x * inputY,
                Mathf.Sin(Time.time * 20) * moveAmplitude.y,
                Mathf.Sin(Time.time * 10) * moveAmplitude.z * inputX
            );
        }

        // ��½���
        if (landTimer > 0)
        {
            targetPos.y -= landShakeAmount * (landTimer / landShakeDuration);
            landTimer -= Time.deltaTime;
        }

        transform.localPosition = Vector3.SmoothDamp(
            transform.localPosition,
            targetPos,
            ref posVelocity,
            positionSmoothTime
        );

        // �ӽ���ת
        Quaternion targetRot = originalRot;
        if (isMoving)
        {
            targetRot *= Quaternion.Euler(
                Mathf.Sin(Time.time * 8) * rotationAmplitude.x * inputY,
                Mathf.Sin(Time.time * 4) * rotationAmplitude.y,
                Mathf.Sin(Time.time * 6) * rotationAmplitude.z * inputX
            );
        }

        transform.localRotation = QuaternionUtil.SmoothDamp(
            transform.localRotation,
            targetRot,
            ref rotVelocity,
            rotationSmoothTime
        );
    }

    // �ڽ�ɫ��½ʱ���ô˷���
    public void TriggerLandShake()
    {
        landTimer = landShakeDuration;
    }
}

// �����ࣺ��Ԫ��ƽ������
public static class QuaternionUtil
{
    public static Quaternion SmoothDamp(Quaternion current, Quaternion target, ref Vector3 velocity, float smoothTime)
    {
        Vector3 c = current.eulerAngles;
        Vector3 t = target.eulerAngles;
        return Quaternion.Euler(
            Mathf.SmoothDampAngle(c.x, t.x, ref velocity.x, smoothTime),
            Mathf.SmoothDampAngle(c.y, t.y, ref velocity.y, smoothTime),
            Mathf.SmoothDampAngle(c.z, t.z, ref velocity.z, smoothTime)
        );
    }
}