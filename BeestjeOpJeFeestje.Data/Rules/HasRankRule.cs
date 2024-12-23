using BeestjeOpJeFeestje.Repository.Enums;
using BeestjeOpJeFeestje.Repository.Models;

namespace BeestjeOpJeFeestje.Data.Rules;

public class HasRankRule
{
    public int UserHasRank(User? user)
    {
        return user?.Rank != Rank.NONE ? 10 : 0;
    }
}