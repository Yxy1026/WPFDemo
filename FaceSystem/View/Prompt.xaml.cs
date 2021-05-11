using FaceSystem.Tool;
using FaceSystem.UserModel;
using FaceSystem.ViewModel;
using System;
using System.IO;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;


namespace FaceSystem.View
{
    /// <summary>
    /// Prompt.xaml 的交互逻辑
    /// </summary>
    public partial class Prompt : Page
    {
        public Prompt()
        {
            Console.WriteLine("提示页面");
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        [DllImport("sdtapi.dll", EntryPoint = "InitComm")]
        private static extern int InitComm(int iPort);

        [DllImport("sdtapi.dll", EntryPoint = "Authenticate")]
        private static extern int Authenticate();

        [DllImport("sdtapi.dll", EntryPoint = "ReadBaseInfosPhoto", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]

        public static extern int ReadBaseInfosPhoto(StringBuilder Name, StringBuilder Gender, StringBuilder Folk, StringBuilder BirthDay, StringBuilder Code, StringBuilder Address, StringBuilder Agency, StringBuilder ExpireStart, StringBuilder ExpireEnd, StringBuilder directory);


        [DllImport("sdtapi.dll", EntryPoint = "ReadBaseInfos", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
        public static extern int ReadBaseInfos(StringBuilder Name, StringBuilder Gender, StringBuilder Folk, StringBuilder BirthDay, StringBuilder Code, StringBuilder Address, StringBuilder Agency, StringBuilder ExpireStart, StringBuilder ExpireEnd);

        [DllImport("Sdtapi.dll")]
        private static extern int CloseComm();//封闭端口

        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        System.Timers.Timer dispatcherTimer;

        int ret;
        int iPort = 1001;
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
                System.Windows.MessageBox.Show("打开身份证失败:" + ret);
            }


            #endregion

            #endregion
        }

        void dispatcherTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            ReadIDCardInfo2();
        }

        private void ReadIDCardInfo2()
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

                string minPath = System.IO.Directory.GetCurrentDirectory() + "\\photo.bmp";
                Console.WriteLine(minPath);

                string minNewPath = Utility.GetPath("BiDui") + "IdCardPhoto.jpg";
                File.Copy(minPath, minNewPath);

                IDCardInfo userEntity = new IDCardInfo();
                userEntity.Name = name.ToString();
                userEntity.Sex = sex.ToString();
                userEntity.Nation = nation.ToString();
                userEntity.Birthday = birthday.ToString();
                userEntity.Idcard = idcard.ToString();
                userEntity.Address = address.ToString();
                userEntity.ImgPath = minNewPath.ToString();
                userEntity.Sign = sign.ToString();
                userEntity.Effitivedate = effitivedate.ToString() + "-" + deadline.ToString();

                ViewModelLocator vm = new ViewModelLocator();
                vm.IDCardData.SetIDCardInfo(userEntity);
                Console.WriteLine(name);


                //dispatcherTimer.Start();
                stopwatch.Stop();
                this.grid_IndexWrap.Dispatcher.Invoke(
                   new Action(
                        delegate
                        {
                            ToNext();
                        }
                   )
                );


                //Read_IdCardInfo read_IdCardInfo = new Read_IdCardInfo();
                //read_IdCardInfo.tb_name.Content = name.ToString();
                //this.NavigationService.Navigate(read_IdCardInfo);

                #endregion

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
                dispatcherTimer.Start();
            }

        }
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
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString());
            }
        }

        public void ToNext()
        {

            NavigationService.GetNavigationService(this).Navigate(new Read_IdCardInfo());

            //this.grid_IndexWrap.Visibility = System.Windows.Visibility.Collapsed;
            //this.PageContext.Visibility = System.Windows.Visibility.Visible;
            //Read_IdCardInfo r = new Read_IdCardInfo();
            //this.PageContext.Content = r;
            Console.WriteLine("转到身份证详情");
        }




    }
}
