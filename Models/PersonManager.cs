namespace AspNetTasks.Models
{
    public class PersonManager
    {
        private List<Person> _persons;
        private PersonViewer _viewer;
        private IPersonsExtractorFromText _extractor;

        public PersonManager(IPersonsExtractorFromText extractor, IPersonPrinter printer)
        {
            _viewer = new PersonViewer(printer);
            _extractor = extractor;
        }

        public void LoadPersons(string filePath)
        {
            string fileText = File.ReadAllText(filePath);
            _persons = _extractor.Extract(fileText);
        }

        public void ViewAllPersons()
        {
            _persons.ForEach(_viewer.View);
        }
    }
}
