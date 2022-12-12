using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
namespace XP.TableModel
{
    /// <summary>
    /// ��Ԫ��
    /// </summary>
    public class Cell : Selectable
    {
        /// <summary>
        /// ����������Բ鿴������
        /// </summary>
        [Header("DebugInfoData")]
        [SerializeField]
        private CellData cellData; 
        /// <summary>
        /// ��Ԫ������
        /// </summary>
        public CellData _CellData { get => cellData; set {
                if (cellData == value) return;
                cellData = value;
                value._Cell = this;
                _Invoke__CellDataChangeEvent(this, value); 
                _CellDataChangedEvents?.Invoke(value); 
            }  }

        /// <summary>
        /// <see cref="_Data"/>�����仯ʱ����
        /// </summary>
        public event _CellDataChanged _CellDataChangeEvent;

        /// <summary>
        /// <see cref="_Data"/>�����仯ʱ����
        /// </summary>
        public CellDataEvent _CellDataChangedEvents;

        /// <summary>
        /// <see cref="_Data"/>�����仯ʱ����,������崫ֵ
        /// </summary>
        public InputField.SubmitEvent _CellDataChangedEvents_String;
        /// <summary>
        /// ������Ԫ�����ݷ����仯�¼�
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