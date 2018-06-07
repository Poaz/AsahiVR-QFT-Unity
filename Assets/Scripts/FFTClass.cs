using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AForge.Math;

public class FFTClass : MonoBehaviour
{
    public AudioClip aClip;
    public AudioClip bClip;

    public AudioSource aSource;
    public AudioSource bSource;

    // Use this for initialization
    void Start ()
    {
        aSource.Play();
        aSource.loop = true;
        //StartCoroutine(delayPlay(0));
        bSource.Play();
        bSource.loop = true;
    }

    IEnumerator delayPlay(float delay)
    {
        yield return new WaitForSeconds(delay);
        bSource.Play();
        StopCoroutine("delayPlay");
    }

    // Update is called once per frame
    void Update () {
        FFTtesting();
        //DisplayFFT();
    }

    private List<float> audioSignalSample;

    //My method
    public void CrossCorrelate(float[] signalA, float[] signalB)
    {
        Complex[] ComplexSignalA = new Complex[signalA.Length];
        Complex[] ComplexSignalB = new Complex[signalB.Length];
        //Convert float values from signal to complex numbers
        for (int i = 0; i < signalA.Length; i++)
        {
            //First parameter is the real value second is the imaginary
            ComplexSignalA[i] = new Complex(signalA[i],0);
            ComplexSignalB[i] = new Complex(signalB[i], 0);
        }

        crossCorrelationCoefficient = Mathf.Lerp((float)crossCorrelationCoefficient,
            (float)Correlation.CorrelationCoefficient(ComplexSignalA, ComplexSignalB), 5*Time.deltaTime);
    }

    [Range(0, 1)]
    public double crossCorrelationCoefficient;
   

    void FFTtesting()
    {
        //float[] newSignalA = new float[aSource.clip.samples * aSource.clip.channels];
        //float[] newSignalB = new float[bSource.clip.samples * bSource.clip.channels];

        //aClip.GetData(newSignalA, 0);
        //bClip.GetData(newSignalB, 0);

        //newSignalA = ZeroPadSIgnal(newSignalA);
        //newSignalB = ZeroPadSIgnal(newSignalB);

        //audioSignalSample = newSignal.ToList();

        //aSource.clip = AudioClip.Create("SampleClip", newSignal.Length, 1, 16000, false);
        //bSource.clip = AudioClip.Create("SampleClip", newSignal.Length, 1, 16000, false);

        //aSource.clip.SetData(newSignalA, 0);
        //bSource.clip.SetData(newSignalB, 0);

        //aSource.Play();
        //bSource.Play();
        int spectrumSize = 1024;
        spectrumA = new float[spectrumSize];
        spectrumB = new float[spectrumSize];

        aSource.GetSpectrumData(spectrumA, 0, FFTWindow.BlackmanHarris);
        bSource.GetSpectrumData(spectrumB, 0, FFTWindow.BlackmanHarris);
        CrossCorrelate(spectrumA, spectrumB);
    }

    void FFT(List<float> _audioSignalSample)
    {
        float[] newSignal = ZeroPadSIgnal(_audioSignalSample.ToArray());
        audioSignalSample = newSignal.ToList();

        aSource.clip = AudioClip.Create("SampleClip", newSignal.Length, 1, 16000, false);
        bSource.clip = AudioClip.Create("SampleClip", newSignal.Length, 1, 16000, false);

        aSource.clip.SetData(newSignal, 0);
        bSource.clip.SetData(newSignal, 0);

        aSource.Play();
        bSource.Play();

        spectrumA = new float[256];
        aSource.GetSpectrumData(spectrumA, 0, FFTWindow.BlackmanHarris);

    }

    private float[] spectrumA = new float[2048];
    private float[] spectrumB = new float[2048];

    public float crossCorrRange = 10;

