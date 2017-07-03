namespace TataApp.Models
{
    using SQLite.Net.Attributes;
    using System;
    using Xamarin.Forms;
    public class Employee
    {
        [PrimaryKey]
        public int EmployeeId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int EmployeeCode { get; set; }

        public int DocumentTypeId { get; set; }

        public int LoginTypeId { get; set; }

        public string Document { get; set; }

        public string Picture { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public byte[] ImageArray { get; internal set; }

        public string AccessToken { get; set; }

        public string TokenType { get; set; }

        public DateTime TokenExpires { get; set; }

        public string Password { get; set; }

        public bool IsRemembered { get; set; }

        public string FullName
        {
            get { return string.Format("{0} {1}", FirstName, LastName); }
        }

        public string FullPicture
        {
            get
            {
                if (string.IsNullOrEmpty(Picture))
                {
                    return "avatar_user.png";
                }

                if (LoginTypeId == 1)
                {
                    var urlAPI = Application.Current.Resources["URLAPI"].ToString();
                    return string.Format("{0}/{1}", urlAPI, Picture.Substring(1));
                }

                return Picture;
            }
        }

        public override int GetHashCode()
        {
            return EmployeeId;
        }
    }
}
