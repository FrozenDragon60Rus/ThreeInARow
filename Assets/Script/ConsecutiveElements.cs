using Assets.Script.Cells;
using Assets.Script.Elements;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

[assembly: InternalsVisibleTo("Tests")]
namespace Assets.Script
{
    public class ConsecutiveElements
    {
        int rowCount,
            colCount,
            score;

        public ConsecutiveElements(int rowCount, int colCount, int score)
        {
            this.rowCount = rowCount;
            this.colCount = colCount;
            this.score = score;
        }

        public List<Cell> FindFromStart(List<Cell> cell, ElementType[] types)
        {
            List<Cell> destroy = new List<Cell>();

            foreach (var type in types)
            {
                var query = cell.Where(c => c.Child.Type == type).ToList();

                //Search repetitive element in row
                GetFromLine(query, destroy, rowCount, true);
                //Search repetitive element in column
                GetFromLine(query, destroy, colCount, false);
            }
            return destroy.Distinct().ToList();
        }

        private void GetFromLine(List<Cell> cell, List<Cell> destroy, int count, bool isRow)
        {
            int pos = 0,
                index,
                elementCount;

            Func<Cell, int, bool> colFunc = (c, pos) => c.col == pos,
                                  rowFunc = (c, pos) => c.row == pos;

            while (pos < count)
            {
                var line = cell.Where(c => isRow ? rowFunc(c, pos) 
                                                 : colFunc(c, pos))
                               .Where(c => !destroy.Contains(c))
                               .ToList();

                index = 0;
                while (index < line.Count)
                {
                    elementCount = ElementsCount(line, index, 1, isRow ? colFunc : rowFunc, 1);

                    if (elementCount > 2)
                    {
                        if (isRow)
                            for (int i = index; i < index + elementCount; i++)
                                GetFromCross(cell, line, destroy, i);

                        destroy.AddRange(
                            line.GetRange(index, elementCount));
                        score += elementCount;
                    }

                    index += elementCount;
                }
                pos++;
            }
        }
        private void GetFromCross(List<Cell> cell, List<Cell> row, List<Cell> destroy, int rowIndex)
        {
            int index, 
                count;
            var Col = cell.Where(c => c.col == rowIndex)
                          .Where(c => !destroy.Contains(c))
                          .ToList();
            index = Col.IndexOf(row[rowIndex]);

            int top = ElementsCount(Col, index, 1, (c, pos) => c.row == pos),
                bottom = ElementsCount(Col, index, -1, (c, pos) => c.row == pos);

            count = top + bottom;

            if (count > 1)
            {
                destroy.AddRange(
                    Col.GetRange(index - bottom, top));
                score += count;
            }
        }

        //
        private int ElementsCount(List<Cell> cell, int index, int direction, Func<Cell, int, bool> func, int count = 0)
        {
            direction = direction < 0 ? -1 : 1;
            int next = index + direction;

            if (next < 0 || index < 0) 
                return count;
            if (next == cell.Count || index == cell.Count) 
                return count;
            
            Cell difference = new Cell(cell[next].col - cell[index].col,
                                       cell[next].row - cell[index].row,
                                       cell[index].status);
            //Debug.Log($"({difference.row},{difference.col}) -> {direction}");
            return func(difference, direction) ? ElementsCount(cell, next, direction, func, count += 1)
                                               : count;
        }
        private int ElementsCount(Cell[] cell, int index, int direction, Func<Cell, int, bool> func, int count = 0)
        {
            direction = direction < 0 ? -1 : 1;
            int next = index + direction;

            if (next < 0 || index < 0) return count;
            if (next == cell.Length || index == cell.Length) return count;
            Cell difference = new Cell(cell[next].col - cell[index].col,
                                       cell[next].row - cell[index].row,
                                       cell[index].status);
            //Debug.Log($"({difference.row},{difference.col}) -> {direction}");
            return func(difference, direction) ? ElementsCount(cell, next, direction, func, count += 1)
                                               : count;
        }

        public List<Cell> FindFromElement(List<Cell> cell, Cell currentCell, GameObject[] bonus)
        {
            var Row = cell.GetRow(currentCell.row);
            var Col = cell.GetColumn(currentCell.col);
            List<Cell> destroy = new List<Cell>();

            /*Func<Cell, int, bool> colFunc = (c, pos) => c.col == pos,
                                  rowFunc = (c, pos) => c.row == pos;

            int row = Array.IndexOf(Row, currentCell),
                col = Array.IndexOf(Col, currentCell);
            int right = ElementsCount(Row, row, 1, colFunc),
                left = ElementsCount(Row, row, -1, colFunc),
                top = ElementsCount(Col, col, 1, rowFunc),
                bottom = ElementsCount(Col, col, -1, rowFunc);

            //Debug.Log($"left:{left}, right:{right}, top:{top}, bottom: {bottom}");
            int horizontal = right + left + 1,
                vertical = top + bottom + 1;
            if (horizontal > 2)
            {
                destroy.AddRange(
                    Row.ToList().GetRange(row - left, horizontal));
                if (vertical > 2)
                {
                    destroy.AddRange(Col.ToList()
                                        .GetRange(col - bottom, vertical));
                    //bonus = Bonus.Bomb;
                    return destroy.Distinct().ToList();
                }
                //bonus = AddBonus(horizontal);
                return destroy;
            }
            if (vertical > 2)
            {
                destroy.AddRange(
                    Col.ToList().GetRange(col - bottom, vertical));
                //AddBonus(vertical, destroy);
            }

            //bonus = new Dictionary<Bonus, Cell>();*/
            return destroy;
        }

        internal int GetDirection(int direction) => direction < 0 ? -1 : 1;
    }
}
