using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinForm.Model.RegistryElementModel
{
    public class VriInfo
    {
        public string organization { get; set; }
        public string signCipher { get; set; }
        public string miOwner { get; set; }
        public string vrfDate { get; set; }
        public string validDate { get; set; }
        public string vriType { get; set; }
        public string docTitle { get; set; }
        public Applicable applicable { get; set; }
    }
}
