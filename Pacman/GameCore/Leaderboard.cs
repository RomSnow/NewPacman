using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Linq;

namespace Pacman.GameCore
{
    class Leaderboard
    {
        private Dictionary<string, int> leaderboard;

        public Leaderboard()
        {
            if (!File.Exists("leaderboard.dat"))
            {
                leaderboard = new Dictionary<string, int>();
                FileStream fs = new FileStream("leaderboard.dat", FileMode.Create);

                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, leaderboard);

                fs.Close();
            }
        }

        public string GetLeaderboard()
        {
            LoadResult();
            var res = leaderboard.OrderByDescending(x => x.Value).Select(x => x.Value + " " + x.Key).ToList();
            var str = "SCORE NAME \n";
            foreach (var gamer in res)
            {
                str += gamer + "\n";
            }
            return str;
        }

        public void AddResult(string name, int score)
        {
            LoadResult();
            if (!leaderboard.ContainsKey(name))
            {
                leaderboard.Add(name, score);
            }
            else if (leaderboard.ContainsKey(name) && leaderboard[name] < score)
            {
                leaderboard.Remove(name);
                leaderboard.Add(name, score);
            }
            SaveResult();
        }

        private void SaveResult()
        {
            FileStream fs = new FileStream("leaderboard.dat", FileMode.Create);

            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(fs, leaderboard);

            fs.Close();
        }

        private void LoadResult()
        {
            FileStream fs = new FileStream("leaderboard.dat", FileMode.Open);

            BinaryFormatter formatter = new BinaryFormatter();
            leaderboard = (Dictionary<string, int>)formatter.Deserialize(fs);

            fs.Close();
        }
    }
}
