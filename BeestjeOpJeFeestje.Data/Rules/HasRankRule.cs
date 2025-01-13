using BeestjeOpJeFeestje.Repository.Enums;
using BeestjeOpJeFeestje.Repository.Models;

namespace BeestjeOpJeFeestje.Data.Rules;

public class HasRankRule
{
    //10% discount if the user has a rank
    public int UserHasRank(User? user)
    {
        if (user == null)
        {
            return 0;
        }
        return user?.Rank != Rank.NONE ? 10 : 0;
    }
}