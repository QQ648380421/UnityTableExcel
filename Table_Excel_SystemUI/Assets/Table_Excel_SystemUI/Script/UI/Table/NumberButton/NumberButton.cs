﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Xp_Table_V1
{


    /// <summary>
    /// 该类描述：暂无描述
    /// 负责人:夏鹏
    /// 联系方式(QQ):648380421
    /// 时间：
    /// </summary>
    public class NumberButton : MonoBehaviour
    {
        TableController tableController;

        public TableController TableController
        {
            get
            {
                if (!tableController)
                {
                    tableController = GetComponentInParent<TableController>();
                }
                return tableController;
            }
        }

        public void OnClick()
        {
            TableController.SelectCells.Clear();
            foreach (var item in TableController.Data.CellDatas)
            {
                TableController.SelectCells.Add(item.TableCell); 
            } 
        }
    }
}