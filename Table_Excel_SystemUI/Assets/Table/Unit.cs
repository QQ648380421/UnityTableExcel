using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
namespace XP.TableModel
{
    /// <summary>
    /// ��Ԫģ��
    /// </summary>
    public static class Unit 
    {
        /// <summary>
        /// �����������ظ����
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