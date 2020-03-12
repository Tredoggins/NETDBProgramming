using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MediaLibraryWithSearch
{
    public abstract class MyFile
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        // public property
        public string filePath { get; set; }
        public List<Media> Medias { get; set; }

        public MyFile(string path)
        {
            filePath = path;
            Medias = new List<Media>();
            if (!File.Exists(filePath))
            {
                logger.Info($"Creating file: {filePath}");
                StreamWriter sw = new StreamWriter(filePath);
                sw.WriteLine("");
                sw.Close();
            }
        }
        public virtual void addMedia(Media media)
        {
            // first generate movie id
            if (Medias.Count == 0)
            {
                media.mediaId = 1;
            }
            else
            {
                media.mediaId = Medias.Max(m => m.mediaId) + 1;
            }
            // if title contains a comma, wrap it in quotes
            StreamWriter sw = new StreamWriter(filePath, true);
            sw.WriteLine(media.FileDisplay());
            sw.Close();
            // add movie details to Lists
            Medias.Add(media);
            // log transaction


        }
        public virtual List<Media> searchByTitle(string titlePart)
        {
            return Medias.Where(m => m.title.Contains(titlePart)).ToList();
        }
    }
    public class MovieFile : MyFile
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public MovieFile(string path) : base(path)
        {
            try
            {
                StreamReader sr = new StreamReader(filePath);
                // first line contains column headers
                sr.ReadLine();
                while (!sr.EndOfStream)
                {
                    // create instance of Movie class
                    Movie movie = new Movie();
                    string line = sr.ReadLine();
                    // first look for quote(") in string
                    // this indicates a comma(,) in movie title
                    int idx = line.IndexOf('"');
                    if (idx == -1)
                    {
                        // no quote = no comma in movie title
                        // movie details are separated with comma(,)
                        string[] movieDetails = line.Split(',');
                        movie.mediaId = UInt64.Parse(movieDetails[0]);
                        movie.title = movieDetails[1];
                        movie.genres = movieDetails[2].Split('|').ToList();
                        movie.director = movieDetails[3];
                        string[] timeBits = movieDetails[4].Split(":");
                        movie.runningTime = new TimeSpan(int.Parse(timeBits[0]), int.Parse(timeBits[1]), int.Parse(timeBits[2]));
                    }
                    else
                    {
                        // quote = comma in movie title
                        // extract the movieId
                        movie.mediaId = UInt64.Parse(line.Substring(0, idx - 1));
                        // remove movieId and first quote from string
                        line = line.Substring(idx + 1);
                        // find the next quote
                        idx = line.LastIndexOf('"');
                        // extract the movieTitle
                        movie.title = line.Substring(0, idx).Replace("\"\"", "\"");
                        // remove title and last comma from the string
                        line = line.Substring(idx + 2);
                        // replace the "|" with ", "
                        List<string> genres = line.Split('|').ToList();
                        List<string> next = genres[genres.Count - 1].Split(",").ToList();
                        genres[genres.Count - 1] = next[0];
                        movie.genres = genres;
                        movie.director = next[1];
                        string[] timeBits = next[2].Split(":");
                        movie.runningTime = new TimeSpan(int.Parse(timeBits[0]), int.Parse(timeBits[1]), int.Parse(timeBits[2]));
                    }
                    Medias.Add(movie);
                }
                // close file when done
                sr.Close();
                logger.Info("Movies in file {Count}", Medias.Count);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }
        public bool isUniqueTitle(string title)
        {
            if (Medias.ConvertAll(m => m.title.ToLower()).Contains(title.ToLower()))
            {
                logger.Info("Duplicate movie title {Title}", title);
                return false;
            }
            return true;
        }
        public override void addMedia(Media media)
        {
            try
            {
                base.addMedia(media);
                logger.Info("Movie id {Id} added", media.mediaId);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }
    }
    public class AlbumFile : MyFile
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public AlbumFile(string path) : base(path)
        {
            try
            {
                StreamReader sr = new StreamReader(filePath);
                // first line contains column headers
                sr.ReadLine();
                while (!sr.EndOfStream)
                {
                    // create instance of Movie class
                    Album album = new Album();
                    string line = sr.ReadLine();
                    // first look for quote(") in string
                    // this indicates a comma(,) in movie title
                    int idx = line.IndexOf('"');
                    if (idx == -1)
                    {
                        // no quote = no comma in album title
                        // album details are separated with comma(,)
                        string[] albumDetails = line.Split(',');
                        album.mediaId = UInt64.Parse(albumDetails[0]);
                        album.title = albumDetails[1];
                        album.genres = albumDetails[2].Split('|').ToList();
                        album.artist = albumDetails[3];
                        album.recordLabel = albumDetails[4];
                    }
                    else
                    {
                        // quote = comma in album title
                        // extract the albumId
                        album.mediaId = UInt64.Parse(line.Substring(0, idx - 1));
                        // remove albumId and first quote from string
                        line = line.Substring(idx + 1);
                        // find the next quote
                        idx = line.LastIndexOf('"');
                        // extract the albumTitle
                        album.title = line.Substring(0, idx).Replace("\"\"", "\"");
                        // remove title and last comma from the string
                        line = line.Substring(idx + 2);
                        // replace the "|" with ", "
                        List<string> genres = line.Split('|').ToList();
                        List<string> next = genres[genres.Count - 1].Split(",").ToList();
                        genres[genres.Count - 1] = next[0];
                        album.genres = genres;
                        album.artist = next[1];
                        album.recordLabel = next[2];
                    }
                    Medias.Add(album);
                }
                // close file when done
                sr.Close();
                logger.Info("Albums in file {Count}", Medias.Count);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }
        public override void addMedia(Media media)
        {
            try
            {
                base.addMedia(media);
                logger.Info("Album id {Id} added", media.mediaId);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }
    }
    public class BookFile : MyFile
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public BookFile(string path) : base(path)
        {
            try
            {
                StreamReader sr = new StreamReader(filePath);
                // first line contains column headers
                sr.ReadLine();
                while (!sr.EndOfStream)
                {
                    // create instance of Movie class
                    Book book = new Book();
                    string line = sr.ReadLine();
                    // first look for quote(") in string
                    // this indicates a comma(,) in movie title
                    int idx = line.IndexOf('"');
                    if (idx == -1)
                    {
                        // no quote = no comma in movie title
                        // movie details are separated with comma(,)
                        string[] bookDetails = line.Split(',');
                        book.mediaId = UInt64.Parse(bookDetails[0]);
                        book.title = bookDetails[1];
                        book.genres = bookDetails[2].Split('|').ToList();
                        book.author = bookDetails[3];
                        book.publisher = bookDetails[4];
                        book.pageCount = ushort.Parse(bookDetails[5]);
                    }
                    else
                    {
                        // quote = comma in movie title
                        // extract the movieId
                        book.mediaId = UInt64.Parse(line.Substring(0, idx - 1));
                        // remove bookId and first quote from string
                        line = line.Substring(idx + 1);
                        // find the next quote
                        idx = line.LastIndexOf('"');
                        // extract the bookTitle
                        book.title = line.Substring(0, idx).Replace("\"\"", "\"");
                        // remove title and last comma from the string
                        line = line.Substring(idx + 2);
                        // replace the "|" with ", "
                        List<string> genres = line.Split('|').ToList();
                        List<string> next = genres[genres.Count - 1].Split(',').ToList();
                        genres[genres.Count - 1] = next[0];
                        book.genres = genres;
                        book.author = next[1];
                        book.publisher = next[2];
                        book.pageCount = ushort.Parse(next[3]);

                    }
                    Medias.Add(book);
                }
                // close file when done
                sr.Close();
                logger.Info("Books in file {Count}", Medias.Count);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }
        public override void addMedia(Media media)
        {
            try
            {
                base.addMedia(media);
                logger.Info("Book id {Id} added", media.mediaId);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }
    }
}
