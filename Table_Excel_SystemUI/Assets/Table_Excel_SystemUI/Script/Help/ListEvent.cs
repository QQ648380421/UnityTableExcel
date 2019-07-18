using System.Collections;
using System.Collections.ObjectModel;
using System;
namespace Xp_ListEvent
{
    /// <summary>
    ///带事件的list
    /// </summary> 
    [Serializable]
    public class ListEvent<T> : Collection<T>,IEnumerable 
    {
        public interface IListEvent {  
            bool ValueChange<T0>(int index, T0 origingValue, T0 value);
            bool InsertEvent<T0>(int index, T0 value);
            bool ClearEvent( );
            bool RemoveEvent<T0>(int index, T0 value);
        }

        /// <summary>
        /// 修改事件委托
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public delegate bool ChangeAction();
        /// <summary>
        /// 修改事件委托
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public delegate bool ChangeAction<T0>(T0 value);
        /// <summary>
        /// 修改事件委托
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public delegate bool ChangeIndexAction<T0>(int index, T0 value);
        /// <summary>
        /// 修改事件委托
        /// </summary>
        /// <typeparam name="T0"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public delegate bool ChangeValueAction<T0>(int index,T0 origingValue, T0 value);

        /// <summary>
        /// 数据发生变化
        /// </summary>
        public event ChangeValueAction<T> ValueChange;
        /// <summary>
        /// 插入事件
        /// </summary>
        public event ChangeIndexAction<T> InsertEvent;
        /// <summary>
        /// 清空事件
        /// </summary>
        public event ChangeAction ClearEvent;
        /// <summary>
        /// 删除事件
        /// </summary>
        public event ChangeIndexAction<T> RemoveEvent;

        public new T this[int index]
        {
            get { return base[index]; }
            set
            { 
                var data = base[index];
                //在数组修改之前，先触发事件
                if (ValueChange != null)
                {
                    if (!ValueChange.Invoke(index, data, value))
                    {
                        return;
                    }

                }
                base.SetItem(index, value); 
            }
        }

        protected override void InsertItem(int index, T item)
        {
            if (InsertEvent != null)
            {
                if (!InsertEvent.Invoke(index, item))
                {
                    return;
                }

            }
            base.InsertItem(index, item);
        }
        protected override void ClearItems()
        {
            if (ClearEvent != null)
            {
                if (!ClearEvent.Invoke())
                {
                    return;
                }

            }
            base.ClearItems();
        }
        protected override void RemoveItem(int index)
        {
            if (RemoveEvent != null)
            {
                if (!RemoveEvent.Invoke(index, this[index]))
                {
                    return;
                }

            }
            base.RemoveItem(index);
        }
    }

}