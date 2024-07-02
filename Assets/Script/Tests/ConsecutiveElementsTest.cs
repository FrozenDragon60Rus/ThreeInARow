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
        public IEnumerator SearchFromStartTest()
        {
            List<Cell> cell = new(),
                       delete = new();
            cell.Generate();
            cell.Element();

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
		public IEnumerator SearchFromStartStaticTest()
		{
			List<Cell> cell = new(),
					   delete = new();
			cell.Generate(3, 10);
			cell.StaticElement30();

			var search = new ConsecutiveElements(3, 10, 0);
			delete.AddRange(
				search.FindFromStart(cell, new ElementType[] { ElementType.Red,
															   ElementType.Green,
															   ElementType.Blue,
															   ElementType.Yellow
															 }
									)
						   );

			foreach (var element in delete)
				Debug.Log(element.row * 10 + element.col);

			Assert.AreEqual(16, delete.Count);
			yield return null;
		}
		[UnityTest]
		public IEnumerator SearchFromElementTest()
		{
			List<Cell> cell = new(),
					   delete = new();
			cell.Generate(1, 10);
			cell.StaticElement10();

			var search = new ConsecutiveElements(1, 10, 0);
			delete.AddRange(
				search.FindFromElement(cell, cell[2], null));

			Assert.NotZero(delete.Count);
			yield return null;
		}
		[UnityTest]
		public IEnumerator SearchFromElementZeroTest()
		{
			List<Cell> cell = new(),
					   delete = new();
			cell.Generate(1, 10);
			cell.StaticElement10();

			Debug.Log("start "+delete.Count);
			var search = new ConsecutiveElements(1, 10, 0);
			delete.AddRange(
				search.FindFromElement(cell, cell[1], null));
			Debug.Log("end " + delete.Count);

			Assert.Zero(delete.Count);
			yield return null;
		}
		[UnityTest]
		public IEnumerator GetFromCrossTest()
		{
			List<Cell> cell = new(),
					   delete = new();
			cell.Generate(3, 10);
			cell.StaticElement30();

			var consecutive = new ConsecutiveElements(3, 10, 0);
			var type = consecutive.GetType();
			var flag = BindingFlags.Instance | BindingFlags.NonPublic;
			var getDirection = type.GetMethod("GetFromCross", flag);
			var parameters = new object[] 
			{ 
				cell, 
				new Cell[]{ cell[0], cell[1], cell[2] },
				delete,
				1
			};

			int direction = (int)getDirection?.Invoke(consecutive, parameters);

			Assert.AreEqual(delete, delete);
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

			cell.Generate();
			cell.Element();

			Func<Cell, int, bool> row = (c, pos) => c.row == pos;

			var consecutive = new ConsecutiveElements(10, 10, 0);
			var type = consecutive.GetType();
			var flag = BindingFlags.Instance | BindingFlags.NonPublic;

			var rangeEnum = type.GetEnumNames();

			var rangeFlag = type.GetField("range", flag);
			rangeFlag.SetValue(consecutive, 1);

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
