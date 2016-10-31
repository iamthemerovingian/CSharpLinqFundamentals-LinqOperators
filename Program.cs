using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Diagnostics;

namespace LinqOperators
{
    class Program
    {
        static void Main(string[] args)
        {
            Filtering();
            Sorting();
            Sets();
            Quantifiers();
            Projection();
            Joins();
            Grouping();
            Generation();
            Aggregation();
        }

        private static void Aggregation()
        {
            Employee employee = new Employee { ID = 2 };

            var rules = new List<Rule<Employee>>()
            {
                new Rule<Employee> { Test= e => !String.IsNullOrEmpty(e.Name), 
                                     Message= "Employee name cannot be empty" },

                new Rule<Employee> { Test = e => e.DepartmentID > 0, 
                                     Message = "Employee must have an assigned department"},

                new Rule<Employee> { Test = e => e.ID > 0, 
                                     Message = "Employee must have an ID"}
            };


            bool isValid = rules.All(r => r.Test(employee));

            if (!isValid)
            {
                var failedRules = rules.Where(r => r.Test(employee) == false);

                string errorMessage =
                            failedRules.Aggregate(new StringBuilder(), 
                                        (sb, r) => sb.AppendLine(r.Message), 
                                        sb => sb.ToString());
            }

        }

        private static void Generation()
        {
            var employees = new List<Employee> {
                new Employee { ID=1, Name="Scott", DepartmentID=1 },
                new Employee { ID=2, Name="Poonam", DepartmentID=1 },
                new Employee { ID=3, Name="Andy", DepartmentID=2}
            };

            var departments = new List<Department> {
                new Department { ID=1, Name="Engineering" },
                new Department { ID=2, Name="Sales" },
                new Department { ID=3, Name="Skunkworks" }
            };

            var query =
                from d in departments
                join e in employees on d.ID equals e.DepartmentID
                    into eg
                from e in eg.DefaultIfEmpty()
                select new
                {
                    DepartmentName = d.Name, 
                    Employee = e
                };
        }

        private static void Grouping()
        {
            var employees = new List<Employee> {
                new Employee { ID=1, Name="Scott", DepartmentID=1 },
                new Employee { ID=2, Name="Poonam", DepartmentID=1 },
                new Employee { ID=3, Name="Andy", DepartmentID=2}
            };


            var query1 =
                from e in employees
                group e by e.DepartmentID into eg
                select new
                {
                    DepartmentID = eg.Key, 
                    Employees = eg
                };

            var query2 =
                  employees.GroupBy(e => e.DepartmentID)
                           .Select(eg => new
                           {
                               DepartmentID = eg.Key,
                               Employees = eg
                           });                                     
        }

        private static void Joins()
        {
            var employees = new List<Employee> {
                new Employee { ID=1, Name="Scott", DepartmentID=1 },
                new Employee { ID=2, Name="Poonam", DepartmentID=1 },
                new Employee { ID=3, Name="Andy", DepartmentID=2}
            };

            var departments = new List<Department> {
                new Department { ID=1, Name="Engineering" },
                new Department { ID=2, Name="Sales" },
                new Department { ID=3, Name="Skunkworks" }
            };

            var query1 =
                from d in departments
                join e in employees on d.ID equals e.DepartmentID
                select new
                {
                    DepartmentName = d.Name,
                    EmployeeName = e.Name 
                };

            var query2 =
                departments.Join(employees,
                                d => d.ID,
                                e => e.DepartmentID,
                                (d, e) => new
                                {
                                    DepartmentName = d.Name,
                                    EmployeeName = e.Name 
                                });

            var query3 =
                departments.GroupJoin(employees,
                                d => d.ID,
                                e => e.DepartmentID,
                                (d, eg) => new
                                {
                                    DepartmentName = d.Name,
                                    Employees = eg
                                });

            var query4 =
                from d in departments
                join e in employees on d.ID equals e.DepartmentID
                    into eg
                select new
                {
                    DepartmentName = d.Name,
                    Employees = eg
                };

        }

        private static void Projection()
        {
            string[] famousQuotes = 
            {
                "Advertising is legalized lying",
                "Advertising is the greatest art form of the twentieth century"
            };

            var query =
                    (from sentence in famousQuotes
                     from word in sentence.Split(' ')
                     select word).Distinct();

            var query2 =
                   famousQuotes.SelectMany(s => s.Split(' '));

        }

        private static void Quantifiers()
        {
            Book book = new Book { Author = "Herman", Name = "Moby Dick" };

            var bookValidationRules = new List<Func<Book, bool>>()
            {
                b => !String.IsNullOrEmpty(b.Name),
                b => !String.IsNullOrEmpty(b.Author),
            };

            bool isBookValid = bookValidationRules.All(rule => rule(book));

        }
 
        private static void Sets()
        {
            var books = new List<Book>
            {
                new Book { Author="Scott", Name="Programming WF" },
                new Book { Author="Fritz", Name="Essential ASP.NET" },
                new Book { Author="Scott", Name="Programming WF" }
            };

            var query = books
                        .Select(b => new { b.Name, b.Author })
                        .Distinct();

            foreach (var book in query)
            {
                Console.WriteLine(book.Name);
            }
        }

        private static void Sorting()
        {
            var query = Process.GetProcesses()
                        .OrderBy(p => p.WorkingSet64)
                        .ThenByDescending(p => p.Threads.Count);

            // error because ThenBy returns IOrderedEnumerable, but 
            // Where return IEnumerable
            //query = query.Where(p => p.ProcessName.Length < 10);

            
        }

        private static void Filtering()
        {
            object[] things = { "Glasses", 2, "Books" };
            ArrayList listOfThings = new ArrayList(things);

            var query =
                things.OfType<string>().Where(s => s.Length < 6);

        }
    }
}
