using UnityEngine;

public class PlayerFog : MonoBehaviour
{
    public float visibleRadius = 0.001f; // 可见范围半径
    private Material fogMaterial;

    void Start()
    {
        Debug.Log("PlayerFog Start");
        //// 创建动态遮罩材质（需Shader支持）
        //fogMaterial = new Material(Shader.Find("Custom/SphereFog"));
        //fogMaterial.SetFloat("_Radius", visibleRadius);
        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.Exponential;
        RenderSettings.fogDensity = 0.01f; // 尝试极大值
    }

    void Update()
    {
        // 更新玩家位置到Shader
        //fogMaterial.SetVector("_PlayerPos", transform.position);
    }
}