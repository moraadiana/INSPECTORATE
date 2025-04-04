using INSPECTORATEJobs.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace INSPECTORATEJobs.Models
{
    public class Attachment
    {
        public HttpPostedFileBase AttachmentFile { get; set; }
        public string ApplicantAttachment { get; set; }
        public List<ApplicantAttachment> ApplicantAttachments { get; set; }

        
    }
}