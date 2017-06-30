using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class Employee
    {
        private String firstName;
        private String lastName;
        private int employeeId;
        private static int id = 1000;
        private int age;
        private double wage;
        private DateTime dateStartCareer;

        public Employee()
        {
            this.employeeId = id;
            this.lastName = "User #" + id;
            this.age = 200;
            this.wage = 20.00;
            this.dateStartCareer = new DateTime(1850, 1, 1);
            id++;
        }

        public Employee(String firstName, String lastName, int age, double wage, DateTime dateStartCareer)
        {
            this.employeeId = id;
            this.firstName = firstName;
            this.lastName = lastName;
            this.age = age;
            this.wage = wage;
            this.dateStartCareer = dateStartCareer;
            id++;
        }

        public String getFirstName()
        {
            return firstName;
        }

        public String getLastName()
        {
            return lastName;
        }

        public int getAge()
        {
            return age;
        }

        public double getWage()
        {
            return wage;
        }

        public DateTime getDateStartedCareer()
        {
            return dateStartCareer;
        }

        public int getEmployeeId()
        {
            return employeeId;
        }

        public void setFirstName(String firstName)
        {
            this.firstName = firstName;
        }

        public void setLastName(String lastName)
        {
            this.lastName = lastName;
        }

        public void setAge(int age)
        {
            this.age = age;
        }

        public void setWage(double wage)
        {
            this.wage = wage;
        }

        public String ToString()
        {
            return "Employee ID: " + employeeId + " Name: " + firstName + " " + lastName + " Age: " + age +
                " Wage: " + wage + " Date Started: " + dateStartCareer.ToShortDateString();
        }
    }
}
