using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
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
        [InfoBox("Condition is a set of predicates, which located in special order.\n" +
                 "First array means logical 'AND' on his elements.\n" +
                 "Every nested array means logical 'OR' on his elements")]
        [Label("AND")]
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