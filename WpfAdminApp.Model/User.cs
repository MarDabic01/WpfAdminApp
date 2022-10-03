using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WpfAdminApp.Model
{
    public class User : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        #region Fields

        private int _id;
        private string _userName;
        private string _password;
        private bool _isAdministrator;

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        private Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>();

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        #endregion

        #region Properties

        public int Id
        {
            get { return _id; }
            set
            {
                if (_id == value)
                {
                    return;
                }
                _id = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Id"));
            }
        }

        public string UserName
        {
            get { return _userName; }
            set
            {
                if (_userName == value)
                {
                    return;
                }
                _userName = value;

                List<string> errors = new List<string>();
                bool valid = true;

                if (value == null || value == "")
                {
                    errors.Add("First name can't be empty.");
                    SetErrors("UserName", errors);
                    valid = false;
                }


                if (!Regex.Match(value, @"^[a-zA-Z' ']+$").Success)
                {
                    errors.Add("Username can only contain letters.");
                    SetErrors("UserName", errors);
                    valid = false;
                }

                if (valid)
                {
                    ClearErrors("UserName");
                }

                OnPropertyChanged(new PropertyChangedEventArgs("UserName"));
            }
        }
        public string Password
        {
            get { return _password; }
            set
            {
                if (_password == value)
                {
                    return;
                }
                _password = value;

                List<string> errors = new List<string>();
                bool valid = true;

                if (value == null || value == "")
                {
                    errors.Add("Password can't be empty.");
                    SetErrors("Password", errors);
                    valid = false;
                }

                if (valid)
                {
                    ClearErrors("Password");
                }

                OnPropertyChanged(new PropertyChangedEventArgs("Password"));
            }
        }

        public bool IsAdministrator
        {
            get { return _isAdministrator; }
            set
            {
                _isAdministrator = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAdministrator"));
            }
        }

        #endregion

        #region Constructors

        public User(string userName, string password, bool isAdmin)
        {
            UserName = userName;
            Password = password;
            IsAdministrator = isAdmin;
        }

        public User(int id, string userName, string password, bool isAdmin)
        {
            UserName = userName;
            Password = password;
            IsAdministrator = isAdmin;
            Id = id;
        }

        public User()
        {
            UserName = "";
            Password = "";
            IsAdministrator = false;
        }

        #endregion
        public static User GetUserFromResultSet(SqlDataReader reader)
        {
            User user = new User((int)reader["id"], (string)reader["UserName"], (string)reader["UserPass"], (bool)reader["IsAdmin"]);
            return user;
        }

        public void UpdatePerson()
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnString"].ToString();
                conn.Open();

                SqlCommand command = new SqlCommand("UPDATE Users SET UserName=@UserName, UserPass=@Password, IsAdmin=@IsAdministrator WHERE id=@Id", conn);

                SqlParameter userNameParam = new SqlParameter("@UserName", SqlDbType.NVarChar);
                userNameParam.Value = this.UserName;

                SqlParameter userPassParam = new SqlParameter("@Password", SqlDbType.NVarChar);
                userPassParam.Value = this.Password;

                SqlParameter isAdministratorParam = new SqlParameter("@IsAdministrator", SqlDbType.Bit);
                isAdministratorParam.Value = this.IsAdministrator;

                SqlParameter myParam = new SqlParameter("@Id", SqlDbType.Int, 11);
                myParam.Value = this.Id;

                command.Parameters.Add(userNameParam);
                command.Parameters.Add(userPassParam);
                command.Parameters.Add(isAdministratorParam);
                command.Parameters.Add(myParam);

                int rows = command.ExecuteNonQuery();
            }
        }
        public void Insert()
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnString"].ToString();
                conn.Open();

                SqlCommand command = new SqlCommand("INSERT INTO Users(UserName, UserPass, IsAdmin) VALUES(@UserName, @Password, @IsAdministrator); SELECT IDENT_CURRENT('Users');", conn);

                SqlParameter userNameParam = new SqlParameter("@UserName", SqlDbType.NVarChar);
                userNameParam.Value = this.UserName;

                SqlParameter userPassParam = new SqlParameter("@Password", SqlDbType.NVarChar);
                userPassParam.Value = this.Password;

                SqlParameter isAdministratorParam = new SqlParameter("@IsAdministrator", SqlDbType.Bit);
                isAdministratorParam.Value = this.IsAdministrator;

                command.Parameters.Add(userNameParam);
                command.Parameters.Add(userPassParam);
                command.Parameters.Add(isAdministratorParam);

                var id = command.ExecuteScalar();

                if (id != null)
                {
                    this.Id = Convert.ToInt32(id);
                }
            }
        }
        public void Save()
        {
            if (Id == 0)
            {
                Insert();
            }
            else
            {
                UpdatePerson();
            }
        }
        public User Clone()
        {
            User clonedPerson = new User();
            clonedPerson.UserName = this.UserName;
            clonedPerson.Password = this.Password;
            clonedPerson.IsAdministrator = this.IsAdministrator;
            clonedPerson.Id = this.Id;

            return clonedPerson;
        }

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return (errors.Values);
            }
            else
            {
                if (errors.ContainsKey(propertyName))
                {
                    return (errors[propertyName]);
                }
                else
                {
                    return null;
                }
            }
        }
        public bool HasErrors
        {
            get
            {
                return (errors.Count > 0);
            }
        }
        private void SetErrors(string propertyName, List<string> propertyErrors)
        {
            errors.Remove(propertyName);
            errors.Add(propertyName, propertyErrors);
            if (ErrorsChanged != null)
                ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
        }

        private void ClearErrors(string propertyName)
        {
            errors.Remove(propertyName);
            if (ErrorsChanged != null)
                ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
        }

        // override object.Equals
        public override bool Equals(object obj)
        {

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            User objPerson = (User)obj;

            if (objPerson.Id == this.Id) return true;

            return false;
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
