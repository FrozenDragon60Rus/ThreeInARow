using Assets.Script.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

[assembly: InternalsVisibleTo("Tests")]
namespace Assets.Script.Cells
{
	internal static class CellConsecutiveExtension
	{
		internal enum Range : Byte
		{
			Empty = 0,
			Row = 1,
			Column = 2
		}

		internal static Range range = Range.Empty;
		private static IEnumerable<Cell> GetLine(this IEnumerable<Cell> cell, int pos) =>
			range == Range.Row ? cell.GetRow(pos) : cell.GetColumn(pos);
		private static IEnumerable<Cell> GetLine(this IEnumerable<Cell> cell, Cell current) =>
			range == Range.Row ? cell.GetRow(current.row) : cell.GetColumn(current.col);

		internal static Range SwitchRange => range == Range.Row ? Range.Column : Range.Row;

		public static List<Cell> FindConsecutiveFromStart(this IEnumerable<Cell> cell, ElementType[] types, ref int score)
		{
			range = Range.Empty;
			List<Cell> destroy = new();
			int RowCount = cell.Select(c => c.row).Max(),
				ColCount = cell.Select(c => c.col).Max();

			foreach (var type in types)
			{
				var query = cell.Get(type); 

				//Search repetitive element in row
				query.GetFromLine(destroy, RowCount, ref score); 
				//Search repetitive element in column
				query.GetFromLine(destroy, ColCount, ref score); 
			}
			return destroy.Distinct().ToList();
		}

		private static void GetFromLine(this IEnumerable<Cell> cell, List<Cell> destroy, int count, ref int score)
		{
			range = SwitchRange;

			int pos = 0,
				index,
				elementCount;
			Range
				currentRange = range,
				crossRange = SwitchRange;

			while (pos < count)
			{
				var line = cell.GetLine(pos)
							   .Where(c => !destroy.Contains(c))
							   .ToList();

				index = 0;

				range = crossRange;
				while (index < line.Count)
				{
					elementCount = line.ElementsInSequence(index, 1, 1);

					if (elementCount > 2)
					{
						for (int i = index; i < index + elementCount; i++)
						{
							cell.GetFromCross(destroy, line[i], ref score);
							range = crossRange;
						}

						destroy.AddRange(
							line.GetRange(index, elementCount));
						score += elementCount;
					}

					index += elementCount;
				}

				range = currentRange;
				pos++;
			}
		}
		private static void GetFromCross
		(
			this IEnumerable<Cell> cell,
			List<Cell> destroy, 
			Cell currentCell, 
			ref int score
		)
		{
			var crossLine = cell.GetLine(currentCell)
								.Where(c => !destroy.Contains(c))
								.ToList();

			int index = crossLine.IndexOf(currentCell); //Debug.Log($"index1: {_index}, index2: {index}");

			range = SwitchRange;
			int top = crossLine.ElementsInSequence(index, 1),
				bottom = crossLine.ElementsInSequence(index, -1),
				count = top + bottom; Debug.Log($"top: {top}, bottom: {bottom}");

			if (count > 1)
			{
				destroy.AddRange(
					crossLine.GetRange(index - bottom, count + 1));
				score += count;
			}
		}

		public static List<Cell> FindConsecutiveFromElement(this IEnumerable<Cell> cell, Cell currentCell, GameObject[] bonus, ref int score)
		{
			range = Range.Column;

			List<Cell> destroy = new();

			var query = cell.Get(currentCell.Child.Type); Debug.Log(currentCell.row + "" + currentCell.col);

			query.GetFromCross(destroy, currentCell, ref score);
			query.GetFromCross(destroy, currentCell, ref score);

			return destroy.Distinct().ToList();
		}

		//
		private static int ElementsInSequence(this IEnumerable<Cell> line, int index, int direction, int count = 0)
		{
			direction = GetDirection(direction);
			int next = index + direction;

			if (next < 0 || index < 0)
				return count;
			if (next == line.Count() || index == line.Count())
				return count;

			int dif = Math.Abs(line.ElementAt(next).col - line.ElementAt(index).col +
							   line.ElementAt(next).row - line.ElementAt(index).row);
			//Debug.Log($"({difference.row},{difference.col}) -> {direction}");
			return dif == 1 ? line.ElementsInSequence(next, direction, count += 1)
							: count;
		}

		private static int GetDirection(int direction) => direction < 0 ? -1 : 1;
	}
}
