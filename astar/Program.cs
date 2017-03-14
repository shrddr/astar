using System;
using System.Collections.Generic;
using AStar;

class RandomMap : Map
{
    Point min;
    Point max;
    List<PointPair> walls;
    List<Point> directions;

    public RandomMap(int Size, int WallCount)
    {
        min = new Point(0, 0);
        max = new Point(Size, Size);

        directions = new List<Point>();
        directions.Add(new Point(-1, 0));
        directions.Add(new Point(1, 0));
        directions.Add(new Point(0, -1));
        directions.Add(new Point(0, 1));

        walls = new List<PointPair>();
        Random rnd = new Random();

        for (int i = 0; i < WallCount; i++)
        {
            int x = rnd.Next(0, Size);
            int y = rnd.Next(0, Size);
            Point from = new Point(x, y);

            int d = rnd.Next(0, directions.Count);
            Point to = from + directions[d];

            walls.Add(new PointPair(from, to));
            walls.Add(new PointPair(to, from));
        }
    }

    public void Print()
    {
        for (int y = min.y; y < max.y; y++) 
        {
            for (int x = min.x; x < max.x; x++)
            {
                Console.Write("O");
                if (walls.Find(pp => pp.a.x == x && pp.a.y == y && pp.b.x == x + 1 && pp.b.y == y) != null)
                    Console.Write("|");
                else
                    Console.Write(" ");
            }
            Console.WriteLine("");

            for (int x = min.x; x < max.x; x++)
            {
                if (walls.Find(pp => pp.a.x == x && pp.a.y == y && pp.b.x == x && pp.b.y == y + 1) != null)
                    Console.Write("-");
                else
                    Console.Write(" ");

                Console.Write(" ");
            }
            Console.WriteLine("");
        }
    }

    public List<Point> GetNeighboors(int x, int y)
    {
        Point current = new Point(x, y);

        var list = new List<Point>();

        foreach (var dir in directions)
        {
            var next = current + dir;
            var movement = new PointPair(current, next);

            bool blocked = walls.Contains(movement);
            blocked |= next.x < min.x;
            blocked |= next.x >= max.x;
            blocked |= next.y < min.y;
            blocked |= next.y >= max.y;

            if (!blocked)
                list.Add(current + dir);
        }

        return list;
    }
}

class Program
{
    static void Main(string[] args)
    {
        var SIZE = 6;
        RandomMap map = new RandomMap(SIZE, (int)(SIZE*SIZE*0.8));
        map.Print();

        Solver s = new Solver(map);
        bool result = s.PathExists(new Point(0, 0), new Point(SIZE-1, 0));
        Console.WriteLine(result);

        if (result)
        {
            foreach (Point p in s.path)
                Console.Write(p + " > ");
            Console.Write("YEAH!");
        }

        Console.ReadLine();
    }
}

