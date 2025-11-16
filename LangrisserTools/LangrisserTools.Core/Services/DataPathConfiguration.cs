using System;
using System.IO;
using System.Text.Json;

namespace LangrisserTools.Core.Services
{
    /// <summary>
    /// 数据路径配置服务
    /// 支持运行时配置数据存储模式
    /// </summary>
    public class DataPathConfiguration
    {
        private const string CONFIG_FILE_NAME = "DataPathConfig.json";
        private static readonly Lazy<DataPathConfiguration> _instance = new Lazy<DataPathConfiguration>(() => new DataPathConfiguration());
        public static DataPathConfiguration Instance => _instance.Value;

        public bool UseUnifiedPath { get; set; } = false;
        public string UnifiedDataRoot { get; set; } = "Data";

        private DataPathConfiguration() 
        {
            LoadConfiguration();
        }

        /// <summary>
        /// 加载配置
        /// </summary>
        private void LoadConfiguration()
        {
            try
            {
                var configPath = GetConfigFilePath();
                if (File.Exists(configPath))
                {
                    var json = File.ReadAllText(configPath);
                    var config = JsonSerializer.Deserialize<DataPathConfigModel>(json);
                    if (config != null)
                    {
                        UseUnifiedPath = config.UseUnifiedPath;
                        UnifiedDataRoot = config.UnifiedDataRoot ?? "Data";
                    }
                }
            }
            catch (Exception ex)
            {
                // 配置加载失败时使用默认值
                Console.WriteLine($"Warning: Failed to load data path configuration: {ex.Message}");
            }
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        public void SaveConfiguration()
        {
            try
            {
                var config = new DataPathConfigModel
                {
                    UseUnifiedPath = UseUnifiedPath,
                    UnifiedDataRoot = UnifiedDataRoot
                };

                var json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
                var configPath = GetConfigFilePath();
                File.WriteAllText(configPath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Warning: Failed to save data path configuration: {ex.Message}");
            }
        }

        /// <summary>
        /// 获取配置文件路径
        /// </summary>
        /// <returns>配置文件完整路径</returns>
        private string GetConfigFilePath()
        {
            var solutionRoot = DataPathService.Instance.GetSolutionRoot();
            return Path.Combine(solutionRoot, CONFIG_FILE_NAME);
        }

        /// <summary>
        /// 切换数据存储模式
        /// </summary>
        /// <param name="useUnifiedPath">是否使用统一路径</param>
        public void SwitchDataMode(bool useUnifiedPath)
        {
            UseUnifiedPath = useUnifiedPath;
            SaveConfiguration();
        }
    }

    /// <summary>
    /// 数据路径配置模型
    /// </summary>
    public class DataPathConfigModel
    {
        public bool UseUnifiedPath { get; set; } = false;
        public string UnifiedDataRoot { get; set; } = "Data";
    }
}



