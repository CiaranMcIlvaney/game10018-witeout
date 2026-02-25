using UnityEngine;

namespace Snowmobile
{
    public class SnowmobileExampleInputs : MonoBehaviour
    {
        // Example Impelentation of Inputs
        private SnowmobilPhysics snowmobile;

        void Start()
        {
            // Get reference of snowmobile physics object
            snowmobile = GetComponent<SnowmobilPhysics>();  
        }
        void Update()
        {
            // set throttle and steering input by vertical and horizontal inputs
            snowmobile.SetInputs(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));
        }
    }
}