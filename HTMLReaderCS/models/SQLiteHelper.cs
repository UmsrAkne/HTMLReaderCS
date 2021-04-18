using System;
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
