using Octokit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service
{
    public interface IGitHubService
    {
        Task<int> GetUsersFollowersAsync(string userName);
        Task<List<Repository>> SearchRepositoriesInCSharpAsync(string keyword);
        Task<List<RepositoryInfo>> GetPortfolioAsync(string username);
    }
}
