using BulletinBoard.BLL.Models.Requests;
using FluentValidation;

namespace BulletinBoard.BLL.Validations.Announcements;

public class CreateNewAnnouncementRequestValidator : AbstractValidator<CreateNewAnnouncementRequest>
{
    public CreateNewAnnouncementRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MinimumLength(3).WithMessage("Title must be at least 3 characters long.")
            .MaximumLength(150).WithMessage("Title cannot exceed 150 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");

        RuleFor(x => x.SubcategoryId)
            .GreaterThan(0).WithMessage("SubcategoryId must be greater than 0.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");
    }

}