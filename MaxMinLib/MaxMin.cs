using System.Drawing;
using System.Security.Cryptography;
using MaxMinLib.Extensions;

namespace MaxMinLib;

public class MaxMin
{
    public List<Point> Points { get; set; } = new();
    public List<MaxMinCluster> Clusters { get; set; } = new();
    private int _iterationsCount;

    public void Init(List<Point> points)
    {
        _iterationsCount = 0;
        Points = new List<Point>(points);
        Clusters = new List<MaxMinCluster>
        {
            new()
            {
                Center = points[RandomNumberGenerator.GetInt32(points.Count)],
                Points = points
            }
        };
    }

    public bool Iterate()
    {
        var (maxPoint, maxDistance) = DefineClustersMaxDistancePoint();
        var result = false;

        if (maxPoint is not null)
        {
            foreach (var cluster in Clusters)
            {
                cluster.Points.Clear();
            }
            
            if (_iterationsCount == 0)
            {
                Clusters.Add(new MaxMinCluster()
                {
                    Center = maxPoint.Value
                });
            }
            else
            {
                if (ShouldCreateCluster(maxDistance))
                {
                    Clusters.Add(new MaxMinCluster()
                    {
                        Center = maxPoint.Value
                    });
                }
                else
                {
                    result =  true;
                }
            }

            DistributePoints();

            _iterationsCount++;
        }

        return result;
    }

    private (Point? point, double distance) DefineClustersMaxDistancePoint()
    {
        var maxDistance = double.MinValue;
        Point? maxPoint = null;

        foreach (var cluster in Clusters)
        {
            foreach (var point in cluster.Points)
            {
                var distance = cluster.Center.CountDistance(point);

                if (distance >= maxDistance)
                {
                    maxDistance = distance;
                    maxPoint = point;
                }
            }
        }

        return (maxPoint, maxDistance);
    }

    private bool ShouldCreateCluster(double pointDistance)
    {
        var distance = 0d;
        var count = Clusters.Count * (Clusters.Count - 1) / 2;

        for (var i = 0; i < Clusters.Count - 1; i++)
        {
            for (var j = i + 1; j < Clusters.Count; j++)
            {
                distance += Clusters[i].Center.CountDistance(Clusters[j].Center);
            }
        }

        return pointDistance > distance / count / 2;
    }
    
    private void DistributePoints()
    {
        foreach (var point in Points)
        {
            var minDistance = double.MaxValue;
            MaxMinCluster? minCluster = null;
        
            foreach (var cluster in Clusters)
            {
                var distance = cluster.Center.CountDistance(point);

                if (distance < minDistance)
                {
                    (minCluster, minDistance) = (cluster, distance);
                }
            }

            minCluster?.Points.Add(point);
        }
    }
}