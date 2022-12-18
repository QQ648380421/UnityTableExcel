# UnityTableAndExcel
<p>在unity中非常简单的使用表格控件和Excel导入导出<p/>
<p>使用教程：<p/>
https://www.bilibili.com/video/BV1dW4y1K78B/?share_source=copy_web&vd_source=f31f50912390f999e135d9533323ad90
<pre><code>
//创建数据类型
//添加特性
//创建数组
List< TestData > _TestDatas = new List< TestData >();
for (int i = 0; i < 100; i++)
{//循环赋值
    var _testData= new TestData();
    _testData._Id = i;
    _testData.Name = "Bind:"+i;
    _testData._Money = UnityEngine.Random.Range(0,999f);
    _testData._Select = i%2==0;
    _testData._Time = DateTime.Now.AddDays(i);
    _testData._Age = UnityEngine.Random.Range(10,80);
    _TestDatas.Add(_testData);
}
//绑定到表格
_Table._BindArray(_TestDatas);
//没了
</code></pre>

