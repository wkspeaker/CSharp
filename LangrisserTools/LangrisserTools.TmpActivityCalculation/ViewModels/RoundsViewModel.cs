using LangrisserTools.TmpActivityCalculation.Models;
using LangrisserTools.TmpActivityCalculation.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace LangrisserTools.TmpActivityCalculation.ViewModels
{
    public class RoundsViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private int _saladAmount;
        private int _normalAmount;
        private int _maxSalad;
        private string _resultText = string.Empty;
        private readonly ActivityReward _activityReward;
        private readonly ActivityRewardDataService _dataService;

        // produced amounts (from last calculation)
        private long _producedBySaladA, _producedBySaladB, _producedBySaladC, _producedBySaladD;
        private long _producedByNormalA, _producedByNormalB, _producedByNormalC, _producedByNormalD;
        private long _totalProducedA, _totalProducedB, _totalProducedC, _totalProducedD;

        public RoundsViewModel()
        {
            CalculateCommand = new RelayCommand(_ => Calculate(), _ => CanCalculate);
            _dataService = new ActivityRewardDataService();
            _activityReward = _dataService.LoadData();
            // defaults if null
            if (_activityReward == null) _activityReward = new ActivityReward();

            // seed ViewModel properties from model
            SaladAmount = _activityReward.RewardSalad != 0 ? _activityReward.RewardSalad : 75;
            NormalAmount = _activityReward.RewardNormal != 0 ? _activityReward.RewardNormal : 1;
            MaxSalad = _activityReward.RoundsLimitSalad != 0 ? _activityReward.RoundsLimitSalad : 40;

            // ensure model totals are initialized
            TotalReqA = _activityReward.TotalReqA;
            TotalReqB = _activityReward.TotalReqB;
            TotalReqC = _activityReward.TotalReqC;
            TotalReqD = _activityReward.TotalReqD;

            CurrentQtyA = _activityReward.CurrentQtyA;
            CurrentQtyB = _activityReward.CurrentQtyB;
            CurrentQtyC = _activityReward.CurrentQtyC;
            CurrentQtyD = _activityReward.CurrentQtyD;

            // names
            NameA = _activityReward.NameA;
            NameB = _activityReward.NameB;
            NameC = _activityReward.NameC;
            NameD = _activityReward.NameD;

            // initialize grid
            ResultGridItems = new ObservableCollection<ResultRow>();
            RefreshResultGrid();
        }

        // Names (editable)
        public string NameA { get => _activityReward.NameA; set { _activityReward.NameA = value; OnPropertyChanged(); RefreshResultGrid(); } }
        public string NameB { get => _activityReward.NameB; set { _activityReward.NameB = value; OnPropertyChanged(); RefreshResultGrid(); } }
        public string NameC { get => _activityReward.NameC; set { _activityReward.NameC = value; OnPropertyChanged(); RefreshResultGrid(); } }
        public string NameD { get => _activityReward.NameD; set { _activityReward.NameD = value; OnPropertyChanged(); RefreshResultGrid(); } }

        // Totals (editable)
        public int TotalReqA { get => _activityReward.TotalReqA; set { _activityReward.TotalReqA = value; OnPropertyChanged(); OnPropertyChanged(nameof(NeedA)); OnPropertyChanged(nameof(CanCalculate)); RefreshResultGrid(); } }
        public int TotalReqB { get => _activityReward.TotalReqB; set { _activityReward.TotalReqB = value; OnPropertyChanged(); OnPropertyChanged(nameof(NeedB)); OnPropertyChanged(nameof(CanCalculate)); RefreshResultGrid(); } }
        public int TotalReqC { get => _activityReward.TotalReqC; set { _activityReward.TotalReqC = value; OnPropertyChanged(); OnPropertyChanged(nameof(NeedC)); OnPropertyChanged(nameof(CanCalculate)); RefreshResultGrid(); } }
        public int TotalReqD { get => _activityReward.TotalReqD; set { _activityReward.TotalReqD = value; OnPropertyChanged(); OnPropertyChanged(nameof(NeedD)); OnPropertyChanged(nameof(CanCalculate)); RefreshResultGrid(); } }

        // Current quantities (editable)
        public int CurrentQtyA { get => _activityReward.CurrentQtyA; set { _activityReward.CurrentQtyA = value; OnPropertyChanged(); OnPropertyChanged(nameof(NeedA)); OnPropertyChanged(nameof(CanCalculate)); RefreshResultGrid(); } }
        public int CurrentQtyB { get => _activityReward.CurrentQtyB; set { _activityReward.CurrentQtyB = value; OnPropertyChanged(); OnPropertyChanged(nameof(NeedB)); OnPropertyChanged(nameof(CanCalculate)); RefreshResultGrid(); } }
        public int CurrentQtyC { get => _activityReward.CurrentQtyC; set { _activityReward.CurrentQtyC = value; OnPropertyChanged(); OnPropertyChanged(nameof(NeedC)); OnPropertyChanged(nameof(CanCalculate)); RefreshResultGrid(); } }
        public int CurrentQtyD { get => _activityReward.CurrentQtyD; set { _activityReward.CurrentQtyD = value; OnPropertyChanged(); OnPropertyChanged(nameof(NeedD)); OnPropertyChanged(nameof(CanCalculate)); RefreshResultGrid(); } }

        // Gaps (read-only)
        public int NeedA => _activityReward.ReqGapA;
        public int NeedB => _activityReward.ReqGapB;
        public int NeedC => _activityReward.ReqGapC;
        public int NeedD => _activityReward.ReqGapD;

        // salad/normal rounds per category (read-only wrappers to model)
        public int RoundsSaladA => _activityReward.RoundsSaladA;
        public int RoundsSaladB => _activityReward.RoundsSaladB;
        public int RoundsSaladC => _activityReward.RoundsSaladC;
        public int RoundsSaladD => _activityReward.RoundsSaladD;

        public int RoundsNormalA => _activityReward.RoundsNormalA;
        public int RoundsNormalB => _activityReward.RoundsNormalB;
        public int RoundsNormalC => _activityReward.RoundsNormalC;
        public int RoundsNormalD => _activityReward.RoundsNormalD;

        public int RoundsTotalA => _activityReward.RoundsA;
        public int RoundsTotalB => _activityReward.RoundsB;
        public int RoundsTotalC => _activityReward.RoundsC;
        public int RoundsTotalD => _activityReward.RoundsD;

        // produced values from last calculation
        public long ProducedBySaladA { get => _producedBySaladA; private set { _producedBySaladA = value; OnPropertyChanged(); } }
        public long ProducedBySaladB { get => _producedBySaladB; private set { _producedBySaladB = value; OnPropertyChanged(); } }
        public long ProducedBySaladC { get => _producedBySaladC; private set { _producedBySaladC = value; OnPropertyChanged(); } }
        public long ProducedBySaladD { get => _producedBySaladD; private set { _producedBySaladD = value; OnPropertyChanged(); } }

        public long ProducedByNormalA { get => _producedByNormalA; private set { _producedByNormalA = value; OnPropertyChanged(); } }
        public long ProducedByNormalB { get => _producedByNormalB; private set { _producedByNormalB = value; OnPropertyChanged(); } }
        public long ProducedByNormalC { get => _producedByNormalC; private set { _producedByNormalC = value; OnPropertyChanged(); } }
        public long ProducedByNormalD { get => _producedByNormalD; private set { _producedByNormalD = value; OnPropertyChanged(); } }

        public long TotalProducedA { get => _totalProducedA; private set { _totalProducedA = value; OnPropertyChanged(); } }
        public long TotalProducedB { get => _totalProducedB; private set { _totalProducedB = value; OnPropertyChanged(); } }
        public long TotalProducedC { get => _totalProducedC; private set { _totalProducedC = value; OnPropertyChanged(); } }
        public long TotalProducedD { get => _totalProducedD; private set { _totalProducedD = value; OnPropertyChanged(); } }

        // DataGrid result items
        public ObservableCollection<ResultRow> ResultGridItems { get; set; }

        public class ResultRow
        {
            public string Label { get; set; } = string.Empty;
            public string A { get; set; } = string.Empty;
            public string B { get; set; } = string.Empty;
            public string C { get; set; } = string.Empty;
            public string D { get; set; } = string.Empty;
        }

        private void RefreshResultGrid()
        {
            if (ResultGridItems == null) return;
            ResultGridItems.Clear();

            ResultGridItems.Add(new ResultRow { Label = "描述", A = NameA, B = NameB, C = NameC, D = NameD });
            ResultGridItems.Add(new ResultRow { Label = "目标总需求", A = TotalReqA.ToString(), B = TotalReqB.ToString(), C = TotalReqC.ToString(), D = TotalReqD.ToString() });
            ResultGridItems.Add(new ResultRow { Label = "当前已有", A = CurrentQtyA.ToString(), B = CurrentQtyB.ToString(), C = CurrentQtyC.ToString(), D = CurrentQtyD.ToString() });
            ResultGridItems.Add(new ResultRow { Label = "缺口", A = NeedA.ToString(), B = NeedB.ToString(), C = NeedC.ToString(), D = NeedD.ToString() });
            ResultGridItems.Add(new ResultRow { Label = "带色拉奖励的执行次数", A = RoundsSaladA.ToString(), B = RoundsSaladB.ToString(), C = RoundsSaladC.ToString(), D = RoundsSaladD.ToString() });
            ResultGridItems.Add(new ResultRow { Label = "普通奖励的执行次数", A = RoundsNormalA.ToString(), B = RoundsNormalB.ToString(), C = RoundsNormalC.ToString(), D = RoundsNormalD.ToString() });
            ResultGridItems.Add(new ResultRow { Label = "总执行次数", A = RoundsTotalA.ToString(), B = RoundsTotalB.ToString(), C = RoundsTotalC.ToString(), D = RoundsTotalD.ToString() });
            ResultGridItems.Add(new ResultRow { Label = "总产出", A = TotalProducedA.ToString(), B = TotalProducedB.ToString(), C = TotalProducedC.ToString(), D = TotalProducedD.ToString() });
        }

        public int SaladAmount { get => _saladAmount; set { _saladAmount = value; OnPropertyChanged(); OnPropertyChanged(nameof(CanCalculate)); } }
        public int NormalAmount { get => _normalAmount; set { _normalAmount = value; OnPropertyChanged(); OnPropertyChanged(nameof(CanCalculate)); } }
        public int MaxSalad { get => _maxSalad; set { _maxSalad = value; OnPropertyChanged(); OnPropertyChanged(nameof(CanCalculate)); } }

        public string ResultText { get => _resultText; set { _resultText = value; OnPropertyChanged(); } }

        public ICommand CalculateCommand { get; }

        public bool CanCalculate => ValidateAll();

        private bool ValidateAll()
        {
            // simple validation: non-negative
            if (TotalReqA < 0 || TotalReqB < 0 || TotalReqC < 0 || TotalReqD < 0) return false;
            if (CurrentQtyA < 0 || CurrentQtyB < 0 || CurrentQtyC < 0 || CurrentQtyD < 0) return false;
            if (MaxSalad < 0) return false;
            if (SaladAmount < 0 || NormalAmount < 0) return false;
            // if no needs and no rewards still ok
            if (NeedA == 0 && NeedB == 0 && NeedC == 0 && NeedD == 0) return true;
            if (SaladAmount == 0 && NormalAmount == 0) return false;
            return true;
        }

        private void Calculate()
        {
            try
            {
                // update model from VM inputs
                _activityReward.RewardSalad = SaladAmount;
                _activityReward.RewardNormal = NormalAmount;
                _activityReward.RoundsLimitSalad = MaxSalad;

                // Totals and current quantities are already stored in model via bindings

                var res = RoundsCalculator.CalculateMinimumRounds(NeedA, NeedB, NeedC, NeedD, SaladAmount, NormalAmount, MaxSalad);
                var sb = new StringBuilder();
                if (!res.Possible)
                {
                    sb.AppendLine("无法达成");
                }
                else
                {
                    sb.AppendLine($"最少需要行动次数: {res.TotalRounds}");
                    sb.AppendLine($"色拉使用次数: {res.TotalSaladUses}");
                    sb.AppendLine($"普通使用次数: {res.TotalNormalUses}");
                    sb.AppendLine();
                    sb.AppendLine("类别 | 色拉 | 普通 | 色拉产出 | 普通产出 | 总产出 : ");
                    for (int i = 0; i < 4; i++)
                    {
                        string cat = i == 0 ? "A" : i == 1 ? "B" : i == 2 ? "C" : "D";
                        sb.AppendLine($"{cat} : {res.SaladUsesPerCategory[i]} | {res.NormalUsesPerCategory[i]} | {res.ProducedBySaladPerCategory[i]} | {res.ProducedByNormalPerCategory[i]} | {res.TotalProducedPerCategory[i]}");
                    }

                    // write back to model
                    _activityReward.RoundsSaladA = res.SaladUsesPerCategory[0];
                    _activityReward.RoundsSaladB = res.SaladUsesPerCategory[1];
                    _activityReward.RoundsSaladC = res.SaladUsesPerCategory[2];
                    _activityReward.RoundsSaladD = res.SaladUsesPerCategory[3];

                    _activityReward.RoundsNormalA = res.NormalUsesPerCategory[0];
                    _activityReward.RoundsNormalB = res.NormalUsesPerCategory[1];
                    _activityReward.RoundsNormalC = res.NormalUsesPerCategory[2];
                    _activityReward.RoundsNormalD = res.NormalUsesPerCategory[3];

                    // fill produced fields for UI
                    ProducedBySaladA = res.ProducedBySaladPerCategory[0];
                    ProducedBySaladB = res.ProducedBySaladPerCategory[1];
                    ProducedBySaladC = res.ProducedBySaladPerCategory[2];
                    ProducedBySaladD = res.ProducedBySaladPerCategory[3];

                    ProducedByNormalA = res.ProducedByNormalPerCategory[0];
                    ProducedByNormalB = res.ProducedByNormalPerCategory[1];
                    ProducedByNormalC = res.ProducedByNormalPerCategory[2];
                    ProducedByNormalD = res.ProducedByNormalPerCategory[3];

                    TotalProducedA = res.TotalProducedPerCategory[0];
                    TotalProducedB = res.TotalProducedPerCategory[1];
                    TotalProducedC = res.TotalProducedPerCategory[2];
                    TotalProducedD = res.TotalProducedPerCategory[3];

                    // notify changes for round counts
                    OnPropertyChanged(nameof(RoundsSaladA)); OnPropertyChanged(nameof(RoundsSaladB)); OnPropertyChanged(nameof(RoundsSaladC)); OnPropertyChanged(nameof(RoundsSaladD));
                    OnPropertyChanged(nameof(RoundsNormalA)); OnPropertyChanged(nameof(RoundsNormalB)); OnPropertyChanged(nameof(RoundsNormalC)); OnPropertyChanged(nameof(RoundsNormalD));
                    OnPropertyChanged(nameof(RoundsTotalA)); OnPropertyChanged(nameof(RoundsTotalB)); OnPropertyChanged(nameof(RoundsTotalC)); OnPropertyChanged(nameof(RoundsTotalD));

                    // persist
                    _dataService.SaveData(_activityReward);
                }
                ResultText = sb.ToString();

                // refresh grid
                RefreshResultGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"计算失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        #endregion

        #region IDataErrorInfo
        public string Error => null;
        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case nameof(TotalReqA): if (TotalReqA < 0) return "值不能为负数"; break;
                    case nameof(TotalReqB): if (TotalReqB < 0) return "值不能为负数"; break;
                    case nameof(TotalReqC): if (TotalReqC < 0) return "值不能为负数"; break;
                    case nameof(TotalReqD): if (TotalReqD < 0) return "值不能为负数"; break;
                    case nameof(CurrentQtyA): if (CurrentQtyA < 0) return "值不能为负数"; break;
                    case nameof(CurrentQtyB): if (CurrentQtyB < 0) return "值不能为负数"; break;
                    case nameof(CurrentQtyC): if (CurrentQtyC < 0) return "值不能为负数"; break;
                    case nameof(CurrentQtyD): if (CurrentQtyD < 0) return "值不能为负数"; break;
                    case nameof(SaladAmount): if (SaladAmount < 0) return "值不能为负数"; break;
                    case nameof(NormalAmount): if (NormalAmount < 0) return "值不能为负数"; break;
                    case nameof(MaxSalad): if (MaxSalad < 0) return "值不能为负数"; break;
                }
                return string.Empty;
            }
        }
        #endregion
    }
}