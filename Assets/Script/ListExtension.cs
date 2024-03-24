using Assets.Script.Cells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script
{
    public static class ListExtension
    {
        public static Cell[] GetRow(this List<Cell> cell, int index) =>
            cell.Where(c => c.row == index).ToArray();
        public static Cell[] GetColumn(this List<Cell> cell, int index) =>
            cell.Where(c => c.col == index).ToArray();
        public static List<Cell> GetNull(this List<Cell> cell) =>
            cell.Where(c => c.Child == null).ToList();
        public static Cell[] GetNullArray(this List<Cell> cell) =>
            cell.Where(c => c.Child == null).ToArray();
        public static Cell Get(this List<Cell> cell, int row, int col) =>
            cell.Where(c => c.row == row && c.col == col).FirstOrDefault();
    }
}
