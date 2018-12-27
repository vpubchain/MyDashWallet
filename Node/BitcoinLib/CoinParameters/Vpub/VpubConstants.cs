using BitcoinLib.CoinParameters.Base;

namespace BitcoinLib.CoinParameters.Vpub
{
    public static class VpubConstants
    {
        public sealed class Constants : CoinConstants<Constants>
        {
            public readonly ushort CoinReleaseHalfsEveryXInYears = 7;
            public readonly ushort DifficultyIncreasesEveryXInBlocks = 34560;
            public readonly uint OneVpubInDuffs = 100000000;
            public readonly decimal OneDuffInVpub = 0.00000001M;
            public readonly decimal OneMicrovpubInVpub = 0.000001M;
            public readonly decimal OneMillivpubInVpub = 0.001M;
            public readonly string Symbol = "VP";
        }
    }
}