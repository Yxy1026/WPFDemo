using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceSystem.UserModel
{
    public class User
    {
        public string userid { get; set; }
        public string name { get; set; }
        public List<Department> department { get; set; }
        public string mobile { get; set; }
        public string gender { get; set; }
        public string email { get; set; }

    }
}
