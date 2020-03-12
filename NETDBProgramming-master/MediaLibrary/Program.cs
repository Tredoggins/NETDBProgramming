using System;
using NLog;

namespace MediaLibrary
{
    class MainClass
    {
        // create a class level instance of logger (can be used in methods other than Main)
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static void Main(string[] args)
        {
            logger.Info("Program started");
            string scrubbedFile = FileScrubber.ScrubMovies("../../../movies.csv");
            MovieFile movieFile = new MovieFile(scrubbedFile);
            AlbumFile albumFile = new AlbumFile("../../../albums.csv");
            BookFile bookFile = new BookFile("../../../books.csv");
            string choice = "";
            string which = "";
            do
            {
                Console.WriteLine("1) Movies");
                Console.WriteLine("2) Albums");
                Console.WriteLine("3) Books");
                Console.WriteLine("Enter to quit");
                which = Console.ReadLine();
                do
                {
                    if (which == "1")
                    {
                        // display choices to user
                        Console.WriteLine("1) Add Movie");
                        Console.WriteLine("2) Display All Movies");
                        Console.WriteLine("Enter to quit");
                        // input selection
                        choice = Console.ReadLine();
                        logger.Info("User choice: {Choice}", choice);
                        if (choice == "1")
                        {
                            // Add movie
                            Movie movie = new Movie();
                            // ask user to input movie title
                            Console.WriteLine("Enter movie title");
                            // input title
                            movie.title = Console.ReadLine();
                            // verify title is unique
                            if (movieFile.isUniqueTitle(movie.title))
                            {
                                // input genres
                                string input;
                                do
                                {
                                    // ask user to enter genre
                                    Console.WriteLine("Enter genre (or done to quit)");
                                    // input genre
                                    input = Console.ReadLine();
                                    // if user enters "done"
                                    // or does not enter a genre do not add it to list
                                    if (input != "done" && input.Length > 0)
                                    {
                                        movie.genres.Add(input);
                                    }
                                } while (input != "done");
                                // specify if no genres are entered
                                if (movie.genres.Count == 0)
                                {
                                    movie.genres.Add("(no genres listed)");
                                }
                                Console.WriteLine("Enter Director: ");
                                movie.director = Console.ReadLine();
                                Console.WriteLine("Enter Runtime Hours: ");
                                int hrs = int.Parse(Console.ReadLine());
                                Console.WriteLine("Enter Runtime Minutes: ");
                                int mins = int.Parse(Console.ReadLine());
                                Console.WriteLine("Enter Runtime Seconds: ");
                                int secs = int.Parse(Console.ReadLine());
                                movie.runningTime = new TimeSpan(hrs, mins, secs);
                                // add movie
                                movieFile.addMedia(movie);
                            }
                            else
                            {
                                Console.WriteLine("Movie title already exists\n");
                            }

                        }
                        else if (choice == "2")
                        {
                            // Display All Movies
                            foreach (Media m in movieFile.Medias)
                            {
                                Console.WriteLine(m.Display());
                            }
                        }
                    }
                    if (which == "2")
                    {
                        // display choices to user
                        Console.WriteLine("1) Add Album");
                        Console.WriteLine("2) Display All Albums");
                        Console.WriteLine("Enter to quit");
                        // input selection
                        choice = Console.ReadLine();
                        logger.Info("User choice: {Choice}", choice);
                        if (choice == "1")
                        {
                            // Add movie
                            Album album = new Album();
                            // ask user to input album title
                            Console.WriteLine("Enter album title");
                            // input title
                            album.title = Console.ReadLine();
                            // input genres
                            string input;
                            do
                            {
                                // ask user to enter genre
                                Console.WriteLine("Enter genre (or done to quit)");
                                // input genre
                                input = Console.ReadLine();
                                // if user enters "done"
                                // or does not enter a genre do not add it to list
                                if (input != "done" && input.Length > 0)
                                {
                                    album.genres.Add(input);
                                }
                            } while (input != "done");
                            // specify if no genres are entered
                            if (album.genres.Count == 0)
                            {
                                album.genres.Add("(no genres listed)");
                            }
                            Console.WriteLine("Enter Artist: ");
                            album.artist = Console.ReadLine();
                            Console.WriteLine("Enter Record Label: ");
                            album.recordLabel = Console.ReadLine();
                            // add album
                            albumFile.addMedia(album);

                        }
                        else if (choice == "2")
                        {
                            // Display All albums
                            foreach (Media m in albumFile.Medias)
                            {
                                Console.WriteLine(m.Display());
                            }
                        }
                    }
                    if (which == "3")
                    {
                        // display choices to user
                        Console.WriteLine("1) Add Book");
                        Console.WriteLine("2) Display All Books");
                        Console.WriteLine("Enter to quit");
                        // input selection
                        choice = Console.ReadLine();
                        logger.Info("User choice: {Choice}", choice);
                        if (choice == "1")
                        {
                            // Add movie
                            Book book = new Book();
                            // ask user to input book title
                            Console.WriteLine("Enter book title");
                            // input title
                            book.title = Console.ReadLine();
                            // input genres
                            string input;
                            do
                            {
                                // ask user to enter genre
                                Console.WriteLine("Enter genre (or done to quit)");
                                // input genre
                                input = Console.ReadLine();
                                // if user enters "done"
                                // or does not enter a genre do not add it to list
                                if (input != "done" && input.Length > 0)
                                {
                                    book.genres.Add(input);
                                }
                            } while (input != "done");
                            // specify if no genres are entered
                            if (book.genres.Count == 0)
                            {
                                book.genres.Add("(no genres listed)");
                            }
                            Console.WriteLine("Enter Author: ");
                            book.author = Console.ReadLine();
                            Console.WriteLine("Enter Publisher: ");
                            book.publisher = Console.ReadLine();
                            Console.WriteLine("Enter Page Count: ");
                            book.pageCount = ushort.Parse(Console.ReadLine());
                            // add book
                            bookFile.addMedia(book);

                        }
                        else if (choice == "2")
                        {
                            // Display All books
                            foreach (Media m in bookFile.Medias)
                            {
                                Console.WriteLine(m.Display());
                            }
                        }
                    }
                } while (choice == "1" || choice == "2");
            } while (which == "1" || which == "2" || which == "3");

            logger.Info("Program ended");
        }
    }
}