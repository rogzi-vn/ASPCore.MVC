using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPCoreMVC.Models.TreeJs
{
    public class TreeJs
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public List<TreeJs> Children { get; set; }

        public TreeJs(string id, string text)
        {
            Id = id;
            Text = text;
            Children = new List<TreeJs>();
        }
    }
}
