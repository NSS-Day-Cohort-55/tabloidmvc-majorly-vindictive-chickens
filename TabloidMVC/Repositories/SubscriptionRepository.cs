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
    public class SubscriptionRepository : BaseRepository, ISubscriptionRepository
    {
        public SubscriptionRepository(IConfiguration config) : base(config) { }

        public List<Subscription> GetAllSubscriptions()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT s.Id, s.SubscriberUserProfileId, s.ProviderUserProfileId,
                              s.BeginDateTime, s.EndDateTime
                         FROM Subscription s";
                             
                    var reader = cmd.ExecuteReader();

                    var subscriptions = new List<Subscription>();

                    while (reader.Read())
                    {
                        subscriptions.Add(NewSubscriptionFromReader(reader));
                    }

                    reader.Close();

                    return subscriptions;
                }
            }
        }

        private Subscription NewSubscriptionFromReader(SqlDataReader reader)
        {
            return new Subscription()
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                SubscriberUserProfileId = reader.GetInt32(reader.GetOrdinal("SubscriberUserProfileId")),
                ProviderUserProfileId = reader.GetInt32(reader.GetOrdinal("ProviderUserProfileId")),
                BeginDateTime = reader.GetDateTime(reader.GetOrdinal("BeginDateTime")),
                EndDateTime = (DateTime)DbUtils.GetNullableDateTime(reader, "EndDateTime")
            };
        }

        public void Add(Subscription subscription)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO Subscription (
                            SubscriberUserProfileId, ProviderUserProfileId, BeginDateTime )
                        OUTPUT INSERTED.ID
                        VALUES ( @subscriberId, @providerId, @beginDateTime )";
                    cmd.Parameters.AddWithValue("@subscriberId", subscription.SubscriberUserProfileId);
                    cmd.Parameters.AddWithValue("@providerId", subscription.ProviderUserProfileId);
                    cmd.Parameters.AddWithValue("@beginDateTime", subscription.BeginDateTime);

                    subscription.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void Update(Subscription subscription)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Subscription 
                                        SET EndDateTime = @endDateTime
                                        WHERE SubscriberUserProfileId = @subscriberId AND 
                                              ProviderUserProfileId = @providerId";

                    cmd.Parameters.AddWithValue("@endDateTime", subscription.EndDateTime);
                    cmd.Parameters.AddWithValue("@subscriberId", subscription.SubscriberUserProfileId);
                    cmd.Parameters.AddWithValue("@providerId", subscription.ProviderUserProfileId);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
