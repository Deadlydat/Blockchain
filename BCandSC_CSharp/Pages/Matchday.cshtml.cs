using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Data;

namespace BCandSC_CSharp.Pages
{
    public class MatchdayModel : PageModel
    {
        [Required]
        [BindProperty]
        public string TeamName { get; set; } = "";
        [BindProperty(SupportsGet = true)]
        public int userId { get; set; }
        public int Matchday { get; set; } = 0;
        Team team { get; set; } = new();


        public IActionResult OnGet()
        {
            Matchday = Enviroment.GetEnviroment().Matchday;
            Team t = team.GetTeam(userId, Matchday);

            if (t.Players.Count == 11)
                return RedirectToPage("/Result", new { userId = userId });

            if (t.Formation != "")
                return RedirectToPage("/PlayerSelection", new { UserId = userId });

            if (t.Id > 0)
                return RedirectToPage("/Formation", new { userId = userId });

            return Page();
        }

        public IActionResult OnPost()
        {
            if (TeamName != null && TeamName.Length > 2)
                team.CreateTeam(userId, TeamName, Enviroment.GetEnviroment().Matchday);
            else
                return RedirectToPage("/Matchday", new { userId = userId });

            return RedirectToPage("/Formation", new { userId = userId });
        }
    }
}