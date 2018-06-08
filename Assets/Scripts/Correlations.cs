using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

public class Correlations : MonoBehaviour {
    private QFT QuaternionFFT;
    public Transform hand_L, hand_R, ActorHand_L, ActorHand_R;
    private Quaternion[] results;
    private double[] resultMagnitude;

    private int count;
    private double[] QHandLeft_real, QHandLeft_i, QHandLeft_j, QHandLeft_k, 
                    QHandLeft_Actor_real, QHandLeft_Actor_i, QHandLeft_Actor_j, QHandLeft_Actor_k;

    private Quaternion[] results_HandLeft, results_HandLeft_Actor;

    private float[] FinalMagnitudesActor, FinalMagnitudesPerformer;

    public List<double> highestValues;
    public List<int> lagIndex;

    private float sum_actor_x = 0;
    private float sum_actor_y = 0;
    private float sum_actor_z = 0;
    private float sum_actor_w = 0;
    private float sum_performer_x = 0;
    private float sum_performer_y = 0;
    private float sum_performer_z = 0;
    private float sum_performer_w = 0;
    private float std_actor_x = 0;
    private float std_actor_y = 0;
    private float std_actor_z = 0;
    private float std_actor_w = 0;
    private float std_performer_x = 0;
    private float std_performer_y = 0;
    private float std_performer_z = 0;
    private float std_performer_w = 0;

    void Start ()
    {
        highestValues.Add(0.01);
        lagIndex.Add(1024);
        count = 0;
        QuaternionFFT = GetComponent<QFT>();
        
        //Initilize Arrays 
        QHandLeft_real = new double[513];
        QHandLeft_i = new double[513];
        QHandLeft_j = new double[513];
        QHandLeft_k = new double[513];

        QHandLeft_Actor_real = new double[513];
        QHandLeft_Actor_i = new double[513];
        QHandLeft_Actor_j = new double[513];
        QHandLeft_Actor_k = new double[513];

        FinalMagnitudesActor = new float[1024];
        FinalMagnitudesPerformer = new float[1024];
        resultMagnitude = new double[1024*2];


        //StartCoroutine(Buffer());
    }
	
	void FixedUpdate () {

        if(count >= 512)
        {
            WindowsRectangle();
            results_HandLeft = QuaternionFFT.QuaternionFourierTransform(512, 2, QHandLeft_real, QHandLeft_i, QHandLeft_j, QHandLeft_k, false);
            results_HandLeft_Actor = QuaternionFFT.QuaternionFourierTransform(512, 2, QHandLeft_Actor_real, QHandLeft_Actor_i, QHandLeft_Actor_j, QHandLeft_Actor_k, false);
            //MeanMagnitude();
            CorrelationCalc(results_HandLeft, results_HandLeft_Actor);
            RotateByN();
            CalcMagnitude();
            LargestIndex();
            count = 0;
        }

	    if (count < 512)
	    {
	        QHandLeft_real[count] = hand_L.rotation.w;
	        QHandLeft_i[count] = hand_L.rotation.x;
	        QHandLeft_j[count] = hand_L.rotation.y;
	        QHandLeft_k[count] = hand_L.rotation.z;

	        QHandLeft_Actor_real[count] = ActorHand_L.rotation.w;
	        QHandLeft_Actor_i[count] = ActorHand_L.rotation.x;
	        QHandLeft_Actor_j[count] = ActorHand_L.rotation.y;
	        QHandLeft_Actor_k[count] = ActorHand_L.rotation.z;
	        count++;
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

            QHandLeft_Actor_real[count] = ActorHand_L.rotation.w;
            QHandLeft_Actor_i[count] = ActorHand_L.rotation.x;
            QHandLeft_Actor_j[count] = ActorHand_L.rotation.y;
            QHandLeft_Actor_k[count] = ActorHand_L.rotation.z;
            count++;
            yield return new WaitForEndOfFrame();
        }
    }

    public void CorrelationCalc(Quaternion[] input_1, Quaternion[] input_2)
    {
        results = QuaternionFFT.CrossCorellateQFTs(input_1, input_2);
    }

