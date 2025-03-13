namespace Aplication.Interfaces
{
    public interface IApiService
    {
        Task<string> PostDataAsync(string endpoint, string jsonContent);
    }
}

