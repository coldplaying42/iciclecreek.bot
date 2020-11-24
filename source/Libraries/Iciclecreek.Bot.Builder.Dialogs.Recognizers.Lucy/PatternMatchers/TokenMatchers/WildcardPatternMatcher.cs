﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Iciclecreek.Bot.Builder.Dialogs.Recognizers.Lucy.PatternMatchers.Matchers;

namespace Iciclecreek.Bot.Builder.Dialogs.Recognizers.Lucy.PatternMatchers
{
    /// <summary>
    /// Will match any unclaimed tokens
    /// </summary>
    public class WildcardPatternMatcher : PatternMatcher
    {
        public const string ENTITYTYPE = "wildcard";

        public WildcardPatternMatcher()
        {
        }

        public override MatchResult Matches(MatchContext context, int start)
        {
            var matchResult = new MatchResult();

            // if the next token is already claimed, then we don't want to keep this
            var tokenTaken = context.Entities.Where(entity =>
                entity.Start >= start && entity.Start <= start + 1 &&
                !String.Equals(entity.Type, TokenPatternMatcher.ENTITYTYPE, StringComparison.OrdinalIgnoreCase) &&
                !String.Equals(entity.Type, FuzzyTokenPatternMatcher.ENTITYTYPE, StringComparison.OrdinalIgnoreCase))
                .Any();
            if (!tokenTaken)
            {
                var token = context.FindNextEntities(TokenPatternMatcher.ENTITYTYPE, start).FirstOrDefault();
                if (token != null)
                {
                    // we add wildcardtoken on first token, and then get it and keep appending until we decide we are done.

                    var wildcardToken = context.CurrentEntity.Children.FirstOrDefault(entity => entity.Type == ENTITYTYPE);
                    if (wildcardToken == null)
                    {
                        wildcardToken = new LucyEntity()
                        {
                            Type = ENTITYTYPE,
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
                    context.CurrentEntity.Resolution = context.Text.Substring(wildcardToken.Start, wildcardToken.End - wildcardToken.Start);
                    context.CurrentEntity.Text = context.Text.Substring(context.CurrentEntity.Start, context.CurrentEntity.End - context.CurrentEntity.Start);

                    matchResult.Matched = true;
                    matchResult.NextStart = token.End;
                }
            }
            return matchResult;
        }

        public override bool IsWildcard() => true;

        public override IEnumerable<string> GetEntityTypeDependencies()
        {
            yield return ENTITYTYPE;
        }

        public override string ToString() => "___";
    }
}
