using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GST_Billing.Models
{
    public class ComposeEmail
    {
        [Required (ErrorMessage ="From email id required.")]
        public string From { get; set; }
        [Required(ErrorMessage = "Reply to email id required.")]
        public string ReplyTo { get; set; }
        [Required(ErrorMessage = "To email id required.")]
        public string SendTo { get; set; }
        public string Cc { get; set; }
        [Required(ErrorMessage = "Subject required.")]
        public string Subject { get; set; }
        [Required(ErrorMessage = "Email body required.")]
        public string Body { get; set; }
        public bool Attachement { get; set; }
    }
}