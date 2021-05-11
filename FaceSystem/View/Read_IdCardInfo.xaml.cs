using FaceSystem.Tool;
using FaceSystem.UserModel;
using FaceSystem.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Caching;
using System.Text;
using System.Web.Caching;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;



namespace FaceSystem.View
{
    /// <summary>
    /// Read_IdCardInfo.xaml 的交互逻辑
    /// </summary>
    public partial class Read_IdCardInfo : Page
    {
        public Read_IdCardInfo()
        {
            InitializeComponent();
            
            Console.WriteLine("身份证");

            this.Loaded += TnitialStar;
            IDCardDetail();
        }

        ViewModelLocator vm = new ViewModelLocator();

        public void IDCardDetail()
        {
            IDCardInfo idcard = vm.IDCardData.GetIDCardInfo();
            this.tb_name.Content = idcard.Name;
            this.tb_nation.Content = idcard.Nation;
            this.tb_id.Content = idcard.Idcard;
            this.tb_sex.Content = idcard.Sex;
            this.tb_effitivedate.Content = idcard.Effitivedate;
            this.tb_address.Content = idcard.Address;
            this.tb_birth.Content = idcard.Birthday;
            this.tb_sign.Content = idcard.Sign;
            this.tb_photo.Source = GetFilebyte(idcard.ImgPath);
        }


        static public BitmapImage GetFilebyte(string filename)
        {
            BitmapImage bmp = null;
            if (File.Exists(filename))
            {
                FileStream stream = new FileStream(filename, FileMode.Open);
                byte[] bts = new byte[stream.Length];
                stream.Read(bts, 0, bts.Length);
                stream.Close();
                stream.Dispose();

                bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.StreamSource = new MemoryStream(bts);
                bmp.EndInit();
            }
            return bmp;
        }

        private void TnitialStar(object sender, RoutedEventArgs e)
        {
            if (_faceThread == null) {
                Console.WriteLine("第一次");
                System.Timers.Timer initialTimer = new System.Timers.Timer();
                initialTimer.Elapsed += initialTimer_Elapsed;
                initialTimer.Interval = 1000;
                initialTimer.Enabled = true;
                initialTimer.AutoReset = false;
            }
            else
            {
                Console.WriteLine("第二次");

                FaceVerification faceVerification = new FaceVerification();
                faceVerification.Show();
                Prompt.Visibility = System.Windows.Visibility.Collapsed;
                faceVerification.Closed += delegate
                {
                    if (MainWindow1.flag == 2)
                    {
                        ToPrint();
                    }
                };
            }
        }
        
        void initialTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            InitFace();
        }

        static public bool isFaceLode = false;

        static public FaceThread _faceThread { get; set; }

        /// <summary>
        /// 初始化人脸
        /// </summary>
        public void InitFace()
        {
            try
            {
                //初始化人脸引擎
                Console.WriteLine("初始化人脸引擎");
                //_faceThread = new FaceThread();
                isFaceLode = true;
                //MessageBox.Show(isFaceLode.ToString());
                App.Current.Dispatcher.Invoke((Action)(() =>
                {
                    FaceVerification faceVerification = new FaceVerification();
                    Prompt.Visibility = System.Windows.Visibility.Collapsed;
                    faceVerification.Show();

                    faceVerification.Closed += delegate 
                    { 
                        if(MainWindow1.flag == 2)
                        {
                            ToPrint();
                        }
                        else
                        {
                            BaoDao(vm.IDCardData.GetIDCardInfo().Idcard);
                            
                            
                            //GetToken();
                            //GetDepart();
                            //Xinxi();
                            //Test(); 
                        }
                    };
                }));  

            }
            catch (Exception ex)
            {
                isFaceLode = false;

                MessageBox.Show(ex.Message);
            }
        }

