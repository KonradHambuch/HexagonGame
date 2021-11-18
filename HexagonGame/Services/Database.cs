using HexagonGame.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Windows.Storage;

namespace HexagonGame.Services
{
    public class Database : IDatabase
    {
        public Dictionary<string, Ranking> Rankings { get; set; } = new Dictionary<string, Ranking>();
        public string DbFilePath { get; set; }
        public Database()
        {
            var appFolder = Windows.Storage.ApplicationData.Current.LocalFolder.Path;
            var work_dir = appFolder + @"\Db_files\";
            DbFilePath = work_dir + "db_file.json";
            if (!Directory.GetFiles(appFolder).Contains(@"\Db_files\"))
            {
                Directory.CreateDirectory(work_dir);
            }
            var files = Directory.GetFiles(work_dir);

            if (files.Contains(DbFilePath))
            {
                LoadRankings();
            }
        }
        ~Database()
        {
            CommitRankings();
        }

        public void AddWinner(Player player)
        {
            if (Rankings.ContainsKey(player.Name))
            {
                Rankings[player.Name].Score++;
            }
            else
            {
                Rankings.Add(player.Name, new Ranking { Name = player.Name, Score = 1 });
            }
            CommitRankings();
        }

        public void LoadRankings()
        {
            using (StreamReader r = new StreamReader(DbFilePath))
            {
                string json = r.ReadToEnd();
                Rankings = JsonConvert.DeserializeObject<Dictionary<string, Ranking>>(json);
            }
        }

        public void CommitRankings()
        {
            var json = JsonConvert.SerializeObject(Rankings);
            using (StreamWriter sw = new StreamWriter(DbFilePath))
            {
                sw.WriteLine(json);
            }
        }

        public Dictionary<string, Ranking> GetRankings()
        {
            return this.Rankings;
        }
    }
}
