using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace XP.TableModel.Test
{
    /// <summary>
    /// 测试脚本
    /// </summary>
    public class TestTable : MonoBehaviour
    {
        public Table _Table;
        public TMP_InputField _ValueInput;
        public Button _AddRow, _AddColumn, _RemoveRow, _RemoveColumn,_ChangeSelectText,_ClearButton,_TestBindArray;
        public int _IniColumn=10, _IniRow=30;
        // Start is called before the first frame update
        /// <summary>
        /// 测试绑定数据
        /// </summary>
        private void _TestBindArr() {
            _Table._ClearTable();
            List<TestData> _TestDatas = new List<TestData>();
            for (int i = 0; i < 100; i++)
            {
                var _testData= new TestData();
                _testData._Id = i;
                _testData.Name = "Bind:"+i;
                _testData._Money = UnityEngine.Random.Range(0,999f);
                _testData._Select = i%2==0;
                _testData._Time = DateTime.Now.AddDays(i);
                _testData._Age = UnityEngine.Random.Range(10,80);
                _TestDatas.Add(_testData);
            }

            _Table._BindArray(_TestDatas);

        }
        IEnumerator Start()
        {
 
            _AddRow.onClick.AddListener(() =>
            {
                StartCoroutine(_AddRowClick());
            });
            _AddColumn.onClick.AddListener(() =>
            {
                StartCoroutine(_AddColumnClick());
            });
            _ClearButton.onClick.AddListener(()=> {
                _Table._ClearTable();
            });
            _RemoveRow.onClick.AddListener(_RemoveRowClick);
            _RemoveColumn.onClick.AddListener(_RemoveColumnClick);

            _ChangeSelectText.onClick.AddListener(__ChangeSelectText);
            _TestBindArray.onClick.AddListener(_TestBindArr);
            for (int i = 0; i < _IniColumn; i++)
            {
                yield return null;
              _Table.  _AddColumn();
            }
            for (int i = 0; i < _IniRow; i++)
            {
                yield return null;
                _Table._AddRow();
            }
            int index = 0;
            foreach (var item in _Table._CellDatas)
            {
                item._Data = index;
                index++;
            }

            


        }
        private void __ChangeSelectText() {
            foreach (var item in _Table._CurrentSelectedCellDatas)
            { 
                item._Data = _ValueInput.text;
            }
        }
        private int _GetIndex()
        {
            int x;
            int.TryParse(_ValueInput.text, out x);  
            return x;
        }
        private IEnumerator _AddRowClick()
        {
            var _count = _GetIndex();
            for (int i = 0; i < _count; i++)
            {
                yield return null;
                _Table._AddRow();
            }

        }
        private IEnumerator _AddColumnClick()
        {
            var _count = _GetIndex();
            for (int i = 0; i < _count; i++)
            {
                yield return null;
                _Table._AddColumn();
            }
        }
        private void _RemoveRowClick()
        { 
            _Table.RemoveSelectedRow();

        }
        private void _RemoveColumnClick()
        {
            _Table.RemoveSelectedColumn();
        }


    }
}