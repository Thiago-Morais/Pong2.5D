using System;
using UnityEngine;
using static UnityEngine.ParticleSystem;

namespace ParticleSystemOverride
{
    public static class Util
    {
        //!! MinMaxCurve
        public static MinMaxCurve Lerp(MinMaxCurve min, MinMaxCurve max, float t)
        {
            MinMaxCurve result;
            if (min.mode == ParticleSystemCurveMode.TwoCurves || max.mode == ParticleSystemCurveMode.TwoCurves)
            {
                result = LerpTwoCurves(min, max, t);
            }
            else if ((min.mode == ParticleSystemCurveMode.TwoConstants || max.mode == ParticleSystemCurveMode.TwoConstants)
                    && (min.mode == ParticleSystemCurveMode.Curve || max.mode == ParticleSystemCurveMode.Curve))
            {
                result = LerpTwoCurves(min, max, t);
            }
            else if (min.mode == ParticleSystemCurveMode.TwoConstants || max.mode == ParticleSystemCurveMode.TwoConstants)
            {
                result = LerpTwoConstants(min, max, t);
            }
            else if (min.mode == ParticleSystemCurveMode.Curve || max.mode == ParticleSystemCurveMode.Curve)
            {
                result = LerpCurve(min, max, t);
            }
            else
            {
                result = LerpConstant(min, max, t);
            }
            return result;
        }
        public static MinMaxCurve LerpTwoCurves(MinMaxCurve min, MinMaxCurve max, float t)
        {
            min = TurnIntoTwoCurves(min);
            max = TurnIntoTwoCurves(max);
            AnimationCurve minCurve = LerpStandardCurve(min.curveMin, max.curveMin, t);
            AnimationCurve maxCurve = LerpStandardCurve(min.curveMax, max.curveMax, t);
            float multiplier = Mathf.Lerp(min.curveMultiplier, max.curveMultiplier, t);
            return new MinMaxCurve(multiplier, minCurve, maxCurve);
        }
        static MinMaxCurve TurnIntoTwoCurves(MinMaxCurve curve)
        {
            if (curve.mode == ParticleSystemCurveMode.Constant)
            {
                curve.curveMultiplier = 1;
                curve.curveMin = curve.curveMax = AnimationCurve.Constant(0, 1, curve.constant);
            }
            else if (curve.mode == ParticleSystemCurveMode.TwoConstants)
            {
                curve.curveMultiplier = 1;
                curve.curveMin = curve.curveMax = AnimationCurve.Linear(0, curve.constantMin, 1, curve.constantMax);
            }
            else if (curve.mode == ParticleSystemCurveMode.Curve)
            {
                curve.curveMultiplier = curve.curveMultiplier;
                curve.curveMin = curve.curveMax = curve.curve;
            }
            curve.mode = ParticleSystemCurveMode.TwoCurves;
            return curve;
        }
        public static MinMaxCurve LerpCurve(MinMaxCurve min, MinMaxCurve max, float t)
        {
            min = TurnIntoCurve(min);
            max = TurnIntoCurve(max);
            AnimationCurve curve = LerpStandardCurve(min.curve, max.curve, t);
            float multiplier = Mathf.Lerp(min.curveMultiplier, max.curveMultiplier, t);
            return new MinMaxCurve(multiplier, curve);
        }
        static MinMaxCurve TurnIntoCurve(MinMaxCurve curve)
        {
            if (curve.mode == ParticleSystemCurveMode.TwoConstants)
            {
                curve.curveMultiplier = 1;
                curve.curve = AnimationCurve.Linear(0, curve.constantMin, 1, curve.constantMax);
            }
            if (curve.mode == ParticleSystemCurveMode.Constant)
            {
                curve.curveMultiplier = 1;
                curve.curve = AnimationCurve.Constant(0, 1, curve.constant);
            }
            curve.mode = ParticleSystemCurveMode.Curve;
            return curve;
        }
        static AnimationCurve LerpStandardCurve(AnimationCurve min, AnimationCurve max, float t)
        {
            if (!EqualLength(min, max))
                return max;

            var result = new AnimationCurve();
            for (int i = 0; i < min.length; i++)
            {
                result.AddKey(new Keyframe(
                    Mathf.Lerp(min[i].time, max[i].time, t),
                    Mathf.Lerp(min[i].value, max[i].value, t),
                    Mathf.Lerp(min[i].inTangent, max[i].inTangent, t),
                    Mathf.Lerp(min[i].outTangent, max[i].outTangent, t)));
            }
            return result;
        }
        static bool EqualLength(AnimationCurve a, AnimationCurve b) => a.length == b.length;

