namespace AspNetTasks.Models
{
    public interface IPersonPrinter
    {
        public void Print(Person person);
    }

    public class PersonFilePrinter : IPersonPrinter
    {
        public void Print(Person person)
        {
            Console.WriteLine($"Print person(of name {person.Name}) to file");
        }
    }

    public class PersonScreenPrinter : IPersonPrinter
    {
        public void Print(Person person)
        {
            Console.WriteLine($"Print person(of name {person.Name}) to screen");
        }
    }

    public class PersonViewer
    {
        private IPersonPrinter _printer;
        public PersonViewer(IPersonPrinter printer)
        {
            _printer = printer;
        }

        public void View(Person person)
        {
            _printer.Print(person);
        }
    }
}
