using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;
using NUnit.Framework;
using Pacman.GameCore;

namespace Pacman.Tests
{
    [TestFixture]
    public static class MethodsTests
    {
        [Test]
        public static void SimplePath()
        {
            var mapString = "######\n" +
                            "#G.. #\n" +
                            "####P#\n" +
                            "######";
            var map = new Map(mapString, 0);
            var result = Searcher.SearchWayToPlayer(map.Field, new Point(1, 1), new Point(4, 2));
            var finalList = new SinglyLinkedList<Point>(new Point(1, 1), null);
            finalList = new SinglyLinkedList<Point>(new Point(2, 1), finalList);
            finalList = new SinglyLinkedList<Point>(new Point(3, 1), finalList);
            finalList = new SinglyLinkedList<Point>(new Point(4, 1), finalList);
            finalList = new SinglyLinkedList<Point>(new Point(4, 2), finalList);
            Assert.AreEqual(finalList, result);
        }

        [Test]
        public static void ShortestPath()
        {
            var mapString = "######\n" +
                            "#G   #\n" +
                            "# ##P#\n" +
                            "#    #\n" +
                            "######\n";
            var map = new Map(mapString, 0);
            var result = Searcher.SearchWayToPlayer(map.Field, new Point(1, 1), new Point(4, 2));
            var finalList = new SinglyLinkedList<Point>(new Point(1, 1), null);
            finalList = new SinglyLinkedList<Point>(new Point(2, 1), finalList);
            finalList = new SinglyLinkedList<Point>(new Point(3, 1), finalList);
            finalList = new SinglyLinkedList<Point>(new Point(4, 1), finalList);
            finalList = new SinglyLinkedList<Point>(new Point(4, 2), finalList);
            Assert.AreEqual(finalList, result);
        }
    }
}
