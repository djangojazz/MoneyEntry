using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MoneyEntry.DataAccess.Tests
{
    [TestClass]
    public class DataRetreivalShould
    {
        private DataRetreival _dataRetreival;

        [TestInitialize]
        public void TestInitialize()
        {
            _dataRetreival = DataRetreival.Instance;
        }

        [TestMethod]
        public void GetTypesShould()
        {
            var types = _dataRetreival.GetTypes();
            Assert.AreEqual(2, types.Count, "There should be two types only");
            Assert.AreEqual("Credit", types[0].Description, "First type should be Credit");
            Assert.AreEqual("Debit", types[1].Description, "Second type should be Debit");
        }

        [TestMethod]
        public async Task GetTypesAsyncShould()
        {
            var types = await _dataRetreival.GetTypesAsync();
            Assert.AreEqual(2, types.Count, "There should be two types only");
            Assert.AreEqual("Credit", types[0].Description, "First type should be Credit");
            Assert.AreEqual("Debit", types[1].Description, "Second type should be Debit");
        }

        [TestMethod]
        public void GetCategoriesShould()
        {
            var categories = _dataRetreival.GetCategories();
            Assert.IsTrue(categories.Count > 1, "There should be more than a single category");
            Assert.IsTrue(categories.Exists(x => x.Description == "Food"), "Food should exist");
        }

        [TestMethod]
        public async Task GetCategoriesAsyncShould()
        {
            var categories = await _dataRetreival.GetCategoriesAsync();
            Assert.IsTrue(categories.Count > 1, "There should be more than a single category");
            Assert.IsTrue(categories.Exists(x => x.Description == "Food"), "Food should exist");
        }
        
        [TestMethod]
        public void GetTransactionViewsShould()
        {
            DateTime start = new DateTime(2017, 12, 17);
            DateTime end = new DateTime(2017, 12, 17);
            int personId = 3;

            var trans = _dataRetreival.GetTransactionViews(start, end, personId);
            Assert.IsTrue(trans.Count > 1, "There should be more than a single transaction");
            Assert.IsTrue(trans.Exists(x => x.Category == "Fun"), "Fun category should exist");
            Assert.IsTrue(trans.Exists(x => x.Category == "Gifts"), "Gifts category should exist");
            Assert.IsTrue(trans.Exists(x => x.Type == "Debit"), "Debit type should exist");
            Assert.IsTrue(trans.Exists(x => x.Type == "Credit"), "Credit Type should exist");
        }

        [TestMethod]
        public async Task GetTransactionViewsAsyncShould()
        {
            DateTime start = new DateTime(2017, 12, 17);
            DateTime end = new DateTime(2017, 12, 17);
            int personId = 3;

            var trans = await _dataRetreival.GetTransactionViewsAsync(start, end, personId);
            Assert.IsTrue(trans.Count > 1, "There should be more than a single transaction");
            Assert.IsTrue(trans.Exists(x => x.Category == "Fun"), "Fun category should exist");
            Assert.IsTrue(trans.Exists(x => x.Category == "Gifts"), "Gifts category should exist");
            Assert.IsTrue(trans.Exists(x => x.Type == "Debit"), "Debit type should exist");
            Assert.IsTrue(trans.Exists(x => x.Type == "Credit"), "Credit Type should exist");
        }

        [TestMethod]
        public void CreateTransactionShould()
        {
            DateTime now = DateTime.Now;
            int typeId = 2;
            int personId = 4;
            int categoryMisc = 26;
            decimal amt = 99.9m;
            string desc = "UnitTest";

            var trans = _dataRetreival.InsertOrUpdateTransaction(0, amt, desc, typeId, categoryMisc, now, personId, false);

            Assert.IsNotNull(trans, "Expected a non null Transaction");
        }
    }
}
