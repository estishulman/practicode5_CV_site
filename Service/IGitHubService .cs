using Octokit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service
{
    public interface IGitHubService
    {
        Task<int> GetUsersFollowersAsync(string userName);
        Task<List<Repository>> SearchRepositoriesAsync(string? name, string? language, string? username);
        Task<List<RepositoryInfo>> GetPortfolioAsync(string username);
    }
}
