using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceSystem.SDKModels
{
    class ASF_MultiFaceInfo
    {
        /// <summary>
        /// 人脸Rect结果集
        /// </summary>
        public IntPtr faceRects;

        /// <summary>
        /// 人脸角度结果集，与faceRects一一对应  对应ASF_OrientCode
        /// </summary>
        public IntPtr faceOrients;
        /// <summary>
        /// 结果集大小
        /// </summary>
        public int faceNum;
        /// <summary>
        /// face ID，IMAGE模式下不返回FaceID
        /// </summary>
        public IntPtr faceID;
    }
}
