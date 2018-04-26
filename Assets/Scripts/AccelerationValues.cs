using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelerationValues : MonoBehaviour
{

    private Vector3 _rightHand, _leftHand, _bodyCenter, _rightFoot, _leftFoot,
        _rightHandComparison, _leftHandComparison, _bodyCenterComparison, _rightFootComparison, _leftFootComparison,
        _rightHandLast, _leftHandLast, _bodyCenterLast, _rightFootLast, _leftFootLast, leftHandDiff, rightHandDiff, leftFootDiff, rightFootDiff;

	// Use this for initialization
	void Start ()
	{

	    _rightHand = GameObject.FindGameObjectWithTag("RightHand").transform.position;
	    _leftHand = GameObject.FindGameObjectWithTag("LeftHand").transform.position;
	    _bodyCenter = GameObject.FindGameObjectWithTag("BodyCenter").transform.position;
	    _rightFoot = GameObject.FindGameObjectWithTag("RightFoot").transform.position;
	    _leftFoot = GameObject.FindGameObjectWithTag("LeftFoot").transform.position;

	    //_rightHand

        _rightHandComparison = GameObject.FindGameObjectWithTag("RightHand2").transform.position;
	    _leftHandComparison = GameObject.FindGameObjectWithTag("LeftHand2").transform.position;
	    _bodyCenterComparison = GameObject.FindGameObjectWithTag("BodyCenter2").transform.position;
	    _rightFootComparison = GameObject.FindGameObjectWithTag("RightFoot2").transform.position;
	    _leftFootComparison= GameObject.FindGameObjectWithTag("LeftFoot2").transform.position;

    }
	
	// Update is called once per frame
	void FixedUpdate ()
	{

	   // rightHandDiff = ;



	}

}
