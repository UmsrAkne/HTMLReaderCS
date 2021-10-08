namespace HTMLReaderCS.Models.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SQLiteHelperTests
    {
        [TestMethod]
        public void SQLiteHelperTest()
        {
            var sqliteHelper = new SQLiteHelper();
        }

        [TestMethod]
        public void InsertTest()
        {
            var sqliteHelper = new SQLiteHelper();
            sqliteHelper.Insert(new OutputFileInfo());
        }

        [TestMethod]
        public void SelectTest()
        {
            var sqliteHelper = new SQLiteHelper();
            var l = sqliteHelper.Select($"select * from {sqliteHelper.TableName};");
        }

        [TestMethod]
        public void GetHistoriesTest()
        {
            var sqliteHelper = new SQLiteHelper();
            var l = sqliteHelper.GetHistories();
        }
    }
}