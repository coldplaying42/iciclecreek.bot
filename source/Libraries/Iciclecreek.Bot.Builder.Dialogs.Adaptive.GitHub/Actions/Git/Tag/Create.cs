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

namespace GitHubClient.Git.Tag
{
    /// <summary>
    /// Action to call GitHubClient.Git.Tag.Create() API.
    /// </summary>
    public class Create : GitHubAction
    {
        /// <summary>
        /// Class identifier.
        /// </summary>
        [JsonProperty("$kind")]
        public const string Kind = "GitHub.Git.Tag.Create";

        /// <summary>
        /// Initializes a new instance of the <see cref="Create"/> class.
        /// </summary>
        /// <param name="callerPath">Optional, source file full path.</param>
        /// <param name="callerLine">Optional, line number in source file.</param>
        public Create([CallerFilePath] string callerPath = "", [CallerLineNumber] int callerLine = 0)
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
        /// (REQUIRED) Gets or sets the expression for api argument tag.
        /// </summary>
        /// <value>
        /// The value or expression to bind to the value for the argument.
        /// </value>
        [Required()]
        [JsonProperty("tag")]
        public ObjectExpression<Octokit.NewTag> Tag  { get; set; }

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
            if (Owner != null && Name != null && Tag != null)
            {
                var ownerValue = Owner.GetValue(dc.State);
                var nameValue = Name.GetValue(dc.State);
                var tagValue = Tag.GetValue(dc.State);
                return await gitHubClient.Git.Tag.Create(ownerValue, nameValue, tagValue).ConfigureAwait(false);
            }
            if (RepositoryId != null && Tag != null)
            {
                var repositoryIdValue = RepositoryId.GetValue(dc.State);
                var tagValue = Tag.GetValue(dc.State);
                return await gitHubClient.Git.Tag.Create((Int64)repositoryIdValue, tagValue).ConfigureAwait(false);
            }

            throw new ArgumentNullException("Required [tag] arguments missing for GitHubClient.Git.Tag.Create");
        }
    }
}
