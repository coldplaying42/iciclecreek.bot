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

namespace GitHubClient.Repository.Branch
{
    /// <summary>
    /// Action to call GitHubClient.Repository.Branch.GetAllProtectedBranchTeamRestrictions() API.
    /// </summary>
    public class GetAllProtectedBranchTeamRestrictions : GitHubAction
    {
        /// <summary>
        /// Class identifier.
        /// </summary>
        [JsonProperty("$kind")]
        public const string Kind = "GitHubClient.Repository.Branch.GetAllProtectedBranchTeamRestrictions";

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllProtectedBranchTeamRestrictions"/> class.
        /// </summary>
        /// <param name="callerPath">Optional, source file full path.</param>
        /// <param name="callerLine">Optional, line number in source file.</param>
        public GetAllProtectedBranchTeamRestrictions([CallerFilePath] string callerPath = "", [CallerLineNumber] int callerLine = 0)
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
        /// (REQUIRED) Gets or sets the expression for api argument branch.
        /// </summary>
        /// <value>
        /// The value or expression to bind to the value for the argument.
        /// </value>
        [Required()]
        [JsonProperty("branch")]
        public StringExpression Branch  { get; set; }

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
            if (Owner != null && Name != null && Branch != null)
            {
                var ownerValue = Owner.GetValue(dc);
                var nameValue = Name.GetValue(dc);
                var branchValue = Branch.GetValue(dc);
                return await gitHubClient.Repository.Branch.GetAllProtectedBranchTeamRestrictions(ownerValue, nameValue, branchValue).ConfigureAwait(false);
            }
            if (RepositoryId != null && Branch != null)
            {
                var repositoryIdValue = RepositoryId.GetValue(dc);
                var branchValue = Branch.GetValue(dc);
                return await gitHubClient.Repository.Branch.GetAllProtectedBranchTeamRestrictions((Int64)repositoryIdValue, branchValue).ConfigureAwait(false);
            }

            throw new ArgumentNullException("Required [branch] arguments missing for GitHubClient.Repository.Branch.GetAllProtectedBranchTeamRestrictions");
        }
    }
}
