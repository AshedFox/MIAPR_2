
using System.Drawing;

namespace MaxMinLib.Extensions;

public static class PointExtension
{
    public static double CountDistance(this Point point1, Point point2)
    {
        return Math.Sqrt(Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2));
    }
}