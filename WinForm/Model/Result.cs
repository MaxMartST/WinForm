using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinForm.Model
{
    internal class Result
    {
        public int Count { get; set; }
        public int Start { get; set; }
        public int Rows { get; set; }
        public List<ItemsItem> Items { get; set; }
    }
}
