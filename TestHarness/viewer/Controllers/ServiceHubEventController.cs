using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.Threading.Tasks;
using viewer.Hubs;
using viewer.Models;


// This solution was downloaded from github and then re-purposed as all I wanted is something to simulate or pretend to be
// a Fenetre Web API so 
// goal of this to expose a single Web API method Post(TaskStatusEvent taskStatusEvent) which will be called from the Service Bus listener
// and obviously put whatever logic is needed to react to Task Status event but in this case, just to make it obvious what's going on,
// this code publishes to a SignalR hub so that the web page grid gets updated immediately without needing to refresh it

// Code is from here: 
// https://github.com/Azure-Samples/azure-event-grid-viewer
// But just to repeat, I got rid of all of the Azure Event grid stuff, i just wanted a single web api with a signalR hub and simple grid
// For reference, I obtained the github linked from this page: https://docs.microsoft.com/en-us/azure/event-grid/custom-event-quickstart

namespace viewer.Controllers
{
    [Route("api/[controller]")]
    public class ServiceHubEventController : Controller
    {
        #region Data Members

        private readonly IHubContext<GridEventsHub> _hubContext;
        private static int sequenceNumber = 0;

        #endregion

        #region Constructors

        public ServiceHubEventController(IHubContext<GridEventsHub> gridEventsHubContext)
        {
            this._hubContext = gridEventsHubContext;
        }

        #endregion

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]TaskStatusEvent taskStatusEvent)
        {
            await this._hubContext.Clients.All.SendAsync(
                "gridupdate",
                sequenceNumber++,
                taskStatusEvent.TaskId,
                taskStatusEvent.Status,
                taskStatusEvent.StatusChangedBy,
                taskStatusEvent.StatusChangedOn,
                JsonConvert.SerializeObject(taskStatusEvent, Formatting.Indented)
            );

            return Ok();
        }

    }
}