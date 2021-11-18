using HexagonGame.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexagonGame.Services
{
    public interface IDatabase
    {
        void AddWinner(Player player);

        void LoadRankings();

        void CommitRankings();

        Dictionary<string, Ranking> GetRankings();
    }
}
