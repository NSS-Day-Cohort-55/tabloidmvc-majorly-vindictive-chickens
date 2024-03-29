﻿using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TabloidMVC.Models;


namespace TabloidMVC.Repositories
{
    public class CommentRepository : BaseRepository, ICommentRepository
    {
        public CommentRepository(IConfiguration config) : base(config) { }
        public List<Comment> GetCommentByPostId(int postId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT c.Id, c.Subject, c.Content, c.PostId, c.CreateDateTime  
                    FROM Comment c 
                    LEFT JOIN UserProfile u ON c.UserProfileId = u.id 
                    Where c.PostId = @postId";

                    cmd.Parameters.AddWithValue("@postId", postId);
                    var reader = cmd.ExecuteReader();

                    List<Comment> comments = new List<Comment>();
                    while (reader.Read())
                    {
                        Comment comment = new Comment
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            PostId = reader.GetInt32(reader.GetOrdinal("PostId")),
                            Subject = reader.GetString(reader.GetOrdinal("Subject")),
                            Content = reader.GetString(reader.GetOrdinal("Content")),
                            CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime"))
                        };
                        comments.Add(comment);


                    }

                    reader.Close();
                    return comments;
                }
            }
        }

        public void Add(Comment comment)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        INSERT INTO Comment
                                        (PostId, UserProfileId, Subject, Content, CreateDateTime)
                                        OUTPUT INSERTED.ID
                                        VALUES (@PostId, @UserProfileId, @Subject, @Content, SysDateTime())";
                    cmd.Parameters.AddWithValue("@PostId", comment.PostId);
                    cmd.Parameters.AddWithValue("@UserProfileId", comment.UserProfileId);
                    cmd.Parameters.AddWithValue("@Subject", comment.Subject );
                    cmd.Parameters.AddWithValue("@Content", comment.Content);

                    comment.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void Delete(Comment comment)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM Comment WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", comment.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Comment GetCommentById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, PostId, UserProfileId, Subject, Content, CreateDateTime
                                        FROM Comment
                                        Where Id = @id";

                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Comment comment = new Comment
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                PostId = reader.GetInt32(reader.GetOrdinal("PostId")),
                                UserProfileId = reader.GetInt32(reader.GetOrdinal("UserProfileId")),
                                Subject = reader.GetString(reader.GetOrdinal("Subject")),
                                Content = reader.GetString(reader.GetOrdinal("Content")),
                                CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime"))
                            };

                            return comment;
                        }
                        else
                        {
                            return null; 
                        }
                    }
                }
            }
        }

        public void UpdateComment(Comment comment)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Comment
                                        Set Subject = @subject,
	                                        Content = @content
	                                    WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", comment.Id);
                    cmd.Parameters.AddWithValue("@subject", comment.Subject);
                    cmd.Parameters.AddWithValue("@content", comment.Content);

                    cmd.ExecuteNonQuery();
                }    
            }
        }

    }

}