    public void MeanMagnitude()
    {
        sum_actor_x = 0;
        sum_actor_y = 0;
        sum_actor_z = 0;
        sum_actor_w = 0;

        sum_performer_x = 0;
        sum_performer_y = 0;
        sum_performer_z = 0;
        sum_performer_w = 0;

        std_actor_x = 0;
        std_actor_y = 0;
        std_actor_z = 0;
        std_actor_w = 0;

        std_performer_x = 0;
        std_performer_y = 0;
        std_performer_z = 0;
        std_performer_w = 0;

        //Mean
        for (int i = 0; i < results_HandLeft.Length; i++)
        {
            sum_actor_x += results_HandLeft_Actor[i].x;
            sum_actor_y += results_HandLeft_Actor[i].y;
            sum_actor_z += results_HandLeft_Actor[i].z;
            sum_actor_w += results_HandLeft_Actor[i].w;

            sum_performer_x += results_HandLeft[i].x;
            sum_performer_y += results_HandLeft[i].y;
            sum_performer_z += results_HandLeft[i].z;
            sum_performer_w += results_HandLeft[i].w;     
        }
    
         sum_performer_x /= results_HandLeft.Length;
         sum_performer_y/= results_HandLeft.Length;
         sum_performer_z/= results_HandLeft.Length;
         sum_performer_w/= results_HandLeft.Length;

         sum_actor_x /= results_HandLeft_Actor.Length;
         sum_actor_y /= results_HandLeft_Actor.Length;
         sum_actor_z /= results_HandLeft_Actor.Length;
         sum_actor_w /= results_HandLeft_Actor.Length;

        //Subtract Mean
        for (int i = 0; i < results_HandLeft.Length; i++)
        {
            results_HandLeft[i].x -= sum_performer_x;
            results_HandLeft[i].y -= sum_performer_y;
            results_HandLeft[i].z -= sum_performer_z;
            results_HandLeft[i].w -= sum_performer_w;

            results_HandLeft_Actor[i].x -= sum_actor_x;
            results_HandLeft_Actor[i].y -= sum_actor_y;
            results_HandLeft_Actor[i].z -= sum_actor_z;
            results_HandLeft_Actor[i].w -= sum_actor_w;

            std_actor_x = results_HandLeft_Actor[i].x * results_HandLeft_Actor[i].x;
            std_actor_y = results_HandLeft_Actor[i].y * results_HandLeft_Actor[i].y;
            std_actor_z = results_HandLeft_Actor[i].z * results_HandLeft_Actor[i].z;
            std_actor_w = results_HandLeft_Actor[i].w * results_HandLeft_Actor[i].w;

            std_performer_x = results_HandLeft[i].x * results_HandLeft[i].x;
            std_performer_y = results_HandLeft[i].y * results_HandLeft[i].y;
            std_performer_z = results_HandLeft[i].z * results_HandLeft[i].z;
            std_performer_w = results_HandLeft[i].w * results_HandLeft[i].w;
        }

        std_actor_x /= results_HandLeft_Actor.Length - 1;
        std_actor_y /= results_HandLeft_Actor.Length - 1;
        std_actor_z /= results_HandLeft_Actor.Length - 1;
        std_actor_w /= results_HandLeft_Actor.Length - 1;

        std_performer_x /= results_HandLeft.Length - 1;
        std_performer_y /= results_HandLeft.Length - 1;
        std_performer_z /= results_HandLeft.Length - 1;
        std_performer_w /= results_HandLeft.Length - 1;

        std_actor_x = Mathf.Sqrt(std_actor_x);
        std_actor_y = Mathf.Sqrt(std_actor_y);
        std_actor_z = Mathf.Sqrt(std_actor_z);
        std_actor_w = Mathf.Sqrt(std_actor_w);

        std_performer_x = Mathf.Sqrt(std_performer_x);
        std_performer_y = Mathf.Sqrt(std_performer_y);
        std_performer_z = Mathf.Sqrt(std_performer_z);
        std_performer_w = Mathf.Sqrt(std_performer_w);

        for (int i = 0; i < results_HandLeft.Length; i++)
        {
            results_HandLeft[i].x /= std_performer_x;
            results_HandLeft[i].y /= std_performer_y;
            results_HandLeft[i].z /= std_performer_z;
            results_HandLeft[i].w /= std_performer_w;

            results_HandLeft_Actor[i].x /= std_actor_x;
            results_HandLeft_Actor[i].y /= std_actor_y;
            results_HandLeft_Actor[i].z /= std_actor_z;
            results_HandLeft_Actor[i].w /= std_actor_w;
        }
    }

    public void RotateByN()
    {
        var maxIndex = (int) (results.Length * 0.5f);
        for (int i = 0; i < maxIndex; i++)
        {
            var tmp = results[i + maxIndex];
            results[i + maxIndex] = results[i];
            results[i] = tmp;
            /*
            var tmp = results_HandLeft[i + maxIndex];
            results_HandLeft[i + maxIndex] = results_HandLeft[i];
            results_HandLeft[i] = tmp;

            var tmpActor = results_HandLeft_Actor[i + maxIndex];
            results_HandLeft_Actor[i + maxIndex] = results_HandLeft_Actor[i];
            results_HandLeft_Actor[i] = tmpActor;
            */
        }
    }

    public float QuaternionMagnitude(float x, float y, float z, float w)
    {
        var result = Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(y, 2) + Mathf.Pow(z, 2) + Mathf.Pow(w, 2));
        return result;
    }

    public void CalcMagnitude()
    {
        for (int i = 0; i < results.Length; i++)
        {

            resultMagnitude[i] = QuaternionMagnitude(results[i].x, results[i].y,
                results[i].z, results[i].w);
            /*
            FinalMagnitudesPerformer[i] = QuaternionMagnitude(results_HandLeft[i].x, results_HandLeft[i].y,
                results_HandLeft[i].z, results_HandLeft[i].w);

            FinalMagnitudesActor[i] = QuaternionMagnitude(results_HandLeft_Actor[i].x, results_HandLeft_Actor[i].y,
                results_HandLeft_Actor[i].z, results_HandLeft_Actor[i].w);
                */
        }
    }

    public void LargestIndex()
    {
        var largest = 0.0;
        var result = 0;

        for(int i = 1; i < resultMagnitude.Length; i++)
        {
            if (resultMagnitude[i] > largest)
            {
                result = i;
                largest = resultMagnitude[i];
            }
        }
        lagIndex.Add(result);
        highestValues.Add(largest);     
    }

    public void WindowsRectangle()
    {
        for (int i = 0; i < QHandLeft_real.Length; i++)
        {
            QHandLeft_real[i] *= 0.5f;
            QHandLeft_i[i] *= 0.5f;
            QHandLeft_j[i] *= 0.5f;
            QHandLeft_k[i] *= 0.5f;

            QHandLeft_Actor_real[i] *= 0.5f;
            QHandLeft_Actor_i[i] *= 0.5f;
            QHandLeft_Actor_j[i] *= 0.5f;
            QHandLeft_Actor_k[i] *= 0.5f;
        }
    }
}
