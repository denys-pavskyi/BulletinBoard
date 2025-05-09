using BulletinBoard.BLL.Models.Requests;
using FluentValidation;

namespace BulletinBoard.BLL.Validations.Announcements;

public interface IAnnouncementCollectionValidators
{
    IValidator<CreateNewAnnouncementRequest> CreateNewAnnouncementRequestValidator { get; }
    IValidator<UpdateAnnouncementRequest> UpdateAnnouncementRequestValidator { get; }
}