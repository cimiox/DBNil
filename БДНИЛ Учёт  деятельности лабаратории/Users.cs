//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace БДНИЛ_Учёт__деятельности_лабаратории
{
    using System;
    using System.Collections.Generic;
    
    public partial class Users
    {
        public int IdUsers { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string UserPatronymic { get; set; }
        public Nullable<int> BanMessageId { get; set; }
        public string UserRoleId { get; set; }
    
        public virtual BanMsg BanMsg { get; set; }
        public virtual UserRoles UserRoles { get; set; }
    }
}
