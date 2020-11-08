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

namespace GitHubClient.Check.Suite
{
    /// <summary>
    /// Action to call GitHubClient.Check.Suite.Get() API.
    /// </summary>
    public class Get : GitHubAction
    {
        /// <summary>
        /// Class identifier.
        /// </summary>
        [JsonProperty("$kind")]
        public const string Kind = "GitHubClient.Check.Suite.Get";

        /// <summary>
        /// Initializes a new instance of the <see cref="Get"/> class.
        /// </summary>
        /// <param name="callerPath">Optional, source file full path.</param>
        /// <param name="callerLine">Optional, line number in source file.</param>
        public Get([CallerFilePath] string callerPath = "", [CallerLineNumber] int callerLine = 0)
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
        /// (REQUIRED) Gets or sets the expression for api argument checkSuiteId.
        /// </summary>
        /// <value>
        /// The value or expression to bind to the value for the argument.
        /// </value>
        [Required()]
        [JsonProperty("checkSuiteId")]
        public IntExpression CheckSuiteId  { get; set; }

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
            if (Owner != null && Name != null && CheckSuiteId != null)
            {
                var ownerValue = Owner.GetValue(dc);
                var nameValue = Name.GetValue(dc);
                var checkSuiteIdValue = CheckSuiteId.GetValue(dc);
                return await gitHubClient.Check.Suite.Get(ownerValue, nameValue, (Int64)checkSuiteIdValue).ConfigureAwait(false);
            }
            if (RepositoryId != null && CheckSuiteId != null)
            {
                var repositoryIdValue = RepositoryId.GetValue(dc);
                var checkSuiteIdValue = CheckSuiteId.GetValue(dc);
                return await gitHubClient.Check.Suite.Get((Int64)repositoryIdValue, (Int64)checkSuiteIdValue).ConfigureAwait(false);
            }

            throw new ArgumentNullException("Required [checkSuiteId] arguments missing for GitHubClient.Check.Suite.Get");
        }
    }
}
