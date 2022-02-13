using System;
namespace algotest.core.models
{
    public class UserModelDefault
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class UserModel : UserModelDefault
    {
        public int Id { get; set; }
        public string CreatedAt { get; set; }
    }
    public class UserAddModel : UserModelDefault { }

}
