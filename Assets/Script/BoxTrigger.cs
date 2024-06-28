using UnityEngine;

namespace Assets.Script
{
    internal class BoxTrigger : MonoBehaviour
    {
        public bool forbidden = false;

        private void OnTriggerExit2D(Collider2D collision) =>
            forbidden = false;
    }

    
}
