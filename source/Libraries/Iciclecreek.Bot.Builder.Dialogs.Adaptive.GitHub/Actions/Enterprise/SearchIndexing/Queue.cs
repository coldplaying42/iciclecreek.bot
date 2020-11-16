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

namespace GitHubClient.Enterprise.SearchIndexing
{
    /// <summary>
    /// Action to call GitHubClient.Enterprise.SearchIndexing.Queue() API.
    /// </summary>
    public class Queue : GitHubAction
    {
        /// <summary>
        /// Class identifier.
        /// </summary>
        [JsonProperty("$kind")]
        public const string Kind = "GitHub.Enterprise.SearchIndexing.Queue";

        /// <summary>
        /// Initializes a new instance of the <see cref="Queue"/> class.
        /// </summary>
        /// <param name="callerPath">Optional, source file full path.</param>
        /// <param name="callerLine">Optional, line number in source file.</param>
        public Queue([CallerFilePath] string callerPath = "", [CallerLineNumber] int callerLine = 0)
        {
           this.RegisterSourceLocation(callerPath, callerLine);
        }

        /// <summary>
        /// (REQUIRED) Gets or sets the expression for api argument owner.
        /// </summary>
        /// <value>
        /// The value or expression to bind to the value for the argument.
        /// </value>
        [Required()]
        [JsonProperty("owner")]
        public StringExpression Owner  { get; set; }

        /// <summary>
        /// (OPTIONAL) Gets or sets the expression for api argument repository.
        /// </summary>
        /// <value>
        /// The value or expression to bind to the value for the argument.
        /// </value>
        [JsonProperty("repository")]
        public StringExpression Repository  { get; set; }

        /// <inheritdoc/>
        protected override async Task<object> CallGitHubApi(DialogContext dc, Octokit.GitHubClient gitHubClient, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (Owner != null && Repository != null)
            {
                var ownerValue = Owner.GetValue(dc.State);
                var repositoryValue = Repository.GetValue(dc.State);
                return await gitHubClient.Enterprise.SearchIndexing.Queue(ownerValue, repositoryValue).ConfigureAwait(false);
            }
            if (Owner != null)
            {
                var ownerValue = Owner.GetValue(dc.State);
                return await gitHubClient.Enterprise.SearchIndexing.Queue(ownerValue).ConfigureAwait(false);
            }

            throw new ArgumentNullException("Required [owner] arguments missing for GitHubClient.Enterprise.SearchIndexing.Queue");
        }
    }
}
