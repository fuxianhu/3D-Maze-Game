using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;


public class MazeRenderer : MonoBehaviour
{
    public GameObject wallPrefab;     // 墙壁预制体
    public float cellSize = 1f;       // 每个格子的大小（需调整为5，见下文）
    public List<List<bool>> mazeData; // 迷宫布尔数组
    public bool debugMode = true;     // 设为true时显示Wall预制体，生成后隐藏
    public Vector3 startPosition;     // 迷宫起点位置
    public Vector3 endPosition;       // 迷宫终点位置
    public bool reachedEnd = false;   // 玩家是否到达终点
    public GameObject player;         // 玩家对象
    public float endMaxDis = 1.3f;    // 玩家到达终点的最大距离
    public GameObject winText;        // 胜利文本对象
    public FireworkController fireworkController; // 烟花控制器
    public System.Random random = new System.Random(); // 随机数生成器
    public long timestamp = 0; // 时间戳

    public void Start()
    {
        Debug.Log("MazeRenderer Start");
        winText.SetActive(false);

        MazePrim mazePrim = new MazePrim();
        mazeData = mazePrim.Generate(21, 21); // 生成一个21x21的迷宫
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
        RenderMaze(); // 渲染迷宫

        player.transform.position = startPosition;
        reachedEnd = false;
        Debug.Log("玩家已传送到起点: " + startPosition);
    }

    private void RenderMaze()
    {
        if (mazeData == null || wallPrefab == null) return;

        int width = mazeData.Count;
        int height = mazeData[0].Count;

        // 清空现有迷宫
        //foreach (Transform child in transform)
        //{
        //    Destroy(child.gameObject);
        //}

        // 计算迷宫中心偏移量（考虑墙壁实际大小）
        float offsetX = (width - 1) * 0.5f * 5f;  // X轴缩放=5
        float offsetZ = (height - 1) * 0.5f * 5f; // Z轴缩放=5

        // 临时显示Wall预制体（仅用于验证）
        if (debugMode)
        {
            wallPrefab.SetActive(true); // 确保预制体可见
        }

        // 生成迷宫墙壁
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 position = new Vector3(
                        x * 5f - offsetX, // X轴缩放=5
                        6.5f,             // Y=13/2=6.5（使墙壁底部在地面）
                        y * 5f - offsetZ  // Z轴缩放=5
                    );
                if (x == 0 && y == 1)
                {
                    startPosition = position; // 设置起点位置
                }
                else if (x == width - 1 && y == height - 2)
                {
                    endPosition = position; // 设置终点位置
                    endPosition.y = 4.55f;  // 终点位置稍微降低一些，方便玩家触发
                    Debug.Log("终点位置已设置: " + endPosition);
                }
                if (mazeData[x][y])
                {
                    GameObject wall = Instantiate(wallPrefab, position, Quaternion.identity, transform);
                    wall.name = $"Wall_{x}_{y}";

                    // 如果是调试模式，生成后立即隐藏
                    if (!debugMode)
                    {
                        wall.SetActive(false);
                    }
                }
            }
        }

        // 生成完毕后隐藏Wall预制体（如果debugMode=true）
        if (debugMode)
        {
            wallPrefab.SetActive(false);
            Debug.Log("Maze生成完毕，Wall预制体已隐藏。");
        }
    }

    public void Update()
    {
        //Debug.Log($"当前距离终点: {Vector3.Distance(player.transform.position, endPosition)}");
        if (!reachedEnd && Vector3.Distance(player.transform.position, endPosition) < endMaxDis)
        {
            winText.SetActive(true);
            reachedEnd = true;
            Vector3 targetPos = new Vector3(endPosition.x, 40, endPosition.z);
            fireworkController.PlayAtPosition(targetPos);
            Debug.Log("The player has reached the end of the maze.");
        }

        // 物品生成机制
        // 如果已经通关，则不生成。
        //if (!reachedEnd && DateTimeOffset.UtcNow.ToUnixTimeSeconds() - timestamp >= 1)
        if (DateTimeOffset.UtcNow.ToUnixTimeSeconds() - timestamp >= 1)
        {
            timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            // 每秒进行一次尝试，而不是每帧。

            // 十分之一的概率尝试，如果随机到的这一格不为墙则生成。
            if (random.Next(10) == 0)
            {
                int x = random.Next(mazeData.Count);
                int y = random.Next(mazeData[0].Count);
                if (!mazeData[x][y])
                {
                    // 计算迷宫中心偏移量（考虑墙壁实际大小）
                    float offsetX = (mazeData.Count - 1) * 0.5f * 5f;  // X轴缩放=5
                    float offsetZ = (mazeData[0].Count - 1) * 0.5f * 5f; // Z轴缩放=5
                    Vector3 position = new Vector3(
                        x * 5f - offsetX, // X轴缩放=5
                        5.0f,
                        y * 5f - offsetZ  // Z轴缩放=5
                    );
                    
                }
            }
        }
        
    }
}