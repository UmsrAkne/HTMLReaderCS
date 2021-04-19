using Microsoft.VisualStudio.TestTools.UnitTesting;
using HTMLReaderCS.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLReaderCS.models.Tests {
    [TestClass()]
    public class SQLiteHelperTests {
        [TestMethod()]
        public void SQLiteHelperTest() {
            var SQLiteHelper = new SQLiteHelper();
        }

        [TestMethod()]
        public void insertTest() {
            var sqliteHelper = new SQLiteHelper();
            sqliteHelper.insert(new OutputFileInfo());
        }

        [TestMethod()]
        public void selectTest() {
            var sqliteHelper = new SQLiteHelper();
            var l = sqliteHelper.select($"select * from {sqliteHelper.TableName};");
        }
    }
}