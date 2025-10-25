using System;
using System.Threading.Tasks;
using System.Windows;
using LangrisserTools.Core.ViewModels;
using LangrisserTools.Core.Services;

namespace LangrisserTools.Main.ViewModels
{
    /// <summary>
    /// 带数据管理的 ViewModel 基类
    /// 提供自动数据加载和保存功能
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public abstract class BaseDataViewModel<T> : BaseViewModel where T : class, new()
    {
        protected readonly DataService<T> DataService;
        protected readonly string DataFileName;
        protected T DataModel;

        protected BaseDataViewModel(string toolName, string dataFileName)
        {
            // 这里需要子类提供具体的 DataService 实现
            DataFileName = dataFileName;
            DataModel = new T();
        }

        /// <summary>
        /// 初始化数据（在构造函数中调用）
        /// </summary>
        protected virtual void InitializeData()
        {
            try
            {
                DataModel = DataService.LoadData(DataFileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading data during initialization: {ex.Message}");
                DataModel = new T();
            }
        }

        /// <summary>
        /// 异步初始化数据
        /// </summary>
        protected virtual async Task InitializeDataAsync()
        {
            try
            {
                DataModel = await DataService.LoadDataAsync(DataFileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading data during async initialization: {ex.Message}");
                DataModel = new T();
            }
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        public virtual void SaveData()
        {
            try
            {
                DataService.SaveData(DataModel, DataFileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving data: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 异步保存数据
        /// </summary>
        public virtual async Task SaveDataAsync()
        {
            try
            {
                await DataService.SaveDataAsync(DataModel, DataFileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving data: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 重新加载数据
        /// </summary>
        public virtual void ReloadData()
        {
            try
            {
                DataModel = DataService.LoadData(DataFileName);
                OnDataReloaded();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reloading data: {ex.Message}");
            }
        }

        /// <summary>
        /// 异步重新加载数据
        /// </summary>
        public virtual async Task ReloadDataAsync()
        {
            try
            {
                DataModel = await DataService.LoadDataAsync(DataFileName);
                OnDataReloaded();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reloading data: {ex.Message}");
            }
        }

        /// <summary>
        /// 数据重新加载后的回调（子类可重写）
        /// </summary>
        protected virtual void OnDataReloaded()
        {
            // 子类可以重写此方法来处理数据重新加载后的逻辑
        }

        /// <summary>
        /// 设置窗口的自动数据管理
        /// </summary>
        /// <param name="window">窗口对象</param>
        public void SetupWindowAutoDataManagement(Window window)
        {
            // 窗口加载时自动加载数据
            window.Loaded += (sender, e) =>
            {
                try
                {
                    var loadedData = DataService.LoadData(DataFileName);
                    if (loadedData != null)
                    {
                        DataModel = loadedData;
                        OnDataReloaded();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading data on window load: {ex.Message}");
                }
            };

            // 窗口关闭时自动保存数据
            window.Closing += (sender, e) =>
            {
                try
                {
                    DataService.SaveData(DataModel, DataFileName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving data on window close: {ex.Message}");
                }
            };
        }
    }
}
