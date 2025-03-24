using Diplom2.DTO;

namespace Diplom2
{
    public class VremUser
    {
        private readonly List<RegistUser> _tempUsers = new List<RegistUser>();

        public void Add(RegistUser user)
        {
            _tempUsers.Add(user);
        }

        public RegistUser GetByEmail(string email)
        {
            return _tempUsers.FirstOrDefault(u => u.EmailUser == email);
        }

        public void Remove(RegistUser user)
        {
            _tempUsers.Remove(user);
        }
    }
}
