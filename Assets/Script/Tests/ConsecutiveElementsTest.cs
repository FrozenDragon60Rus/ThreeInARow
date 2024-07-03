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
using NUnit.Framework.Constraints;
using System.Linq;


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
            cell.GenerateElement();

			int score = 0;
            delete.AddRange(
                cell.FindConsecutiveFromStart(
					new ElementType[] { 
						ElementType.Red,
                        ElementType.Green,
                        ElementType.Blue,
                        ElementType.Yellow
                    },
					ref score));

            Assert.NotZero(delete.Count);
            yield return null;
        }
		[UnityTest]
		public IEnumerator SearchFromStartStaticTest()
		{
			List<Cell> cell = new(),
					   delete = new();

			cell.StaticElement30();

			int score = 0;
			delete.AddRange(
				cell.FindConsecutiveFromStart(
					new ElementType[] { 
						ElementType.Red,
						ElementType.Green,
						ElementType.Blue,
						ElementType.Yellow
					},
					ref score
				)
			);

			foreach (var element in delete)
				Debug.Log(element.row * 10 + element.col);

			Assert.AreEqual(19, delete.Count);
			yield return null;
		}
		[UnityTest]
		public IEnumerator SearchFromElementTest()
		{
			List<Cell> cell = new(),
					   delete = new();
			cell.StaticElement10();

			int score = 0;
			delete.AddRange(
				cell.FindConsecutiveFromElement(cell[2], null, ref score));

			Assert.NotZero(delete.Count);
			yield return null;
		}
		[UnityTest]
		public IEnumerator SearchFromElementZeroTest()
		{
			List<Cell> cell = new(),
					   delete = new();
			cell.StaticElement10();

			Debug.Log("start " + delete.Count);
			int score = 0;
			delete.AddRange(
				cell.FindConsecutiveFromElement(cell[1], null, ref score));
			Debug.Log("end " + delete.Count);

			Assert.Zero(delete.Count);
			yield return null;
		}
		[TestCase(2, 3),
			TestCase(12, 3),
			TestCase(22, 3)]
		public void GetFromCrossColumnTest(int index, int expectedCount)
		{
			List<Cell> cell = new(),
					   delete = new();
			cell.StaticElement30();

			var type = typeof(CellConsecutiveExtension); ;
			var flag = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static;
			var getFromCross = type.GetMethod("GetFromCross", flag);
			var query = cell.Get(cell[index].Child.Type);

			CellConsecutiveExtension.range = CellConsecutiveExtension.Range.Column;

			int score = 0;
			var parameters = new object[] 
			{
				query, 
				delete,
				cell[index],
				score
			};

			getFromCross?.Invoke(null, parameters);

			Assert.AreEqual(expectedCount, delete.Count);
		}
		[TestCase(23, 3),
			TestCase(24, 3),
			TestCase(25, 3)]
		public void GetFromCrossRowTest(int index, int expectedCount)
		{
			List<Cell> cell = new(),
					   delete = new();
			cell.StaticElement30();

			var type = typeof(CellConsecutiveExtension); ;
			var flag = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static;
			var getFromCross = type.GetMethod("GetFromCross", flag);
			var query = cell.Get(cell[index].Child.Type);

			CellConsecutiveExtension.range = CellConsecutiveExtension.Range.Row;

			int score = 0;
			var parameters = new object[]
			{
				query,
				delete,
				cell[index],
				score
			};

			getFromCross?.Invoke(null, parameters);

			Assert.AreEqual(expectedCount, delete.Count);
		}

		[TestCase(100, 1),
		 TestCase(1, 1),
		 TestCase(0, 1),
		 TestCase(-1, -1),
		 TestCase(-100, -1)]
		public void GetDirectionTest(int input, int output)
        {
			var type = typeof(CellConsecutiveExtension);
			var flag = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static;
            var getDirection = type.GetMethod("GetDirection", flag);
            var parameters = new object[] { input };

            int direction = (int)getDirection?.Invoke(null, parameters);

            Assert.AreEqual(output, direction);
		}
        [TestCase(0, 1, 1),
		 TestCase(2, 1, 3),
		 TestCase(4, -1, 3)]
        public void ElementsInSequenceTest(int index, int direction, int expecnedCount)
        {
            List<Cell> cell = new();

			cell.StaticElement10();
			var query = cell.Get(cell[index].Child.Type).ToList();

			var type = typeof(CellConsecutiveExtension);
			var flag = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static;

			var sequence = type.GetMethod("ElementsInSequence", flag);
			int queryIndex = query.IndexOf(cell[index]);
			var parameters = new object[] 
            {
				query,
				queryIndex,
				direction,
				1
			};

            int count = (int)sequence?.Invoke(null, parameters);

			Assert.AreEqual(expecnedCount, count);
		}

	}
}
