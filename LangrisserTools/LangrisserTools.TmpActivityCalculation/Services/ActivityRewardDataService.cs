using System;
using System.Threading.Tasks;
using LangrisserTools.Core.Services;
using LangrisserTools.TmpActivityCalculation.Models;

namespace LangrisserTools.TmpActivityCalculation.Services
{
    /// <summary>
    /// 活动轮数计算工具的数据服务
    /// 继承自 Core 的通用数据服务基类
    /// </summary>
    public class ActivityRewardDataService : DataService<ActivityReward>
    {
        private const string DATA_FILE_NAME = "ActivityReward.json";

        public ActivityRewardDataService() : base("TmpActivityCalculation")
        {
        }

        /// <summary>
        /// 加载数据（使用默认文件名）
        /// </summary>
        /// <returns>加载的 ActivityReward 对象</returns>
        public override ActivityReward LoadData()
        {
            try
            {
                var path = DataPathService.GetDataFilePath(ToolName, DATA_FILE_NAME, null, false);
                Console.WriteLine($"[ActivityRewardDataService] Loading data from: {path}");
                var result = base.LoadData(DATA_FILE_NAME);
                Console.WriteLine($"[ActivityRewardDataService] Load completed. Found file: {System.IO.File.Exists(path)}");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ActivityRewardDataService] LoadData error: {ex}");
                return new ActivityReward();
            }
        }

        /// <summary>
        /// 异步加载数据（使用默认文件名）
        /// </summary>
        /// <returns>加载的 ActivityReward 对象</returns>
        public async Task<ActivityReward> LoadDataAsync()
        {
            try
            {
                var path = DataPathService.GetDataFilePath(ToolName, DATA_FILE_NAME, null, false);
                Console.WriteLine($"[ActivityRewardDataService] Loading data async from: {path}");
                var result = await base.LoadDataAsync(DATA_FILE_NAME);
                Console.WriteLine($"[ActivityRewardDataService] Async load completed. Found file: {System.IO.File.Exists(path)}");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ActivityRewardDataService] LoadDataAsync error: {ex}");
                return new ActivityReward();
            }
        }

        /// <summary>
        /// 保存数据（使用默认文件名）
        /// </summary>
        /// <param name="data">要保存的 ActivityReward 对象</param>
        public override void SaveData(ActivityReward data)
        {
            try
            {
                var path = DataPathService.GetDataFilePath(ToolName, DATA_FILE_NAME, null, true);
                Console.WriteLine($"[ActivityRewardDataService] Saving data to: {path}");
                base.SaveData(data, DATA_FILE_NAME);
                Console.WriteLine("[ActivityRewardDataService] Save completed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ActivityRewardDataService] SaveData error: {ex}");
                throw;
            }
        }

        /// <summary>
        /// 异步保存数据（使用默认文件名）
        /// </summary>
        /// <param name="data">要保存的 ActivityReward 对象</param>
        public async Task SaveDataAsync(ActivityReward data)
        {
            try
            {
                var path = DataPathService.GetDataFilePath(ToolName, DATA_FILE_NAME, null, true);
                Console.WriteLine($"[ActivityRewardDataService] Saving data async to: {path}");
                await base.SaveDataAsync(data, DATA_FILE_NAME);
                Console.WriteLine("[ActivityRewardDataService] Async save completed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ActivityRewardDataService] SaveDataAsync error: {ex}");
                throw;
            }
        }
    }
}

