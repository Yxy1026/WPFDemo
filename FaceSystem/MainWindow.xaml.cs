using FaceSystem.Tool;
using FaceSystem.UserModel;
using FaceSystem.ViewModel;
using QSNetFaceLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace FaceSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;
            this.Loaded += MainWindow_Loaded;
            this.Closed += MainWindow_Closed;

            CameraHelper.IsDisplay = true;
            CameraHelper.SourcePlayer = player;
            CameraHelper.UpdateCameraDevices();
            if (CameraHelper.CameraDevices.Count > 0)
            {
                CameraHelper.SetCameraDevice(0);
            }
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            #region 系统关闭
            //关闭身份证连接 
            CloseComm();
            #endregion
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            #region 系统加载
            #region  1.二代身份证
            //打开端口
            ret = InitComm(iPort);
            Console.WriteLine(ret);
            if (ret == 1)
            {

                //身份证自动读取
                dispatcherTimer = new System.Timers.Timer();
                dispatcherTimer.Elapsed += new System.Timers.ElapsedEventHandler(dispatcherTimer_Elapsed);
                dispatcherTimer.Interval = 800;
                dispatcherTimer.Enabled = true;
                dispatcherTimer.AutoReset = true;
            }
            else
            {
                MessageBox.Show("打开身份证失败:" + ret);
            }

            #endregion

            #region 2.人脸
            TnitialStar();
            #endregion

            #endregion
        }

        void dispatcherTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            RedIDCardInfo2();
        }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void btn_Url_Click(object sender, RoutedEventArgs e)
        {
            if (grid_IndexWarp.Visibility == System.Windows.Visibility.Visible)
            {
                grid_IndexWarp.Visibility = System.Windows.Visibility.Collapsed;
                this.PageContext.Visibility = System.Windows.Visibility.Visible;
            }
            var btn = e.Source as Button;
            if (btn != null && btn.Tag != null)
            {
                this.PageContext.Source = new Uri(btn.Tag.ToString(), UriKind.Relative);
            }
        }
        private void btn_Vrs_Click(object sender, RoutedEventArgs e)
        {
            grid_IndexWarp.Visibility = System.Windows.Visibility.Visible;

            this.PageContext.Visibility = System.Windows.Visibility.Collapsed;
        }
        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            //调用

           this.Close();
        }

        #region 身份证读取模块
        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        System.Timers.Timer dispatcherTimer;
        #region 读取身份证


        [DllImport("sdtapi.dll", EntryPoint = "InitComm")]
        private static extern int InitComm(int iPort);

        [DllImport("sdtapi.dll", EntryPoint = "Authenticate")]
        private static extern int Authenticate();

        [DllImport("sdtapi.dll", EntryPoint = "ReadBaseInfosPhoto", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]

        public static extern int ReadBaseInfosPhoto(StringBuilder Name, StringBuilder Gender, StringBuilder Folk, StringBuilder BirthDay, StringBuilder Code, StringBuilder Address, StringBuilder Agency, StringBuilder ExpireStart, StringBuilder ExpireEnd, StringBuilder directory);


        [DllImport("sdtapi.dll", EntryPoint = "ReadBaseInfos", CharSet = CharSet.Ansi,
        CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int ReadBaseInfos(StringBuilder Name, StringBuilder Gender, StringBuilder Folk, StringBuilder BirthDay, StringBuilder Code, StringBuilder Address, StringBuilder Agency, StringBuilder ExpireStart, StringBuilder ExpireEnd);


        [DllImport("Sdtapi.dll")]
        private static extern int CloseComm();//封闭端口
        private void RedIDCardInfoAuto(object sender, EventArgs e)
        {
            RedIDCardInfo2();
        }
        int ret;
        int iPort = 1001;

        ViewModelLocator vm = new ViewModelLocator();
        private void RedIDCardInfo2()
        {

            try
            {
                #region MyRegion
                dispatcherTimer.Stop();
                stopwatch.Start();
                ret = Authenticate();

                if (ret != 1)//
                {
                    dispatcherTimer.Start();
                    return;
                }



                stopwatch.Start();

                int IntInfo = 0; //判断读取身份证信息
                StringBuilder name, sex, nation, birthday, address, idcard, sign, effitivedate, deadline;
                name = new StringBuilder(31);
                sex = new StringBuilder(3);
                nation = new StringBuilder(10);
                birthday = new StringBuilder(9);
                idcard = new StringBuilder(19);
                address = new StringBuilder(71);
                sign = new StringBuilder(31);
                effitivedate = new StringBuilder(9);
                deadline = new StringBuilder(9);

                IntInfo = ReadBaseInfos(name, sex, nation, birthday, idcard, address, sign, effitivedate, deadline);
                if (IntInfo != 1)
                {
                    dispatcherTimer.Start();
                    return;
                }
                SpeechDuDu();

                Console.WriteLine("读到身份信息！");

                this.grid_IDCard.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                {

                    // SpeechDuDu();
                    //身份证小图
                    string minPath = System.IO.Directory.GetCurrentDirectory() + "\\photo.bmp";
                    Console.WriteLine(minPath);
                    //身份证信息界面赋值
                    tb_name.Content = name.ToString();
                    tb_sex.Content = sex.ToString();
                    tb_nation.Content = nation.ToString();
                    tb_birth.Content = birthday.ToString();
                    tb_id.Content = idcard.ToString();
                    tb_address.Content = address.ToString();
                    tb_sign.Content = sign.ToString();
                    tb_photo.Source = GetFilebyte(minPath);

                    string minNewPath = Utility.GetPath("BiDui") + "IdCardPhoto.jpg";
                    File.Copy(minPath, minNewPath);
                    // img_sfzView.Source = GetFilebyte(minPath);

                    //  CloseComm();

                    tb_effitivedate.Content = effitivedate.ToString() + deadline.ToString();


                    string sj = string.Format("读身份证耗时{0}秒 ", stopwatch.Elapsed.TotalSeconds);
                    stopwatch.Reset();

                    Console.WriteLine(sj);

                    //自动生成唯一图片路径
                    string minPhotoPath = Utility.GetPath("Photo") + "Photo.jpg";

                    string minPhotoNewPath = CameraHelper.CaptureImage(minPhotoPath);
                    tb_photo2.Source = GetFilebyte(minPhotoNewPath);
                    #region 人证比对

                    if (!string.IsNullOrEmpty(minNewPath) && !string.IsNullOrEmpty(minPhotoNewPath))
                    {
                        this.Dispatcher.Invoke(new Action(delegate
                        {
                            //Console.WriteLine(minNewPath);
                            //Console.WriteLine(minPhotoNewPath);
                            // TuPianBiDui("1.jpg", "2.jpg");
                            TuPianBiDui(minNewPath, minPhotoNewPath);
                        }));

                    }

                    #endregion
                    //tb_similarity.Content = "90%";
                    //tb_compareResult.Content = "成功";
                    UserInfo userEntity = new UserInfo();
                    userEntity.Name = name.ToString();
                    userEntity.Sex = sex.ToString();
                    userEntity.Nation = nation.ToString();
                    userEntity.Birthday = birthday.ToString();
                    userEntity.Idcard = idcard.ToString();
                    userEntity.Address = address.ToString();
                    userEntity.ImgPath = minNewPath.ToString();
                    userEntity.CompareDateTime = DateTime.Now;
                    //模拟赋值
                    userEntity.ID = "0";
                    userEntity.SetSimilarity = "90%";
                    userEntity.CompareResult = "成功";
                    userEntity.ImgRLPath = minPhotoNewPath.ToString();
                    vm.UserData.AddUser(userEntity);



                }));

                dispatcherTimer.Start();

                #endregion

            }
            catch (Exception ex)
            {
                dispatcherTimer.Start();
            }

        }

        #endregion
        #endregion

        #region 工具类

        /// <summary>
        /// 播放提示声音
        /// </summary>
        public void SpeechDuDu()
        {
            string url = AppDomain.CurrentDomain.BaseDirectory + "beep.wav";
            SoundPlayer player = new SoundPlayer();
            player.SoundLocation = url;
            try
            {
                player.Load();
                player.Play();
            }
            catch (Exception)
            {
            }
        }

        public static string GetPath(string FileName)
        {
            string now = DateTime.Now.ToString("yyMMddHHmmss");
            if (!Directory.Exists(ConfigHelper.GetAppSetting("FileSavePath") + FileName))
            {
                Directory.CreateDirectory(ConfigHelper.GetAppSetting("FileSavePath") + FileName); //新建文件夹  
            }
            return ConfigHelper.GetAppSetting("FileSavePath") + FileName + "\\" + now + "_";
        }

        public BitmapImage GetFilebyte(string filename)
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
        #endregion

        #region 图片比对

        /// <summary>
        /// 人证比对延时加载
        /// </summary>
        public void TnitialStar()
        {
            System.Timers.Timer initialTimer = new System.Timers.Timer();
            initialTimer.Elapsed += initialTimer_Elapsed;
            initialTimer.Interval = 1000;
            initialTimer.Enabled = true;
            initialTimer.AutoReset = false;
        }

        void initialTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            InitFace();
        }

        bool isFaceLode = false;


        private FaceThread _faceThread { get; set; }



        /// <summary>
        /// 初始化人脸
        /// </summary>
        public void InitFace()
        {
            try
            {
                //初始化人脸引擎
                _faceThread = new FaceThread();
                isFaceLode = true;

            }
            catch (Exception ex)
            {
                isFaceLode = false;

                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 图片比对
        /// </summary>
        /// <param name="sfzImg">身份证图片</param>
        /// <param name="paiZhaoImg">抓拍照片</param>
        public void TuPianBiDui(string sfzImg, string paiZhaoImg)
        {
            if (isFaceLode && _faceThread != null)
            {
                try
                {
                    QSNetFaceEngine.FaceModel[] faces2 = new QSNetFaceEngine.FaceModel[1];
                    int num2 = _faceThread.qsFaceEngine.detectFaces(paiZhaoImg, faces2, 1);
                    if (num2 < 1)
                    {
                        Console.WriteLine("没有检测到人脸");
                    }


                    QSNetFaceEngine.FaceModel[] faces1 = new QSNetFaceEngine.FaceModel[1];
                    int num1 = _faceThread.qsFaceEngine.detectFaces(sfzImg, faces1, 1);
                    if (num1 < 1)
                    {
                        Console.WriteLine("没有检测到人脸");
                    }


                    float score = _faceThread.qsFaceEngine.compare2Feature(faces1[0].feature, faces2[0].feature);

                    Console.WriteLine("相似度:" + score);
                    if (score >= GetFaceThreshold())
                    {

                        SetSimilarity(score);
                    }
                    else
                    {

                        SetSimilarity(score);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("人脸识别出错" + ex.Message);
                    //Trace.Write(ex.Message, "人脸识别出错");
                }
            }
            else
            {
                MessageBox.Show("加载人脸识别");
            }

        }

        /// <summary>
        /// 卸载人脸
        /// </summary>
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

                    // throw;
                }
            }
        }


        /// <summary>
        /// 获取人脸识别阈值
        /// </summary>
        /// <returns></returns>
        public float GetFaceThreshold()
        {
            return float.Parse(App.YuZhi.ToString());
        }

        /// <summary>
        /// 显示相似度
        /// </summary>
        /// <param name="score"></param>
        public void SetSimilarity(float score)
        {
            try
            {
                this.tb_compareResult.Content = "相似度：" + (score * 100).ToString("F") + "%";
            }
            catch
            {
                this.tb_compareResult.Content = "相似度：0%";
            }
        }






        #endregion

        private void WindowsFormsHost_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {

        }
    }
}
