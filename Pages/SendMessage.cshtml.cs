using GrindSoft.Models;
using GrindSoft.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GrindSoft.Pages
{
    public class SendMessageModel : PageModel
    {
        private readonly SessionManager _sessionManager;
        private readonly AppDbContext _dbContext;

        public SendMessageModel(SessionManager sessionManager, AppDbContext dbContext)
        {
            _sessionManager = sessionManager;
            _dbContext = dbContext;
        }

        [BindProperty]
        public string AccessToken { get; set; }

        [BindProperty]
        public string UserAgent { get; set; }

        [BindProperty]
        public string? ServerId { get; set; }

        [BindProperty]
        public string ChannelId { get; set; }

        [BindProperty]
        public string Prompt { get; set; }

        [BindProperty]
        public int MessageCount { get; set; }

        [BindProperty]
        public int DelayBetweenMessages { get; set; }

        [BindProperty]
        public int ModeType { get; set; }

        [BindProperty]
        public string TargetUserId { get; set; }

        [BindProperty]
        public string Message { get; set; }

        public string? Response { get; set; }

        public int? SessionId { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModeType == 2 && string.IsNullOrWhiteSpace(TargetUserId))
                ModelState.AddModelError("TargetUserId", "TargetUserId is required for Mode 2.");

            if (ModeType == 3 && string.IsNullOrWhiteSpace(Message))
                ModelState.AddModelError("Message", "Message is required for Mode 3.");

            if (ModeType == 1 || ModeType == 2)
                ModelState.Remove("Message");

            if (ModeType == 1 || ModeType == 3)
                ModelState.Remove("TargetUserId");

            if (ModeType == 3)
                ModelState.Remove("Prompt");

            if (!ModelState.IsValid)
            {
                Response = "Please fill in all required fields.";
                return Page();
            }

            var session = new Session
            {
                AccessToken = AccessToken,
                UserAgent = UserAgent,
                ServerId = ServerId ?? "@me",
                ChannelId = ChannelId,
                Prompt = ModeType == 3 ? null : Prompt,
                Message = ModeType == 3 ? Message : null,
                Status = "In Progress",
                MessageCount = MessageCount,
                DelayBetweenMessages = DelayBetweenMessages,
                ModeType = ModeType,
                TargetUserId = ModeType == 2 ? TargetUserId : "",
                StartTime = DateTime.UtcNow
            };

            _sessionManager.AddSession(session);
            await _dbContext.SaveChangesAsync();

            SessionId = _sessionManager.GetSessions();

            Response = "Session started and is in progress.";
            return Page();
        }
    }
}