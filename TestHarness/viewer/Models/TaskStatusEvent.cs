using System;
using System.Collections.Generic;

namespace viewer.Models
{
    // Stole this class from https://wtwcrb.visualstudio.com/_git/ServiceHub?path=%2FApplication%2FWTW.ServiceHub%2FWTW.ServiceHub.SharedEntity%2FExternalTaskSourceResponseMessage.cs&version=GBmaster
    // as this is what the Service Hub web app uses for the response message
    public class TaskStatusEvent
    {
        /// <summary>
        /// Gets or sets the ExternalTaskSourceResponseMessage
        /// </summary>
        /// <value>
        /// ExternalTaskSourceResponseMessage
        /// </value>
        public TaskStatusEvent()
        {
            AdditionalDetails = new Dictionary<string, string>();
        }
        /// <summary>
        /// TaskId
        /// </summary>
        public int TaskId { get; set; }

        /// <summary>
        /// StatusId
        /// </summary>
        public int StatusId { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Task Template
        /// </summary>
        public string TaskTemplate { get; set; }

        /// <summary>
        /// Status Change Reason
        /// </summary>
        public string StatusChangeReason { get; set; }

        /// <summary>
        /// Comments
        /// </summary>
        public string Comments { get; set; }

        /// <summary>
        /// Status Changed By 
        /// </summary>
        public string StatusChangedBy { get; set; }

        /// <summary>
        /// Status Changed On
        /// </summary>
        public DateTime? StatusChangedOn { get; set; }

        /// <summary>
        /// Summarised form of ExtensibleMetadata expressed as dictionary of name/value pairs
        /// </summary>
        /// <remarks>
        /// Will contains just the Extensible meta data which is not shown in Task Details page which the original source application passed into TaskManagement CreateTask expressed as a Dictionary of Name/Value pairs 
        /// Remember is being serialised to JSON and passing over the Service Bus to multiple interested listeners so need to strive for as compact a message format as possible so
        /// there is no point passing back the full Extensible Meta data object with all the UI specific fields such DisplayName or IsVisibleInGrid and the other 6 fields, a calling application
        /// will probably just only need the names/values for purposes of routing Service Bus messages and data that is useful to the processor of the status update (back in the source system) to be able to react accordingly
        /// For example their Source Application Entity Id e.g. PlacementId or some other form of id which helps identify the record back in the source system
        /// </remarks>
        public Dictionary<string, string> AdditionalDetails { get; set; }
    }
}
