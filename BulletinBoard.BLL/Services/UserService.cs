using AutoMapper;
using BulletinBoard.BLL.Interfaces;
using BulletinBoard.DAL.Repositories.Interfaces;

namespace BulletinBoard.BLL.Services;

public class UserService: IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }
}