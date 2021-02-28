using System;
using System.Collections.Generic;
using System.Text;

namespace ASPCoreMVC
{
    public static class AppContractConsts
    {
        public const long MaxFileSizeInGB = 1;
        public const long MaxFileSizeInMB = MaxFileSizeInGB * 1024;
        public const long MaxFileSizeInKB = MaxFileSizeInMB * 1024;
        public const long MaxFileSizeInBytes = MaxFileSizeInKB * 1024;
    }
}
