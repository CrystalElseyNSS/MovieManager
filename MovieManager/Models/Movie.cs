namespace MovieManager.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public decimal Rating { get; set; }
        public int Year { get; set; }
        public int GenreId { get; set; }
        public Genre Genre { get; set; }
    }
}
