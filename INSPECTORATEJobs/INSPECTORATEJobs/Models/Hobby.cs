using INSPECTORATEJobs.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace INSPECTORATEJobs.Models
{
    public class Hobby
    {
        public string ApplicantHobby { get; set; }
        public List<ApplicantHobby> ApplicantHobbies { get; set; }
    }
}