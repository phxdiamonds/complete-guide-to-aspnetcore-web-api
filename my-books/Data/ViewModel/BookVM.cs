namespace my_books.Data.ViewModel
{
    public class BookVM
    {

        public string Title { get; set; }
        public string Description { get; set; }

        public bool IsRead { get; set; }

        public DateTime? DateRead { get; set; } // ? makes optional

        public int? Rate { get; set; }

        public string Genre { get; set; }

        public string CoverUrl { get; set; }

        public DateTime DateAdded { get; set; }

        public int PublisherId { get; set; }

        //A book can have multiple authors

        public List<int> AuthorIds { get; set; }
    }

    public class BookWithAuthorsVM
    {
  
        public string Title { get; set; }
        public string Description { get; set; }

        public bool IsRead { get; set; }

        public DateTime? DateRead { get; set; } // ? makes optional

        public int? Rate { get; set; }

        public string Genre { get; set; }

        public string CoverUrl { get; set; }

        public DateTime DateAdded { get; set; }

        public string Publishername { get; set; }

        //A book can have multiple authors

        public List<string> AuthorNames { get; set; }
    }
}
