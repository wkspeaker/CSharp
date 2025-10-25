using System.Threading.Tasks;

namespace LangrisserTools.Core.Services
{
 public interface IDataService<T> where T : class, new()
 {
 T LoadData();
 Task<T> LoadDataAsync();
 void SaveData(T data);
 Task SaveDataAsync(T data);
 }
}
