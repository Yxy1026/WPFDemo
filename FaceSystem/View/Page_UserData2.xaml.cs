using FaceSystem.UserModel;
using FaceSystem.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using FaceSystem.Tool;
using System.Windows.Threading;
using System.Data;
using System.Collections.Generic;

namespace FaceSystem.View
{
    /// <summary>
    /// Page_UserData2.xaml 的交互逻辑
    /// </summary>
    public partial class Page_UserData2 : Page
    {
        private delegate void DoPrintMethod(PrintDialog pdlg, DocumentPaginator paginator);
        private delegate void EnableButtonMethod();
        private System.Threading.Timer m_timerToEnableButton;

        public Page_UserData2()
        {
            InitializeComponent();
            Console.WriteLine("打印实例化");
        }

        ViewModelLocator vm = new ViewModelLocator();

        public IDCardInfo GetIDCardInfo()
        {
            IDCardInfo iDCardInfo = vm.IDCardData.GetIDCardInfo();
            return iDCardInfo;
        }

        private void Btn_Preview_Click(object sender, RoutedEventArgs e)
        {   
            PrintPreview previewWnd = new PrintPreview("View/ReportCard.xaml", Chengji(), new ScoreDocumentRenderer());
            previewWnd.ShowInTaskbar = false;
            previewWnd.ShowDialog();
        }

        private void Btn_Test(object sender, RoutedEventArgs e)
        {
            PrintPreview previewWnd = new PrintPreview("View/XueWeiZheng.xaml", Xinxi());
            previewWnd.ShowInTaskbar = false;
            previewWnd.ShowDialog();
        }

        private void DoPrint(PrintDialog pdlg, DocumentPaginator paginator)
        {
            pdlg.PrintDocument(paginator, "Report Card Document");
        }

        private void btnPrintDirect_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog pdlg = new PrintDialog();
            FlowDocument doc = PrintPreview.LoadDocumentAndRender("View/ReportCard.xaml", Chengji(), new ScoreDocumentRenderer());
            Dispatcher.BeginInvoke(new DoPrintMethod(DoPrint), DispatcherPriority.ApplicationIdle, pdlg, ((IDocumentPaginatorSource)doc).DocumentPaginator);
        }

        private void btnPrintDirect_Click2(object sender, RoutedEventArgs e)
        {
            PrintDialog pdlg = new PrintDialog();
            FlowDocument doc = PrintPreview.LoadDocumentAndRender("View/XueWeiZheng.xaml", Xinxi());
            Dispatcher.BeginInvoke(new DoPrintMethod(DoPrint), DispatcherPriority.ApplicationIdle, pdlg, ((IDocumentPaginatorSource)doc).DocumentPaginator);
        }


        private List<StudentScore> Chengji()
        {

            HttpRequestHelper.studentinfo = HttpRequestHelper.GetInfo("http://student.dlnu.edu.cn/queryUids/queryInfo", GetIDCardInfo().Idcard);
            List <StudentScore> list = HttpRequestHelper.GetList<StudentScore>("http://student.dlnu.edu.cn/queryUids/queryScores", HttpRequestHelper.studentinfo.XH);
            return list;
        }

        private StudentInfo Xinxi()
        {
            HttpRequestHelper.studentinfo = HttpRequestHelper.GetInfo("http://student.dlnu.edu.cn/queryUids/queryInfo",GetIDCardInfo().Idcard);
            HttpRequestHelper.studentinfo.BYDM = Convert.ToString(Int32.Parse(HttpRequestHelper.studentinfo.NJDM) + 4);
            HttpRequestHelper.studentinfo.SJ = DateTime.Now.ToLongDateString().ToString();
            return HttpRequestHelper.studentinfo;
        }

       
    }
}
