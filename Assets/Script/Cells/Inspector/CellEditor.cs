using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using Assets.Script.SQLite;

namespace Assets.Script.Cells.Inspector
{
    public class CellEditor : MonoBehaviour
    {
        [HideInInspector]
        public string[] levelList;
        private int number;
        public int Number
        {
            set 
            { 
                number = value;
            }
            get => number;
        }
    }
}
