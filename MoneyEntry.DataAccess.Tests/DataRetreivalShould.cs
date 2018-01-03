using System;
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
    }
}
