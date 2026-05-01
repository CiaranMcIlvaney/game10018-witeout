using UnityEngine;

namespace Snowmobile
{
    public class SnowmobileCamera : MonoBehaviour
    {
        public Transform target; // The target object to orbit around
        public float distance = 10.0f; // Distance from the target object
        public float xSpeed = 120.0f; // Speed of the camera rotation around the x-axis
        public float ySpeed = 120.0f; // Speed of the camera rotation around the y-axis
        public float yMinLimit = -20f; // Minimum vertical angle
        public float yMaxLimit = 80f; // Maximum vertical angle
        public float distanceMin = 5f; // Minimum distance
        public float distanceMax = 20f; // Maximum distance
        public float zoomSpeed = 5.0f; // Speed of zooming

        private float x = 0.0f;
        private float y = 0.0f;
   void Start()
        {
            Vector3 angles = transform.eulerAngles;
            x = angles.y;
            y = angles.x;

            if (GetComponent<Rigidbody>())
            {
                GetComponent<Rigidbody>().freezeRotation = true;
            }
        }
        void LateUpdate()
        {
            if (target)
            {

                x += Input.GetAxis("Mouse X") * xSpeed * Time.deltaTime;
                y -= Input.GetAxis("Mouse Y") * ySpeed * Time.deltaTime;

                y = ClampAngle(y, yMinLimit, yMaxLimit);


                // Adjust distance with mouse wheel
                distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * zoomSpeed, distanceMin, distanceMax);

                Quaternion rotation = Quaternion.Euler(y, x, 0);
                Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.position;

                transform.rotation = rotation;


                transform.position = position;// - transform.position) * Time.deltaTime * 10f;
            }
        }
        static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360F)
                angle += 360F;
            if (angle > 360F)
                angle -= 360F;
            return Mathf.Clamp(angle, min, max);
        }
    }
}