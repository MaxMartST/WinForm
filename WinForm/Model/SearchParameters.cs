using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinForm.Model
{
    public class SearchParameters
    {
        public string? RegistrationNumber { get; private set; }
        public int? YearVerification { get; private set; }
        public string? RankCode { get; private set; }

        public SearchParameters(string? registrationNumber, int? yearVerification, string? rankCode)
        { 
            RegistrationNumber = registrationNumber;
            YearVerification = yearVerification;
            RankCode = rankCode;
        }
    }
}
