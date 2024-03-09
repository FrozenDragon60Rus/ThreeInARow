using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Data.SQLite;
using System;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;
using Unity.VisualScripting;

namespace Assets.Script.SQLite
{
    internal class DataBase
    {
        SQLiteConnection connection;
        string dataBase;

        public DataBase(string dataBase)
        {
            this.dataBase = dataBase;
            connection = Connect();
        }
        private SQLiteConnection Connect() =>
            new SQLiteConnection(@$"Data Source={Application.dataPath}/Data/{dataBase}");

        public Table.Level LoadLevel(int Id)
        {
            Table.Level level = new Table.Level();

            string commandText = $"SELECT * FROM Level WHERE Number = {Id}";

            connection.Open();
            SQLiteCommand cmd = new SQLiteCommand(commandText, connection);
            using(SQLiteDataReader reader = cmd.ExecuteReader())
                if (reader.Read())
                {
                    level.parameter.Add(nameof(level.Number), Id);
                    level.parameter.Add(nameof(level.Cell), reader[nameof(level.Cell)]);
                    level.parameter.Add(nameof(level.Column), reader[nameof(level.Column)]);
                    level.parameter.Add(nameof(level.Row), reader[nameof(level.Row)]);
                }
            connection.Close();

            return level;
        }
        public int LoadLevelCount()
        {
            int count = 0;

            string commandText = $"SELECT COUNT(*) as Count FROM Level";

            connection.Open();
            SQLiteCommand cmd = new SQLiteCommand(commandText, connection);
            using (SQLiteDataReader reader = cmd.ExecuteReader())
                if (reader.Read())
                    count = Convert.ToInt32(reader["Count"]);
            connection.Close();

            return count;
        }
        public void WriteLevel(Table.Level level)
        {
            string commandText = $"INSERT INTO Level (Number, Cell, Column, Row) \r\n" +
                                  "VALUES(@Number, @Cell, @Column, @Row) \r\n" +
                                  "ON CONFLICT(Number) \r\n" +
                                  "DO UPDATE SET \r\n" +
                                  "Cell = excluded.Cell, \r\n" +
                                  "Column = excluded.Column, \r\n" +
                                  "Row = excluded.Row";

            Send(new SQLiteCommand(commandText, connection), level.parameter);
        }

        private void Send(SQLiteCommand command, Dictionary<string, object> parameter)
        {
            connection.Open();

            command.Parameters.AddWithValue("@Number", parameter["Number"]);
            command.Parameters.AddWithValue("@Cell", parameter["Cell"]);
            command.Parameters.AddWithValue("@Column", parameter["Column"]);
            command.Parameters.AddWithValue("@Row", parameter["Row"]);
            command.ExecuteNonQuery();

            connection.Close();
        }
    }
}
