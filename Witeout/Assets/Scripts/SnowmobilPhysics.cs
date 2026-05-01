using UnityEngine;

namespace Snowmobile
{
    public class SnowmobilPhysics : MonoBehaviour
    {
        [Header("Vehicle Config")]
        [SerializeField] private float maxSteering = 20f;
        [SerializeField] private float maxVelocity = 50f;
        [SerializeField] private Vector3 centerOfMass;

        [Header("Motor")]
        [SerializeField] private float motorSmoothing = 1f;

        [Header("Boost")]
        [SerializeField] private float boostMultiplier = 1.5f;
        [SerializeField] private float boostFuelDrainPerSecond = 8f;

        [Header("Fuel")]
        [SerializeField] private float maxFuel = 100f;
        [SerializeField] private float currentFuel = 100f;
        [SerializeField] private float normalFuelDrainPerSecond = 2f;
        [SerializeField] private bool canMoveWithoutFuel = false;

        [Header("Motor Audio")]
        [SerializeField] private float masterVolume = 0.2f;
        [SerializeField] private float volumeOffset = 0.25f;
        [SerializeField] private float volumeFactor = 1f;
        [SerializeField] private float pitchOffset = 1f;
        [SerializeField] private float pitchFactor = 5f;

        [Header("Track Animation")]
        [SerializeField] private float wheelToTrackFactor = 0.075f;

        [Header("Dependencies")]
        [SerializeField] private Rigidbody wheelRB;
        [SerializeField] private ConfigurableJoint wheel;
        [SerializeField] private ConfigurableJoint leftSkii;
        [SerializeField] private ConfigurableJoint rightSkii;
        [SerializeField] private Transform steering;

        private float motorValue = 0f;
        private AudioSource source;
        private TrackAnimation trackAnimation;
        private Rigidbody rb;

        private float throttleInput;
        private float steeringInput;
        private bool boostInput;

        public float CurrentFuel => currentFuel;
        public float MaxFuel => maxFuel;
        public bool HasFuel => currentFuel > 0f;

        void Start()
        {
            foreach (Collider col1 in GetComponentsInChildren<Collider>())
            {
                foreach (Collider col2 in GetComponentsInChildren<Collider>())
                {
                    Physics.IgnoreCollision(col1, col2, true);
                }
            }

            foreach (Rigidbody childRb in GetComponentsInChildren<Rigidbody>())
            {
                childRb.solverIterations = 255;
                childRb.solverVelocityIterations = 255;
            }

            trackAnimation = GetComponent<TrackAnimation>();
            source = GetComponent<AudioSource>();
            rb = GetComponent<Rigidbody>();

            currentFuel = Mathf.Clamp(currentFuel, 0f, maxFuel);
        }

        void Update()
        {
            rb.centerOfMass = centerOfMass;

            Quaternion targetRotation = Quaternion.Euler(new Vector3(-maxSteering * steeringInput, 0, 0));
            leftSkii.targetRotation = targetRotation;
            rightSkii.targetRotation = targetRotation;

            float effectiveThrottle = throttleInput;
            float effectiveMaxVelocity = maxVelocity;
            bool isBoosting = false;

            // If out of fuel, block movement unless you want the sled to still creep
            if (!HasFuel && !canMoveWithoutFuel)
            {
                effectiveThrottle = Mathf.Min(0f, throttleInput); // allows reverse only
            }

            // Boost only works while moving forward and having fuel
            if (boostInput && effectiveThrottle > 0f && HasFuel)
            {
                effectiveMaxVelocity *= boostMultiplier;
                isBoosting = true;
            }

            motorValue += (effectiveThrottle - motorValue) * Time.deltaTime * motorSmoothing;
            motorValue = Mathf.Clamp(motorValue, -0.2f, 1f);

            wheel.targetAngularVelocity = new Vector3(motorValue * effectiveMaxVelocity, 0, 0);

            if (trackAnimation != null && wheelRB != null)
            {
                trackAnimation.trackValue += transform.InverseTransformDirection(wheelRB.angularVelocity).x * Time.deltaTime * wheelToTrackFactor;
            }

            if (steering != null)
            {
                steering.localRotation = leftSkii.transform.localRotation;
            }

            if (source != null)
            {
                source.volume = (volumeOffset + Mathf.Abs(motorValue) * volumeFactor) * masterVolume;
                source.pitch = pitchOffset + Mathf.Abs(motorValue) * pitchFactor + (isBoosting ? 0.2f : 0f);
            }

            HandleFuelDrain(isBoosting);
        }

        private void HandleFuelDrain(bool isBoosting)
        {
            if (currentFuel <= 0f)
            {
                currentFuel = 0f;
                return;
            }

            // Only drain fuel while actually trying to move forward
            if (throttleInput > 0.05f)
            {
                float drain = normalFuelDrainPerSecond;

                if (isBoosting)
                {
                    drain += boostFuelDrainPerSecond;
                }

                currentFuel -= drain * Time.deltaTime;
                currentFuel = Mathf.Clamp(currentFuel, 0f, maxFuel);
            }
        }

        public void SetInputs(float throttle, float steering, bool boostPressed)
        {
            throttleInput = throttle;
            steeringInput = steering;
            boostInput = boostPressed;
        }

        public void AddFuel(float amount)
        {
            currentFuel = Mathf.Clamp(currentFuel + amount, 0f, maxFuel);
        }

        public void SetFuel(float amount)
        {
            currentFuel = Mathf.Clamp(amount, 0f, maxFuel);
        }
    }
}