using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FaceSystem.Tool
{
    public class ScoreMaster
    {
        
        public List<ScoreDetail> ScoreDetails
        {
            get
            {
                return m_scoreDetails;
            }  
        }

        public List<ScoreDetail> m_scoreDetails = new List<ScoreDetail>();

    }

    public class ScoreDetail
    {
        public string Xh { get; set; }
        public string XQ { get; set; }
        public string ClassName { get; set; }
        public string ClassType { get; set; }
        public string Period { get; set; }
        public string Credit { get; set; }
        public string Score { get; set; }

    }
}
