﻿using AdaptiveExpressions.Properties;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Iciclecreek.Bot.Builder.Dialogs.Database.Cosmos
{
    /// <summary>
    /// Create cosmos db item in container
    /// </summary>
    [Description("Upsert item in a cosmos container")]
    public class UpsertItem : Dialog
    {
        [JsonProperty("$kind")]
        public const string Kind = "Iciclecreek.Cosmos.UpsertItem";

        [JsonConstructor]
        public UpsertItem([CallerFilePath] string callerPath = "", [CallerLineNumber] int callerLine = 0)
        {
            this.RegisterSourceLocation(callerPath, callerLine);
        }

        /// <summary>
        /// Gets or sets the disabled state for the action.
        /// </summary>
        [JsonProperty("disabled")]
        [Description("Disable this action")]
        public BoolExpression Disabled { get; set; }

        /// <summary>
        /// Gets or sets the ConnectionString for querying the database.
        /// </summary>
        [JsonProperty("connectionString")]
        [Description("Connection string for cosmosdb.")]
        [Required]
        public StringExpression ConnectionString { get; set; }

        /// <summary>
        /// database name
        /// </summary>
        [JsonProperty("database")]
        [Description("Database name.")]
        [Required]
        public StringExpression Database { get; set; }

        /// <summary>
        /// Container name
        /// </summary>
        [JsonProperty("container")]
        [Description("Name of the Container.")]
        [Required]
        public StringExpression Container { get; set; }

        /// <summary>
        /// Item 
        /// </summary>
        [JsonProperty("item")]
        [Description("Item to upsert.")]
        [Required]
        public ObjectExpression<object> Item { get; set; }

        /// <summary>
        /// PartitionKey value (optional)
        /// </summary>
        [JsonProperty("partitionKey")]
        [Description("PartitionKey of iem to upsert.")]
        public StringExpression PartitionKey { get; set; }

        /// <summary>
        /// Gets or sets the property path to store the query result in.
        /// </summary>
        /// <value>
        /// The property path to store the dialog result in.
        /// </value>
        [JsonProperty("resultProperty")]
        [Description("Property to put result of this operation into.")]
        public StringExpression ResultProperty { get; set; }

        public override async Task<DialogTurnResult> BeginDialogAsync(DialogContext dc, object options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (this.Disabled != null && this.Disabled.GetValue(dc.State) == true)
            {
                return await dc.EndDialogAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
            }

            var connectionString = ConnectionString.GetValue(dc.State);
            var databaseName = Database.GetValue(dc.State);
            var containerName = Container.GetValue(dc.State);
            var item = Item.GetValue(dc.State);
            var partitionKeyValue = PartitionKey.GetValue(dc.State);
            PartitionKey? partitionKey = (!String.IsNullOrEmpty(partitionKeyValue)) ? new PartitionKey(partitionKeyValue) : (PartitionKey?)null;
            var client = CosmosClientCache.GetClient(connectionString);
            var database = client.GetDatabase(databaseName);
            var container = database.GetContainer(containerName);

            var result = await container.UpsertItemAsync(item, partitionKey: partitionKey, cancellationToken: cancellationToken).ConfigureAwait(false);
            
            if (this.ResultProperty != null)
            {
                dc.State.SetValue(this.ResultProperty.GetValue(dc.State), result.Resource);
            }

            return await dc.EndDialogAsync(result: result.Resource, cancellationToken: cancellationToken).ConfigureAwait(false);
        }
    }
}
