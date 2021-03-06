﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCoreReact.Helpers;
using NetCoreReact.Models;
using NetCoreReact.Models.Documents;
using NetCoreReact.Models.DTO;
using NetCoreReact.Models.Email;
using NetCoreReact.Services.Business;
using NetCoreReact.Services.Business.Interfaces;

namespace NetCoreReact.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class EmailController : ControllerBase
	{
		private readonly IEventService _eventService;
		private readonly IEmailService _emailService;
		private readonly IAuthenticationService _authenticationService;

		public EmailController(IEventService eventService, IEmailService emailService, IAuthenticationService authenticationService)
		{
			this._eventService = eventService;
			this._emailService = emailService;
			this._authenticationService = authenticationService;
		}

		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		[HttpPost("[action]")]
		public async Task<DataResponse<Event>> AddEmail([FromBody] DataInput<Participant> newParticipant)
		{
			try
			{
				var currentEvent = await _eventService.AddParticipant(newParticipant);
				var response = await _emailService.SendConfirmationEmail(newParticipant.Data.Email, currentEvent.Data.FirstOrDefault());
				var result = await _eventService.UpdateEvent(response.Data.FirstOrDefault());
				return currentEvent;
			}
			catch (Exception ex)
			{
				LoggerHelper.Log(ex);
				return new DataResponse<Event>()
				{
					Errors = new Dictionary<string, List<string>>()
					{
						["*"] = new List<string> { ex.Message },
					},
					Success = false
				};
			}
		}

		[HttpPost("[action]")]
		public async Task<DataResponse<Event>> ConfirmEmail([FromBody] DataInput<string> token)
		{
			try
			{
				var authenticate = _authenticationService.AuthenticateToken(token.Data, AppSettingsModel.appSettings.ConfirmEmailJwtSecret);
				var currentEvent = await _eventService.ConfirmEmail(authenticate.Data[0], authenticate.Data[1]);
				return currentEvent;
			}
			catch (Exception ex)
			{
				LoggerHelper.Log(ex);
				return new DataResponse<Event>()
				{
					Errors = new Dictionary<string, List<string>>()
					{
						["*"] = new List<string> { ex.Message },
					},
					Success = false
				};
			}
		}

		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		[HttpPost("[action]")]
		public async Task<DataResponse<Event>> SendGenericEmail([FromBody] DataInput<EmailTemplateData> email)
		{
			try
			{
				var sendEmail = await _emailService.SendGenericEmail(email);
				return sendEmail;
			}
			catch (Exception ex)
			{
				LoggerHelper.Log(ex);
				return new DataResponse<Event>()
				{
					Errors = new Dictionary<string, List<string>>()
					{
						["*"] = new List<string> { ex.Message },
					},
					Success = false
				};
			}
		}

		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		[HttpPost("[action]")]
		public async Task<DataResponse<Event>> SendFeedbackEmail([FromBody] DataInput<string> data)
		{
			try
			{
				var currentEvent = await _eventService.GetEvent(data.EventId);
				var response = await _emailService.SendFeedbackEmail(data.Data, currentEvent.Data.FirstOrDefault());
				var result = await _eventService.UpdateEvent(response.Data.FirstOrDefault());
				return response;
			}
			catch (Exception ex)
			{
				LoggerHelper.Log(ex);
				return new DataResponse<Event>()
				{
					Errors = new Dictionary<string, List<string>>()
					{
						["*"] = new List<string> { ex.Message },
					},
					Success = false
				};
			}
		}

		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		[HttpPost("[action]")]
		public async Task<DataResponse<Event>> SendConfirmationEmail([FromBody] DataInput<string> data)
		{
			try
			{
				var currentEvent = await _eventService.GetEvent(data.EventId);
				var response = await _emailService.SendConfirmationEmail(data.Data, currentEvent.Data.FirstOrDefault());
				var result = await _eventService.UpdateEvent(response.Data.FirstOrDefault());
				return response;
			}
			catch (Exception ex)
			{
				LoggerHelper.Log(ex);
				return new DataResponse<Event>()
				{
					Errors = new Dictionary<string, List<string>>()
					{
						["*"] = new List<string> { ex.Message },
					},
					Success = false
				};
			}
		}

		[HttpPost("[action]")]
		public async Task<DataResponse<Event>> RemoveEmail([FromBody] DataInput<string> token)
		{
			try
			{
				var authenticate = _authenticationService.AuthenticateToken(token.Data, AppSettingsModel.appSettings.RemoveEmailJwtSecret);
				var result = await _eventService.RemoveEmail(authenticate.Data[0]);
				return result;
			}
			catch (Exception ex)
			{
				LoggerHelper.Log(ex);
				return new DataResponse<Event>()
				{
					Errors = new Dictionary<string, List<string>>()
					{
						["*"] = new List<string> { ex.Message },
					},
					Success = false
				};
			}
		}
	}
}