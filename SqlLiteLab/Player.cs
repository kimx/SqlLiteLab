using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlLiteLab
{
    public class Player
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime RegDate { get; set; }
        public int Score { get; set; }
        public byte[] BinData { get; set; }
        public Player(string id, string name, DateTime regDate, int score)
        {
            Id = id;
            Name = name;
            RegDate = regDate;
            Score = score;
            BinData = Guid.NewGuid().ToByteArray().Take(4).ToArray();
        }
    }
}
