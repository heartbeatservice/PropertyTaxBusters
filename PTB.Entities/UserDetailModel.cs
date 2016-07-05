using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PTB.Entities.Utility;

namespace PTB.Entities
{
    public class UserDetailModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int FailedLoginAttempts { get; set; }
        public bool IsActive { get; set; }

        public string FullName
        {
            get
            {
                return string.Format("{0} {1}", FirstName, LastName);
            }
        }
        public UserDetailModel()
        {
        }
        public UserDetailModel(IDataReader dbReader)
        {
            if (dbReader.HasColumn("UserId") && dbReader["UserId"] != DBNull.Value) UserId = (int)dbReader["UserId"];
            if (dbReader.HasColumn("UserName") && dbReader["UserName"] != DBNull.Value) UserName = (string)dbReader["UserName"];
            if (dbReader.HasColumn("Password") && dbReader["Password"] != DBNull.Value) Password = (string)dbReader["Password"];
            if (dbReader.HasColumn("PasswordSalt") && dbReader["PasswordSalt"] != DBNull.Value) PasswordSalt = (string)dbReader["PasswordSalt"];
            if (dbReader.HasColumn("FirstName") && dbReader["FirstName"] != DBNull.Value) FirstName = (string)dbReader["FirstName"];
            if (dbReader.HasColumn("LastName") && dbReader["LastName"] != DBNull.Value) LastName = (string)dbReader["LastName"];
            if (dbReader.HasColumn("Email") && dbReader["Email"] != DBNull.Value) Email = (string)dbReader["Email"];
            if (dbReader.HasColumn("Phone") && dbReader["Phone"] != DBNull.Value) Phone = (string)dbReader["Phone"];
            if (dbReader.HasColumn("PasswordFailuresSinceLastSuccess") && dbReader["PasswordFailuresSinceLastSuccess"] != DBNull.Value) FailedLoginAttempts = (int)dbReader["PasswordFailuresSinceLastSuccess"];
            if (dbReader.HasColumn("IsActive") && dbReader["IsActive"] != DBNull.Value) IsActive = (bool)dbReader["IsActive"];
            //Customer = new Customer(dbReader);
            //PrimaryModel = new Model(dbReader);
        }
    }
}
