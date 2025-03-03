
namespace AspNetTasks.Models
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
