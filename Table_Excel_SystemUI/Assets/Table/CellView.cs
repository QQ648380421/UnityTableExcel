using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace XP.TableModel
{
    /// <summary>
    /// 单元格容器，用于刷新单元格位置
    /// </summary>
    public class CellView : MonoBehaviour
    {
        Table _table;
        /// <summary>
        /// 表格控制器
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
       /// 滚动容器被移动
       /// </summary>
       /// <param name="vector2"></param>
        private void _ScrollRectValueChanged(Vector2 vector2) { 
            //刷新单元格

        }
    }
}