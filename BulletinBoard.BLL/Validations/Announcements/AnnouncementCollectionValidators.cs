using BulletinBoard.BLL.Models.Requests;
using FluentValidation;

namespace BulletinBoard.BLL.Validations.Announcements;

public class AnnouncementCollectionValidators: IAnnouncementCollectionValidators
{
    public IValidator<CreateNewAnnouncementRequest> CreateNewAnnouncementRequestValidator { get; }
    public IValidator<UpdateAnnouncementRequest> UpdateAnnouncementRequestValidator { get; }

    public AnnouncementCollectionValidators()
    {
        CreateNewAnnouncementRequestValidator = new CreateNewAnnouncementRequestValidator();
        UpdateAnnouncementRequestValidator = new UpdateAnnouncementRequestValidator();
    }
}