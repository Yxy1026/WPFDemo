using FaceSystem.Tool;
using FaceSystem.UserModel;
using FaceSystem.View;
using FaceSystem.ViewModel;
using QSNetFaceLib;
using System;
using System.Windows;
using System.Diagnostics;
using System.Windows.Media.Imaging;
using System.IO;
using System.Drawing;
using FaceSystem.Utils;
using FaceSystem.SDKModels;
using System.Configuration;
using FaceSystem.SDKUtil;
using System.Threading;
using System.Collections.Generic;

namespace FaceSystem
{

    /// <summary>
    /// FaceVerification.xaml 的交互逻辑
    /// </summary>
    public partial class FaceVerification : Window
    {
        private IntPtr pEngine = IntPtr.Zero;

        //身份证图片byte
        private byte[] byteImage = new byte[40000];

        //是否显示相似度
        private bool isShow = false;

        private bool isRead = false;



        public FaceVerification()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            this.Loaded += TnitialStar;
            CameraHelper.IsDisplay = true;
            CameraHelper.SourcePlayer = player;

            CameraHelper.UpdateCameraDevices();
            if (CameraHelper.CameraDevices.Count > 0)
            {
                CameraHelper.SetCameraDevice(0);
            }
            Console.WriteLine("加载摄像头");
        }

        System.Timers.Timer timer;
        ViewModelLocator vm = new ViewModelLocator();

        private void TnitialStar(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("加载1");
            InitEngines();
            InitEngines2();
            DetectFace();
        }

        private void DetectFace()
        {
            Console.WriteLine("加载2");

            int Interval = 3000;
            timer = new System.Timers.Timer(Interval);
            timer.Elapsed += Test;
            timer.Enabled = true;
            timer.AutoReset = true;

        }


        private void InitEngines()
        {
            //读取配置文件
            AppSettingsReader reader = new AppSettingsReader();
            string appId = (string)reader.GetValue("APP_ID", typeof(string));
            string sdkKey64 = (string)reader.GetValue("SDKKEY64", typeof(string));
            string sdkKey32 = (string)reader.GetValue("SDKKEY32", typeof(string));

            var is64CPU = Environment.Is64BitProcess;
            if (is64CPU)
            {
                if (string.IsNullOrWhiteSpace(appId) || string.IsNullOrWhiteSpace(sdkKey64))
                {
                    MessageBox.Show("请在App.config配置文件中先配置APP_ID和SDKKEY64!");
                    return;
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(appId) || string.IsNullOrWhiteSpace(sdkKey32))
                {

                    MessageBox.Show("请在App.config配置文件中先配置APP_ID和SDKKEY32!");
                    return;
                }
            }

            //激活引擎    如出现错误，1.请先确认从官网下载的sdk库已放到对应的bin中，2.当前选择的CPU为x86或者x64
            int retCode = 0;

            try
            {
                retCode = ASIDCardFunctions.ArcSoft_FIC_Activate(appId, is64CPU ? sdkKey64 : sdkKey32);
            }
            catch (Exception ex)
            {
                if (ex.Message.IndexOf("无法加载 DLL") > -1)
                {
                    MessageBox.Show("请将sdk相关DLL放入bin对应的x86或x64下的文件夹中!");
                }
                else
                {
                    MessageBox.Show("激活引擎失败!");
                }
                return;
            }
            Console.WriteLine("Activate Result:" + retCode);

            //初始化引擎
            retCode = ASIDCardFunctions.ArcSoft_FIC_InitialEngine(ref pEngine);
            Console.WriteLine("InitEngine Result:" + retCode);
            if (retCode != 0)
            {
                MessageBox.Show(string.Format("引擎初始化失败!错误码为:{0}\n", retCode));
                return;
            }

        }

        private int rgbCameraIndex = 0;

        private IntPtr pVideoEngine = IntPtr.Zero;

        private IntPtr pVideoRGBImageEngine = IntPtr.Zero;

        private IntPtr pVideoIRImageEngine = IntPtr.Zero;

        private IntPtr pImageEngine = IntPtr.Zero;

