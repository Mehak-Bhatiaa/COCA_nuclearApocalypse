﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace LandscapeBuilder
{
    public class LBNoise
    {
        // Noise class 

        /// <summary>
        /// Returns the perlin fractal noise value at xPos, yPos
        /// </summary>
        /// <returns>The fractal noise.</returns>
        /// <param name="xPos">X position.</param>
        /// <param name="yPos">Y position.</param>
        /// <param name="octaves">Octaves.</param>
        /// <param name="lacunarity">Lacunarity.</param>
        /// <param name="gain">Gain.</param>
        /// <param name="ridgedNoise">If set to <c>true</c> ridged noise.</param>
        public static float PerlinFractalNoise(float xPos, float yPos, int octaves, float lacunarity, float gain, bool ridgedNoise)
        {
            // Input the position into the fractal noise function to get a value between 0 and 1
            // Fractal noise is generated by getting perlin noise output from world coordinate input
            // then adding scaled versions of itself to it
            float value = 0f;
            float powFloat = 1f;
            float powFloat2 = gain;
            for (int o = 0; o < octaves; o++)
            {
                //float octaveValue = Mathf.PerlinNoise(xPos * IntPow(lacunarity, o), yPos * IntPow(lacunarity, o));
                float octaveValue = Mathf.PerlinNoise(xPos * powFloat, yPos * powFloat);
                powFloat *= lacunarity;
                // Ridged noise gets "absolute value" of the noise
                // It does this by scaling the noise so that instead of being from 0 to 1 it is from -1 to 1
                // Then it takes the absolute value of the resulting function and inverts it
                if (ridgedNoise) { octaveValue = 1f - Mathf.Abs((octaveValue * 2f) - 1f); }
                //octaveValue *= IntPow(gain, o+1);
                octaveValue *= powFloat2;
                powFloat2 *= gain;
                value += octaveValue;
            }
            return value;
        }

        /// <summary>
        /// Returns the perlin fractal noise value at xPos, yPos modified by a type of fake erosion 
        /// </summary>
        /// <returns>The fractal noise.</returns>
        /// <param name="xPos">X position.</param>
        /// <param name="yPos">Y position.</param>
        /// <param name="octaves">Octaves.</param>
        /// <param name="lacunarity">Lacunarity.</param>
        /// <param name="gain">Gain.</param>
        /// <param name="ridgedNoise">If set to <c>true</c> ridged noise.</param>
        /// <param name="fakeErosionCurve">Fake erosion curve.</param>
        /// <param name="fakeErosionType">Fake erosion type.</param>
        public static float PerlinFractalNoise(float xPos, float yPos, int octaves, float lacunarity, float gain, bool ridgedNoise, AnimationCurve fakeErosionCurve, int fakeErosionType)
        {
            // Input the position into the fractal noise function to get a value between 0 and 1
            // Fractal noise is generated by getting perlin noise output from world coordinate input
            // then adding scaled versions of itself to it
            float value = 0f;
            for (int o = 0; o < octaves; o++)
            {
                // WARNING: This function has not been optimised like the others (still uses pow)
                float octaveValue = Mathf.PerlinNoise(xPos * IntPow(lacunarity, o), yPos * IntPow(lacunarity, o));
                // Ridged noise gets "absolute value" of the noise
                // It does this by scaling the noise so that instead of being from 0 to 1 it is from -1 to 1
                // Then it takes the absolute value of the resulting function and inverts it
                if (ridgedNoise) { octaveValue = 1f - Mathf.Abs((octaveValue * 2f) - 1f); }
                // Fake erosion - runs some octaves of the noise through a specific curve modifier
                if (fakeErosionCurve != null)
                {
                    // Add erosion
                    if (o < 2 && fakeErosionType != 2) { octaveValue = fakeErosionCurve.Evaluate(octaveValue); }
                    else if (octaves - o < 2 && fakeErosionType != 1) { octaveValue = fakeErosionCurve.Evaluate(octaveValue); }
                }
                octaveValue *= IntPow(gain, o + 1);
                value += octaveValue;
            }
            return value;
        }

        /// <summary>
        /// Returns the perlin fractal noise value at xPos, yPos modified by a list of curve modifiers affecting each octave
        /// </summary>
        /// <returns>The fractal noise.</returns>
        /// <param name="xPos">X position.</param>
        /// <param name="yPos">Y position.</param>
        /// <param name="octaves">Octaves.</param>
        /// <param name="lacunarity">Lacunarity.</param>
        /// <param name="gain">Gain.</param>
        /// <param name="curveModifiers">Curve modifiers.</param>
        public static float PerlinFractalNoise(float xPos, float yPos, int octaves, float lacunarity, float gain, List<AnimationCurve> curveModifiers)
        {
            // Input the position into the fractal noise function to get a value between 0 and 1
            // Fractal noise is generated by getting perlin noise output from world coordinate input
            // then adding scaled versions of itself to it
            float value = 0f;
            float powFloat = 1f;
            float powFloat2 = gain;
            for (int o = 0; o < octaves; o++)
            {
                //float octaveValue = Mathf.PerlinNoise(xPos * IntPow(lacunarity, o), yPos * IntPow(lacunarity, o));
                float octaveValue = Mathf.PerlinNoise(xPos * powFloat, yPos * powFloat);
                powFloat *= lacunarity;
                if (curveModifiers != null)
                {
                    for (int cm = 0; cm < curveModifiers.Count; cm++)
                    {
                        octaveValue = curveModifiers[cm].Evaluate(octaveValue);
                    }
                }
                //octaveValue *= IntPow(gain, o+1);
                octaveValue *= powFloat2;
                powFloat2 *= gain;
                value += octaveValue;
            }
            return value;
        }

        /// <summary>
        /// Returns the perlin fractal noise value at xPos, yPos
        /// </summary>
        /// <returns>The fractal noise.</returns>
        /// <param name="xPos">X position.</param>
        /// <param name="yPos">Y position.</param>
        /// <param name="octaves">Octaves.</param>
        public static float PerlinFractalNoise(float xPos, float yPos, int octaves)
        {
            // Input the position into the fractal noise function to get a value between 0 and 1
            // Fractal noise is generated by getting perlin noise output from world coordinate input
            // then adding scaled versions of itself to it
            float value = 0f;
            int powInt = 1;
            for (int o = 0; o < octaves; o++)
            {
                value += Mathf.PerlinNoise(xPos * powInt, yPos * powInt) / (powInt * 2);
                powInt *= 2;
            }
            return value;
        }

        // Faster version of Mathf.Pow for integer exponents
        public static float IntPow(float num, int pow)
        {
            if (pow != 0)
            {
                float ans = num;
                for (int i = 1; i < pow; i++)
                {
                    ans *= num;
                }
                return ans;
            }
            else { return 1f; }
        }

        // Faster version of Mathf.Pow for integer exponents and bases
        public static int IntPow(int num, int pow)
        {
            if (pow != 0)
            {
                int ans = num;
                for (int i = 1; i < pow; i++)
                {
                    ans *= num;
                }
                return ans;
            }
            else { return 1; }
        }
    }
}