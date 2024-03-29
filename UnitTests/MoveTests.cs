﻿using System;
using System.Collections.Generic;
using System.Linq;
using AmazonsGameLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class MoveTests
    {
        [TestMethod]
        public void PointsOutFrom()
        {
            HashSet<Point> answer = new HashSet<Point>() { Point.Get(0, 4), Point.Get(0, 5), Point.Get(1, 4), Point.Get(2, 5), Point.Get(3, 6), Point.Get(4, 7), Point.Get(5, 8), Point.Get(1, 3), Point.Get(2, 3), Point.Get(3, 3), Point.Get(4, 3), Point.Get(5, 3), Point.Get(6, 3), Point.Get(7, 3), Point.Get(8, 3), Point.Get(0, 2), Point.Get(0, 1), Point.Get(0, 0), Point.Get(1, 2), Point.Get(2, 1), };
            PieceGrid grid = new PieceGrid(10, PieceHelpers.GetInitialAmazonPositions(10));
            IEnumerable<Point> points = grid.GetOpenPointsOutFrom(Point.Get(0, 3));
            Assert.IsTrue(points.ToSet().SetEquals(answer));
        }

        [TestMethod]
        public void PointsOutFrom2()
        {
            HashSet<Point> answer = new HashSet<Point>() { Point.Get(9, 0), Point.Get(8, 1), Point.Get(7, 2), Point.Get(6, 3), Point.Get(5, 0), Point.Get(5, 1), Point.Get(5, 2), Point.Get(5, 3), Point.Get(1, 0), Point.Get(2, 1), Point.Get(3, 2), Point.Get(4, 3), Point.Get(9, 4), Point.Get(8, 4), Point.Get(7, 4), Point.Get(6, 4), Point.Get(0, 4), Point.Get(1, 4), Point.Get(2, 4), Point.Get(3, 4), Point.Get(4, 4), Point.Get(9, 8), Point.Get(8, 7), Point.Get(7, 6), Point.Get(6, 5), Point.Get(5, 9), Point.Get(5, 8), Point.Get(5, 7), Point.Get(5, 6), Point.Get(5, 5), Point.Get(0, 9), Point.Get(1, 8), Point.Get(2, 7), Point.Get(3, 6), Point.Get(4, 5), };
            PieceGrid grid = new PieceGrid(10, PieceHelpers.GetInitialAmazonPositions(10));
            grid.PointPieces[Point.Get(0, 3)] = Open.Get();
            grid.PointPieces[Point.Get(5, 4)] = AmazonPlayer1.Get();
            IEnumerable<Point> points = grid.GetOpenPointsOutFrom(Point.Get(5, 4));
            Assert.IsTrue(points.ToSet().SetEquals(answer));
        }

        [TestMethod]
        public void PointsOutFrom3()
        {
            HashSet<Point> answer = new HashSet<Point>() { };
            PieceGrid grid = new PieceGrid(10, PieceHelpers.GetInitialAmazonPositions(10));
            grid.PointPieces[Point.Get(0, 4)] = ArrowPlayer1.Get();
            grid.PointPieces[Point.Get(1, 4)] = ArrowPlayer1.Get();
            grid.PointPieces[Point.Get(1, 3)] = ArrowPlayer1.Get();
            grid.PointPieces[Point.Get(1, 2)] = ArrowPlayer1.Get();
            grid.PointPieces[Point.Get(0, 2)] = ArrowPlayer1.Get();
            IEnumerable<Point> points = grid.GetOpenPointsOutFrom(Point.Get(0, 3));
            Assert.IsTrue(points.ToSet().SetEquals(answer));
        }

        [TestMethod]
        public void PointsOutFromArrow()
        {
            HashSet<Point> answer = new HashSet<Point>() { Point.Get(5, 1), Point.Get(4, 2), Point.Get(3, 1), Point.Get(3, 2), Point.Get(0, 0), Point.Get(1, 1), Point.Get(2, 2), Point.Get(8, 3), Point.Get(7, 3), Point.Get(6, 3), Point.Get(5, 3), Point.Get(4, 3), Point.Get(0, 3), Point.Get(1, 3), Point.Get(2, 3), Point.Get(9, 9), Point.Get(8, 8), Point.Get(7, 7), Point.Get(6, 6), Point.Get(5, 5), Point.Get(4, 4), Point.Get(3, 8), Point.Get(3, 7), Point.Get(3, 6), Point.Get(3, 5), Point.Get(3, 4), Point.Get(1, 5), Point.Get(2, 4), };
            PieceGrid grid = new PieceGrid(10, PieceHelpers.GetInitialAmazonPositions(10));
            IEnumerable<Point> points = grid.GetOpenPointsOutFrom(Point.Get(3, 3), Point.Get(0,3));
            Assert.IsTrue(points.Contains(Point.Get(0, 3)));
            //string code = GetPointsString(points);
            Assert.IsTrue(points.ToSet().SetEquals(answer));
        }

        [TestMethod]
        public void ArrowsOutFromPoint()
        {
            HashSet<Point> answer = new HashSet<Point>() { Point.Get(3, 7), Point.Get(4, 6), Point.Get(1, 4), Point.Get(7, 4) };
            PieceGrid grid = GetPieceGrid();
            IEnumerable<Point> points = grid.GetArrowsOutFrom(Point.Get(4, 7), Owner.Player2, false);
            Assert.IsTrue(points.ToSet().SetEquals(answer));
        }

        [TestMethod]
        public void ArrowsOutFromPoint2()
        {
            HashSet<Point> answer = new HashSet<Point>() { Point.Get(3, 7), Point.Get(4, 6), Point.Get(1, 4), Point.Get(7, 4), Point.Get(2, 7), Point.Get(4, 9), Point.Get(6, 7) };
            PieceGrid grid = GetPieceGrid();
            IEnumerable<Point> points = grid.GetArrowsOutFrom(Point.Get(4, 7), Owner.Player2, true);
            Assert.IsTrue(points.ToSet().SetEquals(answer));
        }

        [TestMethod]
        public void ArrowsOutFromPoint3()
        {
            HashSet<Point> answer = new HashSet<Point>() { Point.Get(3, 7), Point.Get(4, 6), Point.Get(1, 4), Point.Get(7, 4), Point.Get(2, 7), Point.Get(4, 9), Point.Get(6, 7), Point.Get(3,8), Point.Get(4,8), Point.Get(5,7), Point.Get(7,7), Point.Get(8,7), Point.Get(2,9), Point.Get(9,2) };
            PieceGrid grid = GetPieceGrid();
            IEnumerable<Point> points = grid.GetArrowsOutFrom(Point.Get(4, 7), Owner.None, true);
            Assert.IsTrue(points.ToSet().SetEquals(answer));
        }

        [TestMethod]
        public void ReverseMovesFrom()
        {
            HashSet<Move> answer = new HashSet<Move>() { new Move(Point.Get(6, 0), Point.Get(6, 1), Point.Get(6, 4)), new Move(Point.Get(5, 0), Point.Get(6, 1), Point.Get(6, 4)), new Move(Point.Get(9, 1), Point.Get(6, 1), Point.Get(6, 4)), new Move(Point.Get(8, 1), Point.Get(6, 1), Point.Get(6, 4)), new Move(Point.Get(7, 1), Point.Get(6, 1), Point.Get(6, 4)), new Move(Point.Get(4, 1), Point.Get(6, 1), Point.Get(6, 4)), new Move(Point.Get(6, 5), Point.Get(6, 1), Point.Get(6, 4)), new Move(Point.Get(6, 4), Point.Get(6, 1), Point.Get(6, 4)), new Move(Point.Get(6, 3), Point.Get(6, 1), Point.Get(6, 4)), new Move(Point.Get(6, 2), Point.Get(6, 1), Point.Get(6, 4)), new Move(Point.Get(2, 5), Point.Get(6, 1), Point.Get(6, 4)), new Move(Point.Get(3, 4), Point.Get(6, 1), Point.Get(6, 4)), new Move(Point.Get(4, 3), Point.Get(6, 1), Point.Get(6, 4)), new Move(Point.Get(5, 2), Point.Get(6, 1), Point.Get(6, 4)), };
            PieceGrid grid = GetPieceGrid();
            IEnumerable<Move> moves = grid.GetReverseMovesFromPoint(Point.Get(6, 1), Owner.Player2);
            string code = GetMoveString(moves);
            Assert.IsFalse(moves.Any(m => m.Origin.Equals(Point.Get(5, 1))));
            Assert.IsTrue(moves.ToSet().SetEquals(answer));
        }

        [TestMethod]
        public void PointsOutFromArrow2()
        {
            HashSet<Point> answer = new HashSet<Point>() { Point.Get(5, 0), Point.Get(4, 1), Point.Get(3, 1), Point.Get(1, 0), Point.Get(2, 1), Point.Get(9, 2), Point.Get(8, 2), Point.Get(7, 2), Point.Get(6, 2), Point.Get(5, 2), Point.Get(4, 2), Point.Get(0, 2), Point.Get(1, 2), Point.Get(2, 2), Point.Get(9, 8), Point.Get(8, 7), Point.Get(7, 6), Point.Get(6, 5), Point.Get(5, 4), Point.Get(4, 3), Point.Get(3, 8), Point.Get(3, 7), Point.Get(3, 6), Point.Get(3, 5), Point.Get(3, 4), Point.Get(3, 3), Point.Get(0, 5), Point.Get(1, 4), Point.Get(2, 3), };
            PieceGrid grid = new PieceGrid(10, PieceHelpers.GetInitialAmazonPositions(10));
            grid.PointPieces[Point.Get(0, 3)] = Open.Get();
            grid.PointPieces[Point.Get(5, 4)] = AmazonPlayer1.Get();
            IEnumerable<Point> points = grid.GetOpenPointsOutFrom(Point.Get(3, 2), Point.Get(5, 4));
            //string code = GetPointsString(points);
            Assert.IsTrue(points.ToSet().SetEquals(answer));
        }

        [TestMethod]
        public void MovesFrom()
        {
            HashSet<Move> answer = new HashSet<Move>() { new Move(Point.Get(0,3), Point.Get(2, 1), Point.Get(2, 0)), new Move(Point.Get(0,3), Point.Get(2, 1), Point.Get(1, 0)), new Move(Point.Get(0,3), Point.Get(2, 1), Point.Get(9, 1)), new Move(Point.Get(0,3), Point.Get(2, 1), Point.Get(8, 1)), new Move(Point.Get(0,3), Point.Get(2, 1), Point.Get(7, 1)), new Move(Point.Get(0,3), Point.Get(2, 1), Point.Get(6, 1)), new Move(Point.Get(0,3), Point.Get(2, 1), Point.Get(5, 1)), new Move(Point.Get(0,3), Point.Get(2, 1), Point.Get(4, 1)), new Move(Point.Get(0,3), Point.Get(2, 1), Point.Get(3, 1)), new Move(Point.Get(0,3), Point.Get(2, 1), Point.Get(0, 1)), new Move(Point.Get(0,3), Point.Get(2, 1), Point.Get(1, 1)), new Move(Point.Get(0,3), Point.Get(2, 1), Point.Get(9, 8)), new Move(Point.Get(0,3), Point.Get(2, 1), Point.Get(8, 7)), new Move(Point.Get(0,3), Point.Get(2, 1), Point.Get(7, 6)), new Move(Point.Get(0,3), Point.Get(2, 1), Point.Get(6, 5)), new Move(Point.Get(0,3), Point.Get(2, 1), Point.Get(5, 4)), new Move(Point.Get(0,3), Point.Get(2, 1), Point.Get(4, 3)), new Move(Point.Get(0,3), Point.Get(2, 1), Point.Get(3, 2)), new Move(Point.Get(0,3), Point.Get(2, 1), Point.Get(2, 9)), new Move(Point.Get(0,3), Point.Get(2, 1), Point.Get(2, 8)), new Move(Point.Get(0,3), Point.Get(2, 1), Point.Get(2, 7)), new Move(Point.Get(0,3), Point.Get(2, 1), Point.Get(2, 6)), new Move(Point.Get(0,3), Point.Get(2, 1), Point.Get(2, 5)), new Move(Point.Get(0,3), Point.Get(2, 1), Point.Get(2, 4)), new Move(Point.Get(0,3), Point.Get(2, 1), Point.Get(2, 3)), new Move(Point.Get(0,3), Point.Get(2, 1), Point.Get(2, 2)), new Move(Point.Get(0,3), Point.Get(2, 1), Point.Get(0, 3)), new Move(Point.Get(0,3), Point.Get(2, 1), Point.Get(1, 2)), new Move(Point.Get(0,3), Point.Get(1, 2), Point.Get(2, 1)), new Move(Point.Get(0,3), Point.Get(1, 2), Point.Get(1, 0)), new Move(Point.Get(0,3), Point.Get(1, 2), Point.Get(1, 1)), new Move(Point.Get(0,3), Point.Get(1, 2), Point.Get(0, 1)), new Move(Point.Get(0,3), Point.Get(1, 2), Point.Get(9, 2)), new Move(Point.Get(0,3), Point.Get(1, 2), Point.Get(8, 2)), new Move(Point.Get(0,3), Point.Get(1, 2), Point.Get(7, 2)), new Move(Point.Get(0,3), Point.Get(1, 2), Point.Get(6, 2)), new Move(Point.Get(0,3), Point.Get(1, 2), Point.Get(5, 2)), new Move(Point.Get(0,3), Point.Get(1, 2), Point.Get(4, 2)), new Move(Point.Get(0,3), Point.Get(1, 2), Point.Get(3, 2)), new Move(Point.Get(0,3), Point.Get(1, 2), Point.Get(2, 2)), new Move(Point.Get(0,3), Point.Get(1, 2), Point.Get(0, 2)), new Move(Point.Get(0,3), Point.Get(1, 2), Point.Get(8, 9)), new Move(Point.Get(0,3), Point.Get(1, 2), Point.Get(7, 8)), new Move(Point.Get(0,3), Point.Get(1, 2), Point.Get(6, 7)), new Move(Point.Get(0,3), Point.Get(1, 2), Point.Get(5, 6)), new Move(Point.Get(0,3), Point.Get(1, 2), Point.Get(4, 5)), new Move(Point.Get(0,3), Point.Get(1, 2), Point.Get(3, 4)), new Move(Point.Get(0,3), Point.Get(1, 2), Point.Get(2, 3)), new Move(Point.Get(0,3), Point.Get(1, 2), Point.Get(1, 9)), new Move(Point.Get(0,3), Point.Get(1, 2), Point.Get(1, 8)), new Move(Point.Get(0,3), Point.Get(1, 2), Point.Get(1, 7)), new Move(Point.Get(0,3), Point.Get(1, 2), Point.Get(1, 6)), new Move(Point.Get(0,3), Point.Get(1, 2), Point.Get(1, 5)), new Move(Point.Get(0,3), Point.Get(1, 2), Point.Get(1, 4)), new Move(Point.Get(0,3), Point.Get(1, 2), Point.Get(1, 3)), new Move(Point.Get(0,3), Point.Get(1, 2), Point.Get(0, 3)), new Move(Point.Get(0,3), Point.Get(0, 0), Point.Get(2, 0)), new Move(Point.Get(0,3), Point.Get(0, 0), Point.Get(1, 0)), new Move(Point.Get(0,3), Point.Get(0, 0), Point.Get(9, 9)), new Move(Point.Get(0,3), Point.Get(0, 0), Point.Get(8, 8)), new Move(Point.Get(0,3), Point.Get(0, 0), Point.Get(7, 7)), new Move(Point.Get(0,3), Point.Get(0, 0), Point.Get(6, 6)), new Move(Point.Get(0,3), Point.Get(0, 0), Point.Get(5, 5)), new Move(Point.Get(0,3), Point.Get(0, 0), Point.Get(4, 4)), new Move(Point.Get(0,3), Point.Get(0, 0), Point.Get(3, 3)), new Move(Point.Get(0,3), Point.Get(0, 0), Point.Get(2, 2)), new Move(Point.Get(0,3), Point.Get(0, 0), Point.Get(1, 1)), new Move(Point.Get(0,3), Point.Get(0, 0), Point.Get(0, 5)), new Move(Point.Get(0,3), Point.Get(0, 0), Point.Get(0, 4)), new Move(Point.Get(0,3), Point.Get(0, 0), Point.Get(0, 3)), new Move(Point.Get(0,3), Point.Get(0, 0), Point.Get(0, 2)), new Move(Point.Get(0,3), Point.Get(0, 0), Point.Get(0, 1)), new Move(Point.Get(0,3), Point.Get(0, 1), Point.Get(1, 0)), new Move(Point.Get(0,3), Point.Get(0, 1), Point.Get(0, 0)), new Move(Point.Get(0,3), Point.Get(0, 1), Point.Get(9, 1)), new Move(Point.Get(0,3), Point.Get(0, 1), Point.Get(8, 1)), new Move(Point.Get(0,3), Point.Get(0, 1), Point.Get(7, 1)), new Move(Point.Get(0,3), Point.Get(0, 1), Point.Get(6, 1)), new Move(Point.Get(0,3), Point.Get(0, 1), Point.Get(5, 1)), new Move(Point.Get(0,3), Point.Get(0, 1), Point.Get(4, 1)), new Move(Point.Get(0,3), Point.Get(0, 1), Point.Get(3, 1)), new Move(Point.Get(0,3), Point.Get(0, 1), Point.Get(2, 1)), new Move(Point.Get(0,3), Point.Get(0, 1), Point.Get(1, 1)), new Move(Point.Get(0,3), Point.Get(0, 1), Point.Get(8, 9)), new Move(Point.Get(0,3), Point.Get(0, 1), Point.Get(7, 8)), new Move(Point.Get(0,3), Point.Get(0, 1), Point.Get(6, 7)), new Move(Point.Get(0,3), Point.Get(0, 1), Point.Get(5, 6)), new Move(Point.Get(0,3), Point.Get(0, 1), Point.Get(4, 5)), new Move(Point.Get(0,3), Point.Get(0, 1), Point.Get(3, 4)), new Move(Point.Get(0,3), Point.Get(0, 1), Point.Get(2, 3)), new Move(Point.Get(0,3), Point.Get(0, 1), Point.Get(1, 2)), new Move(Point.Get(0,3), Point.Get(0, 1), Point.Get(0, 5)), new Move(Point.Get(0,3), Point.Get(0, 1), Point.Get(0, 4)), new Move(Point.Get(0,3), Point.Get(0, 1), Point.Get(0, 3)), new Move(Point.Get(0,3), Point.Get(0, 1), Point.Get(0, 2)), new Move(Point.Get(0,3), Point.Get(0, 2), Point.Get(2, 0)), new Move(Point.Get(0,3), Point.Get(0, 2), Point.Get(1, 1)), new Move(Point.Get(0,3), Point.Get(0, 2), Point.Get(0, 0)), new Move(Point.Get(0,3), Point.Get(0, 2), Point.Get(0, 1)), new Move(Point.Get(0,3), Point.Get(0, 2), Point.Get(9, 2)), new Move(Point.Get(0,3), Point.Get(0, 2), Point.Get(8, 2)), new Move(Point.Get(0,3), Point.Get(0, 2), Point.Get(7, 2)), new Move(Point.Get(0,3), Point.Get(0, 2), Point.Get(6, 2)), new Move(Point.Get(0,3), Point.Get(0, 2), Point.Get(5, 2)), new Move(Point.Get(0,3), Point.Get(0, 2), Point.Get(4, 2)), new Move(Point.Get(0,3), Point.Get(0, 2), Point.Get(3, 2)), new Move(Point.Get(0,3), Point.Get(0, 2), Point.Get(2, 2)), new Move(Point.Get(0,3), Point.Get(0, 2), Point.Get(1, 2)), new Move(Point.Get(0,3), Point.Get(0, 2), Point.Get(7, 9)), new Move(Point.Get(0,3), Point.Get(0, 2), Point.Get(6, 8)), new Move(Point.Get(0,3), Point.Get(0, 2), Point.Get(5, 7)), new Move(Point.Get(0,3), Point.Get(0, 2), Point.Get(4, 6)), new Move(Point.Get(0,3), Point.Get(0, 2), Point.Get(3, 5)), new Move(Point.Get(0,3), Point.Get(0, 2), Point.Get(2, 4)), new Move(Point.Get(0,3), Point.Get(0, 2), Point.Get(1, 3)), new Move(Point.Get(0,3), Point.Get(0, 2), Point.Get(0, 5)), new Move(Point.Get(0,3), Point.Get(0, 2), Point.Get(0, 4)), new Move(Point.Get(0,3), Point.Get(0, 2), Point.Get(0, 3)), new Move(Point.Get(0,3), Point.Get(8, 3), Point.Get(9, 2)), new Move(Point.Get(0,3), Point.Get(8, 3), Point.Get(8, 0)), new Move(Point.Get(0,3), Point.Get(8, 3), Point.Get(8, 1)), new Move(Point.Get(0,3), Point.Get(8, 3), Point.Get(8, 2)), new Move(Point.Get(0,3), Point.Get(8, 3), Point.Get(5, 0)), new Move(Point.Get(0,3), Point.Get(8, 3), Point.Get(6, 1)), new Move(Point.Get(0,3), Point.Get(8, 3), Point.Get(7, 2)), new Move(Point.Get(0,3), Point.Get(8, 3), Point.Get(0, 3)), new Move(Point.Get(0,3), Point.Get(8, 3), Point.Get(1, 3)), new Move(Point.Get(0,3), Point.Get(8, 3), Point.Get(2, 3)), new Move(Point.Get(0,3), Point.Get(8, 3), Point.Get(3, 3)), new Move(Point.Get(0,3), Point.Get(8, 3), Point.Get(4, 3)), new Move(Point.Get(0,3), Point.Get(8, 3), Point.Get(5, 3)), new Move(Point.Get(0,3), Point.Get(8, 3), Point.Get(6, 3)), new Move(Point.Get(0,3), Point.Get(8, 3), Point.Get(7, 3)), new Move(Point.Get(0,3), Point.Get(8, 3), Point.Get(9, 4)), new Move(Point.Get(0,3), Point.Get(8, 3), Point.Get(8, 9)), new Move(Point.Get(0,3), Point.Get(8, 3), Point.Get(8, 8)), new Move(Point.Get(0,3), Point.Get(8, 3), Point.Get(8, 7)), new Move(Point.Get(0,3), Point.Get(8, 3), Point.Get(8, 6)), new Move(Point.Get(0,3), Point.Get(8, 3), Point.Get(8, 5)), new Move(Point.Get(0,3), Point.Get(8, 3), Point.Get(8, 4)), new Move(Point.Get(0,3), Point.Get(8, 3), Point.Get(2, 9)), new Move(Point.Get(0,3), Point.Get(8, 3), Point.Get(3, 8)), new Move(Point.Get(0,3), Point.Get(8, 3), Point.Get(4, 7)), new Move(Point.Get(0,3), Point.Get(8, 3), Point.Get(5, 6)), new Move(Point.Get(0,3), Point.Get(8, 3), Point.Get(6, 5)), new Move(Point.Get(0,3), Point.Get(8, 3), Point.Get(7, 4)), new Move(Point.Get(0,3), Point.Get(7, 3), Point.Get(9, 1)), new Move(Point.Get(0,3), Point.Get(7, 3), Point.Get(8, 2)), new Move(Point.Get(0,3), Point.Get(7, 3), Point.Get(7, 0)), new Move(Point.Get(0,3), Point.Get(7, 3), Point.Get(7, 1)), new Move(Point.Get(0,3), Point.Get(7, 3), Point.Get(7, 2)), new Move(Point.Get(0,3), Point.Get(7, 3), Point.Get(4, 0)), new Move(Point.Get(0,3), Point.Get(7, 3), Point.Get(5, 1)), new Move(Point.Get(0,3), Point.Get(7, 3), Point.Get(6, 2)), new Move(Point.Get(0,3), Point.Get(7, 3), Point.Get(8, 3)), new Move(Point.Get(0,3), Point.Get(7, 3), Point.Get(0, 3)), new Move(Point.Get(0,3), Point.Get(7, 3), Point.Get(1, 3)), new Move(Point.Get(0,3), Point.Get(7, 3), Point.Get(2, 3)), new Move(Point.Get(0,3), Point.Get(7, 3), Point.Get(3, 3)), new Move(Point.Get(0,3), Point.Get(7, 3), Point.Get(4, 3)), new Move(Point.Get(0,3), Point.Get(7, 3), Point.Get(5, 3)), new Move(Point.Get(0,3), Point.Get(7, 3), Point.Get(6, 3)), new Move(Point.Get(0,3), Point.Get(7, 3), Point.Get(9, 5)), new Move(Point.Get(0,3), Point.Get(7, 3), Point.Get(8, 4)), new Move(Point.Get(0,3), Point.Get(7, 3), Point.Get(7, 9)), new Move(Point.Get(0,3), Point.Get(7, 3), Point.Get(7, 8)), new Move(Point.Get(0,3), Point.Get(7, 3), Point.Get(7, 7)), new Move(Point.Get(0,3), Point.Get(7, 3), Point.Get(7, 6)), new Move(Point.Get(0,3), Point.Get(7, 3), Point.Get(7, 5)), new Move(Point.Get(0,3), Point.Get(7, 3), Point.Get(7, 4)), new Move(Point.Get(0,3), Point.Get(7, 3), Point.Get(1, 9)), new Move(Point.Get(0,3), Point.Get(7, 3), Point.Get(2, 8)), new Move(Point.Get(0,3), Point.Get(7, 3), Point.Get(3, 7)), new Move(Point.Get(0,3), Point.Get(7, 3), Point.Get(4, 6)), new Move(Point.Get(0,3), Point.Get(7, 3), Point.Get(5, 5)), new Move(Point.Get(0,3), Point.Get(7, 3), Point.Get(6, 4)), new Move(Point.Get(0,3), Point.Get(6, 3), Point.Get(9, 0)), new Move(Point.Get(0,3), Point.Get(6, 3), Point.Get(8, 1)), new Move(Point.Get(0,3), Point.Get(6, 3), Point.Get(7, 2)), new Move(Point.Get(0,3), Point.Get(6, 3), Point.Get(6, 1)), new Move(Point.Get(0,3), Point.Get(6, 3), Point.Get(6, 2)), new Move(Point.Get(0,3), Point.Get(6, 3), Point.Get(4, 1)), new Move(Point.Get(0,3), Point.Get(6, 3), Point.Get(5, 2)), new Move(Point.Get(0,3), Point.Get(6, 3), Point.Get(8, 3)), new Move(Point.Get(0,3), Point.Get(6, 3), Point.Get(7, 3)), new Move(Point.Get(0,3), Point.Get(6, 3), Point.Get(0, 3)), new Move(Point.Get(0,3), Point.Get(6, 3), Point.Get(1, 3)), new Move(Point.Get(0,3), Point.Get(6, 3), Point.Get(2, 3)), new Move(Point.Get(0,3), Point.Get(6, 3), Point.Get(3, 3)), new Move(Point.Get(0,3), Point.Get(6, 3), Point.Get(4, 3)), new Move(Point.Get(0,3), Point.Get(6, 3), Point.Get(5, 3)), new Move(Point.Get(0,3), Point.Get(6, 3), Point.Get(8, 5)), new Move(Point.Get(0,3), Point.Get(6, 3), Point.Get(7, 4)), new Move(Point.Get(0,3), Point.Get(6, 3), Point.Get(6, 8)), new Move(Point.Get(0,3), Point.Get(6, 3), Point.Get(6, 7)), new Move(Point.Get(0,3), Point.Get(6, 3), Point.Get(6, 6)), new Move(Point.Get(0,3), Point.Get(6, 3), Point.Get(6, 5)), new Move(Point.Get(0,3), Point.Get(6, 3), Point.Get(6, 4)), new Move(Point.Get(0,3), Point.Get(6, 3), Point.Get(0, 9)), new Move(Point.Get(0,3), Point.Get(6, 3), Point.Get(1, 8)), new Move(Point.Get(0,3), Point.Get(6, 3), Point.Get(2, 7)), new Move(Point.Get(0,3), Point.Get(6, 3), Point.Get(3, 6)), new Move(Point.Get(0,3), Point.Get(6, 3), Point.Get(4, 5)), new Move(Point.Get(0,3), Point.Get(6, 3), Point.Get(5, 4)), new Move(Point.Get(0,3), Point.Get(5, 3), Point.Get(8, 0)), new Move(Point.Get(0,3), Point.Get(5, 3), Point.Get(7, 1)), new Move(Point.Get(0,3), Point.Get(5, 3), Point.Get(6, 2)), new Move(Point.Get(0,3), Point.Get(5, 3), Point.Get(5, 0)), new Move(Point.Get(0,3), Point.Get(5, 3), Point.Get(5, 1)), new Move(Point.Get(0,3), Point.Get(5, 3), Point.Get(5, 2)), new Move(Point.Get(0,3), Point.Get(5, 3), Point.Get(2, 0)), new Move(Point.Get(0,3), Point.Get(5, 3), Point.Get(3, 1)), new Move(Point.Get(0,3), Point.Get(5, 3), Point.Get(4, 2)), new Move(Point.Get(0,3), Point.Get(5, 3), Point.Get(8, 3)), new Move(Point.Get(0,3), Point.Get(5, 3), Point.Get(7, 3)), new Move(Point.Get(0,3), Point.Get(5, 3), Point.Get(6, 3)), new Move(Point.Get(0,3), Point.Get(5, 3), Point.Get(0, 3)), new Move(Point.Get(0,3), Point.Get(5, 3), Point.Get(1, 3)), new Move(Point.Get(0,3), Point.Get(5, 3), Point.Get(2, 3)), new Move(Point.Get(0,3), Point.Get(5, 3), Point.Get(3, 3)), new Move(Point.Get(0,3), Point.Get(5, 3), Point.Get(4, 3)), new Move(Point.Get(0,3), Point.Get(5, 3), Point.Get(9, 7)), new Move(Point.Get(0,3), Point.Get(5, 3), Point.Get(8, 6)), new Move(Point.Get(0,3), Point.Get(5, 3), Point.Get(7, 5)), new Move(Point.Get(0,3), Point.Get(5, 3), Point.Get(6, 4)), new Move(Point.Get(0,3), Point.Get(5, 3), Point.Get(5, 9)), new Move(Point.Get(0,3), Point.Get(5, 3), Point.Get(5, 8)), new Move(Point.Get(0,3), Point.Get(5, 3), Point.Get(5, 7)), new Move(Point.Get(0,3), Point.Get(5, 3), Point.Get(5, 6)), new Move(Point.Get(0,3), Point.Get(5, 3), Point.Get(5, 5)), new Move(Point.Get(0,3), Point.Get(5, 3), Point.Get(5, 4)), new Move(Point.Get(0,3), Point.Get(5, 3), Point.Get(0, 8)), new Move(Point.Get(0,3), Point.Get(5, 3), Point.Get(1, 7)), new Move(Point.Get(0,3), Point.Get(5, 3), Point.Get(2, 6)), new Move(Point.Get(0,3), Point.Get(5, 3), Point.Get(3, 5)), new Move(Point.Get(0,3), Point.Get(5, 3), Point.Get(4, 4)), new Move(Point.Get(0,3), Point.Get(4, 3), Point.Get(7, 0)), new Move(Point.Get(0,3), Point.Get(4, 3), Point.Get(6, 1)), new Move(Point.Get(0,3), Point.Get(4, 3), Point.Get(5, 2)), new Move(Point.Get(0,3), Point.Get(4, 3), Point.Get(4, 0)), new Move(Point.Get(0,3), Point.Get(4, 3), Point.Get(4, 1)), new Move(Point.Get(0,3), Point.Get(4, 3), Point.Get(4, 2)), new Move(Point.Get(0,3), Point.Get(4, 3), Point.Get(1, 0)), new Move(Point.Get(0,3), Point.Get(4, 3), Point.Get(2, 1)), new Move(Point.Get(0,3), Point.Get(4, 3), Point.Get(3, 2)), new Move(Point.Get(0,3), Point.Get(4, 3), Point.Get(8, 3)), new Move(Point.Get(0,3), Point.Get(4, 3), Point.Get(7, 3)), new Move(Point.Get(0,3), Point.Get(4, 3), Point.Get(6, 3)), new Move(Point.Get(0,3), Point.Get(4, 3), Point.Get(5, 3)), new Move(Point.Get(0,3), Point.Get(4, 3), Point.Get(0, 3)), new Move(Point.Get(0,3), Point.Get(4, 3), Point.Get(1, 3)), new Move(Point.Get(0,3), Point.Get(4, 3), Point.Get(2, 3)), new Move(Point.Get(0,3), Point.Get(4, 3), Point.Get(3, 3)), new Move(Point.Get(0,3), Point.Get(4, 3), Point.Get(9, 8)), new Move(Point.Get(0,3), Point.Get(4, 3), Point.Get(8, 7)), new Move(Point.Get(0,3), Point.Get(4, 3), Point.Get(7, 6)), new Move(Point.Get(0,3), Point.Get(4, 3), Point.Get(6, 5)), new Move(Point.Get(0,3), Point.Get(4, 3), Point.Get(5, 4)), new Move(Point.Get(0,3), Point.Get(4, 3), Point.Get(4, 9)), new Move(Point.Get(0,3), Point.Get(4, 3), Point.Get(4, 8)), new Move(Point.Get(0,3), Point.Get(4, 3), Point.Get(4, 7)), new Move(Point.Get(0,3), Point.Get(4, 3), Point.Get(4, 6)), new Move(Point.Get(0,3), Point.Get(4, 3), Point.Get(4, 5)), new Move(Point.Get(0,3), Point.Get(4, 3), Point.Get(4, 4)), new Move(Point.Get(0,3), Point.Get(4, 3), Point.Get(0, 7)), new Move(Point.Get(0,3), Point.Get(4, 3), Point.Get(1, 6)), new Move(Point.Get(0,3), Point.Get(4, 3), Point.Get(2, 5)), new Move(Point.Get(0,3), Point.Get(4, 3), Point.Get(3, 4)), new Move(Point.Get(0,3), Point.Get(3, 3), Point.Get(5, 1)), new Move(Point.Get(0,3), Point.Get(3, 3), Point.Get(4, 2)), new Move(Point.Get(0,3), Point.Get(3, 3), Point.Get(3, 1)), new Move(Point.Get(0,3), Point.Get(3, 3), Point.Get(3, 2)), new Move(Point.Get(0,3), Point.Get(3, 3), Point.Get(0, 0)), new Move(Point.Get(0,3), Point.Get(3, 3), Point.Get(1, 1)), new Move(Point.Get(0,3), Point.Get(3, 3), Point.Get(2, 2)), new Move(Point.Get(0,3), Point.Get(3, 3), Point.Get(8, 3)), new Move(Point.Get(0,3), Point.Get(3, 3), Point.Get(7, 3)), new Move(Point.Get(0,3), Point.Get(3, 3), Point.Get(6, 3)), new Move(Point.Get(0,3), Point.Get(3, 3), Point.Get(5, 3)), new Move(Point.Get(0,3), Point.Get(3, 3), Point.Get(4, 3)), new Move(Point.Get(0,3), Point.Get(3, 3), Point.Get(0, 3)), new Move(Point.Get(0,3), Point.Get(3, 3), Point.Get(1, 3)), new Move(Point.Get(0,3), Point.Get(3, 3), Point.Get(2, 3)), new Move(Point.Get(0,3), Point.Get(3, 3), Point.Get(9, 9)), new Move(Point.Get(0,3), Point.Get(3, 3), Point.Get(8, 8)), new Move(Point.Get(0,3), Point.Get(3, 3), Point.Get(7, 7)), new Move(Point.Get(0,3), Point.Get(3, 3), Point.Get(6, 6)), new Move(Point.Get(0,3), Point.Get(3, 3), Point.Get(5, 5)), new Move(Point.Get(0,3), Point.Get(3, 3), Point.Get(4, 4)), new Move(Point.Get(0,3), Point.Get(3, 3), Point.Get(3, 8)), new Move(Point.Get(0,3), Point.Get(3, 3), Point.Get(3, 7)), new Move(Point.Get(0,3), Point.Get(3, 3), Point.Get(3, 6)), new Move(Point.Get(0,3), Point.Get(3, 3), Point.Get(3, 5)), new Move(Point.Get(0,3), Point.Get(3, 3), Point.Get(3, 4)), new Move(Point.Get(0,3), Point.Get(3, 3), Point.Get(1, 5)), new Move(Point.Get(0,3), Point.Get(3, 3), Point.Get(2, 4)), new Move(Point.Get(0,3), Point.Get(2, 3), Point.Get(5, 0)), new Move(Point.Get(0,3), Point.Get(2, 3), Point.Get(4, 1)), new Move(Point.Get(0,3), Point.Get(2, 3), Point.Get(3, 2)), new Move(Point.Get(0,3), Point.Get(2, 3), Point.Get(2, 0)), new Move(Point.Get(0,3), Point.Get(2, 3), Point.Get(2, 1)), new Move(Point.Get(0,3), Point.Get(2, 3), Point.Get(2, 2)), new Move(Point.Get(0,3), Point.Get(2, 3), Point.Get(0, 1)), new Move(Point.Get(0,3), Point.Get(2, 3), Point.Get(1, 2)), new Move(Point.Get(0,3), Point.Get(2, 3), Point.Get(8, 3)), new Move(Point.Get(0,3), Point.Get(2, 3), Point.Get(7, 3)), new Move(Point.Get(0,3), Point.Get(2, 3), Point.Get(6, 3)), new Move(Point.Get(0,3), Point.Get(2, 3), Point.Get(5, 3)), new Move(Point.Get(0,3), Point.Get(2, 3), Point.Get(4, 3)), new Move(Point.Get(0,3), Point.Get(2, 3), Point.Get(3, 3)), new Move(Point.Get(0,3), Point.Get(2, 3), Point.Get(0, 3)), new Move(Point.Get(0,3), Point.Get(2, 3), Point.Get(1, 3)), new Move(Point.Get(0,3), Point.Get(2, 3), Point.Get(8, 9)), new Move(Point.Get(0,3), Point.Get(2, 3), Point.Get(7, 8)), new Move(Point.Get(0,3), Point.Get(2, 3), Point.Get(6, 7)), new Move(Point.Get(0,3), Point.Get(2, 3), Point.Get(5, 6)), new Move(Point.Get(0,3), Point.Get(2, 3), Point.Get(4, 5)), new Move(Point.Get(0,3), Point.Get(2, 3), Point.Get(3, 4)), new Move(Point.Get(0,3), Point.Get(2, 3), Point.Get(2, 9)), new Move(Point.Get(0,3), Point.Get(2, 3), Point.Get(2, 8)), new Move(Point.Get(0,3), Point.Get(2, 3), Point.Get(2, 7)), new Move(Point.Get(0,3), Point.Get(2, 3), Point.Get(2, 6)), new Move(Point.Get(0,3), Point.Get(2, 3), Point.Get(2, 5)), new Move(Point.Get(0,3), Point.Get(2, 3), Point.Get(2, 4)), new Move(Point.Get(0,3), Point.Get(2, 3), Point.Get(0, 5)), new Move(Point.Get(0,3), Point.Get(2, 3), Point.Get(1, 4)), new Move(Point.Get(0,3), Point.Get(1, 3), Point.Get(4, 0)), new Move(Point.Get(0,3), Point.Get(1, 3), Point.Get(3, 1)), new Move(Point.Get(0,3), Point.Get(1, 3), Point.Get(2, 2)), new Move(Point.Get(0,3), Point.Get(1, 3), Point.Get(1, 0)), new Move(Point.Get(0,3), Point.Get(1, 3), Point.Get(1, 1)), new Move(Point.Get(0,3), Point.Get(1, 3), Point.Get(1, 2)), new Move(Point.Get(0,3), Point.Get(1, 3), Point.Get(0, 2)), new Move(Point.Get(0,3), Point.Get(1, 3), Point.Get(8, 3)), new Move(Point.Get(0,3), Point.Get(1, 3), Point.Get(7, 3)), new Move(Point.Get(0,3), Point.Get(1, 3), Point.Get(6, 3)), new Move(Point.Get(0,3), Point.Get(1, 3), Point.Get(5, 3)), new Move(Point.Get(0,3), Point.Get(1, 3), Point.Get(4, 3)), new Move(Point.Get(0,3), Point.Get(1, 3), Point.Get(3, 3)), new Move(Point.Get(0,3), Point.Get(1, 3), Point.Get(2, 3)), new Move(Point.Get(0,3), Point.Get(1, 3), Point.Get(0, 3)), new Move(Point.Get(0,3), Point.Get(1, 3), Point.Get(7, 9)), new Move(Point.Get(0,3), Point.Get(1, 3), Point.Get(6, 8)), new Move(Point.Get(0,3), Point.Get(1, 3), Point.Get(5, 7)), new Move(Point.Get(0,3), Point.Get(1, 3), Point.Get(4, 6)), new Move(Point.Get(0,3), Point.Get(1, 3), Point.Get(3, 5)), new Move(Point.Get(0,3), Point.Get(1, 3), Point.Get(2, 4)), new Move(Point.Get(0,3), Point.Get(1, 3), Point.Get(1, 9)), new Move(Point.Get(0,3), Point.Get(1, 3), Point.Get(1, 8)), new Move(Point.Get(0,3), Point.Get(1, 3), Point.Get(1, 7)), new Move(Point.Get(0,3), Point.Get(1, 3), Point.Get(1, 6)), new Move(Point.Get(0,3), Point.Get(1, 3), Point.Get(1, 5)), new Move(Point.Get(0,3), Point.Get(1, 3), Point.Get(1, 4)), new Move(Point.Get(0,3), Point.Get(1, 3), Point.Get(0, 4)), new Move(Point.Get(0,3), Point.Get(5, 8), Point.Get(9, 4)), new Move(Point.Get(0,3), Point.Get(5, 8), Point.Get(8, 5)), new Move(Point.Get(0,3), Point.Get(5, 8), Point.Get(7, 6)), new Move(Point.Get(0,3), Point.Get(5, 8), Point.Get(6, 7)), new Move(Point.Get(0,3), Point.Get(5, 8), Point.Get(5, 0)), new Move(Point.Get(0,3), Point.Get(5, 8), Point.Get(5, 1)), new Move(Point.Get(0,3), Point.Get(5, 8), Point.Get(5, 2)), new Move(Point.Get(0,3), Point.Get(5, 8), Point.Get(5, 3)), new Move(Point.Get(0,3), Point.Get(5, 8), Point.Get(5, 4)), new Move(Point.Get(0,3), Point.Get(5, 8), Point.Get(5, 5)), new Move(Point.Get(0,3), Point.Get(5, 8), Point.Get(5, 6)), new Move(Point.Get(0,3), Point.Get(5, 8), Point.Get(5, 7)), new Move(Point.Get(0,3), Point.Get(5, 8), Point.Get(0, 3)), new Move(Point.Get(0,3), Point.Get(5, 8), Point.Get(1, 4)), new Move(Point.Get(0,3), Point.Get(5, 8), Point.Get(2, 5)), new Move(Point.Get(0,3), Point.Get(5, 8), Point.Get(3, 6)), new Move(Point.Get(0,3), Point.Get(5, 8), Point.Get(4, 7)), new Move(Point.Get(0,3), Point.Get(5, 8), Point.Get(9, 8)), new Move(Point.Get(0,3), Point.Get(5, 8), Point.Get(8, 8)), new Move(Point.Get(0,3), Point.Get(5, 8), Point.Get(7, 8)), new Move(Point.Get(0,3), Point.Get(5, 8), Point.Get(6, 8)), new Move(Point.Get(0,3), Point.Get(5, 8), Point.Get(0, 8)), new Move(Point.Get(0,3), Point.Get(5, 8), Point.Get(1, 8)), new Move(Point.Get(0,3), Point.Get(5, 8), Point.Get(2, 8)), new Move(Point.Get(0,3), Point.Get(5, 8), Point.Get(3, 8)), new Move(Point.Get(0,3), Point.Get(5, 8), Point.Get(4, 8)), new Move(Point.Get(0,3), Point.Get(5, 8), Point.Get(5, 9)), new Move(Point.Get(0,3), Point.Get(5, 8), Point.Get(4, 9)), new Move(Point.Get(0,3), Point.Get(4, 7), Point.Get(9, 2)), new Move(Point.Get(0,3), Point.Get(4, 7), Point.Get(8, 3)), new Move(Point.Get(0,3), Point.Get(4, 7), Point.Get(7, 4)), new Move(Point.Get(0,3), Point.Get(4, 7), Point.Get(6, 5)), new Move(Point.Get(0,3), Point.Get(4, 7), Point.Get(5, 6)), new Move(Point.Get(0,3), Point.Get(4, 7), Point.Get(4, 0)), new Move(Point.Get(0,3), Point.Get(4, 7), Point.Get(4, 1)), new Move(Point.Get(0,3), Point.Get(4, 7), Point.Get(4, 2)), new Move(Point.Get(0,3), Point.Get(4, 7), Point.Get(4, 3)), new Move(Point.Get(0,3), Point.Get(4, 7), Point.Get(4, 4)), new Move(Point.Get(0,3), Point.Get(4, 7), Point.Get(4, 5)), new Move(Point.Get(0,3), Point.Get(4, 7), Point.Get(4, 6)), new Move(Point.Get(0,3), Point.Get(4, 7), Point.Get(0, 3)), new Move(Point.Get(0,3), Point.Get(4, 7), Point.Get(1, 4)), new Move(Point.Get(0,3), Point.Get(4, 7), Point.Get(2, 5)), new Move(Point.Get(0,3), Point.Get(4, 7), Point.Get(3, 6)), new Move(Point.Get(0,3), Point.Get(4, 7), Point.Get(9, 7)), new Move(Point.Get(0,3), Point.Get(4, 7), Point.Get(8, 7)), new Move(Point.Get(0,3), Point.Get(4, 7), Point.Get(7, 7)), new Move(Point.Get(0,3), Point.Get(4, 7), Point.Get(6, 7)), new Move(Point.Get(0,3), Point.Get(4, 7), Point.Get(5, 7)), new Move(Point.Get(0,3), Point.Get(4, 7), Point.Get(0, 7)), new Move(Point.Get(0,3), Point.Get(4, 7), Point.Get(1, 7)), new Move(Point.Get(0,3), Point.Get(4, 7), Point.Get(2, 7)), new Move(Point.Get(0,3), Point.Get(4, 7), Point.Get(3, 7)), new Move(Point.Get(0,3), Point.Get(4, 7), Point.Get(5, 8)), new Move(Point.Get(0,3), Point.Get(4, 7), Point.Get(4, 9)), new Move(Point.Get(0,3), Point.Get(4, 7), Point.Get(4, 8)), new Move(Point.Get(0,3), Point.Get(4, 7), Point.Get(2, 9)), new Move(Point.Get(0,3), Point.Get(4, 7), Point.Get(3, 8)), new Move(Point.Get(0,3), Point.Get(3, 6), Point.Get(9, 0)), new Move(Point.Get(0,3), Point.Get(3, 6), Point.Get(8, 1)), new Move(Point.Get(0,3), Point.Get(3, 6), Point.Get(7, 2)), new Move(Point.Get(0,3), Point.Get(3, 6), Point.Get(6, 3)), new Move(Point.Get(0,3), Point.Get(3, 6), Point.Get(5, 4)), new Move(Point.Get(0,3), Point.Get(3, 6), Point.Get(4, 5)), new Move(Point.Get(0,3), Point.Get(3, 6), Point.Get(3, 1)), new Move(Point.Get(0,3), Point.Get(3, 6), Point.Get(3, 2)), new Move(Point.Get(0,3), Point.Get(3, 6), Point.Get(3, 3)), new Move(Point.Get(0,3), Point.Get(3, 6), Point.Get(3, 4)), new Move(Point.Get(0,3), Point.Get(3, 6), Point.Get(3, 5)), new Move(Point.Get(0,3), Point.Get(3, 6), Point.Get(0, 3)), new Move(Point.Get(0,3), Point.Get(3, 6), Point.Get(1, 4)), new Move(Point.Get(0,3), Point.Get(3, 6), Point.Get(2, 5)), new Move(Point.Get(0,3), Point.Get(3, 6), Point.Get(8, 6)), new Move(Point.Get(0,3), Point.Get(3, 6), Point.Get(7, 6)), new Move(Point.Get(0,3), Point.Get(3, 6), Point.Get(6, 6)), new Move(Point.Get(0,3), Point.Get(3, 6), Point.Get(5, 6)), new Move(Point.Get(0,3), Point.Get(3, 6), Point.Get(4, 6)), new Move(Point.Get(0,3), Point.Get(3, 6), Point.Get(1, 6)), new Move(Point.Get(0,3), Point.Get(3, 6), Point.Get(2, 6)), new Move(Point.Get(0,3), Point.Get(3, 6), Point.Get(5, 8)), new Move(Point.Get(0,3), Point.Get(3, 6), Point.Get(4, 7)), new Move(Point.Get(0,3), Point.Get(3, 6), Point.Get(3, 8)), new Move(Point.Get(0,3), Point.Get(3, 6), Point.Get(3, 7)), new Move(Point.Get(0,3), Point.Get(3, 6), Point.Get(0, 9)), new Move(Point.Get(0,3), Point.Get(3, 6), Point.Get(1, 8)), new Move(Point.Get(0,3), Point.Get(3, 6), Point.Get(2, 7)), new Move(Point.Get(0,3), Point.Get(2, 5), Point.Get(7, 0)), new Move(Point.Get(0,3), Point.Get(2, 5), Point.Get(6, 1)), new Move(Point.Get(0,3), Point.Get(2, 5), Point.Get(5, 2)), new Move(Point.Get(0,3), Point.Get(2, 5), Point.Get(4, 3)), new Move(Point.Get(0,3), Point.Get(2, 5), Point.Get(3, 4)), new Move(Point.Get(0,3), Point.Get(2, 5), Point.Get(2, 0)), new Move(Point.Get(0,3), Point.Get(2, 5), Point.Get(2, 1)), new Move(Point.Get(0,3), Point.Get(2, 5), Point.Get(2, 2)), new Move(Point.Get(0,3), Point.Get(2, 5), Point.Get(2, 3)), new Move(Point.Get(0,3), Point.Get(2, 5), Point.Get(2, 4)), new Move(Point.Get(0,3), Point.Get(2, 5), Point.Get(0, 3)), new Move(Point.Get(0,3), Point.Get(2, 5), Point.Get(1, 4)), new Move(Point.Get(0,3), Point.Get(2, 5), Point.Get(9, 5)), new Move(Point.Get(0,3), Point.Get(2, 5), Point.Get(8, 5)), new Move(Point.Get(0,3), Point.Get(2, 5), Point.Get(7, 5)), new Move(Point.Get(0,3), Point.Get(2, 5), Point.Get(6, 5)), new Move(Point.Get(0,3), Point.Get(2, 5), Point.Get(5, 5)), new Move(Point.Get(0,3), Point.Get(2, 5), Point.Get(4, 5)), new Move(Point.Get(0,3), Point.Get(2, 5), Point.Get(3, 5)), new Move(Point.Get(0,3), Point.Get(2, 5), Point.Get(0, 5)), new Move(Point.Get(0,3), Point.Get(2, 5), Point.Get(1, 5)), new Move(Point.Get(0,3), Point.Get(2, 5), Point.Get(5, 8)), new Move(Point.Get(0,3), Point.Get(2, 5), Point.Get(4, 7)), new Move(Point.Get(0,3), Point.Get(2, 5), Point.Get(3, 6)), new Move(Point.Get(0,3), Point.Get(2, 5), Point.Get(2, 9)), new Move(Point.Get(0,3), Point.Get(2, 5), Point.Get(2, 8)), new Move(Point.Get(0,3), Point.Get(2, 5), Point.Get(2, 7)), new Move(Point.Get(0,3), Point.Get(2, 5), Point.Get(2, 6)), new Move(Point.Get(0,3), Point.Get(2, 5), Point.Get(0, 7)), new Move(Point.Get(0,3), Point.Get(2, 5), Point.Get(1, 6)), new Move(Point.Get(0,3), Point.Get(1, 4), Point.Get(5, 0)), new Move(Point.Get(0,3), Point.Get(1, 4), Point.Get(4, 1)), new Move(Point.Get(0,3), Point.Get(1, 4), Point.Get(3, 2)), new Move(Point.Get(0,3), Point.Get(1, 4), Point.Get(2, 3)), new Move(Point.Get(0,3), Point.Get(1, 4), Point.Get(1, 0)), new Move(Point.Get(0,3), Point.Get(1, 4), Point.Get(1, 1)), new Move(Point.Get(0,3), Point.Get(1, 4), Point.Get(1, 2)), new Move(Point.Get(0,3), Point.Get(1, 4), Point.Get(1, 3)), new Move(Point.Get(0,3), Point.Get(1, 4), Point.Get(0, 3)), new Move(Point.Get(0,3), Point.Get(1, 4), Point.Get(9, 4)), new Move(Point.Get(0,3), Point.Get(1, 4), Point.Get(8, 4)), new Move(Point.Get(0,3), Point.Get(1, 4), Point.Get(7, 4)), new Move(Point.Get(0,3), Point.Get(1, 4), Point.Get(6, 4)), new Move(Point.Get(0,3), Point.Get(1, 4), Point.Get(5, 4)), new Move(Point.Get(0,3), Point.Get(1, 4), Point.Get(4, 4)), new Move(Point.Get(0,3), Point.Get(1, 4), Point.Get(3, 4)), new Move(Point.Get(0,3), Point.Get(1, 4), Point.Get(2, 4)), new Move(Point.Get(0,3), Point.Get(1, 4), Point.Get(0, 4)), new Move(Point.Get(0,3), Point.Get(1, 4), Point.Get(5, 8)), new Move(Point.Get(0,3), Point.Get(1, 4), Point.Get(4, 7)), new Move(Point.Get(0,3), Point.Get(1, 4), Point.Get(3, 6)), new Move(Point.Get(0,3), Point.Get(1, 4), Point.Get(2, 5)), new Move(Point.Get(0,3), Point.Get(1, 4), Point.Get(1, 9)), new Move(Point.Get(0,3), Point.Get(1, 4), Point.Get(1, 8)), new Move(Point.Get(0,3), Point.Get(1, 4), Point.Get(1, 7)), new Move(Point.Get(0,3), Point.Get(1, 4), Point.Get(1, 6)), new Move(Point.Get(0,3), Point.Get(1, 4), Point.Get(1, 5)), new Move(Point.Get(0,3), Point.Get(1, 4), Point.Get(0, 5)), new Move(Point.Get(0,3), Point.Get(0, 5), Point.Get(5, 0)), new Move(Point.Get(0,3), Point.Get(0, 5), Point.Get(4, 1)), new Move(Point.Get(0,3), Point.Get(0, 5), Point.Get(3, 2)), new Move(Point.Get(0,3), Point.Get(0, 5), Point.Get(2, 3)), new Move(Point.Get(0,3), Point.Get(0, 5), Point.Get(1, 4)), new Move(Point.Get(0,3), Point.Get(0, 5), Point.Get(0, 0)), new Move(Point.Get(0,3), Point.Get(0, 5), Point.Get(0, 1)), new Move(Point.Get(0,3), Point.Get(0, 5), Point.Get(0, 2)), new Move(Point.Get(0,3), Point.Get(0, 5), Point.Get(0, 3)), new Move(Point.Get(0,3), Point.Get(0, 5), Point.Get(0, 4)), new Move(Point.Get(0,3), Point.Get(0, 5), Point.Get(9, 5)), new Move(Point.Get(0,3), Point.Get(0, 5), Point.Get(8, 5)), new Move(Point.Get(0,3), Point.Get(0, 5), Point.Get(7, 5)), new Move(Point.Get(0,3), Point.Get(0, 5), Point.Get(6, 5)), new Move(Point.Get(0,3), Point.Get(0, 5), Point.Get(5, 5)), new Move(Point.Get(0,3), Point.Get(0, 5), Point.Get(4, 5)), new Move(Point.Get(0,3), Point.Get(0, 5), Point.Get(3, 5)), new Move(Point.Get(0,3), Point.Get(0, 5), Point.Get(2, 5)), new Move(Point.Get(0,3), Point.Get(0, 5), Point.Get(1, 5)), new Move(Point.Get(0,3), Point.Get(0, 5), Point.Get(4, 9)), new Move(Point.Get(0,3), Point.Get(0, 5), Point.Get(3, 8)), new Move(Point.Get(0,3), Point.Get(0, 5), Point.Get(2, 7)), new Move(Point.Get(0,3), Point.Get(0, 5), Point.Get(1, 6)), new Move(Point.Get(0,3), Point.Get(0, 4), Point.Get(4, 0)), new Move(Point.Get(0,3), Point.Get(0, 4), Point.Get(3, 1)), new Move(Point.Get(0,3), Point.Get(0, 4), Point.Get(2, 2)), new Move(Point.Get(0,3), Point.Get(0, 4), Point.Get(1, 3)), new Move(Point.Get(0,3), Point.Get(0, 4), Point.Get(0, 0)), new Move(Point.Get(0,3), Point.Get(0, 4), Point.Get(0, 1)), new Move(Point.Get(0,3), Point.Get(0, 4), Point.Get(0, 2)), new Move(Point.Get(0,3), Point.Get(0, 4), Point.Get(0, 3)), new Move(Point.Get(0,3), Point.Get(0, 4), Point.Get(9, 4)), new Move(Point.Get(0,3), Point.Get(0, 4), Point.Get(8, 4)), new Move(Point.Get(0,3), Point.Get(0, 4), Point.Get(7, 4)), new Move(Point.Get(0,3), Point.Get(0, 4), Point.Get(6, 4)), new Move(Point.Get(0,3), Point.Get(0, 4), Point.Get(5, 4)), new Move(Point.Get(0,3), Point.Get(0, 4), Point.Get(4, 4)), new Move(Point.Get(0,3), Point.Get(0, 4), Point.Get(3, 4)), new Move(Point.Get(0,3), Point.Get(0, 4), Point.Get(2, 4)), new Move(Point.Get(0,3), Point.Get(0, 4), Point.Get(1, 4)), new Move(Point.Get(0,3), Point.Get(0, 4), Point.Get(5, 9)), new Move(Point.Get(0,3), Point.Get(0, 4), Point.Get(4, 8)), new Move(Point.Get(0,3), Point.Get(0, 4), Point.Get(3, 7)), new Move(Point.Get(0,3), Point.Get(0, 4), Point.Get(2, 6)), new Move(Point.Get(0,3), Point.Get(0, 4), Point.Get(1, 5)), new Move(Point.Get(0,3), Point.Get(0, 4), Point.Get(0, 5)), };
            PieceGrid grid = new PieceGrid(10, PieceHelpers.GetInitialAmazonPositions(10));
            IEnumerable<Move> moves = grid.GetMovesFromPoint(Point.Get(0,3));
            //string code = GetMoveString(moves);
            Assert.IsTrue(moves.ToSet().SetEquals(answer));
        }

        [TestMethod]
        public void ApplyMove()
        {
            PieceGrid grid = new PieceGrid(10, PieceHelpers.GetInitialAmazonPositions(10));
            Assert.AreEqual(AmazonPlayer1.Get(), grid.PointPieces[Point.Get(0, 3)]);
            Assert.AreEqual(Open.Get(), grid.PointPieces[Point.Get(6, 3)]);
            Assert.AreEqual(Open.Get(), grid.PointPieces[Point.Get(6, 7)]);
            Assert.IsTrue(grid.Amazon1Points.Contains(Point.Get(0, 3)));
            Assert.IsFalse(grid.Amazon1Points.Contains(Point.Get(6, 3)));
            grid.ApplyMove(new Move(Point.Get(0, 3), Point.Get(6, 3), Point.Get(6, 7)));
            Assert.AreEqual(Open.Get(), grid.PointPieces[Point.Get(0, 3)]);
            Assert.AreEqual(AmazonPlayer1.Get(), grid.PointPieces[Point.Get(6, 3)]);
            Assert.AreEqual(ArrowPlayer1.Get(), grid.PointPieces[Point.Get(6, 7)]);
            Assert.IsTrue(grid.Amazon1Points.Contains(Point.Get(6, 3)));
            Assert.IsFalse(grid.Amazon1Points.Contains(Point.Get(0, 3)));
        }

        [TestMethod]
        public void AllFirstMoves()
        {
            Board board = new Board(10);
            IEnumerable<Move> moves = board.GetAvailableMoves(Owner.Player1);
            //string code = GetMoveString(moves);
            Assert.AreEqual(2176, moves.Count());
        }

        [TestMethod]
        public void ApplyMoveBoard()
        {
            Board board = new Board(10);
            Assert.AreEqual(0, board.Player1MoveCount);
            Assert.AreEqual(0, board.Player2MoveCount);
            Assert.AreEqual(Owner.Player1, board.CurrentPlayer);
            Assert.IsTrue(board.IsPlayer1Turn);
            board.ApplyMove(new Move(Point.Get(0, 3), Point.Get(6, 3), Point.Get(6, 7)));
            Assert.IsFalse(board.IsPlayer1Turn);
            Assert.AreEqual(Owner.Player2, board.CurrentPlayer);
            Assert.AreEqual(1, board.Player1MoveCount);
            Assert.AreEqual(0, board.Player2MoveCount);
        }

        [TestMethod]
        public void ApplyReverseMove()
        {
            PieceGrid grid = GetPieceGrid();
            Assert.AreEqual(AmazonPlayer2.Get(), grid.PointPieces[Point.Get(6, 1)]);
            Assert.AreEqual(ArrowPlayer2.Get(), grid.PointPieces[Point.Get(6, 4)]);
            Assert.AreEqual(Open.Get(), grid.PointPieces[Point.Get(5, 0)]);
            Assert.IsTrue(grid.Amazon2Points.Contains(Point.Get(6, 1)));
            Assert.IsFalse(grid.Amazon2Points.Contains(Point.Get(5, 0)));
            grid.ApplyReverseMove(new Move(Point.Get(5, 0), Point.Get(6, 1), Point.Get(6, 4)));
            Assert.AreEqual(Open.Get(), grid.PointPieces[Point.Get(6, 1)]);
            Assert.AreEqual(AmazonPlayer2.Get(), grid.PointPieces[Point.Get(5, 0)]);
            Assert.AreEqual(Open.Get(), grid.PointPieces[Point.Get(6, 4)]);
            Assert.IsTrue(grid.Amazon2Points.Contains(Point.Get(5, 0)));
            Assert.IsFalse(grid.Amazon2Points.Contains(Point.Get(6, 1)));
        }

        [TestMethod]
        public void ApplyReverseMoveBoard()
        {
            Board board = new Board(GetPieceGrid());
            Assert.AreEqual(13, board.Player1MoveCount);
            Assert.AreEqual(13, board.Player2MoveCount);
            Assert.AreEqual(Owner.Player2, board.PreviousPlayer);
            Assert.IsTrue(board.IsPlayer1Turn);
            board.ApplyReverseMove(new Move(Point.Get(5, 0), Point.Get(6, 1), Point.Get(6, 4)));
            Assert.IsFalse(board.IsPlayer1Turn);
            Assert.AreEqual(Owner.Player1, board.PreviousPlayer);
            Assert.AreEqual(13, board.Player1MoveCount);
            Assert.AreEqual(12, board.Player2MoveCount);
        }

        [TestMethod]
        public void AllReverseMoves()
        {
            Board board = new Board(GetPieceGrid());
            IEnumerable<Move> moves = board.GetAvailableReverseMovesForPreviousPlayer();
            string code = GetMoveString(moves);
            Assert.AreEqual(71, moves.Count());
        }

        [TestMethod]
        public void BeginGame()
        {
            Game game = new Game();
            game.Begin(null, null, 10);
            Assert.AreEqual(2176, game.CurrentMoves.Count);
            Assert.AreEqual(1, game.Boards.Count);
            Assert.AreEqual(Owner.Player1, game.CurrentPlayer);
        }

        [TestMethod]
        public void ApplyMoveGame()
        {
            Game game = new Game();
            game.Begin(null, null, 10);
            Assert.AreEqual(2176, game.CurrentMoves.Count);
            Assert.AreEqual(1, game.Boards.Count);
            Assert.AreEqual(Owner.Player1, game.CurrentPlayer);
            game.ApplyMove(new Move(Point.Get(0, 3), Point.Get(6, 3), Point.Get(6, 7)));
            Assert.AreEqual(2, game.Boards.Count);
            Assert.AreEqual(Owner.Player2, game.CurrentPlayer);
        }

        public string GetPointsString(IEnumerable<Point> points) => points.Select(p => $"Point.Get({p.X},{p.Y}), ").Aggregate("", (s1, ss) => ss += s1);
        public string GetMoveString(IEnumerable<Move> moves) => moves.Select(m => $"new Move(Point.Get({m.Origin.X},{m.Origin.Y}), Point.Get({m.AmazonsPoint.X},{m.AmazonsPoint.Y}), Point.Get({m.ArrowPoint.X},{m.ArrowPoint.Y})), ").Aggregate("", (s1, ss) => ss += s1);

        private PieceGrid GetPieceGrid()
        {
            Point[] arrow1Points = { Point.Get(7, 3), Point.Get(1, 6), Point.Get(2, 4), Point.Get(2, 9), Point.Get(3, 8), Point.Get(4, 8), Point.Get(5, 4), Point.Get(5, 5), Point.Get(5, 7), Point.Get(7, 0), Point.Get(7, 7), Point.Get(8, 7), Point.Get(9, 2) };
            Point[] arrow2Points = { Point.Get(0, 0), Point.Get(0, 5), Point.Get(1, 4), Point.Get(2, 6), Point.Get(2, 7), Point.Get(3, 7), Point.Get(4, 6), Point.Get(4, 9), Point.Get(6, 4), Point.Get(6, 7), Point.Get(1, 2), Point.Get(7, 4), Point.Get(8, 6) };
            Dictionary<Point, Amazon> amazonsDict = new Dictionary<Point, Amazon> {
                { Point.Get(2,8), AmazonPlayer1.Get() }, { Point.Get(6,6), AmazonPlayer1.Get() }, { Point.Get(3,3), AmazonPlayer1.Get() }, { Point.Get(7,2), AmazonPlayer1.Get() },
                { Point.Get(3,9), AmazonPlayer2.Get() }, { Point.Get(4,7), AmazonPlayer2.Get() }, { Point.Get(3,1), AmazonPlayer2.Get() }, { Point.Get(6,1), AmazonPlayer2.Get() }
            };
            PieceGrid grid = new PieceGrid(10, amazonsDict);
            foreach (Point p in arrow1Points)
            {
                grid.PointPieces[p] = ArrowPlayer1.Get();
            }
            foreach (Point p in arrow2Points)
            {
                grid.PointPieces[p] = ArrowPlayer2.Get();
            }
            return grid;
        }
    }

}
