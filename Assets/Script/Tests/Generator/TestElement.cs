using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Script;

namespace Tests.Generator
{
    public static class TestElement
    {
        static public ElementInfo Red =>
            new ElementInfo
            {
                prefab = Resources.Load<GameObject>(@"Prefabs\Element\RedElement"),
                rate = 1f
            };
        static public ElementInfo Blue =>
            new ElementInfo
            {
                prefab = Resources.Load<GameObject>(@"Prefabs\Element\BlueElement"),
                rate = 1f
            };
        static public ElementInfo Green =>
            new ElementInfo
            {
                prefab = Resources.Load<GameObject>(@"Prefabs\Element\GreenElement"),
                rate = 1f
            };
        static public ElementInfo Yellow =>
            new ElementInfo
            {
                prefab = Resources.Load<GameObject>(@"Prefabs\Element\YellowElement"),
                rate = 1f
            };
    }
}
