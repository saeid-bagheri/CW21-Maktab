using CW21.DAL.Entities;
using CW21.Models.ViewModels;
using CW21.Repositories;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace CW21.Controllers
{
    public class AccountController : Controller
    {


        private readonly IPatientRepository _patientRepository;

        public AccountController(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RegisterPatient()
        {
            return View();
        }
        [HttpPost]
        public IActionResult RegisterPatient(Patient patient)
        {

            int result = _patientRepository.RegisterPatient(patient);
            if (result == 0)
            {
                ViewBag.message = "این کاربر موجود است";
                return View();
            }
            else
            {
                ViewBag.message = "کاربر ثبت شد";
                return View();
            }

        }


        public IActionResult LoginPatient()
        {
            _patientRepository.GetPatients();
            //try
            //{
            //    int x = 10;
            //    int y = 0;
            //    int z = x / y;
            //}
            //catch (Exception ex)
            //{

            //    //Log.ForContext("test", "error message").Information("wrong devvvv");
            //    Log.ForContext("CrudState", "insert")
            //        .ForContext("ProductId", 10)
            //        .Information("log in sql");
            //    Log.Error("wrong devvvv with file");
            //    Log.Warning("this is warning brother");
            //    throw;
            //}
            return View();
        }

        [HttpPost]
        public IActionResult LoginPatient(LoginViewModel model)
        {
            var result = _patientRepository.LoginPatient(model);
            if (result)
            {
                return Redirect("/Dashbored/Index");
            }
            else
            {
                ViewBag.message = "نام کاربری یا کلمه عبور اشتباه است";
                return View();
            }
        }

    }
}
