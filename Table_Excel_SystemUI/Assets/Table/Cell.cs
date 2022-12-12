using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
namespace XP.TableModel
{
    /// <summary>
    /// 单元格
    /// </summary>
    public class Cell : Selectable
    {
        /// <summary>
        /// 用来方便调试查看的数据
        /// </summary>
        [Header("DebugInfoData")]
        [SerializeField]
        private CellData cellData; 
        /// <summary>
        /// 单元格数据
        /// </summary>
        public CellData _CellData { get => cellData; set {
                if (cellData == value) return;
                cellData = value;
                value._Cell = this;
                _Invoke__CellDataChangeEvent(this, value); 
                _CellDataChangedEvents?.Invoke(value); 
            }  }

        /// <summary>
        /// <see cref="_Data"/>发生变化时触发
        /// </summary>
        public event _CellDataChanged _CellDataChangeEvent;

        /// <summary>
        /// <see cref="_Data"/>发生变化时触发
        /// </summary>
        public CellDataEvent _CellDataChangedEvents;

        /// <summary>
        /// <see cref="_Data"/>发生变化时触发,方便面板传值
        /// </summary>
        public InputField.SubmitEvent _CellDataChangedEvents_String;
        /// <summary>
        /// 触发单元格数据发生变化事件
        /// </summary>
        /// <param name="_cell"></param>
        /// <param name="_cellData"></param>
        public void _Invoke__CellDataChangeEvent(Cell _cell, CellData _cellData) {
            _CellDataChangeEvent?.Invoke(_cell, _cellData);
        }
        protected override void Start()
        {
            base.Start(); 

        }
    }
}