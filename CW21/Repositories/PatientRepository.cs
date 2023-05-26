using CW21.Cache;
using CW21.DAL.Context;
using CW21.DAL.Entities;
using CW21.Models;
using CW21.Models.ViewModels;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace CW21.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly AppDbContext _context;
        private readonly IMemoryCache _memoryCache;
        private readonly Configs _configs;
        private readonly IOptions<Configs> _options;

        public PatientRepository(AppDbContext context, IMemoryCache memoryCache, IOptions<Configs> options)
        {
            _context = context;
            _memoryCache = memoryCache;
            _options = options;
        }

        public List<Patient> GetPatients()
        {

            var patients = _memoryCache.Get<List<Patient>>("patients");

            var absoluteExpiration = _options.Value.patient.AbsoluteExpiration;
            var slidingExpiration = _options.Value.patient.SlidingExpiration;

            if (patients is null)
            {
                MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();
                options.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(absoluteExpiration);
                options.SlidingExpiration = TimeSpan.FromSeconds(slidingExpiration);
                var result = _context.Patients.ToList();
                _memoryCache.Set("patients", result, options);
                return result;
            }


            return patients;

        }

        public bool LoginPatient(LoginViewModel model)
        {
            var result = _context.Patients.FirstOrDefault(p => p.UserName == model.UserName && p.PassWord == model.Password);
            if (result == null)
            {
                return false;
            }
            else
            {
                CurrentUser.UserId = result.Id;
                CurrentUser.Role = RoleEnum.Patient;
                return true;
            }
        }

        public int RegisterPatient(Patient patient)
        {
            var existedPatient = _context.Patients.FirstOrDefault(p => p.UserName == patient.UserName);
            if (existedPatient != null)
            {
                _context.Patients.Add(patient);
                _context.SaveChanges();
                return patient.Id;
            }
            else
            {
                return 0;
            }
        }
    }
}
