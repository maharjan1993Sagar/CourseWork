using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace coursework02.ViewModels
{
    public class MemberLoanDetails
    {
        public string MemberName { get; set; }
        public int TotalLoan  { get; set; }
        public string Status { get; set; }
        public int MaxLoan { get; set; }
    }
}