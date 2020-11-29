using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Common.Enums
{
    public enum UserReturnTypes
    {
        [Description("Giriş işlemi başarılı.")]
        Success,
        [Description("Kullanıcı bulunamadı.")]
        Err_NotValidUser
    }
}
