using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Assets.Script;
using Assets.Script.Cells;
using Tests.Generator;
using Assets.Script.Elements;
using System.Reflection;
using UnityEngine.Windows;
using System;


namespace Tests
{
    public class ConsecutiveElementsTest
    {
        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator ConsecutiveElementsSearchFromStartTest()
        {
            List<Cell> cell = new(),
                       delete = new();
            Generate.Cell(cell);
            Generate.Element(cell);

            var search = new ConsecutiveElements(10, 10, 0);
            delete.AddRange(
                search.FindFromStart(cell, new ElementType[] { ElementType.Red,
                                                               ElementType.Green,
                                                               ElementType.Blue,
                                                               ElementType.Yellow
                                                             }
                                    )
                           );

            Assert.NotZero(delete.Count);
            yield return null;
        }
		[UnityTest]
		public IEnumerator ConsecutiveElementsSearchFromElementTest()
		{
			List<Cell> cell = new(),
					   delete = new();
			Generate.Cell(cell, 1, 10);
			Generate.StaticElement(cell);

			var search = new ConsecutiveElements(1, 10, 0);
			delete.AddRange(
				search.FindFromElement(cell, cell[2], null));

			Assert.NotZero(delete.Count);
			yield return null;
		}
		[UnityTest]
		public IEnumerator ConsecutiveElementsSearchFromElementZeroTest()
		{
			List<Cell> cell = new(),
					   delete = new();
			Generate.Cell(cell, 1, 10);
			Generate.StaticElement(cell);

			var search = new ConsecutiveElements(1, 10, 0);
			delete.AddRange(
				search.FindFromElement(cell, cell[1], null));

			Assert.Zero(delete.Count);
			yield return null;
		}

		[TestCase(100, 1),
		 TestCase(1, 1),
		 TestCase(0, 1),
		 TestCase(-1, -1),
		 TestCase(-100, -1)]
		public void GetDirectionTest(int input, int output)
        {
            var consecutive = new ConsecutiveElements(10, 10, 0);
            var type = consecutive.GetType();
            var flag = BindingFlags.Instance | BindingFlags.NonPublic;
            var getDirection = type.GetMethod("GetDirection", flag);
            var parameters = new object[] { input };

            int direction = (int)getDirection?.Invoke(consecutive, parameters);

            Assert.AreEqual(output, direction);
		}
        [TestCase(0, 1, 1)]
        public void ElementsCountTest(int index, int direction, int expecnedCount)
        {
            List<Cell> cell = new();

			Generate.Cell(cell);
			Generate.Element(cell);

			Func<Cell, int, bool> row = (c, pos) => c.row == pos;

			var consecutive = new ConsecutiveElements(10, 10, 0);
			var type = consecutive.GetType();
			var flag = BindingFlags.Instance | BindingFlags.NonPublic;
			var elementsCount = type.GetMethod("GetDirection", flag);
			var parameters = new object[] 
            {
				cell,
                index,
				direction,
				row
			};

            int count = (int)elementsCount?.Invoke(consecutive, parameters);

			Assert.AreEqual(expecnedCount, count);
		}

	}
}
