using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace XP.TableModel
{
    /// <summary>
    /// ��Ԫ������������ˢ�µ�Ԫ��λ��
    /// </summary>
    public class CellView : MonoBehaviour
    {
        Table _table;
        /// <summary>
        /// ��������
        /// </summary>
        public Table _Table
        {
            get
            {
                if (!_table)
                {
                    _table = GetComponentInParent<Table>();
                }
                return _table;
            }
        }

        private void Start()
        {
            _Table._ScrollRect.onValueChanged.RemoveListener(_ScrollRectValueChanged);
            _Table._ScrollRect.onValueChanged.AddListener(_ScrollRectValueChanged);
        }
       /// <summary>
       /// �����������ƶ�
       /// </summary>
       /// <param name="vector2"></param>
        private void _ScrollRectValueChanged(Vector2 vector2) { 
            //ˢ�µ�Ԫ��

        }
    }
}