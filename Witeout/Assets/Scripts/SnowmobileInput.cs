using UnityEngine;

namespace Snowmobile
{
    public class SnowmobileInput : MonoBehaviour
    {
        private SnowmobilPhysics snowmobile;

        void Start()
        {
            snowmobile = GetComponent<SnowmobilPhysics>();  
        }
        void Update()
        {
            if (snowmobile == null)
            {
                return;
            }

            float throttle = Input.GetAxis("Vertical");
            float steering = Input.GetAxis("Horizontal");
            bool boostPressed = Input.GetKey(KeyCode.LeftShift);

            snowmobile.SetInputs(throttle, steering, boostPressed);
        }
    }
}