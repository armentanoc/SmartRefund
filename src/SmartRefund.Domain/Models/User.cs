namespace SmartRefund.Domain.Models
{
    public class User
    {
        public User(string id, string userName, string password, string userType)
        {
            Id = id;
            UserName = userName;
            Password = password;
            UserType = userType;
        }

        public string Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }
    }
}
