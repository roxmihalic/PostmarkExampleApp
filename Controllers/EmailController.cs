using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PostmarkDotNet;

namespace PostmarkExampleApp.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class EmailController : ControllerBase
	{
		private static readonly string[] Summaries = new[]
		{
			"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
		};

		private readonly ILogger<EmailController> _logger;

		public EmailController(ILogger<EmailController> logger)
		{
			_logger = logger;
		}

		[HttpGet]
		public async Task<IEnumerable<Email>> GetAsync()
		{
			var rng = new Random();
			var sendMessageResult = await SendMessageAsync();
			return Enumerable.Range(1, 5).Select(index => new Email
			{
				Date = DateTime.Now.AddDays(index),
				TemperatureC = rng.Next(-20, 55),
				Summary = Summaries[rng.Next(Summaries.Length)],
				SendMessageResult = sendMessageResult
			})
			.ToArray();
		}

		private async Task<bool> SendMessageAsync()
		{
			// Send an email asynchronously:
			var message = new PostmarkMessage()
			{
				To = "r.mihalic@netmatch.ro",
				From = "r.mihalic@netmatch.nl",
				TrackOpens = true,
				Subject = "Test email",
				TextBody = "Plain Text Body",
				HtmlBody = "HTML goes here",
				Tag = "Test email"
			};

			var client = new PostmarkClient("0f0e4b96-75c9-4532-af69-5a8fa7fed50b");
			var sendResult = await client.SendMessageAsync(message);

			if (sendResult.Status == PostmarkStatus.Success) { return true; }
			else { return false;  }
		}
	}
}
