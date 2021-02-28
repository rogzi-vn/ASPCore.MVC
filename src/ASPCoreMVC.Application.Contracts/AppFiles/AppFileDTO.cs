using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.AppFiles
{
    public class AppFileDTO : EntityDto<Guid>
    {
        public Guid? ParentId { get; set; }
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
        [Required]
        public string Path { get; set; }
        public bool IsDirectory { get; set; } = false;
        [Required]
        public double Length { get; set; }
       
    }
}
