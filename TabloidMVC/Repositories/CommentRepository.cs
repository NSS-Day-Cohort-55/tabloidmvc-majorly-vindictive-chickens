﻿using System.Collections.Generic;
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
                    cmd.CommandText = @"SELECT c.Subject, c.Content,         c.CreateDateTime  
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
                            PostId = reader.GetInt32(reader.GetOrdinal("PostId")),
                            Subject = reader.GetString(reader.GetOrdinal("Subject")),
                            Content = reader.GetString(reader.GetOrdinal("Content")),
                            CreatedDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime"))
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
                                        PostId, UserProfileId, Subject, Content
                                        OUPUT INSERTED.ID
                                        VALUES (@Subject, @Content)";
                    cmd.Parameters.AddWithValue("@PostId", comment.PostId);
                    cmd.Parameters.AddWithValue("@UserProfileId", comment.UserProfileId);
                    cmd.Parameters.AddWithValue("@Subject", comment.Subject );
                    cmd.Parameters.AddWithValue("@Content", comment.Content);

                    comment.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

    }

}

