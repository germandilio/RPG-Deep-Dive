using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.GameplayCore.Core.Conditions
{
    /// <summary>
    /// Condition represents conjunctive normal form (CNF).
    /// </summary>
    /// <remarks>Link to wiki: https://en.wikipedia.org/wiki/Conjunctive_normal_form</remarks>
    [Serializable]
    public class Condition
    {
        [SerializeField]
        private Disjunction[] conjunctionSet;
        
        public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
        {
            var predicateEvaluators = evaluators.ToArray();

            foreach (var disjunction in conjunctionSet)
            {
                if (!disjunction.Check(predicateEvaluators)) return false;
            }

            return true;
        }
    }
}