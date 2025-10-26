using System;
using System.Collections.Generic;
using System.Linq;
using LangrisserTools.TmpActivityCalculation.Services;

namespace LangrisserTools.TmpActivityCalculation.Models
{
 public class ActivityReward
 {
 // display names
 public string NameA { get; set; } = "A";
 public string NameB { get; set; } = "B";
 public string NameC { get; set; } = "C";
 public string NameD { get; set; } = "D";

 // current quantities
 public int CurrentQtyA { get; set; }
 public int CurrentQtyB { get; set; }
 public int CurrentQtyC { get; set; }
 public int CurrentQtyD { get; set; }

 // total requirements (computed)
 public int TotalReqA { get; set; }
 public int TotalReqB { get; set; }
 public int TotalReqC { get; set; }
 public int TotalReqD { get; set; }

 // requirement gap
 public int ReqGapA => Math.Max(0, TotalReqA - CurrentQtyA);
 public int ReqGapB => Math.Max(0, TotalReqB - CurrentQtyB);
 public int ReqGapC => Math.Max(0, TotalReqC - CurrentQtyC);
 public int ReqGapD => Math.Max(0, TotalReqD - CurrentQtyD);

 // salad/normal rounds per category
 public int RoundsSaladA { get; set; }
 public int RoundsSaladB { get; set; }
 public int RoundsSaladC { get; set; }
 public int RoundsSaladD { get; set; }

 public int RoundsNormalA { get; set; }
 public int RoundsNormalB { get; set; }
 public int RoundsNormalC { get; set; }
 public int RoundsNormalD { get; set; }

 public int RoundsA => RoundsSaladA + RoundsNormalA;
 public int RoundsB => RoundsSaladB + RoundsNormalB;
 public int RoundsC => RoundsSaladC + RoundsNormalC;
 public int RoundsD => RoundsSaladD + RoundsNormalD;

 // rewards
 public int RewardBase { get; set; }
 public int RewardPlus { get; set; }
 public int RewardNormal { get; set; }
 public int RewardSalad { get; set; }
 public int RoundsLimitSalad { get; set; } =40;

 public int TotalRoundsSalad => RoundsSaladA + RoundsSaladB + RoundsSaladC + RoundsSaladD;
 public int TotalRoundsNormal => RoundsNormalA + RoundsNormalB + RoundsNormalC + RoundsNormalD;
 public int TotalRounds => RoundsA + RoundsB + RoundsC + RoundsD;

 public List<RewardDetail> RewardDetails { get; set; } = new List<RewardDetail>();

 public void CalculateRequirementsTotal()
 {
 var included = RewardDetails.Where(r => r.IncludedInCal);
 TotalReqA = included.Sum(r => r.RewardA * r.RequiredQty);
 TotalReqB = included.Sum(r => r.RewardB * r.RequiredQty);
 TotalReqC = included.Sum(r => r.RewardC * r.RequiredQty);
 TotalReqD = included.Sum(r => r.RewardD * r.RequiredQty);
 }

 public void CalculateRounds()
 {
 var res = RoundsCalculator.CalculateMinimumRounds(ReqGapA, ReqGapB, ReqGapC, ReqGapD, RewardSalad, RewardNormal, RoundsLimitSalad);
 if (!res.Possible) return;
 RoundsSaladA = res.SaladUsesPerCategory[0];
 RoundsSaladB = res.SaladUsesPerCategory[1];
 RoundsSaladC = res.SaladUsesPerCategory[2];
 RoundsSaladD = res.SaladUsesPerCategory[3];

 RoundsNormalA = res.NormalUsesPerCategory[0];
 RoundsNormalB = res.NormalUsesPerCategory[1];
 RoundsNormalC = res.NormalUsesPerCategory[2];
 RoundsNormalD = res.NormalUsesPerCategory[3];
 }
 }
}
