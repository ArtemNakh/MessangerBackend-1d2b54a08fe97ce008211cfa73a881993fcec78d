using MessangerBackend.Core.Interfaces;
using MessangerBackend.Core.Models;

namespace MessangerBackend.Core.Services;

public class UserService : IUserService
{
    private readonly IRepository _repository;

    public UserService(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<User> Login(string nickname, string password)
    {
        if (nickname == null || string.IsNullOrEmpty(nickname.Trim()) ||
            password == null || string.IsNullOrEmpty(password.Trim()))
        {
            throw new ArgumentNullException();
        }

        return (await _repository.GetQuery<User>(u => u.Nickname == nickname && u.Password == password)).SingleOrDefault();
     }

    public Task<User> Register(string nickname, string password)
    {
        var User = _repository.GetQuery<User>(u => u.Nickname == nickname && u.Password==password);
        if (User != null)
            throw new ArgumentNullException();

        User newUser = new User()
        {
            Nickname = nickname,
            Password = password,
            CreatedAt = DateTime.Now,
            LastSeenOnline = DateTime.Now
        };


        return _repository.Add(newUser);
    }

    public Task<User> GetUserById(int id)
    {
        return _repository.GetById<User>(id);
    }

    public IEnumerable<User> GetUsers(int page, int size)
    {
        if (page <= 0 || size <= 0)
            throw new ArgumentNullException();

        return (_repository.GetQuery<User>(u => true).Result).Skip((page - 1) * size).Take(size).ToList();
    }


    public IEnumerable<User> SearchUsers(string nickname)
    {
        return (IEnumerable<User>)_repository.GetQuery<User>(u => u.Nickname == nickname);
    }
}