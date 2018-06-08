using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AForge.Math;
using System;
using UnityEngine.Experimental.UIElements;

public class QFT : MonoBehaviour {
    //Convert the output array to euler angles and the magnitude of these can be used  to determine correlation
    public Quaternion[] CrossCorellateQFTs(Quaternion[] G, Quaternion[] H)
    {
        double[] realW = new double[G.Length];
        double[] imagI = new double[G.Length];
        double[] imagJ = new double[G.Length];
        double[] imagK = new double[G.Length];

        Quaternion[] temp = new Quaternion[G.Length];
        for (int ii = 0; ii < G.Length; ii++)
        {
            double a = G[ii].w;
            double b = G[ii].x;
            double c = H[ii].w;
            double d = H[ii].x;
            double e = H[(G.Length - 1) - ii].y;
            double f = H[(G.Length - 1) - ii].z;
            
            double u = G[ii].y;
            double v = G[ii].z;
            double w = H[(G.Length - 1) - ii].w;
            double x = H[(G.Length - 1) - ii].x;
            double y = H[ii].y;
            double z = H[ii].z;
            
            double i, j, k, ww;
            ww = (a * c) + (b * d) + (u * y) + (v * z);
            j = -(a * d) + (b * c) - (u * z) + (v * y);
            k = -(a * e) + (b * f) + (u * w) - (v * x);
            i = -(a * f) - (b * e) + (u * x) + (v * w);
            //temp[ii] = new Quaternion(i, j, k, ww);
            realW[ii] = ww;
            imagI[ii] = i;
            imagJ[ii] = j;
            imagK[ii] = k;
        }
        temp = QuaternionFourierTransform(realW.Length, 2, realW, imagI, imagJ, imagK, true);
        return temp;
    }

    public Quaternion[] QuaternionFourierTransform(int dim1, int dim2, double[] real, double[] i, double[] j, double[] k, bool inverse)
    {
        //Create Arrays of Complex data
        Complex[,] f1 = new Complex[dim1, dim2];
        Complex[,] f2 = new Complex[dim1, dim2];

        //For loop for filling in the data to two complex arrays one consisting of r and i.
        //The other j and k.
        
        for (int x = 0; x < dim1-4; x++)
        {
            for (int y = 0; y < dim2; y++)
            {
                f1[x, y] = new Complex(real[y * dim2 + x], i[y * dim2 + x]);
                f2[x, y] = new Complex(j[y * dim2 + x], k[y * dim2 + x]);
            }
        }

        //If statement to check whether to do forward or backward FFT.
        if (inverse)
        {
            FourierTransform.FFT2(f1, FourierTransform.Direction.Backward);
            FourierTransform.FFT2(f2, FourierTransform.Direction.Backward);
        }
        else
        {
            FourierTransform.FFT2(f1, FourierTransform.Direction.Forward);
            FourierTransform.FFT2(f2, FourierTransform.Direction.Forward);
        }

        //Concat the two FFTS to one single quaternion array.
        Quaternion[] Concat = new Quaternion[dim1*dim2];

        for (int x = 0; x < dim1; x++)
        {
            for (int y = 0; y < dim2; y++)
            {
                Concat[x + y * dim2] = new Quaternion((float)f1[x, y].Im, (float)f2[x, y].Re, (float)f2[x, y].Im, (float)f1[x, y].Re);
            }
        }
        return Concat;


        //Concatenate

        //int align = System.Runtime.InteropServices.Marshal.SizeOf(typeof(Complex));// sizeof(Complex);
        // Do something on frequency domain

        //backward_1.fftNormalized(f1);
        //backward_2.fftNormalized(f2);
        //
        //for (int j = 0; j < dim1; j++)
        //{
        //    for (int i = 0; i < dim2; i++)
        //    {
        //        double p1 = real(f1(i, j));
        //        double p2 = imag(f1(i, j));
        //        double p3 = real(f2(i, j));
        //        double p4 = imag(f2(i, j));
        //
        //        // Do something after inverse transform
        //    }
        //}
    }

    public void BitReverseIndices(float[] real, float[] imag)
    {
        var N = 512;
        var hieghestBit = N >> 1;
        var nextBit = 0;
        var nReversed = 0;

        for (int n = 1; n < N; n++)
        {
            nextBit = hieghestBit;
            while ((nextBit + nReversed) > N - 1)
            {
                nextBit >>= 1;
            }

            nReversed &= nextBit - 1;
            nReversed |= nextBit;

            if (nReversed > n)
            {
                var tmp = real[n];
                real[n] = real[nReversed];
                real[nReversed] = tmp;
                tmp = imag[n];
                imag[n] = imag[nReversed];
                imag[nReversed] = tmp;
            }
        }
    }

}