        public void DisposeFace()
        {
            if (isFaceLode == false && _faceThread != null)
            {
                try
                {
                    _faceThread.qsFaceEngine.dispose();
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }
        }

        private Token GetToken()
        {
            HttpRequestHelper.token = HttpRequestHelper.GetToken("https://qyapi.weixin.qq.com/cgi-bin/gettoken", "wxccf4abb85e4bddfc", "65YqY9_1ZlPO9iGeNkeUCPiJl2kSElDaJ0JalsbHv9E");
            if (HttpRequestHelper.token.errcode != "0")
            {
                MessageBox.Show("err!");
            }
            Console.WriteLine(HttpRequestHelper.token.access_token);
            return HttpRequestHelper.token;
        }

        private DepartmentInfo GetDepart()
        {
            HttpRequestHelper.departmentInfo = HttpRequestHelper.GetDepart("https://qyapi.weixin.qq.com/cgi-bin/department/list", HttpRequestHelper.token.access_token);
            MessageBox.Show(HttpRequestHelper.departmentInfo.department[0].id + "+" + HttpRequestHelper.departmentInfo.department.Count);
            //153
            return HttpRequestHelper.departmentInfo;
        }

        private StudentInfo Xinxi()
        {
            HttpRequestHelper.studentinfo = HttpRequestHelper.GetInfo("http://student.dlnu.edu.cn/queryUids/queryInfo",vm.IDCardData.GetIDCardInfo().Idcard);
            HttpRequestHelper.studentinfo.BYDM = Convert.ToString(Int32.Parse(HttpRequestHelper.studentinfo.NJDM) + 4);
            HttpRequestHelper.studentinfo.SJ = DateTime.Now.ToLongDateString().ToString();
            HttpRequestHelper.studentinfo.BJH = HttpRequestHelper.studentinfo.BJH + "班";
         
            return HttpRequestHelper.studentinfo;
        }

        private void Test()
        {
            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("{");
            sBuilder.Append("\"userid\": \"" + HttpRequestHelper.studentinfo.XH + "\",");
            sBuilder.Append("\"name\": \"" + HttpRequestHelper.studentinfo.XM + "\",");
            StringBuilder dBuilder = new StringBuilder();
            for (int i = 0; i < HttpRequestHelper.departmentInfo.department.Count; i ++ )
            {
                Console.WriteLine(HttpRequestHelper.departmentInfo.department[i].name);
                if(HttpRequestHelper.studentinfo.BJH == HttpRequestHelper.departmentInfo.department[i].name)
                {
                    dBuilder.Append(HttpRequestHelper.departmentInfo.department[i].id);
                }
                
            }
            if (dBuilder.Length == 0)
            {
                MessageBox.Show("没有查询到本人！");
                return;
            }
            sBuilder.Append("\"department\": [" + dBuilder + "],");
            sBuilder.Append("\"mobile\": \"" + HttpRequestHelper.studentinfo.DH + "\",");
            sBuilder.Append("\"email\": \"" + HttpRequestHelper.studentinfo.EMAIL + "\"");
            sBuilder.Append("}");

            Console.WriteLine(sBuilder.ToString());

            string postUrl = "https://qyapi.weixin.qq.com/cgi-bin/user/create?access_token=" + HttpRequestHelper.token.access_token;
            MessageBox.Show(HttpRequestHelper.departmentInfo.department[0].id + "+" + HttpRequestHelper.departmentInfo.department.Count);
            string jsonStr = sBuilder.ToString();
            byte[] bytes = Encoding.UTF8.GetBytes(jsonStr);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(postUrl);

            request.Method = "POST";
            request.ContentLength = bytes.Length;
            request.ContentType = "text/xml";
            Stream reqstream = request.GetRequestStream();
            reqstream.Write(bytes, 0, bytes.Length);

            request.Timeout = 90000;
            
            request.Headers.Set("Pragma", "no-cache");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream streamReceive = response.GetResponseStream();
            Encoding encoding = Encoding.UTF8;

            StreamReader streamReader = new StreamReader(streamReceive, encoding);
            string strResult = streamReader.ReadToEnd();
            streamReceive.Dispose();
            streamReader.Dispose();
            MessageBox.Show("END");

        }


        public void ToPrint()
        {
            NavigationService.GetNavigationService(this).Navigate(new Page_UserData2());
        }

        public void BaoDao(string sfz)
        {
            string time = DateTime.Now.ToString();

            string[] datas = new string[] {sfz,time};
            

            string line = "";
            using (StreamReader sr = new StreamReader("baodao.txt"))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    if (line == sfz)
                    {
                        MessageBox.Show("请勿重复报到！");
                        return;
                    }
                    Console.WriteLine(line);
                }
            }

            FileStream fileStream = null;
            fileStream = new FileStream("baodao.txt", FileMode.Append);//以覆盖写入的方式打开这个文件
            using (StreamWriter sw = new StreamWriter(fileStream))
            {
                foreach (string s in datas)
                {
                    Console.WriteLine(s);   
                    sw.WriteLine(s);
                }
            }
            
            Console.Read();
            MessageBox.Show("报到成功！");
        }
    }
} 
