using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Serialization;
using System.Windows;

namespace Pacman.GameCore
{
    public class Player : FieldItem, IPlayer
    {
        public MoveDirection direction;
        public Point location;
        private Map map;
        public int timeToEndboost;
        private HashSet<Point> coinsLocations;
        private HashSet<Point> bigCoinsLocations;

        public Player(Map map, Point point)
        {
            location = point;
            this.map = map;
            coinsLocations = map.CoinsLocations;
            bigCoinsLocations = map.BigCoinsLocations;
        }

        public void Move(out FieldItem collisionObject)
        {
            if (map.IsPlayerBoost && timeToEndboost == 0)
            {
                map.IsPlayerBoost = false;
            }
            else if (map.IsPlayerBoost)
            {
                timeToEndboost -= 1;
            }
            if (direction == MoveDirection.Right &&
                !(map.Field[(int)location.Y, (int)location.X + 1] is Wall))
            {
                collisionObject = map.Field[(int)location.Y, (int)location.X + 1];
                map.Field[(int)location.Y, (int)location.X] = new Empty();
                location = new Point(location.X + 1, location.Y);
                map.Field[(int)location.Y, (int)location.X] = this;
            }
            else if (direction == MoveDirection.Left &&
                !(map.Field[(int)location.Y, (int)location.X - 1] is Wall))
            {
                collisionObject = map.Field[(int)location.Y, (int)location.X - 1];
                map.Field[(int)location.Y, (int)location.X] = new Empty();
                location = new Point(location.X - 1, location.Y);
                map.Field[(int)location.Y, (int)location.X] = this;
            }
            else if (direction == MoveDirection.Down &&
                !(map.Field[(int)location.Y + 1, (int)location.X] is Wall))
            {
                map.Field[(int)location.Y, (int)location.X] = new Empty();
                location = new Point(location.X, location.Y + 1);
                collisionObject = map.Field[(int)location.Y, (int)location.X];
                map.Field[(int)location.Y, (int)location.X] = this;
            }
            else if (direction == MoveDirection.Up &&
                !(map.Field[(int)location.Y - 1, (int)location.X] is Wall))
            {
                map.Field[(int)location.Y, (int)location.X] = new Empty();
                location = new Point(location.X, location.Y - 1);
                collisionObject = map.Field[(int)location.Y, (int)location.X];
                map.Field[(int)location.Y, (int)location.X] = this;
            }
            else
            {
                collisionObject = map.Field[(int)location.Y, (int)location.X];
            }
        }

        public void Collision(FieldItem obj)
        {
            if (obj is Coin)
            {
                map.Score += 50;
                coinsLocations.Remove(location);
            }
            if (obj is BigCoin)
            {
                map.Score += 200;
                bigCoinsLocations.Remove(location);
                map.IsPlayerBoost = true;
                map.IsAttackMode = false;
                timeToEndboost = 10;
            }
            if (obj is Ghost)
            {
                var ghost = (Ghost)obj;
                if (!map.IsPlayerBoost && ghost.IsGhostAlive)
                {
                    map.HealthPoints -= 1;
                    map.Field[location.Y, location.X] = obj;
                    location = map.RespawnPoint;
                    map.Field[location.Y, location.X] = this;
                }
                else if (ghost.IsGhostAlive)
                {
                    map.Score += 200;
                    ghost.IsGhostAlive = false;
                    map.Field[location.Y, location.X] = this;
                    map.KillEnemy(obj);
                }
            }
        }

        public void SetMoveDirection(MoveDirection direction)
        {
            this.direction = direction;
        }

        public Point GetLocation()
        {
            return location;
        }
    }
}