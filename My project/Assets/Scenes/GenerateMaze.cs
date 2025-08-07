using System;
using System.Collections.Generic;


//MazePrim mazeGenerator = new MazePrim();
//var maze = mazeGenerator.Generate(21, 21); // 生成21x21的迷宫

//// 打印迷宫
//for (int i = 0; i < maze.Count; i++)
//{
//    for (int j = 0; j < maze[i].Count; j++)
//    {
//        Console.Write(maze[i][j] ? "██" : "  ");
//    }
//    Console.WriteLine();
//}


public class MazePrim
{
    public List<List<bool>> Generate(int width, int height)
    {
        // 确保尺寸为奇数
        width = width % 2 == 0 ? width + 1 : width;
        height = height % 2 == 0 ? height + 1 : height;

        var maze = new List<List<bool>>();
        var walls = new List<(int, int, int, int)>();
        var random = new Random();

        // 初始化迷宫，全部设为墙(true)
        for (int i = 0; i < height; i++)
        {
            var row = new List<bool>();
            for (int j = 0; j < width; j++)
            {
                row.Add(true);
            }
            maze.Add(row);
        }

        // 随机选择起点
        int startX = 1, startY = 1;
        maze[startY][startX] = false;

        // 添加起点相邻的墙
        AddWalls(startX, startY, width, height, maze, walls);

        while (walls.Count > 0)
        {
            // 随机选择一堵墙
            var wall = walls[random.Next(walls.Count)];
            walls.Remove(wall);

            int x = wall.Item1, y = wall.Item2;
            int nx = wall.Item3, ny = wall.Item4;

            if (nx > 0 && nx < width - 1 && ny > 0 && ny < height - 1 && maze[ny][nx])
            {
                // 打通墙和相邻的单元格
                maze[y][x] = false;
                maze[ny][nx] = false;

                // 添加新单元格的相邻墙
                AddWalls(nx, ny, width, height, maze, walls);
            }
        }

        // 设置入口和出口
        maze[0][1] = false;
        maze[height - 1][width - 2] = false;

        return maze;
    }

    private void AddWalls(int x, int y, int width, int height, List<List<bool>> maze, List<(int, int, int, int)> walls)
    {
        // 上
        if (y > 1 && maze[y - 2][x])
            walls.Add((x, y - 1, x, y - 2));
        // 下
        if (y < height - 2 && maze[y + 2][x])
            walls.Add((x, y + 1, x, y + 2));
        // 左
        if (x > 1 && maze[y][x - 2])
            walls.Add((x - 1, y, x - 2, y));
        // 右
        if (x < width - 2 && maze[y][x + 2])
            walls.Add((x + 1, y, x + 2, y));
    }
}