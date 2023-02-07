using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Data;

namespace BCandSC_CSharp.Pages
{
    public class MatchdayModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int userId { get; set; }
        public int Matchday { get; set; } = 0;
        Team team { get; set; } = new();
        public List<Team> teams { get; set; } = new();
        public User user { get; set; } = new();
        public double AccountBalanceETH { get; set; } = 0;
        public double AccountBalanceEUR { get; set; } = 0;
        public double AccountBalanceUSD { get; set; } = 0;

        public IActionResult OnGet()
        {
            Matchday = Enviroment.GetEnviroment().Matchday;
            Team t = team.GetTeam(userId, Matchday);

            //if (t.Players.Count == 11)
            //    return RedirectToPage("/Result", new { userId = userId });

            //if (t.Formation != "")
            //    return RedirectToPage("/PlayerSelection", new { UserId = userId });

            //if (t.Id > 0)
            //    return RedirectToPage("/Formation", new { userId = userId });
            if (t.Formation != "" && t.Done == false)
                return RedirectToPage("/PlayerSelection", new { UserId = userId });

            if (t.Id > 0 && t.Done == false)
                return RedirectToPage("/Formation", new { userId = userId });


            teams = t.GetTeamList(userId);
            user = user.GetUser(userId);






            MoneyConversion.DataObject data = MoneyConversion.GetAccountBalanceInFiatMoney(user);
            BlockchainInterface blockchainInterface = new();

            AccountBalanceEUR = data.EUR;
            AccountBalanceUSD = data.USD;
            AccountBalanceETH = Decimal.ToDouble(blockchainInterface.GetAccountBalance(user.Address));



            return Page();
        }

        public IActionResult OnPost()
        {
            User u = new();
            u = u.GetUser(userId);

            team.CreateTeam(userId, $"Team {u.Name} f�r Spieltag {Enviroment.GetEnviroment().Matchday}", Enviroment.GetEnviroment().Matchday);


            return RedirectToPage("/Formation", new { userId = userId });
        }
    }
}
