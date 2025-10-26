using System;

namespace LangrisserTools.TmpActivityCalculation.Models
{
 /// <summary>
 ///兑换奖励的明细
 /// </summary>
 public class RewardDetail
 {
 /// <summary>
 ///该奖励明细是否包含到奖励需求数量统计中
 /// </summary>
 public bool IncludedInCal { get; set; } = true;

 public int RewardA { get; set; }
 public int RewardB { get; set; }
 public int RewardC { get; set; }
 public int RewardD { get; set; }

 /// <summary>
 ///该奖励可重复领取次数
 /// </summary>
 public int RequiredQty { get; set; } =1;
 }
}
