using Assets.Script.Cells;
using Assets.Script.Elements;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Assets.Script
{
    public class ConsecutiveElements
    {
		enum Range : Byte
		{
			Empty = 0,
			Row = 1,
			Column = 2
		}

        Range range = Range.Empty;

		int RowCount { get; }
        int ColCount { get; }
        int score;

		private readonly Func<Cell, int, bool> checkCol = (c, pos) => c.col == pos,
											   checkRow = (c, pos) => c.row == pos;
		private Func<Cell, int, bool> GetRange =>
			range == Range.Row ? checkRow : checkCol;

		private Range SwitchRange => range == Range.Row ? Range.Column : Range.Row;

		public ConsecutiveElements(int rowCount, int colCount, int score)
        {
            RowCount = rowCount;
            ColCount = colCount;
            this.score = score;
        }

        public List<Cell> FindFromStart(IEnumerable<Cell> cell, ElementType[] types)
        {
            range = Range.Empty;
            List<Cell> destroy = new();

            foreach (var type in types)
            {
                var query = cell.Where(c => c.Child.Type == type).ToList();

                //Search repetitive element in row
                GetFromLine(query, destroy, RowCount);
				//Search repetitive element in column
				GetFromLine(query, destroy, ColCount);
            }
            return destroy.Distinct().ToList();
        }

        private void GetFromLine(IEnumerable<Cell> cell, List<Cell> destroy, int count)
        {
			int pos = 0,
                index,
                elementCount;

            while (pos < count)
            {
				range = SwitchRange;
				var line = cell.Where(c => GetRange(c, pos))
                               .Where(c => !destroy.Contains(c))
                               .ToList();

                index = 0;
                Range 
                    currenRange = range,
                    crossrange;
                while (index < line.Count)
                {
					range = SwitchRange;
					elementCount = ElementsCount(line, index, 1, 1);

                    if (elementCount > 2)
                    {
                        crossrange = range;
                        for (int i = index; i < index + elementCount; i++)
                        {
                            GetFromCross(cell, line, destroy, i);
                            range = crossrange;
                        }

						destroy.AddRange(
                            line.GetRange(index, elementCount));
                        score += elementCount;
                    }
					range = currenRange;

					index += elementCount;
				}
                pos++;
            }
        }
        private void GetFromCross(IEnumerable<Cell> cell, IEnumerable<Cell> line, List<Cell> destroy, int _index)
        {
            int index, 
                count;

            var crossLine = cell.Where(c => GetRange(c, _index))
                                .Where(c => !destroy.Contains(c))
                                .ToList();

			index = crossLine.IndexOf(line.ElementAt(_index));

			range = SwitchRange;
			int top = ElementsCount(crossLine, index, 1),
                bottom = ElementsCount(crossLine, index, -1); Debug.Log($"top: {top}, bottom: {bottom}, cross: {crossLine.Count} ");

			count = top + bottom;

            if (count > 1)
            {
                destroy.AddRange(
					crossLine.GetRange(index - bottom, top));
                score += count;
            }
        }

		public List<Cell> FindFromElement(IEnumerable<Cell> cell, Cell currentCell, GameObject[] bonus)
        {
			range = Range.Column;

			List<Cell> destroy = new();

            var query = cell.Where(c => c.Child.Type == currentCell.Child.Type);
            var rowList = query.GetRow(currentCell.row);
			var colList = query.GetColumn(currentCell.col);
                
			GetFromCross(query, rowList, destroy, currentCell.col);
			GetFromCross(query, colList, destroy, currentCell.row);

			return destroy.Distinct().ToList();
		}

		//
		private int ElementsCount(IEnumerable<Cell> line, int index, int direction, int count = 0)
		{
			direction = GetDirection(direction);
			int next = index + direction;

			if (next < 0 || index < 0)
				return count;
			if (next == line.Count() || index == line.Count())
				return count;

			var difference = new Cell(line.ElementAt(next).col - line.ElementAt(index).col,
									  line.ElementAt(next).row - line.ElementAt(index).row,
									  line.ElementAt(index).status);
			//Debug.Log($"({difference.row},{difference.col}) -> {direction}");
			return GetRange(difference, direction) ? ElementsCount(line, next, direction, count += 1)
											       : count;
		}

		private int GetDirection(int direction) => direction < 0 ? -1 : 1;
	}
}
