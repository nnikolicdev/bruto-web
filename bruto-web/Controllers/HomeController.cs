using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using bruto_web.Models;
using Microsoft.AspNetCore.Http;
using bruto_web.Util;
using bruto_web.Data;
using System.Web.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace bruto_web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ISession _session => HttpContext.Session;

        private readonly DbUserContext _context;


        public HomeController(ILogger<HomeController> logger, DbUserContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {

            UserModel user = _session.GetObjectFromJson<UserModel>("session");
            CombinedModel combined = new CombinedModel();

            if (user != null)
            {
                var update = await _context.Users
                        .Include(u => u.models)
                        .Where(u => u.Email == user.Email)                        
                        .FirstOrDefaultAsync();
       
                combined.user = update;

                return View(combined);
            }

            return RedirectToAction("LogIn");

        }

        #region LogIn
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn([Bind("Email,Password")] UserModel userModel)
        {


            if (ModelState.IsValid)
            {
                var stored = await _context.Users.FirstOrDefaultAsync(u => u.Email == userModel.Email);

                if (stored != null)
                {

                    if (Crypto.VerifyHashedPassword(stored.Password, userModel.Password))
                    {
                        _session.SetObjectAsJson("session", stored);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View(null);
                    };


                }
                else
                {
                    return View(null);
                }

            }
            else
            {
                return View(null);
            }
        }

        #endregion


        #region Register
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Name,LastName,Email,Password")] UserModel userModel)
        {
            if (ModelState.IsValid)
            {


                // Ovde da proverimo dali korisnik se vec registrovao

                var stored = await _context.Users.FirstOrDefaultAsync(u => u.Email == userModel.Email);

                if (stored != null)
                {
                    return View();
                }
                else
                {

                    // Hashujem sifru
                    userModel.Password = Crypto.HashPassword(userModel.Password);

                    _context.Users.Add(userModel);
                    await _context.SaveChangesAsync();

                    _session.SetObjectAsJson("session", userModel);
                    _logger.LogInformation($"Imamo novog korisnika {userModel.Name} {userModel.LastName} [{userModel.Email}]");
                    return RedirectToAction("Index");
                }

            }
            else
            {
                return View();
            }
        }

        #endregion


        #region LogOut
        public IActionResult LogOut()
        {
            _session.Remove("session");
            return RedirectToAction("LogIn");
        }

        #endregion

        #region Bruto

        #region Konstante

        // Za kalkulaciju cu koristiti ove 3 promenljive konstante

        // Dosta varira ovo od formula kojih sam nalazio, ali ovo bi trebalo da popravi predhodnu kalkulaciju

        // 19% (14%) Fod PIO
        private const double _pio = 0.195;
        // 5~7% Zdravstveno osiguranje
        private const double _zo = 0.07106;
        // 0.75~1% Osiguranje od nezaposlenosti
        private const double _odn = 0.01034;

        // (~10%) Porez na zareda 
        private const double _porez = 0.10;
        #endregion

        public IActionResult BrutoIndex()
        {
            UserModel user = _session.GetObjectFromJson<UserModel>("session");
            CombinedModel combined = new CombinedModel();
            combined.user = user;

            if (user != null)
            {
                return View("Bruto", combined);
            }

            return RedirectToAction("LogIn");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Calculate([Bind("Neto")] BrutoModel brutoModel)
        {
            UserModel user = _session.GetObjectFromJson<UserModel>("session");

            if (ModelState.IsValid)
            {
                int neto = brutoModel.Neto;

                double pioCalc = neto * _pio;
                double zoCalc = neto * _zo;
                double odnCalc = neto * _odn;
                double porezCalc = neto * _porez;

                // Sve zajedno = bruto
                int bruto = (int)Math.Round(pioCalc + zoCalc + odnCalc + neto + porezCalc);

                BrutoViewModel brutoCalc = new BrutoViewModel(
                    neto,
                    bruto,
                    pioCalc,
                    zoCalc,
                    odnCalc,
                    porezCalc,
                    bruto
                );

                CombinedModel combined = new CombinedModel();
                combined.user = user;
                combined.viewModel = brutoCalc;

                brutoModel.UserId = user.Id;
                brutoModel.Bruto = bruto;
                brutoModel.Date = DateTime.Now;

                _context.models.Add(brutoModel);
                await _context.SaveChangesAsync();


                return View("Result", combined);

            }
            else
            {
                return View();
            }


        }

        #endregion


        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
