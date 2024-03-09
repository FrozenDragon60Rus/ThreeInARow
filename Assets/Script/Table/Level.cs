using Assets.Script.SQLite;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Assets.Script.Table
{
    [Serializable]
    internal class Level
    {
        public int Number {
            set => Load(value);
            get => (int)parameter[nameof(Number)]; }
        public string[] Cell { get => parameter[nameof(Cell)].ToString().Split(','); }
        public int Column { get => Convert.ToInt32(parameter[nameof(Column)]); }
        public int Row { get => Convert.ToInt32(parameter[nameof(Row)]); }

        public Dictionary<string, object> parameter = new Dictionary<string, object>();

        private void Load(int value)
        {
            DataBase db = new DataBase("info");
            parameter = db.LoadLevel(value).parameter;
        }
    }
}
