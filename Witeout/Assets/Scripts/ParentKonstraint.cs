using UnityEngine;

namespace Snowmobile
{
    public class ParentKonstraint : MonoBehaviour
    {
        [Header("Parent Constraint")]
        [SerializeField]
        private Transform source;
        private void Start()
        {
            transform.parent = source;
        }
    }
}