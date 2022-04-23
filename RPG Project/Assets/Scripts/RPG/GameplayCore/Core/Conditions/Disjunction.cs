using System;
using UnityEngine;

namespace RPG.GameplayCore.Core.Conditions
{
    [Serializable]
    public class Disjunction
    {
        [SerializeField]
        private Predicate[] disjunctionSet;
        
        public bool Check(IPredicateEvaluator[] evaluators)
        {
            foreach (var predicate in disjunctionSet)
            {
                if (predicate.Check(evaluators)) return true;
            }

            return false;
        }
    }
}