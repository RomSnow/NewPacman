using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Pacman.GameCore
{
    public class Map
    {
        public FieldItem[,] Field { get; set; }
        public int Score { get; set; }
        public int HealthPoints { get; set; }
        public bool IsGameOver { get; private set; }
        public HashSet<Point> CoinsLocations { get; set; }
        public HashSet<Point> BigCoinsLocations { get; set; }
        public Point RespawnPoint { get; private set; }
        public int EnemyCount { get; set; }
        public bool IsPlayerBoost { get; set; }
        public bool IsAttackMode { get; set; }

        private Dictionary<char, Func<Map, Point, FieldItem>> convertDict = 
            new Dictionary<char, Func<Map, Point, FieldItem>>()
            {
                {'P', (Map map, Point point) => new Player(map, point)},
                {'#', (map, point) => new Wall()},
                {'G', (Map map, Point point) => new Ghost(map, point)},
                {'.', (Map map, Point point) => new Coin(map, point)},
                {'*', (Map map, Point point) => new BigCoin(map, point)},
                {'R', (Map map, Point point) => new Respawn(point)},
                {' ', (Map map, Point point) => new Empty()}
            };

        public Player player;

        private List<IMovable> persons;

        private string startMap;
        private int startHP;

        public Map() { }

        public Map(string fieldString, int healthPoints)
        {
            startMap = fieldString;
            startHP = healthPoints;
            CoinsLocations = new HashSet<Point>();
            BigCoinsLocations = new HashSet<Point>();
            IsAttackMode = false;
            IsGameOver = false;
            IsPlayerBoost = false;
            Score = 0;
            HealthPoints = healthPoints;
            persons = new List<IMovable>();
            Field = CreateFieldByString(fieldString);
        }

        public void SetPlayerMoveDirection(MoveDirection direction)
        {
            player.SetMoveDirection(direction);
        }

        public void Update()
        {
            var localPersons = new List<IMovable>(persons);
            foreach (var person in localPersons.Where(p => p != null))
            {
                var lastPosition = person.GetLocation();
                person.Move(out var collisionItem);
                person.Collision(collisionItem);
                var currentPosition = person.GetLocation();
                if (lastPosition != currentPosition)
                {
                    if (CoinsLocations.Contains(lastPosition))
                    {
                        Field[(int)lastPosition.Y, (int)lastPosition.X] = new Coin(this, lastPosition);
                    }
                    else if (BigCoinsLocations.Contains(lastPosition))
                    {
                        Field[(int)lastPosition.Y, (int)lastPosition.X] = new BigCoin(this, lastPosition);
                    }
                }
                if (HealthPoints == 0 || EnemyCount == 0 || CoinsLocations.Count == 0)
                {
                    IsGameOver = true;
                    return;
                }
            }

            persons = persons.Where(p => p != null).ToList();
        }

        private FieldItem[,] CreateFieldByString(string fieldString)
        {
            var lines = fieldString.Split('\n')
                .Select(s => s.Trim('\r'))
                .Where(s => s != "")
                .ToArray<string>();
            var field = new FieldItem[lines.Length, lines[0].Length];

            for (var l = 0; l < lines.Length; l++)
            {
                for (var c = 0; c < lines[0].Length; c++)
                {
                    field[l, c] = convertDict[lines[l][c]](this, new Point(c, l));
                    SetItems(field[l, c]);
                }
            }

            return field;
        }

        private void SetItems(FieldItem item)
        {
            if (item is IMovable movable)
            {
                if (movable is Player playerObj)
                    player = playerObj;
                else
                    EnemyCount++;
                
                persons.Add(movable);
            }
            else if (item is Respawn respawnObj)
                RespawnPoint = respawnObj.Location;
            else if (item is Coin coinObj)
                CoinsLocations.Add(coinObj.Location);
            else if (item is BigCoin bigCoinObj)
                BigCoinsLocations.Add(bigCoinObj.Location);
                
        }

        public void KillEnemy(FieldItem ghost)
        {
            EnemyCount--;
            persons.Remove((IMovable)ghost);
        }

        public Tuple<string, int> GetStartMap()
        {
            return Tuple.Create(startMap, startHP);
        }

        public override string ToString() 
        {
            var fieldString = "";
            for (var i = 0; i < Field.GetLength(0); i++)
            {
                var stringBuilder = new StringBuilder();
                stringBuilder.Append('\n');
                for (var j = 0; j < Field.GetLength(1); j++)
                {
                    var sym = ' ';
                    if (Field[i, j] is Wall)
                        sym = '#';
                    else if (Field[i, j] is Player)
                        sym = 'P';
                    else if (Field[i, j] is Ghost)
                        sym = 'G';
                    else if (Field[i, j] is Coin)
                        sym = '.';
                    else if (Field[i, j] is BigCoin)
                        sym = '*';
                    else if (Field[i, j] is Respawn)
                        sym = 'R';
                    stringBuilder.Append(sym);
                }
                fieldString += stringBuilder.ToString();
            }
            
            return fieldString.Remove(0, 1);
        }
    }
}