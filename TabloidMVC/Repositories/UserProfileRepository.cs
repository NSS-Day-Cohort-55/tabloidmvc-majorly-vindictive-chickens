using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using TabloidMVC.Models;
using TabloidMVC.Utils;
using System;

namespace TabloidMVC.Repositories
{
    public class UserProfileRepository : BaseRepository, IUserProfileRepository
    {
        public UserProfileRepository(IConfiguration config) : base(config) { }

        public UserProfile GetByEmail(string email)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT u.id, u.FirstName, u.LastName, u.DisplayName, u.Email,
                              u.CreateDateTime, u.ImageLocation, u.UserTypeId,
                              ut.[Name] AS UserTypeName
                         FROM UserProfile u
                              LEFT JOIN UserType ut ON u.UserTypeId = ut.id
                        WHERE email = @email";
                    cmd.Parameters.AddWithValue("@email", email);

                    UserProfile userProfile = null;
                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        userProfile = new UserProfile()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            DisplayName = reader.GetString(reader.GetOrdinal("DisplayName")),
                            CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                            ImageLocation = DbUtils.GetNullableString(reader, "ImageLocation"),
                            UserTypeId = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                            UserType = new UserType()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                                Name = reader.GetString(reader.GetOrdinal("UserTypeName"))
                            },
                        };
                    }

                    reader.Close();

                    return userProfile;
                }
            }
        }
        public List<UserProfile> GetAllUserProfiles()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT up.Id, up.FirstName, up.LastName, up.DisplayName, up.UserTypeId, 
                              ut.Name AS UserType
                         FROM UserProfile up
                              LEFT JOIN UserType ut ON ut.Id = up.UserTypeId
                         ORDER BY up.DisplayName";
               
                    var reader = cmd.ExecuteReader();

                    var userprofiles = new List<UserProfile>();

                    while (reader.Read())
                    {
                        UserProfile userProfile = new UserProfile
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            DisplayName = reader.GetString(reader.GetOrdinal("DisplayName")),
                            UserTypeId = reader.GetInt32(reader.GetOrdinal("UserTypeId")),

                            UserType = new UserType { Name = reader.GetString(reader.GetOrdinal("UserType")) }
                        };
                        userprofiles.Add(userProfile);
                    }
                    return userprofiles;
                }
            }
        }

        public void Register(UserProfile newUserProfile)
        {
            using (var conn = Connection)
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Userprofile 
                                            ( FirstName, LastName, DisplayName, Email, CreateDateTime, UserTypeId )
                                            OUTPUT INSERTED.Id
                                            VALUES (@firstName, @lastName, @displayName, @email, @dateTime, @userTypeId)";
                    cmd.Parameters.AddWithValue("@firstName", newUserProfile.FirstName);
                    cmd.Parameters.AddWithValue("@lastName", newUserProfile.LastName);
                    cmd.Parameters.AddWithValue("@displayName", newUserProfile.DisplayName);
                    cmd.Parameters.AddWithValue("@email", newUserProfile.Email);
                    cmd.Parameters.AddWithValue("@dateTime", newUserProfile.CreateDateTime);
                    cmd.Parameters.AddWithValue("@userTypeId", newUserProfile.UserTypeId);

                    int id = (int)cmd.ExecuteScalar();

                    newUserProfile.Id = id;

                }
            }
        }
       
        public UserProfile GetUserProfileById(int userId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT u.id, u.FirstName, u.LastName, u.DisplayName, u.Email,
                              u.CreateDateTime, u.ImageLocation, u.UserTypeId,
                              ut.[Name] AS UserTypeName
                         FROM UserProfile u
                              LEFT JOIN UserType ut ON u.UserTypeId = ut.id
                        WHERE u.Id = @id";
                    cmd.Parameters.AddWithValue("@id", userId);

                    UserProfile userProfile = null;
                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        userProfile = new UserProfile()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            DisplayName = reader.GetString(reader.GetOrdinal("DisplayName")),
                            CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                            ImageLocation = DbUtils.GetNullableString(reader, "ImageLocation"),
                            UserTypeId = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                            UserType = new UserType()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                                Name = reader.GetString(reader.GetOrdinal("UserTypeName"))
                            },
                        };
                    }

                    return userProfile;
                }
            }
        }

        public void UpdateUser(UserProfile userProfile)
        {
            using (var conn = Connection)
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            UPDATE UserProfile
                            SET 
                                FirstName = @firstName, 
                                LastName = @lastName,
                                DisplayName = @displayName,
                                Email = @email, 
                                UserTypeId = @userTypeId, 
                                ImageLocation = @imageLocation
                            WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@firstName", userProfile.FirstName);
                    cmd.Parameters.AddWithValue("@lastName", userProfile.LastName);
                    cmd.Parameters.AddWithValue("@displayName", userProfile.DisplayName);
                    cmd.Parameters.AddWithValue("@email", userProfile.Email);
                    cmd.Parameters.AddWithValue("@userTypeId", userProfile.UserTypeId);
                    cmd.Parameters.AddWithValue("@id", userProfile.Id);

                    cmd.Parameters.AddWithValue("@imageLocation", !String.IsNullOrEmpty(userProfile.ImageLocation) ? (object)userProfile.ImageLocation : DBNull.Value);

                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
