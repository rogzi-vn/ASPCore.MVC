using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPCoreMVC.Web.Models
{
    public class SmallAppUser
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public string Picture { get; set; }
        public string ConnectionId { get; set; }
    }
}
