using UnityEngine;

public class PlayerFog : MonoBehaviour
{
    public float visibleRadius = 0.001f; // �ɼ���Χ�뾶
    private Material fogMaterial;

    void Start()
    {
        Debug.Log("PlayerFog Start");
        //// ������̬���ֲ��ʣ���Shader֧�֣�
        //fogMaterial = new Material(Shader.Find("Custom/SphereFog"));
        //fogMaterial.SetFloat("_Radius", visibleRadius);
        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.Exponential;
        RenderSettings.fogDensity = 0.01f; // ���Լ���ֵ
    }

    void Update()
    {
        // �������λ�õ�Shader
        //fogMaterial.SetVector("_PlayerPos", transform.position);
    }
}