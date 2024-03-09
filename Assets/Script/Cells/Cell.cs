using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script.Cells
{
    public enum CellStatus : byte
    {
        Default,
        Empty
    }
    public class Cell
    {
        public readonly Vector2 position;
        public readonly int col, row;
        public CellStatus status;
        public static float size = 0.5f;
        [NonSerialized]
        private Element child;
        public Element Child
        {
            set
            {
                child = value;
                if(value != null)
                    child.Parent = this;
            }
            get => child;
        }
        public void RemoveChild() =>
            child = null;

        public Cell(int col, int row, CellStatus status)
        {
            this.col = col;
            this.row = row;
            this.status = status;
            position = new Vector2(col * size, row * size);
        }
        
        public object Clone() => MemberwiseClone();
    }
}
