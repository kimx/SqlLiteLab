using Dapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
///參考文章 http://blog.darkthread.net/post-2017-06-05-dapper-sqlite.aspx
/// </summary>
namespace SqlLiteLab
{
    class Program
    {
        static string dbPath = @".\Database.sqlite";
        static string cnStr = "data source=" + dbPath;
        static void Main(string[] args)
        {
            Console.WriteLine("Begin");
            InitDb();
            Insert();
            Select();
            Console.WriteLine("End");
            Console.Read();
        }

        static Player[] TestData = new Player[]
      {
            new Player("P01", "Jeffrey", DateTime.Now, 32767),
            new Player("P02", "Darkthread", DateTime.Now, 65535),
      };
        static void Insert()
        {
            using (var conn = new SQLiteConnection(cnStr))
            {
                conn.Execute("DELETE FROM Player");
                var insertScript = "INSERT INTO Player VALUES (@Id, @Name, @RegDate, @Score, @BinData)";
                conn.Execute(insertScript, TestData[0]);
                //測試Primary Key
                try
                {
                    //故意塞入錯誤資料
                    conn.Execute(insertScript, TestData[0]);
                    throw new ApplicationException("失敗：未阻止資料重複");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"測試成功:{ex.Message}");
                }
            }
        }

        static void Select()
        {
            using (var cn = new SQLiteConnection(cnStr))
            {
                var list = cn.Query("SELECT * FROM Player");
                Console.WriteLine(JsonConvert.SerializeObject(list, Formatting.Indented));
            }
        }

        static void InitDb()
        {
            if (File.Exists(dbPath)) return;
            Console.WriteLine("Creating Database");
            using (var conn = new SQLiteConnection(cnStr))
            {
                conn.Execute(@"
                CREATE TABLE Player (
                Id VARCHAR(16),
                Name VARCHAR(32),
                RegDate DATETIME,
                Score INTEGER,
                BinData BLOB,
                CONSTRAINT Player_PK PRIMARY KEY (Id)
                )");
            }
            Console.WriteLine("Created Database");


        }
    }
}
