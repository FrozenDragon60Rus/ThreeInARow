using Assets.Script.Cells;
using System;
using System.Collections.Generic;
using Assets.Script.Elements;
using Assets.Script.Table;
using UnityEngine;

namespace Tests.Generator
{
    public static class GenerateCell 
    {
        public static void Generate(this List<Cell> cell)
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
		public static void Generate(this List<Cell> cell, int rows, int columns)
		{
			for (int row = 0; row < rows; row++)
				for (int col = 0; col < columns; col++)
					cell.Add(new Cell(col,
									  row,
									  CellStatus.Default
									  ));
		}
		public static void GenerateElement(this List<Cell> cell)
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
        public static void StaticElement10(this List<Cell> cell)
        {
			cell.Generate(1, 10);
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
		public static void StaticElement30(this List<Cell> cell)
		{
			cell.Generate(3, 10);

			ActiveElement[] element = new[]
			{
				MonoBehaviour.Instantiate(TestElement.Blue.prefab.GetComponent<ActiveElement>()),
				MonoBehaviour.Instantiate(TestElement.Green.prefab.GetComponent<ActiveElement>()),
				MonoBehaviour.Instantiate(TestElement.Red.prefab.GetComponent<ActiveElement>()),
				MonoBehaviour.Instantiate(TestElement.Yellow.prefab.GetComponent<ActiveElement>())
			};

			cell[0].Child = element[0]; cell[10].Child = element[0]; cell[20].Child = element[0];
			cell[1].Child = element[2]; cell[11].Child = element[1]; cell[21].Child = element[3];
			cell[2].Child = element[1]; cell[12].Child = element[1]; cell[22].Child = element[1];
			cell[3].Child = element[1]; cell[13].Child = element[3]; cell[23].Child = element[3];
			cell[4].Child = element[1]; cell[14].Child = element[0]; cell[24].Child = element[3];
			cell[5].Child = element[3]; cell[15].Child = element[3]; cell[25].Child = element[3];
			cell[6].Child = element[0]; cell[16].Child = element[0]; cell[26].Child = element[0];
			cell[7].Child = element[2]; cell[17].Child = element[1]; cell[27].Child = element[2];
			cell[8].Child = element[3]; cell[18].Child = element[1]; cell[28].Child = element[0];
			cell[9].Child = element[3]; cell[19].Child = element[1]; cell[29].Child = element[2];
		}
	}
}
