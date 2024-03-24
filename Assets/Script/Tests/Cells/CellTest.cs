using System;
using Assets.Script.Cells;
using Assets.Script.Elements;
using NUnit.Framework;
using Tests.Generator;
using UnityEngine;

namespace Tests.Cells
{
    [TestFixture]
    internal class CellTest
    {
        Cell cell;

        [SetUp]
        public void CellInit()
        {
            cell = new Cell(10, 10, CellStatus.Default);

            Cell.size = 0.5f;

            cell.Child = MonoBehaviour.Instantiate<Element>(
                            TestElement.Blue.prefab.GetComponent<ActiveElement>());
        }
        [Test]
        public void CellPositionTest() =>
            Assert.AreEqual(cell.position, new Vector2(5, 5));
        [Test]
        public void CellChildInitTest() =>
            Assert.IsNotNull(cell.Child);
        [Test]
        public void CellChildParantSameTest() =>
            Assert.AreSame(cell, cell.Child.Parent);
        [Test]
        public void CellCloneTest()
        {
            Cell clone = cell.Clone() as Cell;

            Assert.AreNotSame(cell, clone);
        }
        [TearDown]
        public void CellChildRemoveTest()
        {
            cell.RemoveChild();

            Assert.IsNull(cell.Child);
        }
    }
}
