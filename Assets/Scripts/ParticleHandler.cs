using System.Collections;
using System.Collections.Generic;
using Kvant;
using UnityEngine;

public class ParticleHandler : MonoBehaviour
{

    public Spray KSpray;

    private Transform _rightHand, _leftHand, _bodyCenter, _leftFoot, _rightFoot, _particleCenter, _emitter;
    public float _rightHandToBody, _leftHandToBody, _rightHandToFoot, _leftHandToFoot, _sprayWidth, accAmplifier, size;
    private Vector3 _tmpRightHandPos, _tmpDirection, _emitterWidth, _rightHandVel,_leftHandVel, _rightFootVel, _leftFootVel, _lastHandPos, _distPerFrame, _handPos;
    public Vector3 _handAcceleration;
    private Material _sprayMaterial;
    private Color _color;

    void Start ()
	{
	    KSpray = GameObject.FindGameObjectWithTag("Spray").GetComponent<Spray>();
	    _particleCenter = KSpray.transform;
	    _sprayMaterial = KSpray.material;
	    _color = _sprayMaterial.color;

	    _emitter = GameObject.FindGameObjectWithTag("Emitter").transform;
        _rightHand = GameObject.FindGameObjectWithTag("RightHand").transform;
	    _leftHand = GameObject.FindGameObjectWithTag("LeftHand").transform;
	    _bodyCenter = GameObject.FindGameObjectWithTag("BodyCenter").transform;
        _rightFoot = GameObject.FindGameObjectWithTag("RightFoot").transform;
        _leftFoot = GameObject.FindGameObjectWithTag("LeftFoot").transform;


        _lastHandPos = Vector3.zero;

        _particleCenter.position = _emitter.position;

    }

    void FixedUpdate()
    {
       

        _rightHandToBody = Vector3.Magnitude(_rightHand.position - _tmpRightHandPos);
       // KSpray.noiseAmplitude = _rightHandToBody*5;
        _tmpRightHandPos = _rightHand.position;

        _rightHandToFoot = Vector3.Magnitude(_rightHand.position - _rightFoot.position);
        // KSpray.acceleration = new Vector3(0, _rightHandToFoot-1, 0);



        //direction of spray
        _tmpDirection = (_rightHand.position + _leftHand.position) / 2 - _bodyCenter.position;
        KSpray.initialVelocity = _tmpDirection*accAmplifier;

        //width of spray
        _sprayWidth = Vector3.Distance(_leftHand.position, _rightHand.position);
        _emitterWidth = new Vector3(_sprayWidth,0.1f,0.1f)* size;
        KSpray.directionSpread = _sprayWidth*size;
        //KSpray.SetEmitterSize(_emitterWidth);

        //acceleration vector
        _handPos = (_rightHand.position + _leftHand.position)/2;
        _distPerFrame = _handPos - _lastHandPos;
        _lastHandPos = _handPos;
        _handAcceleration = _distPerFrame * Time.deltaTime;
        

        //material color
        _color += new Color(_handAcceleration.x,_handAcceleration.y,_handAcceleration.z);
        //_sprayMaterial.color = _color;
        
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_rightHand.position, _bodyCenter.position);
        Gizmos.DrawLine(_rightHand.position, _rightFoot.position);
        Gizmos.color = Color.green;
        Gizmos.DrawLine((_rightHand.position+_leftHand.position)/2 , _bodyCenter.position);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(_leftHand.position, _rightHand.position);
    }
}
