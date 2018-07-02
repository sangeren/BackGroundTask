using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace WebApplication1.Pages
{
    public class IndexModel : PageModel
    {
        private IApplicationLifetime _appLifetime;
        private ILogger<IndexModel> _logger;
        public IBackgroundTaskQueue Queue { get; }
        public IndexModel(IBackgroundTaskQueue queue, IApplicationLifetime appLifetime, ILogger<IndexModel> logger)
        {
            Queue = queue;
            _appLifetime = appLifetime;
            _logger = logger;
        }
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            Queue.QueueBackgroundWorkItem(async token =>
            {
                var guid = Guid.NewGuid().ToString();

                for (int delayLoop = 0; delayLoop < 3; delayLoop++)
                {
                    _logger.LogInformation($"Queued Background Task {guid} is running. {delayLoop}/3");
                    await Task.Delay(TimeSpan.FromSeconds(5), token);
                }

                _logger.LogInformation($"Queued Background Task {guid} is complete. 3/3");
            });

            return RedirectToPage("/Index");
        }
    }
}