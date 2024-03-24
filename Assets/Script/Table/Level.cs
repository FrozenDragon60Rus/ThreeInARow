using Assets.Script.SQLite;
using System;
using System.Collections.Generic;

namespace Assets.Script.Table
{
    [Serializable]
    public class Level
    {
        public int Number {
            set => Load(value);
            get => Convert.ToInt32(parameter[nameof(Number)]); }
        public string[] Cell { get => parameter[nameof(Cell)].ToString().Split(','); }
        public int Column { get => Convert.ToInt32(parameter[nameof(Column)]); }
        public int Row { get => Convert.ToInt32(parameter[nameof(Row)]); }

        public Dictionary<string, object> parameter = new Dictionary<string, object>();

        private void Load(int value)
        {
            DataBase db = new DataBase("info");
            parameter = db.LoadLevel(value).parameter;
            if(parameter.Count == 0) parameter = db.LoadLevel(0).parameter;
        }
    }
}
