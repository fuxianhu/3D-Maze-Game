using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;


public class MazeRenderer : MonoBehaviour
{
    public GameObject wallPrefab;     // ǽ��Ԥ����
    public float cellSize = 1f;       // ÿ�����ӵĴ�С�������Ϊ5�������ģ�
    public List<List<bool>> mazeData; // �Թ���������
    public bool debugMode = true;     // ��Ϊtrueʱ��ʾWallԤ���壬���ɺ�����
    public Vector3 startPosition;     // �Թ����λ��
    public Vector3 endPosition;       // �Թ��յ�λ��
    public bool reachedEnd = false;   // ����Ƿ񵽴��յ�
    public GameObject player;         // ��Ҷ���
    public float endMaxDis = 1.3f;    // ��ҵ����յ��������
    public GameObject winText;        // ʤ���ı�����
    public FireworkController fireworkController; // �̻�������
    public System.Random random = new System.Random(); // �����������
    public long timestamp = 0; // ʱ���

    public void Start()
    {
        Debug.Log("MazeRenderer Start");
        winText.SetActive(false);

        MazePrim mazePrim = new MazePrim();
        mazeData = mazePrim.Generate(21, 21); // ����һ��21x21���Թ�
        string maze = "";
        for (int i = 0; i < mazeData.Count; i++)
        {
            for (int j = 0; j < mazeData[i].Count; j++)
            {
                maze += mazeData[i][j] ? "#" : " ";
            }
            maze += "\n";
        }
        Debug.Log(maze);
        RenderMaze(); // ��Ⱦ�Թ�

        player.transform.position = startPosition;
        reachedEnd = false;
        Debug.Log("����Ѵ��͵����: " + startPosition);
    }

    private void RenderMaze()
    {
        if (mazeData == null || wallPrefab == null) return;

        int width = mazeData.Count;
        int height = mazeData[0].Count;

        // ��������Թ�
        //foreach (Transform child in transform)
        //{
        //    Destroy(child.gameObject);
        //}

        // �����Թ�����ƫ����������ǽ��ʵ�ʴ�С��
        float offsetX = (width - 1) * 0.5f * 5f;  // X������=5
        float offsetZ = (height - 1) * 0.5f * 5f; // Z������=5

        // ��ʱ��ʾWallԤ���壨��������֤��
        if (debugMode)
        {
            wallPrefab.SetActive(true); // ȷ��Ԥ����ɼ�
        }

        // �����Թ�ǽ��
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 position = new Vector3(
                        x * 5f - offsetX, // X������=5
                        6.5f,             // Y=13/2=6.5��ʹǽ�ڵײ��ڵ��棩
                        y * 5f - offsetZ  // Z������=5
                    );
                if (x == 0 && y == 1)
                {
                    startPosition = position; // �������λ��
                }
                else if (x == width - 1 && y == height - 2)
                {
                    endPosition = position; // �����յ�λ��
                    endPosition.y = 4.55f;  // �յ�λ����΢����һЩ��������Ҵ���
                    Debug.Log("�յ�λ��������: " + endPosition);
                }
                if (mazeData[x][y])
                {
                    GameObject wall = Instantiate(wallPrefab, position, Quaternion.identity, transform);
                    wall.name = $"Wall_{x}_{y}";

                    // ����ǵ���ģʽ�����ɺ���������
                    if (!debugMode)
                    {
                        wall.SetActive(false);
                    }
                }
            }
        }

        // ������Ϻ�����WallԤ���壨���debugMode=true��
        if (debugMode)
        {
            wallPrefab.SetActive(false);
            Debug.Log("Maze������ϣ�WallԤ���������ء�");
        }
    }

    public void Update()
    {
        //Debug.Log($"��ǰ�����յ�: {Vector3.Distance(player.transform.position, endPosition)}");
        if (!reachedEnd && Vector3.Distance(player.transform.position, endPosition) < endMaxDis)
        {
            winText.SetActive(true);
            reachedEnd = true;
            Vector3 targetPos = new Vector3(endPosition.x, 40, endPosition.z);
            fireworkController.PlayAtPosition(targetPos);
            Debug.Log("The player has reached the end of the maze.");
        }

        // ��Ʒ���ɻ���
        // ����Ѿ�ͨ�أ������ɡ�
        //if (!reachedEnd && DateTimeOffset.UtcNow.ToUnixTimeSeconds() - timestamp >= 1)
        if (DateTimeOffset.UtcNow.ToUnixTimeSeconds() - timestamp >= 1)
        {
            timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            // ÿ�����һ�γ��ԣ�������ÿ֡��

            // ʮ��֮һ�ĸ��ʳ��ԣ�������������һ��Ϊǽ�����ɡ�
            if (random.Next(10) == 0)
            {
                int x = random.Next(mazeData.Count);
                int y = random.Next(mazeData[0].Count);
                if (!mazeData[x][y])
                {
                    // �����Թ�����ƫ����������ǽ��ʵ�ʴ�С��
                    float offsetX = (mazeData.Count - 1) * 0.5f * 5f;  // X������=5
                    float offsetZ = (mazeData[0].Count - 1) * 0.5f * 5f; // Z������=5
                    Vector3 position = new Vector3(
                        x * 5f - offsetX, // X������=5
                        5.0f,
                        y * 5f - offsetZ  // Z������=5
                    );
                    
                }
            }
        }
        
    }
}