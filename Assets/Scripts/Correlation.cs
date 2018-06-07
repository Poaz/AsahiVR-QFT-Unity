using UnityEngine;
using System.Collections;
using System.Linq;
using AForge.Math;

public class Correlation : MonoBehaviour {

    public static Complex[] CrossCorrelation(Complex[] ffta, Complex[] fftb)
    {
        var conj = ffta.Select(i => new Complex(i.Re, -i.Im)).ToArray();

        for (int a = 0; a < conj.Length; a++)
            conj[a] = Complex.Multiply(conj[a], fftb[a]);

        FourierTransform.FFT(conj, FourierTransform.Direction.Backward);

        return conj;
    }

    public static double CorrelationCoefficient(Complex[] ffta, Complex[] fftb)
    {
        var correlation = CrossCorrelation(ffta, fftb);
        var a = CrossCorrelation(ffta, ffta);
        var b = CrossCorrelation(fftb, fftb);

        var numerator = correlation.Select(i => i.SquaredMagnitude).Max();
        var denominatora = a.Select(i => i.Magnitude).Max();
        var denominatorb = b.Select(i => i.Magnitude).Max();

        return numerator / (denominatora * denominatorb);
    }
}
