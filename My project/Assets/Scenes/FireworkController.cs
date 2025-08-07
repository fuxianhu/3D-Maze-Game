using UnityEngine;

public class FireworkController : MonoBehaviour
{
    [Header("必填项")]
    public ParticleSystem fireworkEffect; // 拖拽绑定或自动获取

    void Start()
    {
        // 自动获取组件（如果Inspector未绑定）
        if (fireworkEffect == null)
        {
            fireworkEffect = GetComponent<ParticleSystem>();
        }

        // 最终检查
        if (fireworkEffect == null)
        {
            Debug.LogError("ParticleSystem缺失！", this);
        }
    }

    public void PlayAtPosition(Vector3 position)
    {
        if (fireworkEffect == null) return; // 防止空引用

        transform.position = position;
        fireworkEffect.Play();
    }
}