using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.Script.Cells;
using System;
using Assets.Script.SQLite;

namespace Assets.Script
{
    public class Field : MonoBehaviour
    {
        //Element[,] cell;
        List<Cell> cell;
        [SerializeField]
        public int rowCount,
                   columnCount;
        public int score = 0;
        [SerializeField]
        private BoxTrigger Trigger;
        [SerializeField]
        private Sprite sprite;

        Element target;
        Vector3 offset;
        bool userControl = false;


        // Start is called before the first frame update
        void Start()
        {
            cell = new List<Cell>();
            LoadLevel(0);
            DrawCell();
            
            //StartCoroutine(FillCell());
        }
        private void Update()
        {
            if(userControl) SelectElement();
        }

        private void LoadLevel(int Number)
        {
            Table.Level level = new Table.Level();

            level.Number = Number;
            rowCount = level.Row;
            columnCount = level.Column;

            for (int row = 0; row < rowCount; row++)
                for (int col = 0; col < columnCount; col++)
                    cell.Add(new Cell(col, 
                                      row, 
                                      (CellStatus) Convert.ToByte(level.Cell[cell.Count])
                                      ));
        }
        private void DrawCell()
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            MergeSprite.Join(sprite, spriteRenderer.sprite, cell);
        }
        private void GenerateRow(Cell[] Row)
        {
            var element = GetComponent<ElementList>();
            int elementIndex;
            foreach (Cell _cell in Row)
            {
                elementIndex = element.GetRandomIndex();
                _cell.Child = Instantiate(
                                CreateElement(element.info[elementIndex], _cell.col));
            }
            Trigger.forbidden = true;
        }

        private Element CreateElement(elementInfo info, int col)
        {
            Element currentElement = info.Prefab
                                         .GetComponent<Element>();
            currentElement.transform.position = new Vector3(col * Cell.size, Trigger.transform.position.y);
            return currentElement;
        }

        private IEnumerator WaitForAllArrived()
        {
            while (cell.Where(c => c.Child.OnPosition).ToArray().Length < cell.Count)
                yield return false;

            FindMatches();
            yield return true;
        }
        private IEnumerator WaitSwap(Cell currentCell, Cell neighbor)
        {
            while (!(currentCell.Child.OnPosition && neighbor.Child.OnPosition)) yield return false;

            if (!FindMatchesFromElement(new List<Cell> { currentCell, neighbor }))
                SwapElements(currentCell, neighbor, true);
            yield return true;
        }

        private void FindMatches()
        {
            List<Cell> delete = new Algoritm(rowCount, columnCount, score)
                                    .FindFromStart(cell, new ElementType[] { ElementType.Red,
                                                                             ElementType.Green,
                                                                             ElementType.Blue,
                                                                             ElementType.Yellow});
            delete = delete.Distinct().ToList();

            RemoveElement(delete);
        }

        private bool FindMatchesFromElement(List<Cell> currentCell)
        {
            List<Cell> delete = new List<Cell>();
            foreach (Cell _cell in currentCell)
            {
                delete.AddRange(new Algoritm(rowCount, columnCount, score)
                                    .FindFromElement(cell.Where(c => c.Child.Type == _cell.Child.Type)
                                                         .ToList(),
                                                     _cell));
            }
            delete = delete.Distinct().ToList();

            RemoveElement(delete);

            return delete.Count > 0;
        }
        private void RemoveElement(List<Cell> delete)
        {
            foreach (Cell cell in delete)
            {
                Destroy(cell.Child.gameObject);
                cell.Child = null;
            }

            if (delete.Count > 0) RelocateElement();
            else userControl = true;
        }
        private void RelocateElement()
        {
            int colIndex = 0, rowIndex = 0;
            Cell[] column;
            Element[] element;
            while (colIndex < columnCount)
            {
                column = cell.GetColumn(colIndex);
                element = column.Select(c => c.Child).Where(e => e != null).ToArray();

                foreach (Element _element in element)
                    column[rowIndex++].Child = _element;
                while (rowIndex < columnCount)
                    column[rowIndex++].Child = null;

                rowIndex = 0;
                colIndex++;
            }

            StartCoroutine(FillCell());
        }
        private IEnumerator FillCell()
        {
            List<Cell> Null = cell.GetNull();
            Cell[] Row;
            int[] rowIndex = Null.Select(c => c.row)
                                 .Distinct()
                                 .ToArray();

            int index = 0;
            while (index < rowIndex.Length)
            {
                if (Trigger.forbidden == false)
                {
                    Row = Null.GetRow(rowIndex[index++]);
                    GenerateRow(Row);
                }
                yield return Trigger.forbidden;
            }

            StartCoroutine(WaitForAllArrived());
        }
        private void SelectElement()
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Input.GetMouseButtonDown(0))
            {
                try
                {
                    target = Physics2D.OverlapPoint(mousePosition).GetComponent<Element>();
                }
                catch (Exception e) { Debug.Log(e.Message); }
                offset = target.transform.position - mousePosition;
            }
            if (target)
            {
                System.Drawing.Point NeighboringDirection = GetNeighboringDirection(target.transform.position, mousePosition + offset);

                if (NeighboringDirection.X + NeighboringDirection.Y != 0) {
                    int row = target.Parent.row + NeighboringDirection.X,
                        col = target.Parent.col + NeighboringDirection.Y;
                    if (row >= 0 && row < rowCount && 
                        col >= 0 && col < columnCount)
                    {
                        userControl = false;
                        Cell neighbor = cell.Where(c => c.row == row && c.col == col)
                                            .ToArray()
                                            .First(),
                             currentCell = target.Parent;

                        SwapElements(neighbor, currentCell, false);

                        target = null;
                    }
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                if(target)
                    target = null;
            }
        }
        private void SwapElements(Cell currentCell, Cell neighbor, bool second)
        {
            //if(currentCell.Child == null || neighbor.Child == null) return;
            Cell swap = currentCell.Clone() as Cell;
            currentCell.Child = neighbor.Child;
            neighbor.Child = swap.Child;

            if (second) return;
            StartCoroutine(WaitSwap(neighbor, currentCell));
        }

        private System.Drawing.Point GetNeighboringDirection(Vector2 start, Vector2 end)
        {
            Vector2 direction = new Vector2(end.x - start.x,
                                            end.y - start.y);
            
            float distance = 0.25f;
            if (Math.Abs(direction.x) < distance && Math.Abs(direction.y) < distance) 
                return new System.Drawing.Point(0, 0);

            return Math.Abs(direction.x) < Math.Abs(direction.y) 
                ? new System.Drawing.Point(direction.y < 0 ? -1 
                                                           : 1, 
                                           0)
                : new System.Drawing.Point(0, 
                                           direction.x < 0 ? -1 
                                                           : 1);
        }
    }
}
