# UnityTableAndExcel
<p>在unity中非常简单的使用表格控件和Excel导入导出<p/>
<p>使用教程：<p/>
https://www.bilibili.com/video/BV1dW4y1K78B/?share_source=copy_web&vd_source=f31f50912390f999e135d9533323ad90
演示效果：
![](https://raw.githubusercontent.com/QQ648380421/UnityTableExcel/master/%E6%BC%94%E7%A4%BA%E6%95%88%E6%9E%9C.gif)
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

<p>测试过数据量50*500=25000的单元格，帧数稳定流畅<p/>

![](https://user-images.githubusercontent.com/40554493/210064241-52f87ccb-b049-4c7a-a73e-2aa53fd9ecae.png)

有朋友反应说中文乱码，是因为TMP字体问题，制作一份中文TMP字体文件，然后修改表头单元格预制体中的字体即可
