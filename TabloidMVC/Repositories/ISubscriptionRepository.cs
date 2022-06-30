using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface ISubscriptionRepository
    {
        List<Subscription> GetAllSubscriptions();
        void Add(Subscription subscription);
        void Update(Subscription subscription);
    }
}
