﻿using NetCoreReact.Models.Documents;
using NetCoreReact.Models.DTO;
using NetCoreReact.Models.Email;
using System.Threading.Tasks;

namespace NetCoreReact.Services.Business.Interfaces
{
	public interface IEmailService
	{
		Task<DataResponse<Event>> SendConfirmationEmail(string email, Event currentEvent);
		Task<DataResponse<Event>> SendFeedbackEmail(string email, Event currentEvent);
		Task<DataResponse<Event>> SendGenericEmail(DataInput<EmailTemplateData> email);
	}
}
