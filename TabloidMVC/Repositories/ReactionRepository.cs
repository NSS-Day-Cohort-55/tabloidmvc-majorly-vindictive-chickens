using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection.PortableExecutable;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TabloidMVC.Models;
using TabloidMVC.Utils;

namespace TabloidMVC.Repositories
{
    public class ReactionRepository : BaseRepository, IReactionRepository
    {
        public ReactionRepository(IConfiguration config) : base(config) { }

        public List<Reaction> GetAllReactions()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT id, name, imageLocation FROM Reaction ORDER BY name";
                    var reader = cmd.ExecuteReader();

                    var reactions = new List<Reaction>();

                    while (reader.Read())
                    {
                        reactions.Add(new Reaction()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("id")),
                            Name = reader.GetString(reader.GetOrdinal("name")),
                            ImageLocation = reader.GetString(reader.GetOrdinal("imageLocation")),
                        });
                    }

                    reader.Close();

                    return reactions;
                }
            }
        }

        public Reaction GetReaction(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Name, ImageLocation
                                        FROM Reaction
                                        WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Reaction reaction = new Reaction()
                            {
                                Id = id,
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                ImageLocation = reader.GetString(reader.GetOrdinal("ImageLocation")),
                            };
                            return reaction;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        public void AddReaction(Reaction reaction)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Reaction (Name, ImageLocation)
                                        OUTPUT INSERTED.Id
                                        VALUES (@Name, @ImageLocation)";
                    cmd.Parameters.AddWithValue("@Name", reaction.Name);
                    cmd.Parameters.AddWithValue("@ImageLocation", reaction.ImageLocation);
                    int id = (int)cmd.ExecuteScalar();
                    reaction.Id = id;
                }
            }
        }

        public void UpdateReaction(Reaction reaction)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Reaction 
                                        SET Name = @Name,
                                            ImageLocation = @ImageLocation
                                        WHERE id = @id";

                    cmd.Parameters.AddWithValue("@Name", reaction.Name);
                    cmd.Parameters.AddWithValue("@ImageLocation", reaction.ImageLocation);
                    cmd.Parameters.AddWithValue("@id", reaction.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteReaction(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM Reaction WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }


    }

}
