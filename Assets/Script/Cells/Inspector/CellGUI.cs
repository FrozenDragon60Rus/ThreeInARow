using Assets.Script.SQLite;
using Assets.Script.Table;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Assets.Script.Cells.Inspector
{
    [CustomEditor(typeof(CellEditor))]
    internal class CellGUI : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            CellEditor editor = target as CellEditor;

            GUIContent Label = new GUIContent("Level");
            UpdateNumber(editor);
            editor.Number = EditorGUILayout.Popup(Label, editor.Number, editor.levelList);

            if (GUILayout.Button("Generate cell"))
            {
                Level level = new Level();
                int[] cell = new int[100];
                int index = 0;
                level.parameter[nameof(level.Number)] = 0;
                level.parameter[nameof(level.Column)] = 10;
                level.parameter[nameof(level.Row)] = 10;

                while (index < 100)
                    cell[index++] = (byte)CellStatus.Default;

                level.parameter[nameof(level.Cell)] = string.Join(",", cell);

                new DataBase("info").WriteLevel(level);
            }
            if (GUILayout.Button("Update cell"))
            {
                
            }
        }
        /*public Cell GetCellByMousePosition {
            get
            {
                editor = target as CellEditor;
                return editor;
            }
        }*/
        public void UpdateNumber(CellEditor Level)
        {
            Level.levelList = new string[new DataBase("info").LoadLevelCount()];
            int index = 0;
            while (index < Level.levelList.Length)
                Level.levelList[index] = index++.ToString();
        }
    }
}
