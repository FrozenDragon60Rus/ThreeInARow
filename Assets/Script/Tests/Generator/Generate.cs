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
            Level level = new Level();

            level.Number = 0;

            for (int row = 0; row < level.Row; row++)
                for (int col = 0; col < level.Column; col++)
                    cell.Add(new Cell(col,
                                      row,
                                      (CellStatus)Convert.ToByte(level.Cell[cell.Count])
                                      ));
        }
        public static void Element(List<Cell> cell)
        {
            int elementIndex = 0;
            ActiveElement[] element = new ActiveElement[]{
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
    }
}
