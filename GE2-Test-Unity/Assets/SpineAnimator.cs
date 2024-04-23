﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpineAnimator : MonoBehaviour {
    public GameObject[] bones;
    public GameObject[] tail;
    public GameObject baseTail;

    public float bondDamping = 25;
    public float angularBondDamping = 25;

    private List<Vector3> offsets = new List<Vector3>();
    
    // Use this for initialization
    void Start ()
    {
        for (int i = 0; i < tail.Length; i++)
        {
            tail[i] = baseTail;
            Instantiate(tail[i]);
        }

        if (bones != null)
        {
            for (int a = 0; a < bones.Length; a++)
            {
                GameObject prevBone = (a == 0)
                    ? this.gameObject
                    : bones[a - 1];
                GameObject bone = bones[a];

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
