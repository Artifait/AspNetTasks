
namespace AspNetTasks.Models.BookModels
{
    public interface IBooksExtractorFromText
    {
        List<Book> Extract(string text);
    }

    public class BooksExtractorFromSimpleText : IBooksExtractorFromText
    {
        public List<Book> Extract(string text)
        {
            var books = new List<Book>();
            var lines = text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < lines.Length; i += 5)
            {
                string title = lines[i];
                string author = lines[i + 1];
                string genre = lines[i + 2];
                int year = int.Parse(lines[i + 3]);
                string additionalInfo = lines[i + 4];

                books.Add(new Book(title, author, genre, year, additionalInfo));
            }

            return books;
        }
    }

    public class BooksExtractorFromTextWithMultipleLineSeparators : IBooksExtractorFromText
    {
        public List<Book> Extract(string text)
        {
            var books = new List<Book>();
            var lines = text.Split(new[] { "\r\n\r\n", "\n\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var block in lines)
            {
                var details = block.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                if (details.Length >= 5)
                {
                    string title = details[0];
                    string author = details[1];
                    string genre = details[2];
                    int year = int.Parse(details[3]);
                    string additionalInfo = details[4];

                    books.Add(new Book(title, author, genre, year, additionalInfo));
                }
            }

            return books;
        }
    }
}
