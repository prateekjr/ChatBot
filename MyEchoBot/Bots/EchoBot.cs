// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with EchoBot .NET Template version v4.10.3

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Teams;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Schema.Teams;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace EchoBot.Bots
{
    public class EchoBot : ActivityHandler
    {
        private const string TextReplyToSend = "Hi Welcome to Edelweiss!! ";
        private string _appId;
        private string _appPassword;

        public EchoBot(IConfiguration config)
        {
            _appId = config["MicrosoftAppId"];
            _appPassword = config["MicrosoftAppPassword"];
        }
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            turnContext.Activity.RemoveRecipientMention();
            var text = turnContext.Activity.Text.Trim().ToLower();
            Console.WriteLine("turnContext :"+turnContext);
            if(text.Contains("Hi"))
                await MentionActivityAsync(turnContext, cancellationToken);
            else if(text.Contains("Case"))
                await GetSingleMemberAsync(turnContext, cancellationToken);
            else if(text.Contains("Report"))
                await CardActivityAsyncReport(turnContext, true, cancellationToken);
            else if(text.Contains("Dashboards"))
                await MessageAllMembersAsync(turnContext, cancellationToken);
            else if(text.Contains("AUM"))
                await DeleteCardActivityAsync(turnContext, cancellationToken);
            else
            {
                //await MentionActivityAsync(turnContext, cancellationToken);
                await CardActivityAsync(turnContext, false, cancellationToken);
            }
                //await CardActivityAsync(turnContext, false, cancellationToken);
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var welcomeText = "Hello and welcome!";
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
                }
            }
        }
    
        private async Task CardActivityAsyncReport(ITurnContext<IMessageActivity> turnContext, bool update, CancellationToken cancellationToken)
        {
            await turnContext.SendActivityAsync("Please tell me your Report Name", cancellationToken: cancellationToken);
        }
        private async Task CardActivityAsync(ITurnContext<IMessageActivity> turnContext, bool update, CancellationToken cancellationToken)
        {

            var card = new HeroCard
            {
                Buttons = new List<CardAction>
                        {
                            new CardAction
                            {
                                Type = ActionTypes.MessageBack,
                                Title = "Pending Account Opening Cases",
                                Text = "PendingAccountOpeningCases"
                            },
                            new CardAction
                            {
                                Type = ActionTypes.MessageBack,
                                Title = "Status Of Your Case",
                                Text = "StatusOfYourCase"
                            },
                            new CardAction
                            {
                                Type = ActionTypes.MessageBack,
                                Title = "Reports and Dashbords",
                                Text = "ReportsandDashbords"
                            },
                             new CardAction
                            {
                                Type = ActionTypes.MessageBack,
                                Title = "Order Entry Query",
                                Text = "OrderEntryQuery"
                            },
                              new CardAction
                            {
                                Type = ActionTypes.MessageBack,
                                Title = "Client AUM",
                                Text = "Client AUM"
                            }
                        }
            };


            if (update)
            {
                //await SendUpdatedCard(turnContext, card, cancellationToken);
            }
            else
            {
                await SendWelcomeCard(turnContext, card, cancellationToken);
            }

        }

        private async Task GetSingleMemberAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
           
            await turnContext.SendActivityAsync("Please write your Case Number", cancellationToken: cancellationToken);
        }

        private async Task DeleteCardActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            await turnContext.SendActivityAsync("Please tell me your Client Name", cancellationToken: cancellationToken);
        }

        // If you encounter permission-related errors when sending this message, see
        // https://aka.ms/BotTrustServiceUrl
        private async Task MessageAllMembersAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            await turnContext.SendActivityAsync("Please tell me your Dashboard Name", cancellationToken: cancellationToken);
        }

        /*private static async Task<List<TeamsChannelAccount>> GetPagedMembers(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
        
        } */

        private static async Task SendWelcomeCard(ITurnContext<IMessageActivity> turnContext, HeroCard card, CancellationToken cancellationToken)
        {
            var initialValue = new JObject { { "count", 0 } };
            card.Title = "Welcome To Edelwessis Bot!";
            card.Text = "Please tell me what do you want to know?";
            card.Buttons.Add(new CardAction
            {
                Type = ActionTypes.MessageBack,
                Title = "Update Card",
                Text = "UpdateCardAction",
                Value = initialValue
            });

            var activity = MessageFactory.Attachment(card.ToAttachment());

            await turnContext.SendActivityAsync(activity, cancellationToken);
        }

        /*private static async Task SendUpdatedCard(ITurnContext<IMessageActivity> turnContext, HeroCard card, CancellationToken cancellationToken)
        {
            
        } */

        private async Task MentionActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
           await turnContext.SendActivityAsync(TextReplyToSend, cancellationToken: cancellationToken);
        }
    }
    
}
