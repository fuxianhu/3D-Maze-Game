using UnityEngine;

public class FireworkController : MonoBehaviour
{
    [Header("������")]
    public ParticleSystem fireworkEffect; // ��ק�󶨻��Զ���ȡ

    void Start()
    {
        // �Զ���ȡ��������Inspectorδ�󶨣�
        if (fireworkEffect == null)
        {
            fireworkEffect = GetComponent<ParticleSystem>();
        }

        // ���ռ��
        if (fireworkEffect == null)
        {
            Debug.LogError("ParticleSystemȱʧ��", this);
        }
    }

    public void PlayAtPosition(Vector3 position)
    {
        if (fireworkEffect == null) return; // ��ֹ������

        transform.position = position;
        fireworkEffect.Play();
    }
}