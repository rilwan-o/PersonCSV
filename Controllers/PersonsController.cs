using CsvHelper;
using CsvHelper.Configuration;
using PersonCSV.DTOs;
using PersonCSV.Mappings;
using PersonCSV.Models;
using PersonCSV.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PersonCSV.Controllers
{
    public class PersonsController : Controller
    {
        private readonly PersonDbContext _context;
        public PersonsController()
        {
            _context = new PersonDbContext();
        }

        //Method to return Partial View
        public ActionResult PersonsTable()
        {
            List<PersonModel> people = new List<PersonModel>();
            var persons = _context.Persons.Where(p => p.DeletedAt == null).ToList();
            foreach (var psn in persons)
            {
                PersonModel person = new PersonModel();
                person.Identity = psn.Identity;
                person.FirstName = psn.FirstName;
                person.Surname = psn.Surname;
                person.Age = psn.Age;
                person.Sex = psn.Sex[0];
                person.Mobile = psn.Mobile;
                person.Active = psn.Active;

                people.Add(person);
            }
            return PartialView(people);
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadFiles()
        {
            var resp = new FileUploadResponse();

            // Checking no of files injected in Request object  
            if (Request.Files.Count > 0)
            {

                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
                        string fname;

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }
                        String timeStamp = DateTime.Now.ToString();

                        // Get the complete folder path and store the file inside it.
                        Directory.CreateDirectory(Server.MapPath("~/Uploads/"));
                        fname = Path.Combine(Server.MapPath("~/Uploads/"), fname);
                        file.SaveAs(fname);

                        SavePersonsCsvData(fname);

                        DirectoryInfo di = new DirectoryInfo(Server.MapPath("~/Uploads/"));
                        //remove file from the folder for space
                        foreach (FileInfo fi in di.GetFiles())
                        {
                            if (fi.FullName.Equals(fname))
                                fi.Delete();
                        }
                    }
                    // Returns message that successfully uploaded
                    resp.Status = true;
                    resp.Message = "File Uploaded Successfully!";
                    return Json(resp);
                }
                catch (Exception ex)
                {
                    resp.Message = "Error occurred. Error details: " + ex.Message;
                    return Json(resp);
                }
            }
            else
            {
                resp.Message = "No files selected.";
                return Json(resp);
            }
        }

        [HttpPost]
        public ActionResult Edit(PersonModel psn)
        {
            if (!Enum.IsDefined(typeof(Gender), psn.Sex.ToString().ToLower())) return RedirectToAction("Index");

            var person = _context.Persons.FirstOrDefault(p => p.Identity == psn.Identity && p.DeletedAt == null);

            if (person == null) return RedirectToAction("Index");

            person.FirstName = psn.FirstName;
            person.Surname = psn.Surname;
            person.Age = psn.Age;
            person.Sex = psn.Sex.ToString();
            person.Mobile = psn.Mobile;
            person.Active = psn.Active;
            person.UpdatedAt = DateTime.Now;

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var psn = _context.Persons.FirstOrDefault(p => p.Identity == id && p.DeletedAt == null);

            if (psn == null) return RedirectToAction("Index");

            PersonModel person = new PersonModel();
            person.Identity = psn.Identity;
            person.FirstName = psn.FirstName;
            person.Surname = psn.Surname;
            person.Age = psn.Age;
            person.Sex = psn.Sex[0];
            person.Mobile = psn.Mobile;
            person.Active = psn.Active;

            return View(person);
        }

        public ActionResult DeleteItem(int id)
        {
            var resp = new FileUploadResponse();
            var person = _context.Persons.FirstOrDefault(x => x.Identity == id);
            if (person == null)
            {
                resp.Status = false;
                resp.Message = $"Item not found !";
            }

            person.DeletedAt = DateTime.Now;
            _context.SaveChanges();
            resp.Status = true;
            resp.Message = $"Deleted Successfully! {id}";
            return Json(resp);

        }

        private void SavePersonsCsvData(string fname)
        {
            int rowNumber = 1;
            try
            {
                var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true,
                    ExceptionMessagesContainRawData = false,
                };
                using (var reader = new StreamReader(fname))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Context.RegisterClassMap<PersonDtoMap>();
                    var records = csv.GetRecords<PersonDto>();

                    foreach (var record in records)
                    {
                        if (!Enum.IsDefined(typeof(Gender), record.Sex.ToString().ToLower())) throw new Exception("list contains wrong sex");
                        var rowExist = _context.Persons.Any(p => p.Identity == record.Identity);
                        if(rowExist) continue;
                        Person person = new Person();
                        person.Identity = record.Identity;
                        person.FirstName = record.FirstName;
                        person.Surname = record.Surname;
                        person.Age = record.Age;
                        person.Sex = record.Sex.ToString();
                        person.Mobile = record.Mobile;
                        person.Active = record.Active;
                        person.CreatedAt = DateTime.Now;

                        _context.Persons.Add(person);
                        _context.SaveChanges();
                        rowNumber++;
                    }
                }
            }
            catch (Exception)
            {

                throw new Exception($"Error on row number {rowNumber}");
            }

        }

    }
}