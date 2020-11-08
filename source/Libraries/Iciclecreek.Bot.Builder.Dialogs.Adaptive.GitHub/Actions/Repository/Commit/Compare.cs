using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using AdaptiveExpressions.Properties;
using Newtonsoft.Json;
using Microsoft.Bot.Builder.Dialogs;
using Octokit;
using System.ComponentModel.DataAnnotations;

namespace GitHubClient.Repository.Commit
{
    /// <summary>
    /// Action to call GitHubClient.Repository.Commit.Compare() API.
    /// </summary>
    public class Compare : GitHubAction
    {
        /// <summary>
        /// Class identifier.
        /// </summary>
        [JsonProperty("$kind")]
        public const string Kind = "GitHubClient.Repository.Commit.Compare";

        /// <summary>
        /// Initializes a new instance of the <see cref="Compare"/> class.
        /// </summary>
        /// <param name="callerPath">Optional, source file full path.</param>
        /// <param name="callerLine">Optional, line number in source file.</param>
        public Compare([CallerFilePath] string callerPath = "", [CallerLineNumber] int callerLine = 0)
        {
           this.RegisterSourceLocation(callerPath, callerLine);
        }

        /// <summary>
        /// (OPTIONAL) Gets or sets the expression for api argument owner.
        /// </summary>
        /// <value>
        /// The value or expression to bind to the value for the argument.
        /// </value>
        [JsonProperty("owner")]
        public StringExpression Owner  { get; set; }

        /// <summary>
        /// (OPTIONAL) Gets or sets the expression for api argument name.
        /// </summary>
        /// <value>
        /// The value or expression to bind to the value for the argument.
        /// </value>
        [JsonProperty("name")]
        public StringExpression Name  { get; set; }

        /// <summary>
        /// (REQUIRED) Gets or sets the expression for api argument base.
        /// </summary>
        /// <value>
        /// The value or expression to bind to the value for the argument.
        /// </value>
        [Required()]
        [JsonProperty("base")]
        public StringExpression Base  { get; set; }

        /// <summary>
        /// (REQUIRED) Gets or sets the expression for api argument head.
        /// </summary>
        /// <value>
        /// The value or expression to bind to the value for the argument.
        /// </value>
        [Required()]
        [JsonProperty("head")]
        public StringExpression Head  { get; set; }

        /// <summary>
        /// (OPTIONAL) Gets or sets the expression for api argument repositoryId.
        /// </summary>
        /// <value>
        /// The value or expression to bind to the value for the argument.
        /// </value>
        [JsonProperty("repositoryId")]
        public IntExpression RepositoryId  { get; set; }

        /// <inheritdoc/>
        protected override async Task<object> CallGitHubApi(DialogContext dc, Octokit.GitHubClient gitHubClient, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (Owner != null && Name != null && Base != null && Head != null)
            {
                var ownerValue = Owner.GetValue(dc);
                var nameValue = Name.GetValue(dc);
                var baseValue = Base.GetValue(dc);
                var headValue = Head.GetValue(dc);
                return await gitHubClient.Repository.Commit.Compare(ownerValue, nameValue, baseValue, headValue).ConfigureAwait(false);
            }
            if (RepositoryId != null && Base != null && Head != null)
            {
                var repositoryIdValue = RepositoryId.GetValue(dc);
                var baseValue = Base.GetValue(dc);
                var headValue = Head.GetValue(dc);
                return await gitHubClient.Repository.Commit.Compare((Int64)repositoryIdValue, baseValue, headValue).ConfigureAwait(false);
            }

            throw new ArgumentNullException("Required [base,head] arguments missing for GitHubClient.Repository.Commit.Compare");
        }
    }
}
