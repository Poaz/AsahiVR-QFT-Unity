using System.Collections;
using System.Collections.Generic;
using Kvant;
using UnityEngine;

public class ParticleHandler : MonoBehaviour
{

    public Spray KSpray;

    private Transform _rightHand, _leftHand, _bodyCenter, _leftFoot, _rightFoot;
    public float _rightHandToBody, _leftHandToBody, _rightHandToFoot, _leftHandToFoot;
    private Vector3 _tmpRightHandPos;


    void Start ()
	{
	    KSpray = GameObject.FindGameObjectWithTag("Spray").GetComponent<Spray>();
        _rightHand = GameObject.FindGameObjectWithTag("RightHand").transform;
	    _bodyCenter = GameObject.FindGameObjectWithTag("BodyCenter").transform;
        _rightFoot = GameObject.FindGameObjectWithTag("RightFoot").transform;
        _leftFoot = GameObject.FindGameObjectWithTag("LeftFoot").transform;
    }
	
	void FixedUpdate ()
	{

        _rightHandToBody = Vector3.Magnitude(_rightHand.position - _tmpRightHandPos);
	    KSpray.noiseAmplitude = _rightHandToBody*5;
	    _tmpRightHandPos = _rightHand.position;

        _rightHandToFoot = Vector3.Magnitude(_rightHand.position - _rightFoot.position);
        KSpray.acceleration = new Vector3(0, _rightHandToFoot-1, 0);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_rightHand.position, _bodyCenter.position);
        Gizmos.DrawLine(_rightHand.position, _rightFoot.position);
    }
}
