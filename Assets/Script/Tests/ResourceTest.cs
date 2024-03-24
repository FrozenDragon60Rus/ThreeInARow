using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;
using UnityEngine;

namespace Tests
{
    internal class ResourceTest
    {
        [UnityTest]
        public IEnumerator ResourceLoadTest()
        {
            var prefab = Resources.Load<GameObject>(@"Prefabs\Element\BlueElement");
            Assert.NotNull(prefab);

            yield return null;
        }
    }
}
