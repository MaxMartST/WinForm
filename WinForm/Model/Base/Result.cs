using System.Collections.Generic;
using WinForm.Model.RegistryElementModel;
using WinForm.Model.VerificationResultModel;

namespace WinForm.Model.Base
{
    public class Result
    {
        public int Count { get; set; }
        public int Start { get; set; }
        public int Rows { get; set; }
        public List<ItemsItem> Items { get; set; }
        public MiInfo miInfo { get; set; }
        public VriInfo vriInfo { get; set; }
        public Means means { get; set; }
        public Info info { get; set; }
    }
}
