using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.GameplayCore.Core
{
    [Serializable]
    public class Condition
    {
        [SerializeField]
        private string predicate;

        [SerializeField]
        private string[] parameters;

        public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
        {
            foreach (var predicateEvaluator in evaluators)
            {
                bool? result  = predicateEvaluator.Evaluate(predicate, parameters);
                if (!result.HasValue) continue;

                if (!result.Value) return false;
            }

            return true;
        }
    }
}