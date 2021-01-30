﻿using System;
public static class UnitaleMath
{
    private static Random rng = new Random(); // unityengine's random can't be used outside of its main thread, so just to be safe here

    // stupid but fairly optimal way of dealing with negative X values because % doesn't by itself
    public static int mod(int x, int mod)
    {
        return ((x % mod) + mod) % mod;
    }

    public static float mod(float x, float mod)
    {
        return ((x % mod) + mod) % mod;
    }

    public static int randomRange(int minInclusive, int maxExclusive)
    {
        return rng.Next(minInclusive, maxExclusive);
    }
}