        private void InitEngines2()
        {
            //读取配置文件
            AppSettingsReader reader = new AppSettingsReader();
            string appId = (string)reader.GetValue("APP_ID", typeof(string));
            string sdkKey64 = (string)reader.GetValue("2SDKKEY64", typeof(string));
            string sdkKey32 = (string)reader.GetValue("2SDKKEY32", typeof(string));
            rgbCameraIndex = (int)reader.GetValue("RGB_CAMERA_INDEX", typeof(int));
            
            //判断CPU位数
            var is64CPU = Environment.Is64BitProcess;
           
            int retCode = 0;
            try
            {
                retCode = ASIDCardFunctions.ASFActivation(appId, is64CPU ? sdkKey64 : sdkKey32);
            }
            catch (Exception ex)
            {
                
                if (ex.Message.Contains("无法加载 DLL"))
                {
                    MessageBox.Show("请将sdk相关DLL放入bin对应的x86或x64下的文件夹中!");
                }
                else
                {
                    MessageBox.Show("激活引擎失败!");
                }
                return;
            }
            Console.WriteLine("Activate Result:" + retCode);

            //初始化引擎
            uint detectMode = DetectionMode.ASF_DETECT_MODE_IMAGE;
            //Video模式下检测脸部的角度优先值
            int videoDetectFaceOrientPriority = ASF_OrientPriority.ASF_OP_0_HIGHER_EXT;
            //Image模式下检测脸部的角度优先值
            int imageDetectFaceOrientPriority = ASF_OrientPriority.ASF_OP_0_ONLY;
            //人脸在图片中所占比例，如果需要调整检测人脸尺寸请修改此值，有效数值为2-32
            int detectFaceScaleVal = 16;
            //最大需要检测的人脸个数
            int detectFaceMaxNum = 5;
            //引擎初始化时需要初始化的检测功能组合
            int combinedMask = FaceEngineMask.ASF_FACE_DETECT | FaceEngineMask.ASF_FACERECOGNITION | FaceEngineMask.ASF_AGE | FaceEngineMask.ASF_GENDER | FaceEngineMask.ASF_FACE3DANGLE;
            //初始化引擎，正常值为0，其他返回值请参考http://ai.arcsoft.com.cn/bbs/forum.php?mod=viewthread&tid=19&_dsign=dbad527e
            retCode = ASIDCardFunctions.ASFInitEngine(detectMode, imageDetectFaceOrientPriority, detectFaceScaleVal, detectFaceMaxNum, combinedMask, ref pImageEngine);
            Console.WriteLine("InitEngine Result:" + retCode);
            MessageBox.Show(((retCode == 0) ? "引擎初始化成功!\n" : string.Format("引擎初始化失败!错误码为:{0}\n", retCode)));
            if (retCode != 0)
            {
                //禁用相关功能按钮
                //ControlsEnable(false, chooseMultiImgBtn, matchBtn, btnClearFaceList, chooseImgBtn);
            }

            //初始化视频模式下人脸检测引擎
            uint detectModeVideo = DetectionMode.ASF_DETECT_MODE_VIDEO;
            int combinedMaskVideo = FaceEngineMask.ASF_FACE_DETECT | FaceEngineMask.ASF_FACERECOGNITION;
            retCode = ASIDCardFunctions.ASFInitEngine(detectModeVideo, videoDetectFaceOrientPriority, detectFaceScaleVal, detectFaceMaxNum, combinedMaskVideo, ref pVideoEngine);
            //RGB视频专用FR引擎
            detectFaceMaxNum = 1;
            combinedMask = FaceEngineMask.ASF_FACE_DETECT | FaceEngineMask.ASF_FACERECOGNITION | FaceEngineMask.ASF_LIVENESS;
            retCode = ASIDCardFunctions.ASFInitEngine(detectMode, imageDetectFaceOrientPriority, detectFaceScaleVal, detectFaceMaxNum, combinedMask, ref pVideoRGBImageEngine);

            //IR视频专用FR引擎
            combinedMask = FaceEngineMask.ASF_FACE_DETECT | FaceEngineMask.ASF_FACERECOGNITION | FaceEngineMask.ASF_IR_LIVENESS;
            retCode = ASIDCardFunctions.ASFInitEngine(detectMode, imageDetectFaceOrientPriority, detectFaceScaleVal, detectFaceMaxNum, combinedMask, ref pVideoIRImageEngine);

            Console.WriteLine("InitVideoEngine Result:" + retCode);

        }


