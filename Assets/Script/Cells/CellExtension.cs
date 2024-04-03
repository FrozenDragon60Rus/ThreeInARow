using Assets.Script.Cells;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Script
{
    public static class CellExtension
    {
        public static IEnumerable<Cell> GetRow(this IEnumerable<Cell> cell, int index) =>
            cell.Where(c => c.row == index);
        public static IEnumerable<Cell> GetColumn(this IEnumerable<Cell> cell, int index) =>
            cell.Where(c => c.col == index);
		public static IEnumerable<Cell> GetNull(this IEnumerable<Cell> cell) =>
			cell.Where(c => c.Child == null);
		public static IEnumerable<Cell> GetNullArray(this IEnumerable<Cell> cell) =>
            cell.Where(c => c.Child == null);
        public static Cell Get(this IEnumerable<Cell> cell, int row, int col) =>
            cell.Where(c => c.row == row && c.col == col).FirstOrDefault();
    }
}
