using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface IUserProfileRepository
    {
        UserProfile GetByEmail(string email);
        List<UserProfile> GetAllUserProfiles();
        void Register(UserProfile newUserProfile);
        UserProfile GetUserProfileById(int userId);
        void UpdateUser(UserProfile userProfile);
        void DeactivateUser(UserProfile userProfile);
    }
}