        private void Test(object sender, System.Timers.ElapsedEventArgs e)
        {
            timer.Stop();
            Console.WriteLine("paizhao");

            string minPhotoPath = Utility.GetPath("Photo") + "Photo.jpg";
            Console.WriteLine(minPhotoPath);

            //抓拍照片
            string minPhotoNewPath = CameraHelper.CaptureImage(minPhotoPath);
            IDCardInfo iDCardInfo = vm.IDCardData.GetIDCardInfo();

            Huoti(iDCardInfo.ImgPath, minPhotoNewPath);

            CompareTest(iDCardInfo.ImgPath, minPhotoNewPath);

        }
        private bool isRGBLock = false;
   
        private MRECT allRect = new MRECT();
        private object rectLock = new object();

        private void Huoti(string sfzImg, string paiZhaoImg)
        {
            MessageBox.Show(paiZhaoImg);
     
            Bitmap bitmap = new Bitmap(paiZhaoImg);
      
            ASF_MultiFaceInfo multiFaceInfo = IDCardUtil.DetectFace(pVideoEngine, bitmap);
        
            ASF_SingleFaceInfo2 maxFace = IDCardUtil.GetMaxFace(multiFaceInfo);
       
            MRECT rect = maxFace.faceRect;
           
            if (isRGBLock == false)
            {
                isRGBLock = true;

                ThreadPool.QueueUserWorkItem(new WaitCallback(delegate
                {
                    if (rect.left != 0 && rect.right != 0 && rect.top != 0 && rect.bottom != 0)
                    {
                        try
                        {
                                bool isLiveness = false;

                                
                                ImageInfo imageInfo = IDCardUtil.ReadBMP(bitmap);
                            if (imageInfo == null)
                            {
                                return;
                            }
                            int retCode_Liveness = -1;
                                //RGB活体检测
                                ASF_LivenessInfo liveInfo = IDCardUtil.LivenessInfo_RGB(pVideoRGBImageEngine, imageInfo, multiFaceInfo, out retCode_Liveness);
                                
                                if (retCode_Liveness == 0 && liveInfo.num > 0)
                            {
                                int isLive = MemoryUtil.PtrToStructure<int>(liveInfo.isLive);
                                isLiveness = (isLive == 1) ? true : false;
                            }
                            if (imageInfo != null)
                            {
                                MemoryUtil.Free(imageInfo.imgData);
                            }
                            if (isLiveness)
                            {
                                    CompareTest(sfzImg, paiZhaoImg);
                            }
                            else
                            {
                                    //显示消息
                                    MessageBox.Show("假体!");
                                    timer.Start();

                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        finally
                        {
                            if (bitmap != null)
                            {
                                bitmap.Dispose();
                            }
                            isRGBLock = false;
                        }
                    }
                    else
                    {
                        lock (rectLock)
                        {
                            allRect.left = 0;
                            allRect.top = 0;
                            allRect.right = 0;
                            allRect.bottom = 0;
                        }
                    }
                    isRGBLock = false;
                }));
            }

        }


        private void CompareTest(string sfzImg, string paiZhaoImg)
        {

            Bitmap Bitmap = new Bitmap(paiZhaoImg);

            int res = IDCardUtil.IdCardDataFeatureExtraction(pEngine, Image.FromFile(sfzImg));
            if (res == 0)
            {
                AFIC_FSDK_FACERES faceInfo = new AFIC_FSDK_FACERES();
                int result2 = IDCardUtil.FaceDataFeatureExtraction(pEngine, false, Bitmap, ref faceInfo);
                if (result2 == 0 && faceInfo.nFace > 0)
                {
                    float pSimilarScore = 0;
                    int pResult = 0;
                    float threshold = 0.82f;
                    int result3 = IDCardUtil.FaceIdCardCompare(ref pSimilarScore, ref pResult, pEngine, threshold);
                    if (result3 == 0)
                    {
                        Console.WriteLine("相似度:" + pSimilarScore);
                        if (pSimilarScore >= GetFaceThreshold())
                        {
                            timer.Close();
                            MessageBox.Show("活体，相似度" + pSimilarScore + ",人脸验证通过！");
                            App.Current.Dispatcher.Invoke((Action)(() =>
                            {
                                this.Close();
                                CameraHelper.CloseDevice();
                                Console.WriteLine("关闭摄像头");
                                int retCode = ASIDCardFunctions.ArcSoft_FIC_UninitialEngine(pEngine);
                                Console.WriteLine("UninitEngine Result:" + retCode);
                                Window_Closed();

                            }));
                        }
                        else
                        {
                            MessageBox.Show("相似度" + pSimilarScore + ",人脸验证未通过！");
                            timer.Start();
                        }
                    }
                    else
                    {
                        MessageBox.Show("识别失败，请重试！");
                        timer.Start();
                    }

                }
                else
                {
                    MessageBox.Show("未检测到人脸，请重试！");
                    timer.Start();

                }
            }
            else
            {
                MessageBox.Show("1" + Convert.ToString(res));
            }

        }
        private void Window_Closed()
        {
            try
            {

                //销毁引擎
                int retCode = ASIDCardFunctions.ASFUninitEngine(pImageEngine);
                Console.WriteLine("UninitEngine pImageEngine Result:" + retCode);
                //销毁引擎
                retCode = ASIDCardFunctions.ASFUninitEngine(pVideoEngine);
                Console.WriteLine("UninitEngine pVideoEngine Result:" + retCode);

                //销毁引擎
                retCode = ASIDCardFunctions.ASFUninitEngine(pVideoRGBImageEngine);
                Console.WriteLine("UninitEngine pVideoImageEngine Result:" + retCode);

                //销毁引擎
                retCode = ASIDCardFunctions.ASFUninitEngine(pVideoIRImageEngine);
                Console.WriteLine("UninitEngine pVideoIRImageEngine Result:" + retCode);
            }
            catch (Exception ex)
            {
                Console.WriteLine("UninitEngine pImageEngine Error:" + ex.Message);
            }
        }

        private void ImgCompare(string sfzImg, string paiZhaoImg)
        {
            if (Read_IdCardInfo.isFaceLode && Read_IdCardInfo._faceThread != null)
            {
                try
                {
                    QSNetFaceEngine.FaceModel[] faces2 = new QSNetFaceEngine.FaceModel[1];
                    int num2 = Read_IdCardInfo._faceThread.qsFaceEngine.detectFaces(paiZhaoImg, faces2, 1);
                    if (num2 < 1)
                    {
                        Console.WriteLine("没有检测到人脸");
                        timer.Start();
                    }

                    QSNetFaceEngine.FaceModel[] faces1 = new QSNetFaceEngine.FaceModel[1];
                    int num1 = Read_IdCardInfo._faceThread.qsFaceEngine.detectFaces(sfzImg, faces1, 1);
                    if (num1 < 1)
                    {
                        Console.WriteLine("没有检测到人脸");
                        timer.Start();

                    }

                    float score = Read_IdCardInfo._faceThread.qsFaceEngine.compare2Feature(faces1[0].feature, faces2[0].feature);

                    Console.WriteLine("相似度:" + score);
                    if (score >= GetFaceThreshold())
                    {
                        timer.Close();
                        MessageBox.Show("相似度" + score + ",人脸验证通过！");
                        App.Current.Dispatcher.Invoke((Action)(() =>
                        {
                            this.Close();
                            CameraHelper.CloseDevice();
                            Console.WriteLine("关闭摄像头");

                        }));
                    }
                    else
                    {
                        MessageBox.Show("相似度" + score + ",人脸验证未通过！");
                        timer.Start();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("人脸识别出错" + ex.Message);
                    timer.Start();
                }
            }
        }

        private void WindowsFormsHost_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you really want to exit?", "", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
        }

        public float GetFaceThreshold()
        {
            return float.Parse(App.YuZhi.ToString());
        }




    }
}
