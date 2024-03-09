using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace Assets.Script.Cells
{
    public enum ElementType
    {
        Red,
        Green, 
        Blue,
        Yellow
    }

    public abstract class Element : MonoBehaviour
    {
        [SerializeField]
        protected ElementType type;
        public bool isFreeze = false;
        protected Cell parent;

        public abstract bool OnPosition { get; }
        public abstract Cell Parent { set; get; }

        public ElementType Type
        {
            get => type;
        }

        protected abstract void Reaction();
    }
}
