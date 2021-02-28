using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace ASPCoreMVC.TCUEnglish.AppFiles
{
    public class AppFile : AuditedAggregateRoot<Guid>
    {
        public string Name { get; set; }
        public Guid? ParentId { get; set; }
        public string Path { get; protected set; }
        public bool IsDirectory { get; set; } = false;
        public double Length { get; set; }

        public string TruePath()
        {
            return Path
                .Replace(@"\", "/")
                .Replace("//", "/")
                .TrimEnd('/')
                .Trim();
        }
        public AppFile SetId(Guid id)
        {
            Id = id;
            return this;
        }
        public AppFile SetPath(string path)
        {
            Path = path;
            return this;
        }
        public AppFile SetId(string id)
        {
            return SetId(Guid.Parse(id));
        }
    }
}
