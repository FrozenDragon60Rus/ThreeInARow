using Assets.Script.Cells;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Script
{
    internal class Algoritm
    {
        int rowCount,
            colCount,
            score;

        public Algoritm(int rowCount, int colCount, int score)
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
                //Debug.Log(type.ToString());
                var query = cell.Where(c => c.Child.Type == type).ToList();
                
                destroy.AddRange(Step(query));
            }
            return destroy;
        }

        private List<Cell> Step(List<Cell> cell)
        {
            List<Cell> destroy = new List<Cell>();

            int rowIndex, 
                colIndex, 
                row = 0, 
                col = 0, 
                countByRow, 
                countByCol;
            while (row < rowCount)
            {
                var Row = cell.Where(c => c.row == row && !destroy.Contains(c))
                              .ToList();
                rowIndex = 0;
                while (rowIndex < Row.Count)
                {
                    countByRow = StepByRow(Row, rowIndex, 1, 1);

                    if (countByRow > 2)
                    {
                        for (int i = rowIndex; i < rowIndex + countByRow; i++)
                        {
                            var Col = cell.Where(c => c.col == i && !destroy.Contains(c))
                                          .ToList();
                            colIndex = Col.IndexOf(Row[rowIndex]);

                            int top = StepByCol(Col, colIndex, 1), 
                                bottom = StepByCol(Col, colIndex, -1);

                            countByCol = top + bottom;

                            if (countByCol > 1)
                            {
                                destroy.AddRange(
                                    Col.GetRange(colIndex - bottom, bottom));
                                destroy.AddRange(
                                    Col.GetRange(colIndex + 1, top));
                                break;
                            }
                            score += countByRow + countByCol;
                        }
                        destroy.AddRange(
                            Row.GetRange(rowIndex, countByRow));
                    }
                    rowIndex += countByRow;
                }
                row++;
            }

            while (col < colCount)
            {
                var Col = cell.Where(c => c.col == col && !destroy.Contains(c))
                              .ToList();

                colIndex = 0;
                while (colIndex < Col.Count)
                {
                    countByCol = StepByCol(Col, colIndex, 1, 1);

                    if (countByCol > 2)
                    {
                        destroy.AddRange(
                            Col.GetRange(colIndex, countByCol));
                        score += countByCol;
                    }

                    colIndex += countByCol;
                }
                col++;
            }
            return destroy;
        }

        #region StepBy
        private int StepByRow(List<Cell> cell, int index, int direction, int count = 0)
        {
            direction = direction ^ 0;
            int next = index + direction;

            if (next < 0 || index < 0) return count;
            if (next == cell.Count || index == cell.Count) return count;

            return cell[index].col == cell[next].col - direction ? StepByRow(cell, next, direction, count += 1)
                                                                     : count;
        }
        private int StepByCol(List<Cell> cell, int index, int direction, int count = 0)
        {
            direction = direction ^ 0;
            int next = index + direction;

            if (next < 0 || index < 0) return count;
            if (next == cell.Count || index == cell.Count) return count;

            return cell[index].row == cell[next].row - direction ? StepByCol(cell, next, direction, count += 1)
                                                                 : count;
        }
        private int StepByRow(Cell[] cell, int index, int direction, int count = 0)
        {
            direction = direction ^ 0;
            int next = index + direction;

            if (next < 0 || index < 0) return count;
            if (next == cell.Length || index == cell.Length) return count;

            return cell[index].col == cell[next].col - direction ? StepByRow(cell, next, direction, count += 1)
                                                                     : count;
        }
        private int StepByCol(Cell[] cell, int index, int direction, int count = 0)
        {
            direction = direction ^ 0;
            int next = index + direction;

            if (next < 0 || index < 0) return count;
            if (next == cell.Length || index == cell.Length) return count;

            return cell[index].row == cell[next].row - direction ? StepByCol(cell, next, direction, count += 1)
                                                                     : count;
        }
        #endregion

        public List<Cell> FindFromElement(List<Cell> cell, Cell currentCell) =>
            StepAround(cell.GetRow(currentCell.row), 
                       cell.GetColumn(currentCell.col), 
                       currentCell);

        public List<Cell> StepAround(Cell[] Row, Cell[] Col, Cell currentCell)
        {
            List<Cell> destroy = new List<Cell>();
            int row = Array.IndexOf(Row, currentCell), 
                col = Array.IndexOf(Col, currentCell);
            int right = StepByRow(Row, row, 1),
                left = StepByRow(Row, row, -1),
                top = StepByCol(Col, col, 1),
                bottom = StepByCol(Col, col, -1);

            //Debug.Log($"left:{left}, right:{right}, top:{top}, bottom: {bottom}");
            int horizontal = right + left + 1;
            if (horizontal > 2)
            {
                destroy.AddRange(
                    Row.ToList().GetRange(row - left, horizontal));
                if (top + bottom > 1)
                {
                        destroy.AddRange(Col.ToList()
                                            .GetRange(col - bottom, bottom));
                        destroy.AddRange(Col.ToList()
                                            .GetRange(col + 1, top));
                    return destroy;
                }
            }
            int vertical = top + bottom + 1;
            if (vertical > 2)
                destroy.AddRange(
                    Col.ToList().GetRange(col - bottom, vertical));

            return destroy;
        }
    }
}
