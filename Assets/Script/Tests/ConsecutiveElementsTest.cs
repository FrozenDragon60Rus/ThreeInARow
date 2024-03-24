using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Assets.Script;
using Assets.Script.Cells;
using Tests.Generator;
using Assets.Script.Elements;


namespace Tests
{
    public class ConsecutiveElementsTest
    {
        // A Test behaves as an ordinary method

        [TestCase(100, 1), 
         TestCase(1, 1),
         TestCase(0, 1),
         TestCase(-1, -1),
         TestCase(-100, -1)]
        public void ConsecutiveElementsDirectionTest(int value, int direction)
        {
            ConsecutiveElements search = new ConsecutiveElements(10, 10, 0);
            Assert.AreEqual(search.GetDirection(value), direction);
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator ConsecutiveElementsSearchFromStartTest()
        {
            List<Cell> cell = new List<Cell>(),
                       delete = new List<Cell>();
            Generate.Cell(cell);
            Generate.Element(cell);

            ConsecutiveElements search = new ConsecutiveElements(10, 10, 0);
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
    }
}
