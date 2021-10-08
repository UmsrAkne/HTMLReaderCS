namespace HTMLReaderCS.Models.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SQLiteHelperTests
    {
        [TestMethod]
        public void SQLiteHelperTest()
        {
            var SQLiteHelper = new SQLiteHelper();
        }

        [TestMethod]
        public void insertTest()
        {
            var sqliteHelper = new SQLiteHelper();
            sqliteHelper.insert(new OutputFileInfo());
        }

        [TestMethod]
        public void selectTest()
        {
            var sqliteHelper = new SQLiteHelper();
            var l = sqliteHelper.select($"select * from {sqliteHelper.TableName};");
        }

        [TestMethod]
        public void getHistoriesTest()
        {
            var sqliteHelper = new SQLiteHelper();
            var l = sqliteHelper.getHistories();
        }
    }
}