        public static float LerpConstant(MinMaxCurve min, MinMaxCurve max, float t)
        {
            return LerpConstantMax(min, max, t);
        }
        public static MinMaxCurve LerpTwoConstants(MinMaxCurve minRange, MinMaxCurve maxRange, float t)
        {
            if (minRange.mode == ParticleSystemCurveMode.Constant)
                minRange.constantMin = minRange.constantMax;
            if (maxRange.mode == ParticleSystemCurveMode.Constant)
                maxRange.constantMin = maxRange.constantMax;
            return new MinMaxCurve(
                LerpConstantMin(minRange, maxRange, t),
                LerpConstantMax(minRange, maxRange, t));
        }
        public static float LerpConstantMin(MinMaxCurve min, MinMaxCurve max, float t)
        {
            return Mathf.Lerp(min.Evaluate(0, 0), max.Evaluate(0, 0), t);
        }
        public static float LerpConstantMax(MinMaxCurve min, MinMaxCurve max, float t)
        {
            return Mathf.Lerp(min.Evaluate(0, 1), max.Evaluate(0, 1), t);
        }
        //!! MinMaxGradient
        public static MinMaxGradient Lerp(MinMaxGradient min, MinMaxGradient max, float t)
        {
            MinMaxGradient result;
            if (min.mode == ParticleSystemGradientMode.TwoGradients || max.mode == ParticleSystemGradientMode.TwoGradients
                || ((min.mode == ParticleSystemGradientMode.TwoColors || max.mode == ParticleSystemGradientMode.TwoColors)
                    && (min.mode == ParticleSystemGradientMode.Gradient || max.mode == ParticleSystemGradientMode.Gradient)))
            {
                result = LerpTwoGradients(min, max, t);
            }
            else if (min.mode == ParticleSystemGradientMode.TwoColors || max.mode == ParticleSystemGradientMode.TwoColors)
            {
                result = LerpTwoColors(min, max, t);
            }
            else if (min.mode == ParticleSystemGradientMode.Gradient || max.mode == ParticleSystemGradientMode.Gradient)
            {
                result = LerpGradient(min, max, t);
            }
            else
            {
                result = LerpColor(min, max, t);
            }
            return result;
        }
        public static MinMaxGradient LerpTwoGradients(MinMaxGradient min, MinMaxGradient max, float t)
        {
            min = TurnIntoTwoGradients(min);
            max = TurnIntoTwoGradients(max);
            Gradient gradientMin = LerpGradient(min.gradientMin, max.gradientMin, t);
            Gradient gradientMax = LerpGradient(max.gradientMax, max.gradientMax, t);
            return new MinMaxGradient(gradientMin, gradientMax);
        }
        static MinMaxGradient TurnIntoTwoGradients(MinMaxGradient gradient)
        {
            if (gradient.mode == ParticleSystemGradientMode.Gradient)
                gradient.gradientMin = gradient.gradientMax = gradient.gradient;
            else if (gradient.mode == ParticleSystemGradientMode.Color)
                gradient.gradientMin = gradient.gradientMax = ColorToGradient(gradient.color, gradient);
            else if (gradient.mode == ParticleSystemGradientMode.TwoColors)
            {
                gradient.gradientMin = ColorToGradient(gradient.colorMin, gradient);
                gradient.gradientMax = ColorToGradient(gradient.colorMax, gradient);
            }
            gradient.mode = ParticleSystemGradientMode.TwoGradients;
            return gradient;
        }
        static Gradient ColorToGradient(Color color, MinMaxGradient gradient)
        {
            var stdGradient = new Gradient();
            stdGradient.SetKeys(new GradientColorKey[] { new(color, 0) }, new GradientAlphaKey[] { new(gradient.color.a, 0) });
            return stdGradient;
        }
        public static Gradient LerpGradient(MinMaxGradient min, MinMaxGradient max, float t)
        {
            return LerpStandardGradient(min.gradient, max.gradient, t);
        }
        static Gradient LerpStandardGradient(Gradient min, Gradient max, float t)
        {
            if (!EqualLength(min, max))
                return max;

            GradientColorKey[] colorKeys = new GradientColorKey[max.colorKeys.Length];
            for (int i = 0; i < max.colorKeys.Length; i++)
            {
                colorKeys[i].color = Color.Lerp(min.colorKeys[i].color, max.colorKeys[i].color, t);
                colorKeys[i].time = Mathf.Lerp(min.colorKeys[i].time, max.colorKeys[i].time, t);
            }
            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[max.alphaKeys.Length];
            for (int i = 0; i < max.alphaKeys.Length; i++)
            {
                alphaKeys[i].alpha = Mathf.Lerp(min.alphaKeys[i].alpha, max.alphaKeys[i].alpha, t);
                alphaKeys[i].time = Mathf.Lerp(min.alphaKeys[i].time, max.alphaKeys[i].time, t);
            }
            var result = new Gradient();
            result.SetKeys(colorKeys, alphaKeys);
            return result;
        }
        public static Color LerpColor(MinMaxGradient min, MinMaxGradient max, float t)
        {
            return GetColorMax(min, max, t);
        }
        public static MinMaxGradient LerpTwoColors(MinMaxGradient minRange, MinMaxGradient maxRange, float t)
        {
            if (minRange.mode == ParticleSystemGradientMode.Color)
                minRange.colorMin = minRange.colorMax;
            if (maxRange.mode == ParticleSystemGradientMode.Color)
                maxRange.colorMin = maxRange.colorMax;
            return new MinMaxGradient(
                GetColorMin(minRange, maxRange, t),
                GetColorMax(minRange, maxRange, t));
        }
        public static Color GetColorMin(MinMaxGradient min, MinMaxGradient max, float t)
        {
            return Color.Lerp(min.Evaluate(0, 0), max.Evaluate(0, 0), t);
        }
        public static Color GetColorMax(MinMaxGradient min, MinMaxGradient max, float t)
        {
            return Color.Lerp(min.Evaluate(0, 1), max.Evaluate(0, 1), t);
        }
        static bool EqualLength(Gradient min, Gradient max) => min.colorKeys.Length == max.colorKeys.Length && min.alphaKeys.Length == max.alphaKeys.Length;
    }
}