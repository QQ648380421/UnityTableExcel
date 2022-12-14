using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace XP.TableModel
{
    /// <summary>
    /// ≤‚ ‘Ω≈±æ
    /// </summary>
    public class TestTable : MonoBehaviour
    {
        public Table _Table;
        public TMP_InputField _ValueInput;
        public Button _AddRow, _AddColumn, _RemoveRow, _RemoveColumn,_ChangeSelectText;

        // Start is called before the first frame update
        void Start()
        {
            _AddRow.onClick.AddListener(() =>
            {
                StartCoroutine(_AddRowClick());
            });
            _AddColumn.onClick.AddListener(() =>
            {
                StartCoroutine(_AddColumnClick());
            });
            _RemoveRow.onClick.AddListener(_RemoveRowClick);
            _RemoveColumn.onClick.AddListener(_RemoveColumnClick);

            _ChangeSelectText.onClick.AddListener(__ChangeSelectText);
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