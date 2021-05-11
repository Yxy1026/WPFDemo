using FaceSystem.SDKModels;
using FaceSystem.SDKUtil;
using FaceSystem.Utils;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace FaceSystem.Utils
{
    class IDCardUtil
    {
        private static object locks = new object();

        /// <summary>
        /// 人脸特征提取
        /// </summary>
        /// <param name="hFICEngine">FIC 引擎Handle</param>
        /// <param name="isVideo">人脸数据类型 1-视频 0-静态图片</param>
        /// <param name="bitmap">人脸图像原始数据</param>
        /// <returns>人脸检测结果</returns>
        public static int FaceDataFeatureExtraction(IntPtr hFICEngine, bool isVideo, Bitmap bitmap, ref AFIC_FSDK_FACERES faceRes)
        {
            lock (locks)
            {
                int result = -1;
                if (bitmap != null)
                {
                    ASVLOFFSCREEN offInput = ImageUtil.ReadBmp(bitmap);

                    IntPtr offInputPtr = MemoryUtil.Malloc(MemoryUtil.SizeOf<ASVLOFFSCREEN>());
                    MemoryUtil.StructureToPtr(offInput, offInputPtr);

                    IntPtr faceResPtr = MemoryUtil.Malloc(MemoryUtil.SizeOf<AFIC_FSDK_FACERES>());
                    result = ASIDCardFunctions.ArcSoft_FIC_FaceDataFeatureExtraction(hFICEngine, isVideo, offInputPtr, faceResPtr);
                    faceRes = MemoryUtil.PtrToStructure<AFIC_FSDK_FACERES>(faceResPtr);
                    MemoryUtil.Free(offInput.ppu8Plane[0]);
                    MemoryUtil.Free(offInputPtr);
                    MemoryUtil.Free(faceResPtr);
                }
                return result;
            }
        }

        /// <summary>
        /// 证件照特征提取
        /// </summary>
        /// <param name="hFICEngine">FIC 引擎Handle</param>
        /// <param name="isVideo">人脸数据类型 1-视频 0-静态图片</param>
        /// <param name="bitmap">人脸图像原始数据</param>
        /// <returns>人脸特征提取结果</returns>
        public static int IdCardDataFeatureExtraction(IntPtr hFICEngine, Image image)
        {
            lock (locks)
            {
                if (image.Width % 4 != 0)
                {
                    image = ImageUtil.ScaleImage(image, image.Width - (image.Width % 4), image.Height);
                }
                //Bitmap bitmap = new Bitmap(image);
                ASVLOFFSCREEN offInput = ImageUtil.ReadBmp(image);

                IntPtr offInputPtr = MemoryUtil.Malloc(MemoryUtil.SizeOf<ASVLOFFSCREEN>());
                MemoryUtil.StructureToPtr(offInput, offInputPtr);

                IntPtr faceResPtr = MemoryUtil.Malloc(MemoryUtil.SizeOf<AFIC_FSDK_FACERES>());
                int result = ASIDCardFunctions.ArcSoft_FIC_IdCardDataFeatureExtraction(hFICEngine, offInputPtr);
                MemoryUtil.Free(offInput.ppu8Plane[0]);
                MemoryUtil.Free(offInputPtr);
                MemoryUtil.Free(faceResPtr);
                return result;
            }
        }

        /// <summary>
        /// 人证比对
        /// </summary>
        /// <param name="pSimilarScore">FIC 引擎Handle</param>
        /// <param name="pResult">人脸数据类型 1-视频 0-静态图片</param>
        /// <param name="hFICEngine">引擎Handle</param>
        /// <param name="threshold">引擎Handle</param>
        /// <returns>人脸比对结果</returns>
        public static int FaceIdCardCompare(ref float pSimilarScore, ref int pResult, IntPtr hFICEngine, float threshold = 0.82f)
        {
            return ASIDCardFunctions.ArcSoft_FIC_FaceIdCardCompare(hFICEngine, threshold, ref pSimilarScore, ref pResult);
        }

        public static ASF_LivenessInfo LivenessInfo_RGB(IntPtr pEngine, ImageInfo imageInfo, ASF_MultiFaceInfo multiFaceInfo, out int retCode)
        {
            IntPtr pMultiFaceInfo = MemoryUtil.Malloc(MemoryUtil.SizeOf<ASF_MultiFaceInfo>());
            MemoryUtil.StructureToPtr(multiFaceInfo, pMultiFaceInfo);

            if (multiFaceInfo.faceNum == 0)
            {
                retCode = -1;
                //释放内存
                MemoryUtil.Free(pMultiFaceInfo);
                return new ASF_LivenessInfo();
            }

            try
            {
                //人脸信息处理
                retCode = ASIDCardFunctions.ASFProcess(pEngine, imageInfo.width, imageInfo.height, imageInfo.format, imageInfo.imgData, pMultiFaceInfo, FaceEngineMask.ASF_LIVENESS);
                if (retCode == 0)
                {
                    //获取活体检测结果
                    IntPtr pLivenessInfo = MemoryUtil.Malloc(MemoryUtil.SizeOf<ASF_LivenessInfo>());
                    retCode = ASIDCardFunctions.ASFGetLivenessScore(pEngine, pLivenessInfo);
                    Console.WriteLine("Get Liveness Result:" + retCode);
                    ASF_LivenessInfo livenessInfo = MemoryUtil.PtrToStructure<ASF_LivenessInfo>(pLivenessInfo);

                    //释放内存
                    MemoryUtil.Free(pMultiFaceInfo);
                    MemoryUtil.Free(pLivenessInfo);
                    return livenessInfo;
                }
                else
                {
                    //释放内存
                    MemoryUtil.Free(pMultiFaceInfo);
                    return new ASF_LivenessInfo();
                }
            }
            catch
            {
                retCode = -1;
                //释放内存
                MemoryUtil.Free(pMultiFaceInfo);
                return new ASF_LivenessInfo();
            }
        }
        public static ASF_MultiFaceInfo DetectFace(IntPtr pEngine, ImageInfo imageInfo)
        {
            ASF_MultiFaceInfo multiFaceInfo = new ASF_MultiFaceInfo();
            IntPtr pMultiFaceInfo = MemoryUtil.Malloc(MemoryUtil.SizeOf<ASF_MultiFaceInfo>());
            int retCode = ASIDCardFunctions.ASFDetectFaces(pEngine, imageInfo.width, imageInfo.height, imageInfo.format, imageInfo.imgData, pMultiFaceInfo);
            if (retCode != 0)
            {
                MemoryUtil.Free(pMultiFaceInfo);
                return multiFaceInfo;
            }
            multiFaceInfo = MemoryUtil.PtrToStructure<ASF_MultiFaceInfo>(pMultiFaceInfo);
            MemoryUtil.Free(pMultiFaceInfo);
            return multiFaceInfo;
        }

        public static ASF_MultiFaceInfo DetectFace(IntPtr pEngine, Image image)
        {
            lock (locks)
            {
                ASF_MultiFaceInfo multiFaceInfo = new ASF_MultiFaceInfo();
                if (image != null)
                {
                    if (image == null)
                    {
                        return multiFaceInfo;
                    }
                    ImageInfo imageInfo = ReadBMP(image);
                    if (imageInfo == null)
                    {
                        return multiFaceInfo;
                    }
                    multiFaceInfo = DetectFace(pEngine, imageInfo);
                    MemoryUtil.Free(imageInfo.imgData);
                    return multiFaceInfo;

                }
                else
                {
                    return multiFaceInfo;
                }
            }
        }

        public static ImageInfo ReadBMP(Image image)
        {
            ImageInfo imageInfo = new ImageInfo();

            //将Image转换为Format24bppRgb格式的BMP
            Bitmap bm = new Bitmap(image);
            BitmapData data = bm.LockBits(new Rectangle(0, 0, bm.Width, bm.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            try
            {
                //位图中第一个像素数据的地址。它也可以看成是位图中的第一个扫描行
                IntPtr ptr = data.Scan0;

                //定义数组长度
                int soureBitArrayLength = data.Height * Math.Abs(data.Stride);
                byte[] sourceBitArray = new byte[soureBitArrayLength];

                //将bitmap中的内容拷贝到ptr_bgr数组中
                MemoryUtil.Copy(ptr, sourceBitArray, 0, soureBitArrayLength);

                //填充引用对象字段值
                imageInfo.width = data.Width;
                imageInfo.height = data.Height;
                imageInfo.format = (int)ASF_ImagePixelFormat.ASVL_PAF_RGB24_B8G8R8;

                imageInfo.imgData = MemoryUtil.Malloc(sourceBitArray.Length);
                MemoryUtil.Copy(sourceBitArray, 0, imageInfo.imgData, sourceBitArray.Length);

                return imageInfo;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                bm.UnlockBits(data);
                bm.Dispose();
            }
        }

        public static ASF_SingleFaceInfo2 GetMaxFace(ASF_MultiFaceInfo multiFaceInfo)
        {
            ASF_SingleFaceInfo2 singleFaceInfo = new ASF_SingleFaceInfo2();
            singleFaceInfo.faceRect = new MRECT();
            singleFaceInfo.faceOrient = 1;

            int maxArea = 0;
            int index = -1;
            for (int i = 0; i < multiFaceInfo.faceNum; i++)
            {
                try
                {
                    MRECT rect = MemoryUtil.PtrToStructure<MRECT>(multiFaceInfo.faceRects + MemoryUtil.SizeOf<MRECT>() * i);
                    int area = (rect.right - rect.left) * (rect.bottom - rect.top);
                    if (maxArea <= area)
                    {
                        maxArea = area;
                        index = i;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            if (index != -1)
            {
                singleFaceInfo.faceRect = MemoryUtil.PtrToStructure<MRECT>(multiFaceInfo.faceRects + MemoryUtil.SizeOf<MRECT>() * index);
                singleFaceInfo.faceOrient = MemoryUtil.PtrToStructure<int>(multiFaceInfo.faceOrients + MemoryUtil.SizeOf<int>() * index);
            }
            return singleFaceInfo;
        }

        /// <summary>
        /// 获取多个人脸检测结果中面积最大的人脸
        /// </summary>
        /// <param name="multiFaceInfo">人脸检测结果</param>
        /// <returns>面积最大的人脸信息</returns>
        public static ASF_SingleFaceInfo GetMaxFace(AFIC_FSDK_FACERES multiFaceInfo)
        {
            ASF_SingleFaceInfo singleFaceInfo = new ASF_SingleFaceInfo();
            singleFaceInfo.faceRect = new RECT();
            singleFaceInfo.faceOrient = 1;

            int maxArea = 0;
            int index = -1;
            for (int i = 0; i < multiFaceInfo.nFace; i++)
            {
                RECT rect = multiFaceInfo.rcFace;
                int area = (rect.right - rect.left) * (rect.bottom - rect.top);
                if (maxArea <= area)
                {
                    maxArea = area;
                    index = i;
                }
            }
            if (index != -1)
            {
                singleFaceInfo.faceRect = multiFaceInfo.rcFace;
            }
            return singleFaceInfo;
        }

        /// <summary>
        /// 提取单人脸特征
        /// </summary>
        /// <param name="pEngine">人脸识别引擎</param>
        /// <param name="image">图片</param>
        /// <param name="singleFaceInfo">单人脸信息</param>
        /// <returns>单人脸特征</returns>
        public static IntPtr ExtractFeature(IntPtr pEngine, Image image, ASF_SingleFaceInfo2 singleFaceInfo)
        {
            ImageInfo imageInfo = ReadBMP(image);
            if (imageInfo == null)
            {
                ASF_FaceFeature emptyFeature = new ASF_FaceFeature();
                IntPtr pEmptyFeature = MemoryUtil.Malloc(MemoryUtil.SizeOf<ASF_FaceFeature>());
                MemoryUtil.StructureToPtr(emptyFeature, pEmptyFeature);
                return pEmptyFeature;
            }
            IntPtr pSingleFaceInfo = MemoryUtil.Malloc(MemoryUtil.SizeOf<ASF_SingleFaceInfo2>());
            MemoryUtil.StructureToPtr(singleFaceInfo, pSingleFaceInfo);

            IntPtr pFaceFeature = MemoryUtil.Malloc(MemoryUtil.SizeOf<ASF_FaceFeature>());
            int retCode = -1;
            try
            {
                retCode = ASIDCardFunctions.ASFFaceFeatureExtract(pEngine, imageInfo.width, imageInfo.height, imageInfo.format, imageInfo.imgData, pSingleFaceInfo, pFaceFeature);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.WriteLine("FR Extract Feature result:" + retCode);

            if (retCode != 0)
            {
                //释放指针
                MemoryUtil.Free(pSingleFaceInfo);
                MemoryUtil.Free(pFaceFeature);
                MemoryUtil.Free(imageInfo.imgData);

                ASF_FaceFeature emptyFeature = new ASF_FaceFeature();
                IntPtr pEmptyFeature = MemoryUtil.Malloc(MemoryUtil.SizeOf<ASF_FaceFeature>());
                MemoryUtil.StructureToPtr(emptyFeature, pEmptyFeature);
                return pEmptyFeature;
            }

            //人脸特征feature过滤
            ASF_FaceFeature faceFeature = MemoryUtil.PtrToStructure<ASF_FaceFeature>(pFaceFeature);
            byte[] feature = new byte[faceFeature.featureSize];
            MemoryUtil.Copy(faceFeature.feature, feature, 0, faceFeature.featureSize);

            ASF_FaceFeature localFeature = new ASF_FaceFeature();
            localFeature.feature = MemoryUtil.Malloc(feature.Length);
            MemoryUtil.Copy(feature, 0, localFeature.feature, feature.Length);
            localFeature.featureSize = feature.Length;
            IntPtr pLocalFeature = MemoryUtil.Malloc(MemoryUtil.SizeOf<ASF_FaceFeature>());
            MemoryUtil.StructureToPtr(localFeature, pLocalFeature);

            //释放指针
            MemoryUtil.Free(pSingleFaceInfo);
            MemoryUtil.Free(pFaceFeature);
            MemoryUtil.Free(imageInfo.imgData);

            return pLocalFeature;
        }
        /// <summary>
        /// 将身份证号字符串的年月日替换为*
        /// </summary>
        /// <param name="IDCard"></param>
        /// <returns></returns>
        public static string repleaseIDCard(string IDCard)
        {
            if (IDCard.Length == 15)
            {
                string date = IDCard.Substring(6, 6);
                return IDCard.Replace(date, "******");
            }
            else if (IDCard.Length == 18)
            {
                string date = IDCard.Substring(6, 8);
                return IDCard.Replace(date, "********");
            }
            else
            {
                return IDCard;
            }
        }
    } 
}

