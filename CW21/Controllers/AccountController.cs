using CW21.DAL.Entities;
using CW21.Models.ViewModels;
using CW21.Repositories;
using Microsoft.AspNetCore.Mvc;

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
