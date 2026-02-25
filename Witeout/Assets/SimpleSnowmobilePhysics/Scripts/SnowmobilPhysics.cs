using UnityEngine;


namespace Snowmobile
{
    public class SnowmobilPhysics : MonoBehaviour
    {
        [Header("Vehicles Config")]
        [SerializeField]
        private float maxSteering = 20f;
        [SerializeField]
        private float maxVelocity = 50f;
        [SerializeField]
        private Vector3 centerOfMass;

        [Header("Motor")]
        [SerializeField]
        private float motorSmoothing = 1f;

        [Header("Motor Audio")]
        [SerializeField]
        private float volumeOffset = 0.25f;
        [SerializeField]
        private float volumeFactor = 1f;
        [SerializeField]
        private float pitchOffset = 1f;
        [SerializeField]
        private float pitchFactor = 5f;

        [Header("Track Animation")]
        [SerializeField]
        private float wheelToTrackFactor = 0.075f;




        [Header("Dependencies")]
        [SerializeField]
        private Rigidbody wheelRB;
        [SerializeField]
        private ConfigurableJoint wheel;
        [SerializeField]
        private ConfigurableJoint leftSkii;
        [SerializeField]
        private ConfigurableJoint rightSkii;
        [SerializeField]
        private Transform steering;

        // private
        private float motorValue = 0f;
        private AudioSource source;
        private TrackAnimation trackAnimation;
        private Rigidbody rb;

        private float throttleInput;
        private float steeringInput;

        void Start()
        {
            // disable intercollision
            foreach(Collider col1 in GetComponentsInChildren<Collider>())
            {
                foreach (Collider col2 in GetComponentsInChildren<Collider>())
                {
                    Physics.IgnoreCollision(col1, col2, true);
                }
            }

            foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
            {
                rb.solverIterations = 255;
                rb.solverVelocityIterations = 255;
            }

            // grab references
            trackAnimation = GetComponent<TrackAnimation>();
            source = GetComponent<AudioSource>();
            rb = GetComponent<Rigidbody>();
        }
        void Update()
        {
            // set  com
            rb.centerOfMass = centerOfMass;

            // steering
            Quaternion targetRotation = Quaternion.Euler(new Vector3(-maxSteering * steeringInput, 0, 0));
            leftSkii.targetRotation = targetRotation;
            rightSkii.targetRotation = targetRotation;

            // motor value
            motorValue += (throttleInput - motorValue) * Time.deltaTime * motorSmoothing;
            motorValue = Mathf.Clamp(motorValue, -0.2f, 1f);

            // wheel
            wheel.targetAngularVelocity = new Vector3(motorValue * maxVelocity, 0, 0);

            // track animation
            trackAnimation.trackValue += transform.InverseTransformDirection(wheelRB.angularVelocity).x * Time.deltaTime * wheelToTrackFactor;

            // steering animation
            steering.localRotation = leftSkii.transform.localRotation;


            // audio
            source.volume = volumeOffset + Mathf.Abs(motorValue) * volumeFactor;
            source.pitch = pitchOffset + Mathf.Abs(motorValue) * pitchFactor;


        }

        public void SetInputs(float throttle, float steering)
        {
            throttleInput = throttle;
            steeringInput = steering;
        }
    }
}