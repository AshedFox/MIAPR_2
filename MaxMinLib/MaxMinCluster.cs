using System.Drawing;

namespace MaxMinLib;

public class MaxMinCluster
{
    public Point Center { get; set; }
    public List<Point> Points { get; set; } = new ();
}