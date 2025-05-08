using AutoMapper;
using BulletinBoard.BLL.Interfaces;
using BulletinBoard.BLL.Models.DtoModels;
using BulletinBoard.DAL.Repositories.Interfaces;

namespace BulletinBoard.BLL.Services;

public class AnnouncementService: IAnnouncementService
{
    private readonly IAnnouncementRepository _announcementRepository;
    private readonly IMapper _mapper;

    public AnnouncementService(IAnnouncementRepository announcementRepository, IMapper mapper)
    {
        _announcementRepository = announcementRepository;
        _mapper = mapper;
    }

    public async Task<List<AnnouncementDto>> GetAllAsync()
    {
        var announcements = await _announcementRepository.GetAllAsync();

        return _mapper.Map<List<AnnouncementDto>>(announcements);
    }



}