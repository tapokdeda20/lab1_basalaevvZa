using System;
using System.IO;

class Program
{
    static string logFile = "log.txt";

    static void Main()
    {
        Console.WriteLine("Введите сторону A:");
        string inputA = Console.ReadLine();

        Console.WriteLine("Введите сторону B:");
        string inputB = Console.ReadLine();

        Console.WriteLine("Введите сторону C:");
        string inputC = Console.ReadLine();

        var result = ProcessTriangle(inputA, inputB, inputC);

        Console.WriteLine("Тип: " + result.type);
        Console.WriteLine("Координаты:");
        foreach (var p in result.points)
        {
            Console.WriteLine($"({p.x}, {p.y})");
        }
    }

    static (string type, (int x, int y)[] points) ProcessTriangle(string aStr, string bStr, string cStr)
    {
        try
        {
            if (!float.TryParse(aStr, out float a) ||
                !float.TryParse(bStr, out float b) ||
                !float.TryParse(cStr, out float c) ||
                a <= 0 || b <= 0 || c <= 0)
            {
                var points = new (int, int)[] { (-2, -2), (-2, -2), (-2, -2) };
                Log("ERROR", aStr, bStr, cStr, "Невалидные данные");
                return ("", points);
            }

            if (a + b <= c || a + c <= b || b + c <= a)
            {
                var points = new (int, int)[] { (-1, -1), (-1, -1), (-1, -1) };
                Log("ERROR", a, b, c, "Не треугольник");
                return ("не треугольник", points);
            }

            string type;

            if (a == b && b == c)
                type = "равносторонний";
            else if (a == b || b == c || a == c)
                type = "равнобедренный";
            else
                type = "разносторонний";

            var coords = CalculateCoordinates(a, b, c);

            Log("INFO", a, b, c, $"{type} | {FormatPoints(coords)}");

            return (type, coords);
        }
        catch (Exception ex)
        {
            var points = new (int, int)[] { (-2, -2), (-2, -2), (-2, -2) };
            Log("ERROR", aStr, bStr, cStr, ex.ToString());
            return ("", points);
        }
    }

    static (int, int)[] CalculateCoordinates(float a, float b, float c)
    {
        // A(0,0), B(c,0)
        float xC = (b * b + c * c - a * a) / (2 * c);
        float yC = (float)Math.Sqrt(Math.Max(0, b * b - xC * xC));

        var A = (x: 0, y: 0);
        var B = (x: (int)c, y: 0);
        var C = (x: (int)xC, y: (int)yC);

        // масштабирование в 100x100
        int max = Math.Max(B.x, Math.Max(C.x, C.y));
        float scale = max > 0 ? 100f / max : 1;

        return new (int, int)[]
        {
            ((int)(A.x * scale), (int)(A.y * scale)),
            ((int)(B.x * scale), (int)(B.y * scale)),
            ((int)(C.x * scale), (int)(C.y * scale))
        };
    }

    static void Log(string level, object a, object b, object c, string message)
    {
        string log = $"{DateTime.Now} [{level}] A={a}, B={b}, C={c} -> {message}";
        Console.WriteLine(log);
        File.AppendAllText(logFile, log + Environment.NewLine);
    }

    static string FormatPoints((int, int)[] pts)
    {
        return $"({pts[0].Item1},{pts[0].Item2}) " +
               $"({pts[1].Item1},{pts[1].Item2}) " +
               $"({pts[2].Item1},{pts[2].Item2})";
    }
}