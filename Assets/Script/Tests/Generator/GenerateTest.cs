using NUnit.Framework;
using Assets.Script.Cells;
using System.Collections.Generic;
using System.Linq;

namespace Tests.Generator
{
    internal class GenerateTest
    {
        [Test]
        public void CellTest()
        {
            List<Cell> cell = new();
            cell.Generate();
            Assert.AreEqual(100, cell.Count);
        }
        [Test]
        public void ElementTest()
        {
            List<Cell> cell = new();
            cell.Generate();
            cell.Element();
            Assert.AreEqual(100, cell.Select(c => c.Child != null).Count());
        }
    }
}
