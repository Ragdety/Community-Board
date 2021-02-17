using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace CommunityBoard.FrontEnd.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public List<TempAnnouncement> Announcements;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            Announcements = new List<TempAnnouncement>{
                new TempAnnouncement
                {
                    Name = "Announcement 1",
                    Description = @"Lorem ipsum dolor sit amet consectetur, adipisicing elit. 
                            Ut eum similique repellat a laborum, rerum voluptates ipsam 
                            eos quo tempore iusto dolore modi dolorum in pariatur. 
                            Incidunt repellendus praesentium quae!",
                    URL = "https://images.unsplash.com/photo-1477862096227-3a1bb3b08330?ixlib=rb-1.2.1&auto=format&fit=crop&w=700&q=60"
                },
                new TempAnnouncement
                {
                    Name = "Announcement 2",
                    Description = @"Lorem ipsum dolor sit amet consectetur, adipisicing elit. Ut eum similique repellat a laborum, rerum voluptates ipsam eos quo tempore iusto dolore modi dolorum in pariatur.  
                                Incidunt repellendus praesentium quae! Ut eum similique repellat a laborum, rerum voluptates ipsam eos quo tempore tempor alt.",
                    URL = "https://images.unsplash.com/photo-1516214104703-d870798883c5?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=crop&w=700&q=60"
                },
                new TempAnnouncement
                {
                    Name = "Announcement 3",
                    Description = @"Lorem ipsum dolor sit ",
                    URL = "https://images.unsplash.com/photo-1477862096227-3a1bb3b08330?ixlib=rb-1.2.1&auto=format&fit=crop&w=700&q=60"
                },
            };
        }
    }

    public class TempAnnouncement
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string URL { get; set; }
    }
}
