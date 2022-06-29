using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface IReactionRepository
    {
        List<Reaction> GetAllReactions();
        public Reaction GetReaction(int id);
        void AddReaction(Reaction Reaction);
        void UpdateReaction(Reaction Reaction);
        void DeleteReaction(int id);
    }
}
