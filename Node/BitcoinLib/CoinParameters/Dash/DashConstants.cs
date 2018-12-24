using BitcoinLib.CoinParameters.Base;

namespace BitcoinLib.CoinParameters.Vpub
{
    public static class DashConstants
    {
        public sealed class Constants : CoinConstants<Constants>
        {
            public readonly ushort CoinReleaseHalfsEveryXInYears = 7;
            public readonly ushort DifficultyIncreasesEveryXInBlocks = 34560;
            public readonly uint OneDashInDuffs = 100000000;
            public readonly decimal OneDuffInDash = 0.00000001M;
            public readonly decimal OneMicrovpubInDash = 0.000001M;
            public readonly decimal OneMillivpubInDash = 0.001M;
            public readonly string Symbol = "VP";
        }
    }
}