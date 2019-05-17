﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonsGameLib
{
    public class Point : IComparable<Point>, IComparable
    {
        protected Point(int x, int y) { X = x; Y = y; }
        public readonly int X;
        public readonly int Y;
        public static Point operator +(Point p1, Point p2) => Get(p1.X + p2.X, p1.Y + p2.Y);
        public static bool operator <(Point p1, Point p2) => p1.CompareTo(p2) < 0;
        public static bool operator >(Point p1, Point p2) => p1.CompareTo(p2) > 0;
        public override bool Equals(object obj)
        {
            Point other = obj as Point;
            if (other == null) return false;
            return Equals(other);
        }
        public bool Equals(Point other) => other?.X == X && other?.Y == Y;
        public override int GetHashCode()
        {
            unchecked // prevent exceptions for int overflow
            {
                return (X.GetHashCode() * 397) ^ Y.GetHashCode();
            }
        }

        public override string ToString() => $"({X},{Y})";

        public int CompareTo(Point other)
        {
            if (Equals(other)) return 0;
            if (Y < other.Y) return -1;
            else if (Y > other.Y) return 1;
            else if (X < other.X) return -1;
            else return 1;
        }

        public int CompareTo(object obj)
        {
            Point other = obj as Point;
            if (other is null) throw new ArgumentNullException("obj");
            return CompareTo(other);
        }

        //singleton repo
        static Point()
        {
            for (int x = -1; x < 11; x++)
            {
                for (int y = -1; y < 11; y++)
                {
                    _pointSet[x+1,y+1] = new Point(x, y);
                }
            }
        }
        private static Point[,] _pointSet = new Point[12, 12];
        public static Point Get(int x, int y)
        {
            if (x > 11 || x < -1 || y > 11 || y < -1) throw new ArgumentException($"We only support point x/y values from -1 through 11. You gave {x},{y}");
            return _pointSet[x+1, y+1];
        }
    }
}
