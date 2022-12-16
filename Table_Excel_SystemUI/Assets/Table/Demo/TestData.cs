using System;
using UnityEngine;

namespace XP.TableModel.Test
{
    public class TestData{

        int Id;
        [Column(0, "Number")]//这个标记加上之后，能自动将该属性分配到该列上
        public int _Id
        {
            get
            {
                return Id;
            }
            set
            {
                if (Id == value) return;
                Id = value;
            }
        }
        [Column(1, "TheName",_Width =300)]
        public string Name { get => name; set => name = value; }

        string name;

        bool select;
        [Column(2)]
        public bool _Select
        {
            get
            {
                return select;
            }
            set
            {
                if (select == value) return;
                select = value;
            }
        }


        float money;
        [Column(3, "NoMoney")]
        public float _Money
        {
            get
            {
                return money;
            }
            set
            {
                if (money == value) return;
                money = value;
            }
        }

        float noColumnTag;
        /// <summary>
        /// 没有标记的列
        /// </summary>
        public float _NoColumnTag
        {
            get
            {
                return noColumnTag;
            }
            set
            {
                if (noColumnTag == value) return;
                noColumnTag = value;
            }
        }

        int age;
        [Column(true)]
        public int _Age
        {
            get
            {
                return age;
            }
            set
            {
                if (age == value) return;
                age = value;
            }
        }



        DateTime time; 
        [Column(_Width =500)]
        public DateTime _Time
        {
            get
            {
                return time;
            }
            set
            {
                if (time == value) return;
                time = value;
            }
        }
        bool erorr;
        /// <summary>
        /// 故意设置的错误标记，会触发警告提示
        /// </summary>
        [Column(2,"ErorrTag")]
        public bool _Erorr
        {
            get
            {
                return erorr;
            }
            set
            {
                if (erorr == value) return;
                erorr = value;
            }
        }

        public void OnColumnCellClick(CellClickData cellClickData)
        {
            Debug.Log("这是在TestData类中触发的表头点击事件",cellClickData._Selectable);
        }

         

    }
}