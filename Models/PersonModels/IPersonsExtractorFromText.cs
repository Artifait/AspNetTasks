
namespace AspNetTasks.Models.PersonModels
{
    public interface IPersonsExtractorFromText
    {
        public List<Person> Extract(string text);
    }

    public class PersonsExtractorFromJsonText : IPersonsExtractorFromText
    {
        public List<Person> Extract(string text)
        {
            return [];
        }
    }
}
