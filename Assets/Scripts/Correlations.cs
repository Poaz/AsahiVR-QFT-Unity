using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Correlations : MonoBehaviour {
    private QFT QuaternionFFT;
    public Transform hand_L, hand_R;


    private int count;
    private float[] QHandLeft_real, QHandLeft_i, QHandLeft_j, QHandLeft_k;
    private Quaternion[] results_HandLeft;
    // Use this for initialization
    void Start () {
        QuaternionFFT = GetComponent<QFT>();
	}
	
	// Update is called once per frame
	void Update () {

        if(count >= 512)
        {
            results_HandLeft = QuaternionFFT.QuaternionFourierTransform(512, 2, QHandLeft_real, QHandLeft_i, QHandLeft_j, QHandLeft_k);
            count = 0;
        }

	}

    IEnumerator Buffer()
    {
        while (true)
        {
            QHandLeft_real[count] = hand_L.rotation.w;
            QHandLeft_i[count] = hand_L.rotation.x;
            QHandLeft_j[count] = hand_L.rotation.y;
            QHandLeft_k[count] = hand_L.rotation.z;
            count++;
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator CorrelationCalc()
    {
        var results = 1;
        QuaternionFFT.CrossCorellateQFTs(results_HandLeft, results_HandLeft);
        yield return "Success";
    }
}
