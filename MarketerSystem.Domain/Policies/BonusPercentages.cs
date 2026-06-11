namespace MarketerSystem.Domain.Policies;

public static class BonusPercentages
{
    public const decimal DirectSeller = 0.10m;
    public const decimal Recommender = 0.05m;
    public const decimal GrandRecommender = 0.01m;

    public static decimal ForUplineLevel(int level) => level switch
    {
        0 => Recommender,
        1 => GrandRecommender,
        _ => 0m
    };
}
