﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lucy.PatternMatchers.Matchers;

namespace Lucy.PatternMatchers
{
    /// <summary>
    /// Will match any unclaimed tokens
    /// </summary>
    public class WildcardPatternMatcher : PatternMatcher
    {
        public const string ENTITYTYPE = "wildcard";

        private string entityType = ENTITYTYPE;

        public WildcardPatternMatcher(string variation = null)
        {
            if (variation != null && variation.IndexOf(":") > 0)
            {
                entityType = variation.Split(':').First().Trim();
            }
        }

        public override MatchResult Matches(MatchContext context, int start)
        {
            var matchResult = new MatchResult();

            var token = context.FindNextTextEntity(start);
            if (token != null)
            {
                // we add wildcardtoken on first token, and then get it and keep appending until we decide we are done.
                var wildcardToken = context.CurrentEntity.Children.FirstOrDefault(entity => entity.Type == entityType);
                if (wildcardToken == null)
                {
                    wildcardToken = new LucyEntity()
                    {
                        Type = entityType,
                        Start = token.Start
                    };
                    context.CurrentEntity.Children.Add(wildcardToken);
                }

                // update wildcardToken
                wildcardToken.End = token.End;
                wildcardToken.Resolution = context.Text.Substring(wildcardToken.Start, wildcardToken.End - wildcardToken.Start);
                wildcardToken.Text = context.Text.Substring(wildcardToken.Start, wildcardToken.End - wildcardToken.Start);

                // update parent token 
                context.CurrentEntity.End = token.End;
                // context.CurrentEntity.Resolution = context.Text.Substring(wildcardToken.Start, wildcardToken.End - wildcardToken.Start);
                // context.CurrentEntity.Text = context.Text.Substring(context.CurrentEntity.Start, context.CurrentEntity.End - context.CurrentEntity.Start);

                matchResult.Matched = true;
                matchResult.NextStart = token.End;
            }
            return matchResult;
        }

        public override bool IsWildcard() => true;

        public override IEnumerable<string> GetEntityReferences()
        {
            yield return ENTITYTYPE;
        }

        public override string ToString() => "___";
    }
}