    private void DisplayFFT()
    {
        int i = 1;
        while (i < spectrumA.Length - 1)
        {
            Debug.DrawLine(new Vector3(i - 1, spectrumA[i] + 10, 0), new Vector3(i, spectrumA[i + 1] + 10, 0), Color.red);
            Debug.DrawLine(new Vector3(i - 1, Mathf.Log(spectrumA[i - 1]) + 10, 2), new Vector3(i, Mathf.Log(spectrumA[i]) + 10, 2), Color.cyan);
            //Logarithmic representation of frequencies
            Debug.DrawLine(new Vector3(Mathf.Log(i - 1), spectrumA[i - 1] - 10, 1), new Vector3(Mathf.Log(i), spectrumA[i] - 10, 1), Color.green);
            Debug.DrawLine(new Vector3(Mathf.Log(i - 1), Mathf.Log(spectrumA[i - 1]), 3), new Vector3(Mathf.Log(i), Mathf.Log(spectrumA[i]), 3), Color.yellow);
            i++;
        }
        int j = 1;
        int yOffset = 25;

        while (j < spectrumA.Length - 1)
        {
            Debug.DrawLine(new Vector3(j - 1, spectrumB[j] + 10 + yOffset, 0), new Vector3(j, spectrumB[j + 1] + 10 + yOffset, 0), Color.red);
            Debug.DrawLine(new Vector3(j - 1, Mathf.Log(spectrumB[j - 1]) + 10 + yOffset, 2), new Vector3(j, Mathf.Log(spectrumB[j]) + 10 + yOffset, 2), Color.cyan);
            //Logarithmic representation of frequencies
            Debug.DrawLine(new Vector3(Mathf.Log(j - 1), spectrumB[j - 1] - 10 + yOffset, 1), new Vector3(Mathf.Log(j), spectrumB[j] - 10 + yOffset, 1), Color.green);
            Debug.DrawLine(new Vector3(Mathf.Log(j - 1), Mathf.Log(spectrumB[j - 1]) + yOffset, 3), new Vector3(Mathf.Log(j), Mathf.Log(spectrumB[j]) + yOffset, 3), Color.yellow);
            j++;
        }

        float[] crossCorrFloats =  MultiplySIgnals(spectrumA, spectrumB);

        int k = 1;
        yOffset = 50;

        while (k < spectrumA.Length - 1)
        {
            Debug.DrawLine(new Vector3(k - 1, crossCorrFloats[k] + 10 + yOffset, 0), new Vector3(k, crossCorrFloats[k + 1] + 10 + yOffset, 0), Color.red);
            Debug.DrawLine(new Vector3(k - 1, Mathf.Log(crossCorrFloats[k - 1]) + 10 + yOffset, 2), new Vector3(k, Mathf.Log(crossCorrFloats[k]) + 10 + yOffset, 2), Color.cyan);
            //Logarithmic representation of frequencies
            Debug.DrawLine(new Vector3(Mathf.Log(k - 1), crossCorrFloats[k - 1] - 10 + yOffset, 1), new Vector3(Mathf.Log(k), crossCorrFloats[k] - 10 + yOffset, 1), Color.green);
            Debug.DrawLine(new Vector3(Mathf.Log(k - 1), Mathf.Log(crossCorrFloats[k - 1]) + yOffset, 3), new Vector3(Mathf.Log(k), Mathf.Log(crossCorrFloats[k]) + yOffset, 3), Color.yellow);
            k++;
        }
    }

 

    public static float[] ZeroPadSIgnal(float[] signalFloats)
    {
        int j = signalFloats.Length;
        List<float> tempSignalFloats = signalFloats.ToList();

        for (int i = 0; i < j; i++)
        {
            tempSignalFloats.Add(0);
        }
        return tempSignalFloats.ToArray();
    }

    public static float[] MultiplySIgnals(float[] spectrumFloatsA, float[] spectrumFloatsB)
    {
        int j = spectrumFloatsA.Length;
        float[] newSpectrumFloats = new float[j];

        for (int i = 0; i < j; i++)
        {
            float val = spectrumFloatsA[i] * spectrumFloatsB[i];
            newSpectrumFloats[i] = val;
        }
        return newSpectrumFloats;
    }
}
