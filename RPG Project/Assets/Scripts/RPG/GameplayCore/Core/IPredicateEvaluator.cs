namespace RPG.GameplayCore.Core
{
    public interface IPredicateEvaluator
    {
        bool? Evaluate(string predicate, string[] parameters);
    }
}