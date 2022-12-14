using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
namespace XP.TableModel
{
    /// <summary>
    /// 单元模块
    /// </summary>
    public static class Unit 
    {
        /// <summary>
        /// 添加组件，不重复添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public static T _AddComponent<T>(GameObject gameObject) where T:Component
        { 
            if (gameObject==null)
            {
                return default(T);
            }
            T _com= gameObject.GetComponent<T>();
            if (_com==null)
            {
                _com= gameObject.AddComponent<T>();
            } 
            return _com;
        }

    }
}