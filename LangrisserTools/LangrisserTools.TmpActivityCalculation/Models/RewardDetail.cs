using System;

namespace LangrisserTools.TmpActivityCalculation.Models
{
 /// <summary>
 ///�һ���������ϸ
 /// </summary>
 public class RewardDetail
 {
 /// <summary>
 ///�ý�����ϸ�Ƿ������������������ͳ����
 /// </summary>
 public bool IncludedInCal { get; set; } = true;

 public int RewardA { get; set; }
 public int RewardB { get; set; }
 public int RewardC { get; set; }
 public int RewardD { get; set; }

 /// <summary>
 ///�ý������ظ���ȡ����
 /// </summary>
 public int RequiredQty { get; set; } =1;
 }
}
