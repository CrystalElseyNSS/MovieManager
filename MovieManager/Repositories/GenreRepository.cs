using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using MovieManager.Models;

namespace GenreManager.Repositories
{
    public class GenreRepository
    {
        private readonly string _connectionString;

        public GenreRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public List<Genre> GetAll()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Name FROM Genre";
                    var reader = cmd.ExecuteReader();

                    var genres = new List<Genre>();
                    while (reader.Read())
                    {
                        genres.Add(new Genre
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                        });
                    }
                    reader.Close();

                    return genres;
                }
            }
        }

        public Genre GetById(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Name FROM Genre WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    var reader = cmd.ExecuteReader();

                    Genre genre = null;
                    if (reader.Read())
                    {
                        genre = new Genre
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                        };
                    }
                    reader.Close();

                    return genre;
                }
            }
        }

        public GenreWithMovies GetWithMoviesById(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT g.Id AS GenreId, g.Name,
                               m.Id AS MovieId, m.Title, m.Url, m.Rating, m.Year 
                          FROM Genre g LEFT JOIN Movie m on  g.Id = m.GenreId
                         WHERE g.Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    var reader = cmd.ExecuteReader();

                    GenreWithMovies genre = null;
                    while (reader.Read())
                    {
                        if (genre == null)
                        {
                            genre = new GenreWithMovies
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("GenreId")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                            };
                        }

                        if (!reader.IsDBNull(reader.GetOrdinal("MovieId")))
                        {
                            genre.Movies.Add(new Movie
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("MovieId")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                Url = reader.GetString(reader.GetOrdinal("Url")),
                                Year = reader.GetInt32(reader.GetOrdinal("Year")),
                                Rating = reader.GetDecimal(reader.GetOrdinal("Rating")),
                                GenreId = reader.GetInt32(reader.GetOrdinal("GenreId")),
                            });
                        }
                    }
                    reader.Close();

                    return genre;
                }
            }
        }

        public void Add(Genre genre)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Genre (Name)
                                        OUTPUT INSERTED.Id
                                        VALUES (@name)";
                    cmd.Parameters.AddWithValue("@name", genre.Name);
                    genre.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void Update(Genre genre)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Genre 
                                           SET Name = @name
                                         WHERE id = @id";
                    cmd.Parameters.AddWithValue("@name", genre.Name);
                    cmd.Parameters.AddWithValue("@id", genre.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Remove(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        DELETE FROM Movie WHERE GenreId = @id;
                        DELETE FROM Genre WHERE Id = @id;
                    ";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
