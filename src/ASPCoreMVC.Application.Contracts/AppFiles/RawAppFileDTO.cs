using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ASPCoreMVC.AppFiles
{
    public class RawAppFileDTO
    {
        [Required]
        public byte[] Content { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
