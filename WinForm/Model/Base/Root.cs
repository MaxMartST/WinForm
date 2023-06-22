using System.Collections.Generic;
using WinForm.Model.EtalonsModel;
using WinForm.Model.RequestModel;

namespace WinForm.Model.Base
{
    public class Root
    {
        public Result Result { get; set; }
        public List<ColumnsItem> columns { get; set; }
        public List<string> sort { get; set; }
        public int limit { get; set; }
        public int offset { get; set; }
        public bool numberOfAllRecords { get; set; }
        public ResponseHeader responseHeader { get; set; }
        public Response response { get; set; }
    }
}
