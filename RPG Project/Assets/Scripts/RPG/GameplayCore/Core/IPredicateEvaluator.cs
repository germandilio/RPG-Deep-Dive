using RPG.GameplayCore.Core.Conditions;

namespace RPG.GameplayCore.Core
{
    public interface IPredicateEvaluator
    {
        bool? Evaluate(PredicateType predicate, string[] parameters);
    }
}