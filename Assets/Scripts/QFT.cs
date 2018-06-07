using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AForge.Math;
using System;

public class QFT : MonoBehaviour {
    //Convert the output array to euler angles and the magnitude of these can be used  to determine correlation
    public Quaternion[] CrossCorellateQFTs(Quaternion[] G, Quaternion[] H)
    {
        Quaternion[] temp = new Quaternion[G.Length];
        for (int ii = 0; ii < G.Length; ii++)
        {
            float a = G[ii].w;
            float b = G[ii].x;
            float c = H[ii].w;
            float d = H[ii].x;
            float e = H[G.Length - 1 - ii].y;
            float f = H[G.Length - 1 - ii].z;

            float u = G[ii].y;
            float v = G[ii].z;
            float w = H[G.Length - 1 - ii].w;
            float x = H[G.Length - 1 - ii].x;
            float y = H[ii].y;
            float z = H[ii].z;

            float i, j, k, ww;
            ww = (a * c) + (b * d) + (u * y) + (v * z);
            j = -(a * d) + (b * c) - (u * z) + (v * y);
            k = -(a * e) + (b * f) + (u * w) - (v * x);
            i = -(a * f) - (b * e) + (u * x) + (v * w);
            temp[ii] = new Quaternion(i, j, k, w);
        }
        return temp;
    }

    public Quaternion[] QuaternionFourierTransform(int dim1, int dim2, float[] real, float[] i, float[] j, float[] k)
    {
        // dim1 is the 1-st dimension of data, dim2 is the 2-nd dimension of data
        //fftw::maxthreads = get_max_threads();
        int align = System.Runtime.InteropServices.Marshal.SizeOf(typeof(Complex));// sizeof(Complex);

        //dim1 length of array dim2 = 1
        Complex[,] f1 = new Complex[dim1, dim2];
        Complex[,] f2 = new Complex[dim1, dim2];

        //fft2d forward_1(-1, f1);
        //fft2d backward_1(1, f1);
        //fft2d forward_2(-1, f2);
        //fft2d backward_2(1, f2);

        for (int x = 0; x < dim1; x++)
        {
            for (int y = 0; y < dim2; y++)
            {
                f1[x, y] = new Complex(real[y * dim2 + x], i[y * dim2 + x]);
                f2[x, y] = new Complex(j[y * dim2 + x], k[y * dim2 + x]);
            }
        }
        FourierTransform.FFT2(f1, FourierTransform.Direction.Forward);
        FourierTransform.FFT2(f2, FourierTransform.Direction.Forward);

        Quaternion[] Concat = new Quaternion[dim1];

        for (int x = 0; x < dim1; x++)
        {
            for (int y = 0; y < dim2; y++)
            {
                Concat[x + y * dim2] = new Quaternion((float)f1[x, y].Im, (float)f2[x, y].Re, (float)f2[x, y].Im, (float)f1[x, y].Re);
            }
        }
        return Concat;

        
        //Concatenate


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
}
