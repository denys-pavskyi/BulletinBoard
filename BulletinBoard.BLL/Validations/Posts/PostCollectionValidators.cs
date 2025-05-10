using BulletinBoard.BLL.Models.Requests;
using FluentValidation;

namespace BulletinBoard.BLL.Validations.Posts;

public class PostCollectionValidators : IPostCollectionValidators
{
    public IValidator<CreateNewPostRequest> CreateNewPostRequestValidator { get; }
    public IValidator<UpdatePostRequest> UpdatePostRequestValidator { get; }

    public PostCollectionValidators()
    {
        CreateNewPostRequestValidator = new CreateNewPostRequestValidator();
        UpdatePostRequestValidator = new UpdatePostRequestValidator();
    }
}