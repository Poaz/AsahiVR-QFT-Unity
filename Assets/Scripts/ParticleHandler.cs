using System.Collections;
using System.Collections.Generic;
using Kvant;
using UnityEngine;

public class ParticleHandler : MonoBehaviour
{

    public Spray KSpray;

    private Transform _rightHand, _leftHand, _bodyCenter;
    private float _rightHandToBody, _leftHandToBody;
    private Vector3 _tmpRightHandPos;


    void Start ()
	{
	    KSpray = GameObject.FindGameObjectWithTag("Spray").GetComponent<Spray>();
        _rightHand = GameObject.FindGameObjectWithTag("RightHand").transform;
	    _bodyCenter = GameObject.FindGameObjectWithTag("BodyCenter").transform;        
    }
	
	void FixedUpdate ()
	{

        _rightHandToBody = Vector3.Magnitude(_rightHand.position - _tmpRightHandPos);
	    KSpray.noiseAmplitude = _rightHandToBody*15;
	    _tmpRightHandPos = _rightHand.position;
	}

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_rightHand.position, _bodyCenter.position);
    }
}
