using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceSystem.SDKModels
{
    class ASF_FaceFeature
    {
        /// <summary>
        /// 特征值 byte[]
        /// </summary>
        public IntPtr feature;

        /// <summary>
        /// 结果集大小
        /// </summary>
        public int featureSize;
    }
}
