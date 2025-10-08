using BulletinBoard.BLL.Models.Requests;
using FluentValidation;

namespace BulletinBoard.BLL.Validations.Posts;

public interface IPostCollectionValidators
{
    IValidator<CreateNewPostRequest> CreateNewPostRequestValidator { get; }
    IValidator<UpdatePostRequest> UpdatePostRequestValidator { get; }
}