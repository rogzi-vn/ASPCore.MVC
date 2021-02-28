using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Users;

namespace ASPCoreMVC.Helpers
{
    public static class CurrentUserHelper
    {
        public static string GetFullName(this ICurrentUser current, bool isDisplayNameSurname = true)
        {
            if (current.Name.IsNullOrEmpty() && current.SurName.IsNullOrEmpty())
            {
                // Nếu tên và họ đều không có, trả về tên đăng nhập
                return current.UserName;
            }
            else if (current.Name.IsNullOrEmpty())
            {
                // Nếu Tên không có thì trả về họ
                return current.SurName;
            }
            else if (current.SurName.IsNullOrEmpty())
            {
                // Nếu không có họ thì trả về tên
                return current.Name;
            }
            else
            {
                // Nếu có đủ họ và tên thì tiến hành trả về đầy đủ
                if (isDisplayNameSurname)
                {
                    // Nếu hiển thị tên trước học
                    return $"{current.Name.Trim()} {current.SurName.Trim()}".ToPascalCase();
                }
                else
                {
                    // Ngược lại thì hiển thị họ trước
                    return $"{current.SurName.Trim()} {current.Name.Trim()}".ToPascalCase();
                }
            }
        }
    }
}
