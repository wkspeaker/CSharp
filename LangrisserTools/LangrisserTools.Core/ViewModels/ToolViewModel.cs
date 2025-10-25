using System;
using System.Threading.Tasks;
using LangrisserTools.Core.Services;

namespace LangrisserTools.Core.ViewModels
{
 /// <summary>
 /// ����ͨ�� ViewModel ���ࣨCore �㣩���ṩ�Զ����ݼ���/������������ڹ���
 /// Core �㲻���� UI ���ͣ������ڷ� UI ������ʹ��
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
 /// �첽��������
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
 /// �첽��������
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
 /// ͬ���������ݣ���������ͬ�����룩
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
 /// ͬ���������ݣ���������ͬ�����룩
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
 /// �������ڴ˷����а� Model ������ӳ�䵽�������ԣ���֪ͨ UI
 /// </summary>
 protected abstract void OnModelLoaded();
 }
}
