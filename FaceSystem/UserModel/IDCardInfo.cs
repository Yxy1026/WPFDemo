using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FaceSystem.UserModel
{
    public class IDCardInfo
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; }

        /// <summary>
        /// 民族
        /// </summary>
        public string Nation { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        public string Birthday { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        public string Idcard { get; set; }

        /// <summary>
        /// 证件图片地址
        /// </summary>
        public string ImgPath { get; set; }

        /// <summary>
        /// 签发地址
        /// </summary>
        public string Sign { get; set; }

        /// <summary>
        /// 证件有效期限
        /// </summary>
        public string Effitivedate { get; set; }

    }
}

