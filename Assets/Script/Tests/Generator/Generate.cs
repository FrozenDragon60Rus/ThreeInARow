using Assets.Script.Cells;
using System;
using System.Collections.Generic;
using Assets.Script.Elements;
using Assets.Script.Table;
using UnityEngine;

namespace Tests.Generator
{
    public static class Generate
    {
        public static void Cell(List<Cell> cell)
        {
			var level = new Level
			{
				Number = 0
			};

			for (int row = 0; row < level.Row; row++)
                for (int col = 0; col < level.Column; col++)
                    cell.Add(new Cell(col,
                                      row,
                                      (CellStatus)Convert.ToByte(level.Cell[cell.Count])
                                      ));
        }
		public static void Cell(List<Cell> cell, int rows, int columns)
		{
			for (int row = 0; row < rows; row++)
				for (int col = 0; col < columns; col++)
					cell.Add(new Cell(col,
									  row,
									  CellStatus.Default
									  ));
		}
		public static void Element(List<Cell> cell)
        {
            int elementIndex = 0;
            ActiveElement[] element = new[]
            {
                MonoBehaviour.Instantiate(TestElement.Blue.prefab.GetComponent<ActiveElement>()),
                MonoBehaviour.Instantiate(TestElement.Green.prefab.GetComponent<ActiveElement>()),
                MonoBehaviour.Instantiate(TestElement.Red.prefab.GetComponent<ActiveElement>()),
                MonoBehaviour.Instantiate(TestElement.Yellow.prefab.GetComponent<ActiveElement>())
            };
            foreach (Cell _cell in cell)
            {
                elementIndex = new System.Random().Next(0, 3);
                _cell.Child = element[elementIndex];
            }
        }
        public static void StaticElement(List<Cell> cell)
        {
			ActiveElement[] element = new[]
			{
				MonoBehaviour.Instantiate(TestElement.Blue.prefab.GetComponent<ActiveElement>()),
				MonoBehaviour.Instantiate(TestElement.Green.prefab.GetComponent<ActiveElement>()),
				MonoBehaviour.Instantiate(TestElement.Red.prefab.GetComponent<ActiveElement>()),
				MonoBehaviour.Instantiate(TestElement.Yellow.prefab.GetComponent<ActiveElement>())
			};

            cell[0].Child = element[0];
			cell[1].Child = element[2];
			cell[2].Child = element[1];
			cell[3].Child = element[1];
			cell[4].Child = element[1];
			cell[5].Child = element[3];
			cell[6].Child = element[0];
			cell[7].Child = element[2];
			cell[8].Child = element[3];
			cell[9].Child = element[3];
		}
    }
}
