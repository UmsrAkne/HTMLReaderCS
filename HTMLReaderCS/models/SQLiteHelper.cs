﻿namespace HTMLReaderCS.Models
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.SQLite;
    using System.Linq;

    public class SQLiteHelper
    {
        public SQLiteHelper()
        {
            ExecuteNonquery(
                $"CREATE TABLE IF NOT EXISTS {TableName} (" +
                $"id                                         INTEGER NOT NULL PRIMARY KEY, " +
                $"{nameof(OutputFileInfo.LengthSec)}         INTEGER NOT NULL, " +
                $"{nameof(OutputFileInfo.FileName)}          TEXT NOT NULL, " +
                $"{nameof(OutputFileInfo.Hash)}              TEXT NOT NULL, " +
                $"{nameof(OutputFileInfo.Position)}          INTEGER NOT NULL, " +
                $"{nameof(OutputFileInfo.OutputDateTime)}    TEXT NOT NULL, " +
                $"{nameof(OutputFileInfo.HtmlFileName)}      TEXT NOT NULL, " +
                $"{nameof(OutputFileInfo.TagName)}           TEXT NOT NULL, " +
                $"{nameof(OutputFileInfo.HeaderText)}        TEXT NOT NULL" +
                $");");
        }

        public string DBFileName => "OutputHistory.sqlite";

        public string TableName => "output_history";

        private SQLiteConnection Connection
        {
            get
            {
                var sqliteSb = new SQLiteConnectionStringBuilder() { DataSource = DBFileName };
                return new SQLiteConnection(sqliteSb.ToString());
            }
        }

        public void Insert(OutputFileInfo outputFileInfo)
        {
            var count = GetCount() + 1;
            ExecuteNonquery($"INSERT INTO {TableName}(" +
                $"id," +
                $"{nameof(OutputFileInfo.LengthSec)}," +
                $"{nameof(OutputFileInfo.FileName)}," +
                $"{nameof(OutputFileInfo.OutputDateTime)}," +
                $"{nameof(OutputFileInfo.HtmlFileName)}," +
                $"{nameof(OutputFileInfo.Hash)}," +
                $"{nameof(OutputFileInfo.Position)}," +
                $"{nameof(OutputFileInfo.TagName)}," +
                $"{nameof(OutputFileInfo.HeaderText)}" +
                $") VALUES (" +
                $"{count}, " +
                $"{outputFileInfo.LengthSec}," +
                $"'{outputFileInfo.FileName}'," +
                $"'{outputFileInfo.OutputDateTime}'," +
                $"'{outputFileInfo.HtmlFileName}'," +
                $"'{outputFileInfo.Hash}'," +
                $"'{outputFileInfo.Position}'," +
                $"'{outputFileInfo.TagName}'," +
                $"'{outputFileInfo.HeaderText}'" +
                $");");
        }

        public long GetCount()
        {
            return (long)Select($"SELECT COUNT(*) FROM {TableName};").First()["COUNT(*)"];
        }

        public List<Hashtable> Select(string sql)
        {
            using (var con = Connection)
            {
                List<Hashtable> resultList = new List<Hashtable>();
                con.Open();
                var command = new SQLiteCommand(sql, con);
                var dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    var hashtable = new Hashtable();
                    for (int i = 0; i < dataReader.FieldCount; i++)
                    {
                        hashtable[dataReader.GetName(i)] = dataReader.GetValue(i);
                    }

                    resultList.Add(hashtable);
                }

                dataReader.Close();
                return resultList;
            }
        }

        public List<OutputFileInfo> GetHistories()
        {
            var list = new List<OutputFileInfo>();
            var hashs = Select($"SELECT * FROM {TableName} ORDER BY {nameof(OutputFileInfo.OutputDateTime)};");
            foreach (var h in hashs)
            {
                var outputFileInfo = new OutputFileInfo();
                outputFileInfo.FileName = (string)h[nameof(outputFileInfo.FileName)];
                outputFileInfo.HeaderText = (string)h[nameof(outputFileInfo.HeaderText)];
                outputFileInfo.HtmlFileName = (string)h[nameof(outputFileInfo.HtmlFileName)];
                outputFileInfo.TagName = (string)h[nameof(outputFileInfo.TagName)];
                outputFileInfo.LengthSec = (int)(long)h[nameof(outputFileInfo.LengthSec)];
                outputFileInfo.OutputDateTime = DateTime.Parse((string)h[nameof(outputFileInfo.OutputDateTime)]);

                list.Add(outputFileInfo);
            }

            foreach (var of in list)
            {
                of.Exists = System.IO.File.Exists($"{Properties.Settings.Default.OutputDirectoryName}\\{of.FileName}");
            }

            return list;
        }

        public int GetUnreadLine(string fileHash)
        {
            var h = Select($"SELECT MAX({nameof(OutputFileInfo.Position)}) " +
                    $"FROM {TableName} " +
                    $"WHERE {nameof(OutputFileInfo.Hash)} = '{fileHash}'" +
                    $"ORDER BY {nameof(OutputFileInfo.Position)}");

            if (h.First()["MAX(Position)"] == DBNull.Value)
            {
                return 0;
            }

            return (int)(long)h.First()["MAX(Position)"];
        }

        private void ExecuteNonquery(string sql)
        {
            using (var con = Connection)
            {
                con.Open();
                using (var command = new SQLiteCommand(con))
                {
                    command.CommandText = sql;
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
