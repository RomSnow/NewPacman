using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Pacman.GameCore
{
    public class Searcher
    {
        public static SinglyLinkedList<Point> SearchWayToPlayer(FieldItem[,] field, Point start, Point objective)
        {
            var queue = new Queue<SinglyLinkedList<Point>>();
            queue.Enqueue(new SinglyLinkedList<Point>(start, null));
            var hashVisited = new HashSet<Point>();

            while (queue.Count != 0)
            {
                var list = queue.Dequeue();
                var currentPoint = list.Value;
                if (hashVisited.Contains(currentPoint))
                {
                    continue;
                }
                hashVisited.Add(currentPoint);
                if (list.Value == objective)
                {
                    return list;
                }
                if (currentPoint.Y < 0 || currentPoint.Y >= field.GetLength(0) ||
                    currentPoint.X < 0 || currentPoint.X >= field.GetLength(1))
                {
                    continue;
                }
                if (field[currentPoint.Y, currentPoint.X] is Wall)
                {
                    continue;
                }
                for (var dy = -1; dy <= 1; dy++)
                    for (var dx = -1; dx <= 1; dx++)
                        if (dx != 0 && dy != 0) continue;
                        else queue.Enqueue(new SinglyLinkedList<Point>(new Point
                        {
                            X = currentPoint.X + dx,
                            Y = currentPoint.Y + dy
                        }, list));
            }
            return new SinglyLinkedList<Point>(start, null);
        }
    }
}
