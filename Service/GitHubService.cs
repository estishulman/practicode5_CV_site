using Octokit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
namespace Service
{
    public class GitHubService : IGitHubService
    {
        private readonly GitHubClient _client;
        private readonly IMemoryCache _cache;
        private readonly IConfiguration _configuration;

        public GitHubService(IMemoryCache cache, IConfiguration configuration)
        {
            _client = new GitHubClient(new ProductHeaderValue("MyApp"));
            var token = configuration["GitHub:Token"];
            if (!string.IsNullOrEmpty(token))
            {
                _client.Credentials = new Credentials(token);
            }

            _cache = cache;
            _configuration = configuration;
        }

        public async Task<int> GetUsersFollowersAsync(string userName)
        {
            var user = await _client.User.Get(userName);
            return user.Followers;
        }

        public async Task<List<Repository>> SearchRepositoriesAsync(string? name, string? language, string? username)
        {
            string cacheKey = $"repos_{name}_{language}_{username}";
            if (_cache.TryGetValue(cacheKey, out List<Repository> cachedRepos))
            {
                return cachedRepos;
            }

            // בונים מחרוזת חיפוש לפי הפרמטרים שקיבלת
            var queryParts = new List<string>();

            if (!string.IsNullOrEmpty(name))
                queryParts.Add(name);

            if (!string.IsNullOrEmpty(language))
                queryParts.Add($"language:{language}");

            if (!string.IsNullOrEmpty(username))
                queryParts.Add($"user:{username}");

            var query = string.Join(" ", queryParts);

            if (string.IsNullOrWhiteSpace(query))
                query = "stars:>0"; // אפשר לתת ביטוי כללי כדי שהחיפוש לא יחזור ריק

            var request = new SearchRepositoriesRequest(query);

            var result = await _client.Search.SearchRepo(request);
            var repos = result.Items.ToList();

            _cache.Set(cacheKey, repos, TimeSpan.FromMinutes(10));
            return repos;
        }



        public async Task<List<RepositoryInfo>> GetPortfolioAsync(string username)
        {
            var repos = await _client.Repository.GetAllForUser(username);
            var result = new List<RepositoryInfo>();

            foreach (var repo in repos)
            {
                var languages = await _client.Repository.GetAllLanguages(repo.Owner.Login, repo.Name);

                string lastCommitMessage = "No commits yet";

                try
                {
                    var commits = await _client.Repository.Commit.GetAll(repo.Owner.Login, repo.Name);
                    lastCommitMessage = commits.FirstOrDefault()?.Commit.Message ?? "No commits yet";
                }
                catch (Octokit.ApiException ex) when (ex.ApiError?.Message == "Git Repository is empty.")
                {
                    // ריפו ריק – כבר הגדרנו את lastCommitMessage כברירת מחדל
                }

                var pullRequests = await _client.PullRequest.GetAllForRepository(repo.Owner.Login, repo.Name);
                var openPrCount = pullRequests.Count(pr => pr.State == ItemState.Open);

                result.Add(new RepositoryInfo
                {
                    Name = repo.Name,
                    Languages = languages.Select(lang => lang.Name).ToList(),
                    LastCommit = lastCommitMessage,
                    Stars = repo.StargazersCount,
                    PullRequests = openPrCount,
                    Link = repo.HtmlUrl
                });
            }

            return result;
        }


    }
}
