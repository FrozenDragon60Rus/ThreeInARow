using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
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
