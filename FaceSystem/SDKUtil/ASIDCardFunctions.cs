using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FaceSystem.SDKUtil
{
    /// <summary>
    /// SDK中与人证相关函数封装类
    /// </summary>
    class ASIDCardFunctions
    {
        /// <summary>
        /// SDK动态链接库路径
        /// </summary>
        public const string Dll_PATH = "libarcsoft_idcardveri.dll";

        public const string Dll_PATH2 = "libarcsoft_face_engine.dll";

        /// <summary>
        /// 激活人证SDK引擎函数
        /// </summary>
        /// <param name="appId">SDK对应的AppID</param>
        /// <param name="sdkKey">SDK对应的SDKKey</param>
        /// <returns>调用结果</returns>
        [DllImport(Dll_PATH, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ArcSoft_FIC_Activate(string appId, string sdkKey);

        /// <summary>
        /// 初始化引擎
        /// </summary>
        /// <param name="pEngine">初始化返回的引擎handle</param>
        /// <returns>调用结果</returns>
        [DllImport(Dll_PATH, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ArcSoft_FIC_InitialEngine(ref IntPtr pEngine);

        /// <summary>
        /// 人脸特征提取 0-静态图片 1-视频 
        /// </summary>
        /// <param name="pEngine">初始化返回的引擎handle</param>
        /// <param name="isVideo"> 人脸数据类型 1-视频 0-静态图片</param>
        /// <param name="pInputFaceData">人脸图像原始数据</param>
        /// <param name="pFaceRes">人脸属性 人脸数/人脸框/角度</param>
        /// <returns>调用结果</returns>
        [DllImport(Dll_PATH, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ArcSoft_FIC_FaceDataFeatureExtraction(IntPtr pEngine, bool isVideo, IntPtr pInputFaceData, IntPtr pFaceRes);

        /// <summary>
        /// 证件照特征提取
        /// </summary>
        /// <param name="pEngine">初始化返回的引擎handle</param>
        /// <param name="pInputFaceData">图像原始数据</param>
        /// <returns>调用结果</returns>
        [DllImport(Dll_PATH, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ArcSoft_FIC_IdCardDataFeatureExtraction(IntPtr pEngine, IntPtr pInputFaceData);

        /// <summary>
        /// 人证比对
        /// </summary>
        /// <param name="pEngine">初始化返回的引擎handle</param>
        /// <param name="threshold">比对阈值</param>
        /// <param name="pSimilarScore">比对结果相似度</param>
        /// <param name="pResult">比对结果</param>
        /// <returns>调用结果</returns>
        [DllImport(Dll_PATH, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ArcSoft_FIC_FaceIdCardCompare(IntPtr pEngine, float threshold, ref float pSimilarScore,ref int pResult);

        /// <summary>
        /// 释放引擎
        /// </summary>
        /// <param name="pResult">引擎Handle</param>
        /// <returns>调用结果</returns>
        [DllImport(Dll_PATH, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ArcSoft_FIC_UninitialEngine(IntPtr pEngine);


        /// <summary>
        /// 激活人脸识别SDK引擎函数
        /// </summary>
        /// <param name="appId">SDK对应的AppID</param>
        /// <param name="sdkKey">SDK对应的SDKKey</param>
        /// <returns>调用结果</returns>
        [DllImport(Dll_PATH2, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ASFActivation(string appId, string sdkKey);

        /// <summary>
        /// 初始化引擎
        /// </summary>
        /// <param name="detectMode">AF_DETECT_MODE_VIDEO 视频模式 | AF_DETECT_MODE_IMAGE 图片模式</param>
        /// <param name="detectFaceOrientPriority">检测脸部的角度优先值，推荐：ASF_OrientPriority.ASF_OP_0_HIGHER_EXT</param>
        /// <param name="detectFaceScaleVal">用于数值化表示的最小人脸尺寸</param>
        /// <param name="detectFaceMaxNum">最大需要检测的人脸个数</param>
        /// <param name="combinedMask">用户选择需要检测的功能组合，可单个或多个</param>
        /// <param name="pEngine">初始化返回的引擎handle</param>
        /// <returns>调用结果</returns>
        [DllImport(Dll_PATH2, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ASFInitEngine(uint detectMode, int detectFaceOrientPriority, int detectFaceScaleVal, int detectFaceMaxNum, int combinedMask, ref IntPtr pEngine);

        /// <summary>
        /// 人脸检测
        /// </summary>
        /// <param name="pEngine">引擎handle</param>
        /// <param name="width">图像宽度</param>
        /// <param name="height">图像高度</param>
        /// <param name="format">图像颜色空间</param>
        /// <param name="imgData">图像数据</param>
        /// <param name="detectedFaces">人脸检测结果</param>
        /// <returns>调用结果</returns>
        [DllImport(Dll_PATH2, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ASFDetectFaces(IntPtr pEngine, int width, int height, int format, IntPtr imgData, IntPtr detectedFaces);

        /// <summary>
        /// 单人脸特征提取
        /// </summary>
        /// <param name="pEngine">引擎handle</param>
        /// <param name="width">图像宽度</param>
        /// <param name="height">图像高度</param>
        /// <param name="format">图像颜色空间</param>
        /// <param name="imgData">图像数据</param>
        /// <param name="faceInfo">单张人脸位置和角度信息</param>
        /// <param name="faceFeature">人脸特征</param>
        /// <returns>调用结果</returns>
        [DllImport(Dll_PATH2, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ASFFaceFeatureExtract(IntPtr pEngine, int width, int height, int format, IntPtr imgData, IntPtr faceInfo, IntPtr faceFeature);

        /// <summary>
        /// 获取RGB活体结果
        /// </summary>
        /// <param name="hEngine">引擎handle</param>
        /// <param name="livenessInfo">活体检测信息</param>
        /// <returns>调用结果</returns>
        [DllImport(Dll_PATH2, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ASFGetLivenessScore(IntPtr hEngine, IntPtr livenessInfo);

        /// <summary>
        /// 人脸信息检测（年龄/性别/人脸3D角度）
        /// </summary>
        /// <param name="pEngine">引擎handle</param>
        /// <param name="width">图像宽度</param>
        /// <param name="height">图像高度</param>
        /// <param name="format">图像颜色空间</param>
        /// <param name="imgData">图像数据</param>
        /// <param name="detectedFaces">人脸信息，用户根据待检测的功能裁减选择需要使用的人脸</param>
        /// <param name="combinedMask">只支持初始化时候指定需要检测的功能，在process时进一步在这个已经指定的功能集中继续筛选例如初始化的时候指定检测年龄和性别， 在process的时候可以只检测年龄， 但是不能检测除年龄和性别之外的功能</param>
        /// <returns>调用结果</returns>
        [DllImport(Dll_PATH2, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ASFProcess(IntPtr pEngine, int width, int height, int format, IntPtr imgData, IntPtr detectedFaces, int combinedMask);


        /// <summary>
        /// 销毁引擎
        /// </summary>
        /// <param name="pEngine">引擎handle</param>
        /// <returns>调用结果</returns>
        [DllImport(Dll_PATH2, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ASFUninitEngine(IntPtr pEngine);
    }
}
