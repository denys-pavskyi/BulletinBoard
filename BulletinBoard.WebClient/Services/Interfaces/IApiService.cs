using BulletinBoard.WebClient.Models.Other;

namespace BulletinBoard.WebClient.Services.Interfaces;

public interface IApiService
{
    Task<Result<T>> HandleApiResponse<T>(HttpResponseMessage response);
}