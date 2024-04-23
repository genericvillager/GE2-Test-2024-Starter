using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Purchasing;
using UnityEngine;

public class SpineAnimator : MonoBehaviour {
    public GameObject[] bones;
    public GameObject baseTail;

    public float bondDamping = 25;
    public float angularBondDamping = 25;

    public Vector3 frequency, baseSize, startAngle, multi;

    private List<Vector3> offsets = new List<Vector3>();
    
    // Use this for initialization
    void Start ()
    {
        if (bones != null)
        {
            for (int i = 0; i < bones.Length; i++)
            {
                bones[i] = baseTail;

                GameObject prevBone = (i == 0)
                    ? this.gameObject
                    : bones[i - 1];
                GameObject bone = bones[i];

                //float startAngle = startAngle - baseSize;

                Vector3 maxSize = baseSize + multi;

                Vector3 minSize = baseSize;

                bones[i] = Instantiate(bones[i]);

                Vector3 offset = bone.transform.position
                    - prevBone.transform.position;

                offset = Quaternion.Inverse(prevBone.transform.rotation)
                    * offset;

                offsets.Add(offset);
            }
        }
    }
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        for (int i = 0; i < bones.Length; i++)
        {
            GameObject prevBone = (i == 0)
                ? this.gameObject
                : bones[i - 1];

            GameObject bone = bones[i];

            //Vector3 wantedPosition = prevBone.transform.TransformPoint(offsets[i]);

            Vector3 wantedPosition = (prevBone.transform.rotation * offsets[i]) + prevBone.transform.position;

            bone.transform.position = Vector3.Lerp(bone.transform.position
                , wantedPosition
                , Time.deltaTime * bondDamping);

            Quaternion wantedRotation = Quaternion.LookRotation(prevBone.transform.position
                - bone.transform.position);

            bone.transform.rotation = Quaternion.Slerp(bone.transform.rotation
                , wantedRotation
                , Time.deltaTime * angularBondDamping);

        }
    }
}
