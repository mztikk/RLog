using System;
using RFReborn.Random;

namespace RLog
{
    internal static class Internals
    {
        internal static Random s_random = new CryptoRandom();
    }
}
