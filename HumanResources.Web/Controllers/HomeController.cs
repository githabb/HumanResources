using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HumanResources.Data;
using HumanResources.Web.Models;


namespace HumanResources.Web.Controllers
{
    public class HomeController : Controller
    {
        #region Drop Downs

        private IList<NameItem> GetEmployeeList(int? exceptId)
        {
            IList<EmployeeModel> employees;
            using (HumanResourceEntities db = new HumanResourceEntities())
            {
                employees = (from b in db.Employees
                    where (exceptId == null || exceptId != b.Id) && b.EndDate == null // проверяем, что сотрудник не уволился
                    select new EmployeeModel
                    {
                        Id = b.Id,
                        Surname = b.Surname,
                        Name = b.Name,
                        Patronymic = b.Patronymic
                    }).OrderBy(e => e.Surname).ThenBy(e => e.Name).ToList();
            }

            IList<NameItem> list = new List<NameItem>();
            list.Insert(0, new NameItem() { Id = null, Name = string.Empty });
            foreach (var m in employees)
            {
                list.Add(new NameItem() { Id = m.Id, Name = EmployeeModel.GetDisplayName(m.Surname, m.Name, m.Patronymic) });
            }
            return list;
        }


        private IList<NameItem> GetCityList()
        {
            IList<CityModel> cities;
            using (HumanResourceEntities db = new HumanResourceEntities())
            {
                cities = (from b in db.Cities
                    select new CityModel
                    {
                        Id = b.Id,
                        Name = b.Name
                    }).OrderBy(e => e.Name).ToList();
            }

            IList<NameItem> list = new List<NameItem>();
            list.Insert(0, new NameItem() { Id = null, Name = string.Empty });
            foreach (var m in cities)
            {
                list.Add(new NameItem() { Id = m.Id, Name = m.Name });
            }
            return list;
        }

        private void LoadDropDowns(int? id)
        {
            var employees = GetEmployeeList(id);
            // передаем всех сотрудников в динамическое свойство EmployeeList в ViewBag
            ViewBag.EmployeeList = new SelectList(employees, "Id", "Name");


            var cities = GetCityList();

            // передаем все города в динамическое свойство CityList в ViewBag
            ViewBag.CityList = new SelectList(cities, "Id", "Name");
        }

        #endregion

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

        [HttpGet]
        public ActionResult Display(int? id)
        {
            using (HumanResourceEntities db = new HumanResourceEntities())
            {
                var employee = (from e in db.Employees
                    where e.Id == id
                    select new EmployeeModel
                    {
                        Id = e.Id,
                        Name = e.Name,
                        Surname = e.Surname,
                        Patronymic = e.Patronymic,
                        CityId = e.CityId,
                        CityName = e.City.Name,
                        StartDate = e.StartDate,
                        EndDate = e.EndDate,
                        MobilePhone =e.MobilePhone,
                        ManagerId = e.ManagerId,
                        DateOfBirth = e.DateOfBirth,
                        Position = e.Position,
                        Phone = e.Phone,
                        Address = e.Address,
                        ManagerName = e.ManagerId == null ? null : e.Employee2.Surname,
                    }).FirstOrDefault();

                if(employee == null)
                    return HttpNotFound();

                return View(employee);
            }

        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            using (HumanResourceEntities db = new HumanResourceEntities())
            {
                LoadDropDowns(id);

                if (id == null)
                    return HttpNotFound();

                var employee = (from e in db.Employees
                                where e.Id == id
                                select new EmployeeModel
                                {
                                    Id = e.Id,
                                    Name = e.Name,
                                    Surname = e.Surname,
                                    Patronymic = e.Patronymic,
                                    CityId = e.CityId,
                                    StartDate = e.StartDate,
                                    EndDate = e.EndDate,
                                    Phone = e.Phone,
                                    MobilePhone = e.MobilePhone,
                                    ManagerId = e.ManagerId,
                                    DateOfBirth = e.DateOfBirth,
                                    Position = e.Position,
                                    Address = e.Address,
                                }).FirstOrDefault();

                if (employee == null)
                    return HttpNotFound();
                
                return View(employee);
            }

        }
        

        [HttpPost]
        public ActionResult Edit(int? id, EmployeeModel employee)
        {
            if (!ModelState.IsValid)
            {
                LoadDropDowns(id);
                return View(employee);
            }

            Employee dbEmployee = new Employee()
            {
                Id = employee.Id,
                Name = employee.Name,
                Surname = employee.Surname,
                Patronymic = employee.Patronymic,
                CityId = employee.CityId,
                StartDate = employee.StartDate,
                EndDate = employee.EndDate,
                MobilePhone = employee.MobilePhone,
                DateOfBirth = employee.DateOfBirth,
                Position = employee.Position,
                Phone = employee.Phone,
                Address = employee.Address,
                ManagerId = employee.ManagerId
            };
            
            using (HumanResourceEntities db = new HumanResourceEntities())
            {
                db.Entry(dbEmployee).State = EntityState.Modified;

                db.SaveChanges();
            }

            // перенаправляем на главную страницу
            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult Create()
        {
            LoadDropDowns(null);


            return View();
        }


        [HttpPost]
        public ActionResult Create(EmployeeModel employee)
        {
            if (!ModelState.IsValid)
                return Create();
                
            Employee dbEmployee = new Employee()
            {
                Name = employee.Name,
                Surname = employee.Surname,
                Patronymic = employee.Patronymic,
                CityId = employee.CityId,
                StartDate = employee.StartDate,
                EndDate = employee.EndDate,
                MobilePhone = employee.MobilePhone,
                DateOfBirth = employee.DateOfBirth,
                Position = employee.Position,
                Phone = employee.Phone,
                Address = employee.Address,
                ManagerId = employee.ManagerId
            };

            using (HumanResourceEntities db = new HumanResourceEntities())
            {
                db.Employees.Add(dbEmployee);
                db.SaveChanges();
            }

            // перенаправляем на главную страницу
            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return HttpNotFound();

            using (HumanResourceEntities db = new HumanResourceEntities())
            {
                Employee employee = new Employee { Id = id.Value };
                db.Entry(employee).State = EntityState.Deleted;
                db.SaveChanges();

                // перенаправляем на главную страницу
                return RedirectToAction("Index");
            }
        }

    }
}