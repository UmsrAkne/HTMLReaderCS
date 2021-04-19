﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLReaderCS.models {
    public class SQLiteHelper {

        public string DBFileName => "OutputHistory.sqlite";
        public string TableName => "output_history";

        private SQLiteConnection Connection {
            get {
                var sqliteSb = new SQLiteConnectionStringBuilder() { DataSource = DBFileName };
                return new SQLiteConnection(sqliteSb.ToString());
            }
        }

        public SQLiteHelper() {
            executeNonQuer(
                $"CREATE TABLE IF NOT EXISTS {TableName} (" +
                $"id                                         INTEGER NOT NULL PRIMARY KEY, " +
                $"{nameof(OutputFileInfo.LengthSec)}         INTEGER NOT NULL, " +
                $"{nameof(OutputFileInfo.FileName)}          TEXT NOT NULL, " +
                $"{nameof(OutputFileInfo.OutputDateTime)}    TEXT NOT NULL, " +
                $"{nameof(OutputFileInfo.HtmlFileName)}      TEXT NOT NULL, " +
                $"{nameof(OutputFileInfo.TagName)}           TEXT NOT NULL, " +
                $"{nameof(OutputFileInfo.HeaderText)}        TEXT NOT NULL" +
                $");"
            );
        }

        public void insert(OutputFileInfo outputFileInfo) {
            var count = getCount() + 1;
            executeNonQuer($"INSERT INTO {TableName}(" +
                $"id," +
                $"{nameof(OutputFileInfo.LengthSec)}," +
                $"{nameof(OutputFileInfo.FileName)}," +
                $"{nameof(OutputFileInfo.OutputDateTime)}," +
                $"{nameof(OutputFileInfo.HtmlFileName)}," +
                $"{nameof(OutputFileInfo.TagName)}," +
                $"{nameof(OutputFileInfo.HeaderText)}" +
                $") VALUES (" +
                $"{count}, " + 
                $"{outputFileInfo.LengthSec}," +
                $"'{outputFileInfo.FileName}'," +
                $"'{outputFileInfo.OutputDateTime}'," +
                $"'{outputFileInfo.HtmlFileName}'," +
                $"'{outputFileInfo.TagName}'," +
                $"'{outputFileInfo.HeaderText}'" +
                $");"
            );
        }

        public long getCount() {
            return (long)select($"SELECT COUNT(*) FROM {TableName};").First()["COUNT(*)"];
        }

        public List<Hashtable> select(string sql) {
            using (var con = Connection) {
                List<Hashtable> resultList = new List<Hashtable>();
                con.Open();
                var command = new SQLiteCommand(sql, con);
                var dataReader = command.ExecuteReader();

                while (dataReader.Read()) {
                    var hashtable = new Hashtable();
                    for (int i = 0; i < dataReader.FieldCount; i++) {
                        hashtable[dataReader.GetName(i)] = dataReader.GetValue(i);
                    }
                    resultList.Add(hashtable);
                }

                dataReader.Close();
                return resultList;
            };
        }

        private void executeNonQuer(string sql) {
            using (var con = Connection) {
                con.Open();
                using (var command = new SQLiteCommand(con)) {
                    command.CommandText = sql;
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}