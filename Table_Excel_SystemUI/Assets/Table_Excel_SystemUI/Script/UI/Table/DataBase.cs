namespace Xp_Table_V1
{
    /// <summary>
    /// 数据基类
    /// </summary>
    public class DataBase
    {
        TableController tableController;

        public DataBase(TableController tableController)
        {
            TableController = tableController;
        }

        /// <summary>
        /// 表格主控制器
        /// </summary>
        public TableController TableController
        {
            get
            {
                return tableController;
            }

            set
            {
                tableController = value;
            }
        }
    }
}