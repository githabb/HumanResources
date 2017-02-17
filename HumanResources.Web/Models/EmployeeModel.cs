using System;
using System.ComponentModel.DataAnnotations;

namespace HumanResources.Web.Models
{
    public class EmployeeModel
    {
        public int Id { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public string Address { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }

        public string MobilePhone { get; set; }
        public string Phone { get; set; }
        public int? ManagerId { get; set; }

        public string ManagerName { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
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