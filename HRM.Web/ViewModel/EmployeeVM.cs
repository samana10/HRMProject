namespace HRM.Web.ViewModel
{
    public partial class Employee
    {
        public int Id { get; set; }

        public string EmployeeName { get; set; } = null!;

        public string Address { get; set; } = null!;

        public int Age { get; set; }

        public string Designation { get; set; } = null!;

        public int Salary { get; set; }

        public string Department { get; set; } = null!;
    }
}
