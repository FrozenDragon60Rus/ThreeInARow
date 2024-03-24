using UnityEngine;
using Assets.Script.Cells;

namespace Assets.Script.Elements
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
