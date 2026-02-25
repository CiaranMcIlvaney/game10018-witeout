using System.Collections.Generic;
using UnityEngine;


namespace Snowmobile
{
    public class TrackPath
    {
        public Vector3 position;
        public Quaternion rotation;
    }
    public class TrackChain
    {
        public float value = 0f;
        public Transform transform;
    }


    public class TrackAnimation : MonoBehaviour
    {
        [Header("Track Value [0, 1]")]
        public float trackValue = 0f;

        [Header("Bones")]
        [SerializeField]
        private Transform steering;

        [SerializeField]
        private Transform leftSki;

        [SerializeField]
        private Transform rightSki;

        [SerializeField]
        private List<Transform> trackBones;
        

        private List<TrackPath> path;
        private List<TrackChain> chain;

        void Start()
        {
            path = new List<TrackPath>();
            chain = new List<TrackChain>();

            int i = 0;
            foreach (Transform t in trackBones)
            {
                TrackPath p = new TrackPath();

                p.position = t.localPosition;
                p.rotation = t.localRotation;

                path.Add(p);

                TrackChain c = new TrackChain();

                c.transform = t;
                c.value = (float)i / (float)(trackBones.Count - 1);

                chain.Add(c);
                i++;
            }
        }
        void Update()
        {
            if (trackValue > 1f)
                trackValue -= 1f;

            if (trackValue < 0)
                trackValue += 1f;

            for (int i = 0; i < chain.Count; i++)
            {
                float val = chain[i].value + trackValue;

                while (val > 1f)
                    val -= 1f;

                TrackPath point = GetPathpointByDec(val);

                chain[i].transform.localPosition = point.position;
                chain[i].transform.localRotation = point.rotation;
            }
        }

        TrackPath GetPathpointByDec(float dec)
        {
            TrackPath point = new TrackPath();

            int index = (int)(dec * (path.Count - 1));
            float interpol = (dec * (path.Count - 1)) - (float)index;

            TrackPath A = path[index];
            TrackPath B;

            if (index >= path.Count - 1)
                B = path[0];
            else
                B = path[index + 1];

            point.position = Vector3.Lerp(A.position, B.position, interpol);
            point.rotation = Quaternion.Lerp(A.rotation, B.rotation, interpol);


            return point;
        }
    }
}