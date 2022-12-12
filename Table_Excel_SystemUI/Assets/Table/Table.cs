using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
namespace XP.TableModel
{ 
    /// <summary>
    /// 表格
    /// </summary>
    public class Table : MonoBehaviour
    {
        /// <summary>
        /// 单元格数据
        /// </summary>
        public readonly ObservableCollection<CellData> _CellDatas = new ObservableCollection<CellData>();
        /// <summary>
        /// 最大行索引
        /// </summary>
        int _MaxRowIndex {
            get {
              return  _CellDatas.Max(p=>p._Row);
            }
        }
        /// <summary>
        /// 最大列索引
        /// </summary>
        int _MaxColumIndex
        {
            get
            {
                return _CellDatas.Max(p => p._Colum);
            }
        }
        /// <summary>
        /// 单元格预制体
        /// </summary>
        public Cell _CellPrefab;
        /// <summary>
        /// 单元格容器
        /// </summary>
        public Transform _CellView;
        /// <summary>
        /// 所有实例单元格
        /// </summary>
        public readonly List<Cell> _Cells = new List<Cell>();

        private void Awake()
        {
          
            _CellDatas.CollectionChanged -= _CellDatas_CollectionChanged;
            _CellDatas.CollectionChanged += _CellDatas_CollectionChanged;
        }

        /// <summary>
        /// 当单元格列表发生变化时触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _CellDatas_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                  var _newCell=  Instantiate(_CellPrefab, _CellView);
                    _newCell._CellData = _CellDatas[e.NewStartingIndex];
                    _Cells.Add(_newCell);
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Remove:
                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    break;
            }

        }

        // Start is called before the first frame update
        void Start()
        {
            _CellDatas.Add(new CellData() { 
             _Data="你好"
            });
            
        }

        /// <summary>
        /// 添加一行新数据
        /// </summary>
        public void _AddNewRow(int columCount) {
          var _maxIndex= _MaxRowIndex+1;
            for (int i = 0; i < columCount; i++)
            {
                _CellDatas.Add(new CellData()
                {
                    _Data = "Cell("+i+","+ _maxIndex + ")",
                    _Row = _maxIndex,
                    _Colum=i
                });
            }
          
        }
      
         
    }
}