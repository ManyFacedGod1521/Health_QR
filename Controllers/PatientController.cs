using Health_QR.DAL;
using Health_QR.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Policy;

namespace Health_QR.Controllers
{
    public class PatientController : Controller
    {

        private PatientDAL _dalPatient;

        public PatientController(ILogger<HomeController> logger)
        {
            _dalPatient = new PatientDAL();
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Patient patient)
        {
            var uniq = int.Parse(DateTime.Now.ToString("yymmssfff"));
            patient.UniquePatientId = uniq;
            patient.Personal_Information.UniquePatientId = uniq;
            patient.Personal_Information.Contact_Information.UniquePatientId = uniq;
            patient.Active_Problems_Diagnoses.UniquePatientId = uniq;
            patient.Active_Problems_Diagnoses.Physical_Examination.UniquePatientId = uniq;
            patient.Active_Problems_Diagnoses.Vital_Signs.UniquePatientId = uniq;

            _dalPatient.AddPatient(patient);
            _dalPatient.AddPatient_Personal_Information(patient.Personal_Information);
            _dalPatient.AddPatient_Contact_Information(patient.Personal_Information.Contact_Information);
            _dalPatient.AddPatient_Active_Problems_Diagnoses(patient.Active_Problems_Diagnoses);
            _dalPatient.AddPatient_Physical_Examination(patient.Active_Problems_Diagnoses.Physical_Examination);
            _dalPatient.AddPatient_Vital_Signs(patient.Active_Problems_Diagnoses.Vital_Signs);
            return RedirectToAction("Create");
        }

        public IActionResult List()
        {
            var patientList = _dalPatient.ListPatients();
            foreach (var patient in patientList)
            {
                patient.Personal_Information = _dalPatient.GetPatient_Personal_Information(patient.UniquePatientId);
                patient.Personal_Information.Contact_Information = _dalPatient.GetPatient_Contact_Information(patient.UniquePatientId);
                patient.Active_Problems_Diagnoses = _dalPatient.GetPatient_Active_Problems_Diagnoses(patient.UniquePatientId);
                patient.Active_Problems_Diagnoses.Physical_Examination = _dalPatient.GetPatient_Physical_Examination(patient.UniquePatientId);
                patient.Active_Problems_Diagnoses.Vital_Signs = _dalPatient.GetPatient_Vital_Signs(patient.UniquePatientId);
            }
            return View(patientList);
        }


        [HttpGet]
        public IActionResult Details(int id)
        {
            Patient patient = new Patient();
            if (id > 0)
            {
                patient = _dalPatient.GetPatient(id);
            }
            return View("Details", patient);
        }
        [HttpGet]
        public IActionResult Delete(int id) 
        {
            var ret =  _dalPatient.GetPatient(id);
            return View(ret);
        }

        [HttpPost]
        public IActionResult Delete(Patient ret)
        {
            _dalPatient.DeleteById(ret.UniquePatientId);
            return RedirectToAction("List");
        }
        public IActionResult Personal_Information_Create()
        {
            return View();
        }
        public IActionResult Contact_InformationCreate()
        {
            return View();
        }
        public IActionResult ActiveProblemsCreate()
        {
            return View();
        }
        public IActionResult Physical_ExaminationCreate()
        {
            return View();
        }
        public IActionResult Vital_SignsCreate()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult Update(int id) => View(_dalPatient.GetPatient(id));

        [HttpPost]
        public IActionResult Update(Patient patient)
        {
            _dalPatient.UpdatePatient(patient);

            return RedirectToAction("Details", patient);
        }
    }
}
