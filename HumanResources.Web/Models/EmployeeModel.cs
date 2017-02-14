using System;

namespace HumanResources.Web.Models
{
    public class EmployeeModel
    {
        public int Id { get; set; }
        public int CityId { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public string Address { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string MobilePhone { get; set; }
        public string Phone { get; set; }
        public int? ManagerId { get; set; }

        public string ManagerName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Position { get; set; }


        public static string GetDisplayName(string surname, string name, string patronymic)
        {
            string result = surname + " " + name[0] + ".";

            if (!string.IsNullOrEmpty(patronymic))
                result += patronymic[0] + ".";

            return result;
        }
    }
}