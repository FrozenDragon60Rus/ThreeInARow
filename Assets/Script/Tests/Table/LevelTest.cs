using Assets.Script.Table;
using NUnit.Framework;

namespace Tests.Table
{
    public class LevelTest
    {
        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void LevelCellCountTest(in int number)
        {
            Level level = new Level();
            level.Number = number;
            Assert.True(level.Cell.Length == level.Row * level.Column);
        }
        [TestCase(0, 10)]
        public void LevelGetColumnTest(in int number, in int column)
        {
            Level level = new Level();
            level.Number = number;
            Assert.AreEqual(column, level.Column);
        }
        [TestCase(0, 10)]
        public void LevelGetRowTest(in int number, in int row)
        {
            Level level = new Level();
            level.Number = number;
            Assert.AreEqual(row, level.Row);
        }
        [TestCase(-1, 0)]
        [TestCase(0, 0)]
        [TestCase(1, 1)]
        [TestCase(1000, 0)]
        public void LevelGetNumberTest(in int number, in int expectedNumber)
        {
            Level level = new Level();
            level.Number = number;
            Assert.AreEqual(expectedNumber, level.Number);
        }
    }
}
