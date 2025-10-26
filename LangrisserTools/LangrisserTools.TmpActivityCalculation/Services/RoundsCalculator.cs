using System;

namespace LangrisserTools.TmpActivityCalculation.Services
{
    /// <summary>
    ///通过枚举色拉次数在多类之间的分配，求解最少总行动次数（保证最优性，适用于 maxSaladUses 较小的情况）
    /// </summary>
    public static class RoundsCalculator
    {
        public sealed class RoundsResult
        {
            public bool Possible { get; init; }
            public int TotalRounds { get; init; }
            public int TotalSaladUses { get; init; }
            public int TotalNormalUses { get; init; }

            // per-category counts in order A,B,C,D
            public int[] SaladUsesPerCategory { get; init; } = new int[4];
            public int[] NormalUsesPerCategory { get; init; } = new int[4];

            // produced amounts for verification
            public long[] ProducedBySaladPerCategory { get; init; } = new long[4];
            public long[] ProducedByNormalPerCategory { get; init; } = new long[4];
            public long[] TotalProducedPerCategory { get; init; } = new long[4];
        }

        /// <summary>
        ///计算最少行动次数（暴力枚举 sA,sB,sC,sD）
        /// </summary>
        public static RoundsResult CalculateMinimumRounds(
        int needA, int needB, int needC, int needD,
        int saladAmount, int normalAmount,
        int maxSaladUses = 40)
        {
            if (needA < 0 || needB < 0 || needC < 0 || needD < 0)
                throw new ArgumentException("需要的奖励数量不得为负数。");
            if (maxSaladUses < 0)
                throw new ArgumentException("maxSaladUses不能为负数。");
            if (saladAmount < 0 || normalAmount < 0)
                throw new ArgumentException("奖励数量不能为负数。");

            // Edge cases
            if (saladAmount == 0 && normalAmount == 0)
            {
                if (needA == 0 && needB == 0 && needC == 0 && needD == 0)
                    return new RoundsResult { Possible = true, TotalRounds = 0, TotalSaladUses = 0, TotalNormalUses = 0 };
                return new RoundsResult { Possible = false, TotalRounds = -1 };
            }

            bool found = false;
            int bestTotal = int.MaxValue;
            int bestSA = 0, bestSB = 0, bestSC = 0, bestSD = 0;
            int bestNA = 0, bestNB = 0, bestNC = 0, bestND = 0;

            for (int sA = 0; sA <= maxSaladUses; sA++)
            {
                for (int sB = 0; sB <= maxSaladUses - sA; sB++)
                {
                    for (int sC = 0; sC <= maxSaladUses - sA - sB; sC++)
                    {
                        int maxSD = maxSaladUses - sA - sB - sC;
                        for (int sD = 0; sD <= maxSD; sD++)
                        {
                            long remA = Math.Max(0, needA - (long)sA * saladAmount);
                            long remB = Math.Max(0, needB - (long)sB * saladAmount);
                            long remC = Math.Max(0, needC - (long)sC * saladAmount);
                            long remD = Math.Max(0, needD - (long)sD * saladAmount);

                            long reqA = remA == 0 ? 0 : (normalAmount > 0 ? CeilDiv(remA, normalAmount) : long.MaxValue);
                            long reqB = remB == 0 ? 0 : (normalAmount > 0 ? CeilDiv(remB, normalAmount) : long.MaxValue);
                            long reqC = remC == 0 ? 0 : (normalAmount > 0 ? CeilDiv(remC, normalAmount) : long.MaxValue);
                            long reqD = remD == 0 ? 0 : (normalAmount > 0 ? CeilDiv(remD, normalAmount) : long.MaxValue);

                            if (reqA == long.MaxValue || reqB == long.MaxValue || reqC == long.MaxValue || reqD == long.MaxValue)
                                continue; // infeasible

                            long n = reqA + reqB + reqC + reqD;
                            long total = sA + sB + sC + sD + n;

                            int saladCount = sA + sB + sC + sD;

                            // Prefer smaller total actions; on tie prefer fewer salad uses
                            if (total < bestTotal || (total == bestTotal && saladCount < (bestSA + bestSB + bestSC + bestSD)))
                            {
                                found = true;
                                bestTotal = (int)total;
                                bestSA = sA; bestSB = sB; bestSC = sC; bestSD = sD;
                                bestNA = (int)reqA; bestNB = (int)reqB; bestNC = (int)reqC; bestND = (int)reqD;
                            }
                        }
                    }
                }
            }

            if (!found)
                return new RoundsResult { Possible = false, TotalRounds = -1 };

            int totalSal = bestSA + bestSB + bestSC + bestSD;
            int totalNor = bestNA + bestNB + bestNC + bestND;

            var result = new RoundsResult
            {
                Possible = true,
                TotalRounds = totalSal + totalNor,
                TotalSaladUses = totalSal,
                TotalNormalUses = totalNor,
                SaladUsesPerCategory = new int[] { bestSA, bestSB, bestSC, bestSD },
                NormalUsesPerCategory = new int[] { bestNA, bestNB, bestNC, bestND }
            };

            for (int i = 0; i < 4; i++)
            {
                result.ProducedBySaladPerCategory[i] = (long)result.SaladUsesPerCategory[i] * saladAmount;
                result.ProducedByNormalPerCategory[i] = (long)result.NormalUsesPerCategory[i] * normalAmount;
                result.TotalProducedPerCategory[i] = result.ProducedBySaladPerCategory[i] + result.ProducedByNormalPerCategory[i];
            }

            return result;
        }

        private static long CeilDiv(long numerator, long denominator)
        {
            if (denominator <= 0) throw new ArgumentException("分母必须为正数。", nameof(denominator));
            return (numerator + denominator - 1) / denominator;
        }
    }
}
