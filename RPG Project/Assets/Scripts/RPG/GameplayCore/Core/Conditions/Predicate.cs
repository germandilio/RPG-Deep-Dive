using System;
using UnityEngine;

namespace RPG.GameplayCore.Core.Conditions
{
    [Serializable]
    public class Predicate
    {
        [SerializeField]
        private PredicateType predicate;

        [Tooltip("Used for 'not {predicate}'. (ex. not HasQuest)")]
        [SerializeField]
        private bool negate;

        [Tooltip("Arguments to pass in predicate")]
        [SerializeField]
        private string[] parameters;

        public bool Check(IPredicateEvaluator[] evaluators)
        {
            foreach (var predicateEvaluator in evaluators)
            {
                bool? result  = predicateEvaluator.Evaluate(predicate, parameters);
                if (!result.HasValue) continue;

                if (result.Value == negate) return false; 
            }

            return true;
        }
    }
}