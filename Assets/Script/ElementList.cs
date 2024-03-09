using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script
{
    [Serializable]
    internal class ElementList : MonoBehaviour
    {
        public List<elementInfo> info = new List<elementInfo>();
        public int GetRandomIndex()
        {
            float[] rate = new float[info.Count];
            for (int i = 0; i < rate.Length; i++)
                rate[i] = UnityEngine.Random.value * info[i].Rate;
            return Array.IndexOf(rate, rate.Max());
        }
    }

    [Serializable]
    class elementInfo
    {
        [SerializeField]
        public GameObject Prefab;
        [SerializeField]
        [Range(0, 1)]
        public float Rate;
    }
}
