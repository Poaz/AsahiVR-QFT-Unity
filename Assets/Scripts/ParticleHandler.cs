using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Kvant;
using UnityEngine;

public class ParticleHandler : MonoBehaviour
{

    public Spray KSpray;
    private Correlations Correlation;

    public Transform _rightHand, _leftHand, _bodyCenter, _leftFoot, _rightFoot, _particleCenter, _emitter;
    public float _rightHandToBody, _leftHandToBody, _rightHandToFoot, _leftHandToFoot, _sprayWidth, accAmplifier, size;
    private Vector3 _tmpRightHandPos, _tmpDirection, _emitterWidth, _rightHandVel,_leftHandVel, _rightFootVel, _leftFootVel, _lastHandPos, _distPerFrame, _handPos;
    public Vector3 _handAcceleration;
    private Material _sprayMaterial;
    private Color _color;

    void Start ()
	{
	    KSpray = GameObject.FindGameObjectWithTag("Spray").GetComponent<Spray>();
	    Correlation = GameObject.FindGameObjectWithTag("Correlation").GetComponent<Correlations>();

        _particleCenter = KSpray.transform;
	    _sprayMaterial = KSpray.material;
	    _color = _sprayMaterial.color;

	    //_emitter = GameObject.FindGameObjectWithTag("Emitter").transform;
        _rightHand = GameObject.FindGameObjectWithTag("RightHand").transform;
	    _leftHand = GameObject.FindGameObjectWithTag("LeftHand").transform;
	    _bodyCenter = GameObject.FindGameObjectWithTag("BodyCenter").transform;
        _rightFoot = GameObject.FindGameObjectWithTag("RightFoot").transform;
        _leftFoot = GameObject.FindGameObjectWithTag("LeftFoot").transform;


        _lastHandPos = Vector3.zero;

        _particleCenter.position = _emitter.position;

	    _tmpRightHandPos = transform.position;
	}

    void FixedUpdate()
    {
        //Amplitude control by the lagIndex, normalized between 0 and 1.
        KSpray.noiseAmplitude = (Correlation.lagIndex[Correlation.lagIndex.Count-1]/-2048)+1;
 
        //Direction of spray controlled by hand positions in relation to bodycenter
        _tmpDirection = (_rightHand.position + _leftHand.position) / 2 - _bodyCenter.position;
        KSpray.initialVelocity = _tmpDirection*accAmplifier;

        //Direction Spread controlled by the distance between hands.
        _sprayWidth = Vector3.Distance(_leftHand.position, _rightHand.position);
        _emitterWidth = new Vector3(_sprayWidth,0.1f,0.1f)* size;
        KSpray.directionSpread = _sprayWidth*size;

         var colour = (float)Correlation.highestValues[Correlation.highestValues.Count-1];        
        //material color
        _color = new Color(0, colour, 0);
        _sprayMaterial.color = _color;
    }
}
