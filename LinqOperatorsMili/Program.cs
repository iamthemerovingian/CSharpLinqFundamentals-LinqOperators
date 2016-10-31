using LinqOperators;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqOperatorsMili
{
    class Program
    {
        static void Main(string[] args)
        {
            //Filetering();
            //Sorting();
            //Sets();
            //Quantifiers();
            //Projection();

        }

        private static void Projection()
        {
            string[] famousQuotes =
            {
                "Advertising is legalized lying",
                "Advertising is the greatest art form of the twentieth century"
            };

            var query1 =
                (from sentance in famousQuotes
                from word in sentance.Split(' ')
                select word).Distinct();

            var query2 = famousQuotes.SelectMany(sentance => sentance.Split(' ')).Distinct();
       }

        private static void Quantifiers()
        {
            Book book = new Book { Author = "Herman", Name = "Moby Dick" };

            var bookValidationRules = new List<Func<Book, bool>>()
            {
                b => !string.IsNullOrWhiteSpace(b.Name),
                b => !string.IsNullOrWhiteSpace(b.Author)
            };

            bool isBookValud = bookValidationRules.All(rule => rule(book));
        }

        private static void Sets()
        {
            var books = new List<Book>
            {
                new Book { Author="Scott", Name="Programming WF" },
                new Book { Author="Fritz", Name="Essential ASP.NET" },
                new Book { Author="Scott", Name="Programming WF" }
            };

            var query = books.Select(b => new{ b.Name, b.Author}) // Calling distince on an anonymous class will cause the distinct method to check each property and decide equals or not.
                             .Distinct(); // In the case of normal classes, it will check the object thoroghly, so even if 2 objects have equal fields, they will not be considered equal.

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

            foreach (var item in query)
            {
                Console.WriteLine(item.WorkingSet64);
            }
        }

        private static void Filetering()
        {
            object[] things = { "Glasses", 2, "Books" };

            ArrayList listOfThings = new ArrayList(things);

            var query =
                listOfThings.OfType<string>().Where(s => s.Length < 6);

            Console.WriteLine(query);
        }
    }
}
