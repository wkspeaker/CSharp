using System;
using System.Threading.Tasks;
using LangrisserTools.Core.Services;

namespace LangrisserTools.Core.ViewModels
{
 /// <summary>
 /// 工具通用 ViewModel 基类（Core 层），提供自动数据加载/保存的生命周期钩子
 /// Core 层不引用 UI 类型，便于在非 UI 测试中使用
 /// </summary>
 public abstract class ToolViewModel<TModel, TDataService> : BaseViewModel
 where TModel : class, new()
 where TDataService : IDataService<TModel>
 {
 protected readonly TDataService DataService;
 protected TModel Model;

 protected ToolViewModel(TDataService dataService)
 {
 DataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
 Model = new TModel();
 }

 /// <summary>
 /// 异步加载数据
 /// </summary>
 public virtual async Task LoadAsync()
 {
 try
 {
 Model = await DataService.LoadDataAsync();
 OnModelLoaded();
 }
 catch (Exception ex)
 {
 Console.WriteLine($"LoadAsync error: {ex}");
 }
 }

 /// <summary>
 /// 异步保存数据
 /// </summary>
 public virtual async Task SaveAsync()
 {
 try
 {
 await DataService.SaveDataAsync(Model);
 }
 catch (Exception ex)
 {
 Console.WriteLine($"SaveAsync error: {ex}");
 }
 }

 /// <summary>
 /// 同步加载数据（兼容现有同步代码）
 /// </summary>
 public virtual void Load()
 {
 try
 {
 Model = DataService.LoadData();
 OnModelLoaded();
 }
 catch (Exception ex)
 {
 Console.WriteLine($"Load error: {ex}");
 }
 }

 /// <summary>
 /// 同步保存数据（兼容现有同步代码）
 /// </summary>
 public virtual void Save()
 {
 try
 {
 DataService.SaveData(Model);
 }
 catch (Exception ex)
 {
 Console.WriteLine($"Save error: {ex}");
 }
 }

 /// <summary>
 /// 派生类在此方法中把 Model 的数据映射到公开属性，并通知 UI
 /// </summary>
 protected abstract void OnModelLoaded();
 }
}
