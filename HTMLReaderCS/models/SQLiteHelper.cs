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

        private SQLiteConnectionStringBuilder SQLiteConnectionStringBuilder { get; set; }
        private SQLiteConnection Connection {
            get {
                var sqliteSb = new SQLiteConnectionStringBuilder() { DataSource = DBFileName };
                return new SQLiteConnection(sqliteSb.ToString());
            }
        }

        public SQLiteHelper() {
            executeNonQuer(
                $"CREATE TABLE IF NOT EXISTS {TableName} (" +
                $"id                INTEGER NOT NULL PRIMARY KEY, " +
                $"lengthSec         INTEGER NOT NULL, " +
                $"fileName          TEXT NOT NULL, " +
                $"outputDateTime    TEXT NOT NULL, " +
                $"htmlFileName      TEXT NOT NULL, " +
                $"tagName           TEXT NOT NULL, " +
                $"headerText        TEXT NOT NULL" +
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
