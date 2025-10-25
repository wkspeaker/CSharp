using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LangrisserTools.Core.ViewModels
{
    /// <summary>
    /// ViewModel 基类
    /// 提供通用的属性变更通知功能
    /// </summary>
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// 触发属性变更通知
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 设置属性值并触发变更通知
        /// </summary>
        /// <typeparam name="T">属性类型</typeparam>
        /// <param name="field">字段引用</param>
        /// <param name="value">新值</param>
        /// <param name="propertyName">属性名称</param>
        /// <returns>如果值发生变更返回 true</returns>
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(field, value))
                return false;

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// 设置属性值并触发变更通知（带验证）
        /// </summary>
        /// <typeparam name="T">属性类型</typeparam>
        /// <param name="field">字段引用</param>
        /// <param name="value">新值</param>
        /// <param name="onChanged">值变更后的回调</param>
        /// <param name="propertyName">属性名称</param>
        /// <returns>如果值发生变更返回 true</returns>
        protected bool SetProperty<T>(ref T field, T value, Action<T> onChanged, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(field, value))
                return false;

            field = value;
            onChanged?.Invoke(value);
            OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// 触发多个属性的变更通知
        /// </summary>
        /// <param name="propertyNames">属性名称数组</param>
        protected void OnPropertiesChanged(params string[] propertyNames)
        {
            foreach (var propertyName in propertyNames)
            {
                OnPropertyChanged(propertyName);
            }
        }
    }
}

