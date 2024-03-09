using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script;
using static UnityEditor.Progress;
using UnityEngine.EventSystems;

namespace Assets.Script.Cells.Inspector
{
    public class CellGeneratonOption
    {
        private List<string> levelList = new List<string>();

        [SerializeField]
        Dropdown level;

        public void Start()
        {
            //levelList = new SQLite.DataBase("info").Load();
            level.ClearOptions();
            level.AddOptions(levelList); 
        }

        public void PointerClick()
        {
            
        }
    }
}
