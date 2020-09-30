using System;
using RFReborn.RandomR;

namespace RLog
{
    internal static class Internals
    {
        internal static Random s_random = new CryptoRandom();
    }
}
