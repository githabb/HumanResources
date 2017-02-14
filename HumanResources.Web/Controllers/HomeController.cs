using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HumanResources.Data;
using HumanResources.Web.Models;


namespace HumanResources.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            // получаем из бд все объекты Book
            using (HumanResourceEntities db = new HumanResourceEntities())
            {
                var employees = (from b in db.Employees
                                 select new EmployeeModel
                                 {
                                     Id = b.Id,
                                     CityId = b.CityId,
                                     Name = b.Name,
                                     Surname = b.Surname,
                                     Patronymic = b.Patronymic,
                                     Address = b.Address,
                                     StartDate = b.StartDate,
                                     EndDate = b.EndDate,
                                     MobilePhone = b.MobilePhone,
                                     ManagerId = b.ManagerId,
                                     ManagerName = b.ManagerId == null ? null : b.Employee2.Surname,
                                     DateOfBirth = b.DateOfBirth,
                                     Position = b.Position,
                                     Phone = b.Phone
                                 }).ToList();

                // передаем все объекты в динамическое свойство Books в ViewBag
                ViewBag.Employees = employees;
            }
            // возвращаем представление
            return View();
        }
    }
}