using Owleye.Shared.Model.MicrosoftTeams;
using System.Threading.Tasks;

namespace Owleye.Shared.MicrosoftTeams
{
    public interface IMicrosoftTeamsService
    {
        Task SendTeamsMessageAsync(SendTeamsUsersMessageRequest request);
    }
}
