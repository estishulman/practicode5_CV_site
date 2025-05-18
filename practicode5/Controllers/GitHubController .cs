using Microsoft.AspNetCore.Mvc;
using Octokit;
using Service; // נניח ששם השירות שלך
using System.Threading.Tasks;
//using Service.RepositoryInfo;

namespace practicode5.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GitHubController : ControllerBase
    {
        private readonly IGitHubService _gitHubService;

        public GitHubController(IGitHubService gitHubService)
        {
            _gitHubService = gitHubService;
        }

        [HttpGet("portfolio")]
        public async Task<List<RepositoryInfo>> GetPortfolio(string username)
        {
            var result = await _gitHubService.GetPortfolioAsync(username);
            return result;
        }


        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string? name, [FromQuery] string? language, [FromQuery] string? username)
        {
            var repositories = await _gitHubService.SearchRepositoriesInCSharpAsync(name ?? "dotnet");
            return Ok(repositories);
        }
    }
}
