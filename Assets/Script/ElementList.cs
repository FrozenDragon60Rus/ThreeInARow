using System;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Assets.Script
{
    [Serializable]
    public class ElementList : MonoBehaviour
    {
        public ElementInfo[] Default;
        public GameObject[] Bonus;
        public GameObject[] Barier;
        public int GetRandomIndex()
        {
            float[] rate = new float[Default.Length];
            for (int i = 0; i < rate.Length; i++)
                rate[i] = UnityEngine.Random.value * Default[i].Rate;
            return Array.IndexOf(rate, rate.Max());
        }
    }

    [Serializable]
    public class ElementInfo
    {
        [SerializeField]
        public GameObject prefab;
        [SerializeField]
        [Range(0, 1)]
        public float rate;
        public GameObject Prefab { get { return prefab; } }
        public float Rate{ get { return rate; } }
    }
}
