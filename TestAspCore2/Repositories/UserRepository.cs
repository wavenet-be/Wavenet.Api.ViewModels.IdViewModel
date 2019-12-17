namespace TestAspCore2.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using TestAspCore2.Models;

    public class UserRepository
    {
        public User Get(int id)
            => this.GetAll().FirstOrDefault(u => u.Id == id);

        public IEnumerable<User> GetAll()
            => Enumerable.Range(1, 10).Select(i => new User { Id = i, Name = $"User {i}" });
    }
}