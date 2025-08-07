using System;
using System.Collections.Generic;


//MazePrim mazeGenerator = new MazePrim();
//var maze = mazeGenerator.Generate(21, 21); // ����21x21���Թ�

//// ��ӡ�Թ�
//for (int i = 0; i < maze.Count; i++)
//{
//    for (int j = 0; j < maze[i].Count; j++)
//    {
//        Console.Write(maze[i][j] ? "����" : "  ");
//    }
//    Console.WriteLine();
//}


public class MazePrim
{
    public List<List<bool>> Generate(int width, int height)
    {
        // ȷ���ߴ�Ϊ����
        width = width % 2 == 0 ? width + 1 : width;
        height = height % 2 == 0 ? height + 1 : height;

        var maze = new List<List<bool>>();
        var walls = new List<(int, int, int, int)>();
        var random = new Random();

        // ��ʼ���Թ���ȫ����Ϊǽ(true)
        for (int i = 0; i < height; i++)
        {
            var row = new List<bool>();
            for (int j = 0; j < width; j++)
            {
                row.Add(true);
            }
            maze.Add(row);
        }

        // ���ѡ�����
        int startX = 1, startY = 1;
        maze[startY][startX] = false;

        // ���������ڵ�ǽ
        AddWalls(startX, startY, width, height, maze, walls);

        while (walls.Count > 0)
        {
            // ���ѡ��һ��ǽ
            var wall = walls[random.Next(walls.Count)];
            walls.Remove(wall);

            int x = wall.Item1, y = wall.Item2;
            int nx = wall.Item3, ny = wall.Item4;

            if (nx > 0 && nx < width - 1 && ny > 0 && ny < height - 1 && maze[ny][nx])
            {
                // ��ͨǽ�����ڵĵ�Ԫ��
                maze[y][x] = false;
                maze[ny][nx] = false;

                // ����µ�Ԫ�������ǽ
                AddWalls(nx, ny, width, height, maze, walls);
            }
        }

        // ������ںͳ���
        maze[0][1] = false;
        maze[height - 1][width - 2] = false;

        return maze;
    }

    private void AddWalls(int x, int y, int width, int height, List<List<bool>> maze, List<(int, int, int, int)> walls)
    {
        // ��
        if (y > 1 && maze[y - 2][x])
            walls.Add((x, y - 1, x, y - 2));
        // ��
        if (y < height - 2 && maze[y + 2][x])
            walls.Add((x, y + 1, x, y + 2));
        // ��
        if (x > 1 && maze[y][x - 2])
            walls.Add((x - 1, y, x - 2, y));
        // ��
        if (x < width - 2 && maze[y][x + 2])
            walls.Add((x + 1, y, x + 2, y));
    }
}