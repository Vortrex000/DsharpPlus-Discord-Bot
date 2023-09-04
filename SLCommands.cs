/* NameSpaces */
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Lavalink;
using DSharpPlus.SlashCommands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
/* NameSpaces */

namespace FlamesBotV2
{
    public class SLCommands : ApplicationCommandModule
    {
        public class Root
        {
            [JsonProperty("users")]
            public Dictionary<string, UserData> Users { get; set; }
        }

        public class UserDatabase
        {
            public Dictionary<string, UserData> Users { get; set; }
        }

        public class UserData
        {
            [JsonProperty("balance")]
            public int Balance { get; set; }

            [JsonProperty("inv_lvl")]
            public int Inv_lvl { get; set; }

            [JsonProperty("slot_lvl")]
            public int Slot_lvl { get; set; }

            [JsonProperty("riot_shield_lvl")]
            public int Riot_shield_lvl { get; set; }

            [JsonProperty("riot_shield_perk")]
            public string Riot_shield_perk { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("position")]
            public string Position { get; set; }

            [JsonProperty("total_fish_count")]
            public int Total_fish_count { get; set; }

            [JsonProperty("total_common_fish")]
            public int Total_common_fish { get; set; }

            [JsonProperty("total_uncommon_fish")]
            public int Total_uncommon_fish { get; set; }

            [JsonProperty("total_rare_fish")]
            public int Total_rare_fish { get; set; }

            [JsonProperty("total_epic_fish")]
            public int Total_epic_fish { get; set; }

            [JsonProperty("total_legendary_fish")]
            public int Total_legendary_fish { get; set; }

            [JsonProperty("total_mythical_fish")]
            public int Total_mythical_fish { get; set; }

            [JsonProperty("total_void_fish")]
            public int Total_void_fish { get; set; }

            [JsonProperty("inv")]
            public List<string> Inv { get; set; }

            [JsonProperty("notes")]
            public List<string> Notes { get; set; }

            [JsonProperty("slots")]
            public Dictionary<string, string> Slots { get; set; }
        }

        private readonly SemaphoreSlim updateSemaphore = new SemaphoreSlim(1, 1);

        protected async Task UpdateJsonFileAsync(UserDatabase userDatabase)
        {
            var serializerSettings = new JsonSerializerSettings { Formatting = Formatting.Indented };
            string updatedJsonString = JsonConvert.SerializeObject(userDatabase, serializerSettings);

            // Use a semaphore to prevent potential concurrency issues when writing to the file.
            await updateSemaphore.WaitAsync();
            try
            {
                using (StreamWriter writer = new StreamWriter(@"C:\Users\Owner\source\repos\FlamesBotV2\Data.json"))
                {
                    await writer.WriteAsync(updatedJsonString);
                }
            }
            finally
            {
                updateSemaphore.Release();
            }
        }

        public class JsonFileHelper     // Helps To Read The Json File
        {
            public async Task<string> ReadJsonFileAsync()
            {
                string jsonString;
                using (StreamReader reader = new StreamReader(@"C:\Users\Owner\source\repos\FlamesBotV2\Data.json"))
                {
                    jsonString = await reader.ReadToEndAsync();
                }
                return jsonString;
            }
        }

        /* Ping Command */
        [SlashCommand("ping", "Ping-Pong Command")]
        public async Task PingSlashCommand(InteractionContext ctx)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("Pong!"));      // The Bot Responds With Pong!
        }
        /* Ping Command */



        /* Echo Command */
        [SlashCommand("echo", "Make Flames say something")]
        public async Task EchoSlashCommand(InteractionContext ctx,
            [Option("message", "The message to be said")] string message,
            [Option("channel", "The channel to be said in")] DiscordChannel channel = null) // By Default The channel Is Null
        {
            try
            {
                if (string.IsNullOrEmpty(message))      // Chekcs If The Message Is Null
                {
                    await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                        .WithContent("**The message can't be empty.**")    // Acknowledges The User That The Message Can't Be Empty
                        .AsEphemeral(true));    // Makes It Visible Only For The User Who Ran The Command
                    return;
                }

                if (channel == null)    // Checks If The Channel Is Null
                {
                    channel = ctx.Channel;      // If The Channel Is Null It Becomes The channel Where The Command Was Ran
                    await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                        .WithContent($"**Message sent!**")      // Acknowledges The User By Sending Ephemeral Message (Only The User Who Ran The Command Can See It)
                        .AsEphemeral(true));
                }
                else
                {
                    await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                        .WithContent($"**Message sent to {channel.Mention}**")      // Acknowledges The User By Sending Ephemeral Message (Only The User Who Ran The Command Can See It)
                        .AsEphemeral(true));
                }

                var channel_message = await channel.SendMessageAsync(message);      // The Bot Sends The Message In The Selected Channel
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");      // Prints The Error In The Console
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                .WithContent("**An error occurred while processing your request.**")    // Acknowledges The User That An Error Occured
                .AsEphemeral(true));    // Makes It Visible Only For The User Who Ran The Command
            }
        }
        /* Echo Command */



        /* Avatar Command */
        [SlashCommand("avatar", "Get the avatar of someone")]
        public async Task AvatarSlashCommand(InteractionContext ctx, [Option("user", "The user to get the avatar of")] DiscordUser user = null)     // By Default The user Is Null
        {
            try
            {
                user = user ?? ctx.User;        // If The User Is Null Then The user Becomes ctx.User ( The User Who Ran The Command )
                string avatarUrl = user.AvatarUrl;      // Gets The User's Avatar

                if (string.IsNullOrEmpty(avatarUrl))    // Checks If The User's Avatar Url Is Null
                {
                    await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                        .WithContent("**The selected user does not have an avatar.**")
                        .AsEphemeral(true));
                    return;
                }

                await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

                var embed = new DiscordEmbedBuilder()
                    .WithTitle($"**{user.Username}'s Avatar**")     // Get The User's Username
                    .WithImageUrl(avatarUrl)        // Displays The User's Avatar
                    .WithColor(DiscordColor.DarkGreen);

                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");      // Prints The Error In The Console
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                .WithContent("**An error occurred while processing your request.**")    // Acknowledges The User That An Error Occured
                .AsEphemeral(true));    // Makes It Visible Only For The User Who Ran The Command
            }
        }
        /* Avatar Command */



        /* Roll Command */
        [SlashCommand("roll", "Roll a dice")]
        public async Task RollSlashCommand(InteractionContext ctx)
        {
            try
            {
                var rng = new Random();     // Creates A New Instance Of The Random Class
                var result = rng.Next(1, 7);    // Generates Random Number From 1 To 6

                await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

                var embed = new DiscordEmbedBuilder()
                        .WithTitle("**Dice**")
                        .WithDescription($"**You rolled a {result}!**")     // Displays The Number
                        .WithColor(DiscordColor.DarkRed);

                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");      // Prints The Error In The Console
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                .WithContent("**An error occurred while processing your request.**")    // Acknowledges The User That An Error Occured
                .AsEphemeral(true));    // Makes It Visible Only For The User Who Ran The Command
            }
        }
        /* Roll Command */



        /* 8ball Command */
        [SlashCommand("8ball", "Ask the magic 8ball a question")]
        public async Task EightBallSlashCommand(InteractionContext ctx, [Option("question", "The question to ask the 8ball")] string question = null)       // The Question Is Null By Default
        {
            try
            {
                var rng = new Random();     // Creates A New Instance Of The Random Class
                string response = Vars.eight_ball_responses[rng.Next(Vars.eight_ball_responses.Length)];    // Displays Random Response From The eight_ball_responses Array     

                await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

                var embed = new DiscordEmbedBuilder()
                    .WithTitle("**Magic 8ball**")
                    .WithDescription(question == null ? response : $"**Question:** {question}\n**Answer:** {response}")     // If The Question Is Null Then Only The Response Is Shown
                    .WithColor(DiscordColor.Purple);

                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");      // Prints The Error In The Console
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                .WithContent("**An error occurred while processing your request.**")    // Acknowledges The User That An Error Occured
                .AsEphemeral(true));    // Makes It Visible Only For The User Who Ran The Command
            }
        }
        /* 8ball Command */



        /* Help Command */
        [SlashCommand("help", "Display the help menu")]
        public async Task HelpSlashCommand(InteractionContext ctx)
        {
            try
            {
                await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

                /* Long Embed Message That Displays All The Commands */
                var embed = new DiscordEmbedBuilder()
                    .WithTitle("**Help Menu**")
                    .WithDescription("**`/8ball (question)`** - Ask the magic 8ball a question.\n" +
                    "**`/armory (weapon)`** - Open the armory menu.\n" +
                    "**`/avatar (user)`** - Get the avatar of someone.\n" +
                    "**`/bal`** - View your balance.\n" +
                    "**`/baltop`** - View the people with the most coins.\n" +
                    "**`/cf (amount) (side)`** - Bet money on coinflip.\n" +
                    "**`/dm (user)`** - Make Flames message specific user.\n" +
                    "**`/echo (message)`** - Make Flames say something.\n" +
                    "**`/fish`** - Fish the fish.\n" +
                    "**`/fishing shop (item)`** - Buy fish-related things.\n" +
                    "**`/fishing stats`** - View your fishing stats.\n" +
                    "**`/inv`** - View your inventory.\n" +
                    "**`/notes add (note)`** - Add a note.\n" +
                    "**`/notes change (note index) (new note)`** - Change a note.\n" +
                    "**`/notes clear (confirm)`** - Clear your notes.\n" +
                    "**`/notes remove (note index)`** - Remove a note.\n" +
                    "**`/notes view`** - View your notes.\n" +
                    "**`/pause`** - Make Flames pause the music.\n" +
                    "**`/pay (@user) (amount)`** - Pay someone.\n" +
                    "**`/ping`** - Make Flames respond with pong.\n" +
                    "**`/play (song)`** - Make Flames play music in the vc you are in.\n" +
                    "**`/position change`** - Change your position.\n" +
                    "**`/position info (position)`** - View the ups and downs of most positions.\n" +
                    "**`/position view`** - View your position.\n" +
                    "**`/raid (place)`** - Raid a place.\n" +
                    "**`/resume`** - Make Flames resume the music.\n" +
                    "**`/riot shield`** - View your riot shield.\n" +
                    "**`/roll`** - Roll a dice.\n" +
                    "**`/slots view`** - Display your slots.\n" +
                    "**`/stop`** - Make Flames stop the music & disconnect from vc.")
                    .WithColor(DiscordColor.DarkBlue);
                /* Long Embed Message That Displays All The Commands */

                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");      // Prints The Error In The Console
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                .WithContent("**An error occurred while processing your request.**")    // Acknowledges The User That An Error Occured
                .AsEphemeral(true));    // Makes It Visible Only For The User Who Ran The Command
            }
        }
        /* Help Command */



        /* Cf Command */
        [SlashCommand("cf", "Flip a coin")]
        public async Task CfSlashCommand(InteractionContext ctx, [Option("amount", "The amount you want to bet")] double amount, [Option("side", "The coin's side")]     // Option For The Coin's Side & The Amount That The User Wants To Bet
        [Choice("heads", "heads")]
        [Choice("tails", "tails")] string side)
        {
            try
            {
                string userId = ctx.User.Id.ToString();     // Gets The User's Id
                string jsonString;
                using (StreamReader reader = new StreamReader(@"C:\Users\Owner\source\repos\FlamesBotV2\Data.json"))    // Reads The Json File
                { jsonString = await reader.ReadToEndAsync(); }
                UserDatabase userDatabase = JsonConvert.DeserializeObject<UserDatabase>(jsonString);    // Deserialize The Json File

                double balance = userDatabase.Users[userId].Balance;    // Gets The User's Balance

                if (!IsWholeNumber(amount) || amount < 1 || amount > balance)       // Checks If The User Entered Whole Number That Is Higher Than Or Equal To 1 Or Less Than Or Equal To The User's Balance
                {
                    await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

                    var embed = new DiscordEmbedBuilder()
                        .WithTitle("**Coinflip**")
                        .WithDescription("**Please enter a valid whole number amount between 1 and your balance.**")
                        .WithColor(DiscordColor.Azure);

                    await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed));
                    return;
                }

                Random rnd = new Random();
                int randomSide = rnd.Next(0, 2);
                string result = randomSide == 0 ? "heads" : "tails";

                if (result == side)
                {
                    balance += 2 * amount;      // Adds The 2x The Amount Bet To The User's Balance If He Wins

                    await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

                    var embed = new DiscordEmbedBuilder()
                        .WithTitle("**Coinflip**")
                        .WithDescription($"**You won!\nNew balance: {balance}**")
                        .WithColor(DiscordColor.Green);

                    await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed));
                }
                else
                {
                    balance -= amount;          // Decreases The User's Balance If He Loses By The Amount Bet

                    await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

                    var embed = new DiscordEmbedBuilder()
                        .WithTitle("**Coinflip**")
                        .WithDescription($"**You lost!\nNew balance: {balance}**")
                        .WithColor(DiscordColor.Red);

                    await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed));
                }

                /* Updates The User's Json File Along With The New User's Balance */
                userDatabase.Users[userId].Balance = (int)balance;
                await UpdateJsonFileAsync(userDatabase);
                /* Updates The User's Json File Along With The New User's Balance */
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");      // Prints The Error In The Console
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                .WithContent("**An error occurred while processing your request.**")    // Acknowledges The User That An Error Occured
                .AsEphemeral(true));    // Makes It Visible Only For The User Who Ran The Command
            }
        }
        private bool IsWholeNumber(double number)
        {
            return Math.Abs(number % 1) < double.Epsilon;       // The Method Used for Deciding If The Amount Is Whole Number
        }
        /* Cf Command */



        /* Bal Command */
        [SlashCommand("bal", "View your balance")]
        public async Task BalSlashCommand(InteractionContext ctx)
        {
            try
            {
                string userId = ctx.User.Id.ToString();     // Gets The User's Id
                string jsonString;
                using (StreamReader reader = new StreamReader(@"C:\Users\Owner\source\repos\FlamesBotV2\Data.json"))    // Reads The Json File
                { jsonString = await reader.ReadToEndAsync(); }
                UserDatabase userDatabase = JsonConvert.DeserializeObject<UserDatabase>(jsonString);    // Deserialize The Json File

                int balance = userDatabase.Users[userId].Balance;

                await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

                var embed = new DiscordEmbedBuilder()
                .WithTitle("**Balance**")
                .WithDescription($"**You have {balance} coins**")       // Displays How Many Coins The User Have
                .WithColor(DiscordColor.Orange);

                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");      // Prints The Error In The Console
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                .WithContent("**An error occurred while processing your request.**")    // Acknowledges The User That An Error Occured
                .AsEphemeral(true));    // Makes It Visible Only For The User Who Ran The Command
            }
        }
        /* Bal Command */



        /* Baltop Command */
        [SlashCommand("baltop", "View the people with most coins")]
        public async Task BalTopSlashCommand(InteractionContext ctx, [Option("page", "The page you want to see")] double? page = 1)
        {
            try
            {
                string jsonString;
                using (StreamReader reader = new StreamReader(@"C:\Users\Owner\source\repos\FlamesBotV2\Data.json"))    // Reads The Json File
                { jsonString = await reader.ReadToEndAsync(); }
                UserDatabase userDatabase = JsonConvert.DeserializeObject<UserDatabase>(jsonString);    // Deserialize The Json File

                if (!IsWholeNumberForBaltop(page))              // Checks If The User Entered Whole Number
                {
                    await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                        .WithContent("**Please enter a whole number.**")
                        .AsEphemeral(true));
                    return;
                }

                var sortedBalances = userDatabase.Users.Select(user => new
                {
                    UserId = user.Key,      // Gets The User's Id
                    user.Value.Balance,         // Gets The User's Balances
                    UserName = user.Value.Name      // Gets The User's Name
                })
                .OrderByDescending(user => user.Balance)        // Sorts The Users Depending On Their Bal
                .ThenBy(user => user.UserName, StringComparer.OrdinalIgnoreCase)        // If The Users Have Same Bal It Sorts Them Alphabetically 
                .ToList();

                var totalPages = Math.Ceiling(sortedBalances.Count / 10.0);     // Divides All Balances Into Groups Of 10

                if (page < 1 || page > totalPages)
                {
                    await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                        .WithContent("**The requested page does not exist.**")      // Acknowledges The User That The Page He Entered Doesn't Exist
                        .AsEphemeral(true));
                    return;
                }

                var itemsperpage = 10;      // Items Per Page
                int startIdx = (int)((page - 1) * itemsperpage);            // Decides With Which Balance To Start On The Current Page
                int endIdx = startIdx + itemsperpage;       // Decides With Which Balance To End On The Current Page
                var pageBalances = sortedBalances.Skip(startIdx).Take(itemsperpage).ToList();       // Creates New List Of Balances For The Current Page

                var embed = new DiscordEmbedBuilder()
                    .WithTitle($"Balance Leaderboard - Page {page}/{totalPages}")       // Displays The Title Along With The Current Baltop Page
                    .WithColor(DiscordColor.Gold);

                var rank = startIdx + 1;    // Decides The Rank For The First User And So On
                foreach (var userData in pageBalances)
                {
                    embed.AddField($"Rank {rank}", $"<@{userData.UserId}>: {userData.Balance} coins", inline: false);       // Lists The First 10 Users Then The Second 10 And So On
                    rank++;     // Increases The Rank With Each User
                }

                await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed));       // Creates The Embed Response
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");      // Prints The Error In The Console
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                .WithContent("**An error occurred while processing your request.**")    // Acknowledges The User That An Error Occured
                .AsEphemeral(true));    // Makes It Visible Only For The User Who Ran The Command
            }
        }
        private bool IsWholeNumberForBaltop(double? number)     // The Method Used for Deciding If The Amount Is Whole Number
        {
            if (number == null)
            {
                return false;
            }

            return Math.Abs(number.Value % 1) < double.Epsilon;
        }
        /* Baltop Command */



        /* Pay Command */
        [SlashCommand("pay", "Pay someone")]
        public async Task PaySlashCommand(InteractionContext ctx,
        [Option("user", "The user you want to pay")] DiscordUser user,
        [Option("amount", "The amount you want to pay")] double amount)
        {
            try
            {
                string payerUserId = ctx.User.Id.ToString();
                string payeeUserId = user.Id.ToString();

                string jsonString;
                using (StreamReader reader = new StreamReader(@"C:\Users\Owner\source\repos\FlamesBotV2\Data.json"))    // Reads The Json File
                { jsonString = await reader.ReadToEndAsync(); }
                UserDatabase userDatabase = JsonConvert.DeserializeObject<UserDatabase>(jsonString);    // Deserialize The Json File

                if (userDatabase.Users.TryGetValue(payerUserId, out var payerUserData) &&
                    userDatabase.Users.TryGetValue(payeeUserId, out var payeeUserData))
                {
                    int payerBalance = payerUserData.Balance;

                    if (!IsWholeNumber(amount) || amount < 1 || amount > payerBalance)       // Checks If The User Entered Whole Number That Is Higher Than Or Equal To 1 Or Less Than Or Equal To The User's Balance
                    {
                        await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                            .WithContent("**Please enter a valid whole number amount between 1 and your balance.**")
                            .AsEphemeral(true));
                        return;
                    }

                    /* Checks If The User Is Trying To Pay Himself */
                    if (payerUserId == payeeUserId)
                    {
                        await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                            .WithContent("**You can't pay yourself!**")
                            .AsEphemeral(true));
                        return;
                    }
                    /* Checks If The User Is Trying To Pay Himself */

                    else
                    {
                        double payeeBalance = payeeUserData.Balance;

                        payerBalance -= (int)amount;     // Removes The Amount From The Payer's Balance
                        payeeBalance += amount;     // Adds It To The Payee's Balance


                        await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

                        var embed = new DiscordEmbedBuilder()
                         .WithTitle("**System**")
                         .WithDescription($"**{ctx.User.Mention} Paid {amount} coins to {user.Mention}**")    // Sends Message To The Chat To Acknowledge Both Users For The Payment
                         .WithColor(DiscordColor.Orange);

                        await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed));

                        /* Updates The Json File Along With The New Payer's Balance & The New Payee's Balance */
                        payerUserData.Balance = payerBalance;
                        payeeUserData.Balance = (int)payeeBalance;

                        await UpdateJsonFileAsync(userDatabase);
                        /* Updates The Json File Along With The New Payer's Balance & The New Payee's Balance */
                    }
                }
                else
                {
                    await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                        .WithContent("An error occurred while processing the payment.")     // Acknowledges The Payer If There Was An Error With The Payment
                        .AsEphemeral(true));
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");      // Prints The Error In The Console
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                .WithContent("**An error occurred while processing your request.**")    // Acknowledges The User That An Error Occured
                .AsEphemeral(true));    // Makes It Visible Only For The User Who Ran The Command
            }
        }
        /* Pay Command */



        /* Inv Command */
        [SlashCommand("inv", "View your inventory")]
        public async Task InvSlashCommand(InteractionContext ctx)
        {
            try
            {
                await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

                string userId = ctx.User.Id.ToString();     // Gets The User's Id
                string jsonString;
                using (StreamReader reader = new StreamReader(@"C:\Users\Owner\source\repos\FlamesBotV2\Data.json"))    // Reads The Json File
                { jsonString = await reader.ReadToEndAsync(); }
                UserDatabase userDatabase = JsonConvert.DeserializeObject<UserDatabase>(jsonString);    // Deserialize The Json File
                int inv_lvl = userDatabase.Users[userId].Inv_lvl;       // Gets The User's Inventory Level
                int inv_count = userDatabase.Users[userId].Inv.Count;     // Gets The Number Of Items Inside The Inventory

                List<string> userInventory = userDatabase.Users[userId].Inv;

                var embed = new DiscordEmbedBuilder()
                .WithTitle("**Inventory**")
                .WithColor(DiscordColor.Teal);

                int maxItems = inv_lvl * 3;

                if (userInventory.Count == 0)
                {
                    embed.WithDescription($"**Inventory Level: {inv_lvl}/3\nItems: {inv_count}/{maxItems}**\nEmpty");       // If The Inventory Is Empty
                }

                else
                {
                    embed.WithDescription($"**Inventory Level: {inv_lvl}/3\nItems: {inv_count}/{maxItems}**\n" + string.Join(", ", userInventory));     // If The Inventory Isn't Empty
                }

                var builder = new DiscordWebhookBuilder();
                var inv_upgrade = new DiscordButtonComponent(ButtonStyle.Primary, "inv_upgrade", "Upgrade", false);     // Creating Inventory Upgrade Button

                builder.AddComponents(inv_upgrade);
                builder.AddEmbed(embed);    // Adds The Embed Messages

                await ctx.EditResponseAsync(builder);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");      // Prints The Error In The Console
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                .WithContent("**An error occurred while processing your request.**")    // Acknowledges The User That An Error Occured
                .AsEphemeral(true));    // Makes It Visible Only For The User Who Ran The Command
            }
        }
        /* Inv Command */



        /* Armory Command */
        [SlashCommand("armory", "Open the armory menu")]
        public async Task ArmorySlashCommand(InteractionContext ctx, [Option("weapon", "Choose a weapon")]
        [Choice("Hawk Eye", "Hawk Eye")]
        [Choice("Glock 17", "Glock 17")]
        [Choice("SPAS-12", "SPAS-12")]
        [Choice("M40A5", "M40A5")]
        [Choice("AK-47", "AK-47")] string weapon)
        {
            try
            {
                var cancel_C = new DiscordButtonComponent(ButtonStyle.Danger, "cancel_C", "Cancel", false);     // Creating Cancel Button (That When Clicked Deletes The Message)

                switch (weapon)
                {
                    case "Hawk Eye":                                                            /* Hawk Eye */
                        {
                            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

                            var embed = new DiscordEmbedBuilder()
                            .WithTitle("**Hawk Eye**")
                            .WithDescription(Vars.CD)
                            .WithColor(DiscordColor.Yellow);
                            var hawk_eye_craft = new DiscordButtonComponent(ButtonStyle.Success, "hawk_eye_craft", "Craft", false);             // Creating Hawk Eye Button

                            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed).AddComponents(hawk_eye_craft, cancel_C));   // Adds The Button Below The Embed Message
                        }
                        break;

                    case "Glock 17":                                                            /* Glock 17 */
                        {
                            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

                            var embed = new DiscordEmbedBuilder()
                                .WithTitle("**Glock 17**")
                                .WithDescription(Vars.CP)
                                .WithColor(DiscordColor.Yellow);
                            var glock_17_craft = new DiscordButtonComponent(ButtonStyle.Success, "glock_17_craft", "Craft", false);             // Creating Glock 17 Button

                            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed).AddComponents(glock_17_craft, cancel_C));   // Adds The Button Below The Embed Message
                        }
                        break;

                    case "SPAS-12":                                                            /* SPAS-12 */
                        {
                            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

                            var embed = new DiscordEmbedBuilder()
                                .WithTitle("**SPAS-12**")
                                .WithDescription(Vars.CS)
                                .WithColor(DiscordColor.Yellow);
                            var spas_12_craft = new DiscordButtonComponent(ButtonStyle.Success, "spas_12_craft", "Craft", false);               // Creating SPAS-12 Button

                            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed).AddComponents(spas_12_craft, cancel_C));    // Adds The Button Below The Embed Message
                        }
                        break;

                    case "M40A5":                                                            /* M40A5 */
                        {
                            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

                            var embed = new DiscordEmbedBuilder()
                                .WithTitle("**M40A5**")
                                .WithDescription(Vars.CSN)
                                .WithColor(DiscordColor.Yellow);
                            var m40a5_craft = new DiscordButtonComponent(ButtonStyle.Success, "m40a5_craft", "Craft", false);                   // Creating M40A5 Button

                            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed).AddComponents(m40a5_craft, cancel_C));      // Adds The Button Below The Embed Message
                        }
                        break;

                    case "AK-47":                                                            /* AK-47 */
                        {
                            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

                            var embed = new DiscordEmbedBuilder()
                                .WithTitle("**AK-47**")
                                .WithDescription(Vars.CR)
                                .WithColor(DiscordColor.Yellow);
                            var ak_47_craft = new DiscordButtonComponent(ButtonStyle.Success, "ak_47_craft", "Craft", false);                   // Creating AK-47 Button

                            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed).AddComponents(ak_47_craft, cancel_C));      // Adds The Button Below The Embed Message
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");      // Prints The Error In The Console
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                .WithContent("**An error occurred while processing your request.**")    // Acknowledges The User That An Error Occured
                .AsEphemeral(true));    // Makes It Visible Only For The User Who Ran The Command
            }
        }
        /* Armory Command */



        /* Slots Commands */
        [SlashCommandGroup("slots", "View Your Slots")]
        public class SlotsGroupContainer : ApplicationCommandModule
        {
            [SlashCommand("view", "View your slots")]
            public async Task SlotsViewSlashCommand(InteractionContext ctx)
            {
                try
                {
                    await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

                    string userId = ctx.User.Id.ToString();     // Gets The User's Id
                    string jsonString;
                    using (StreamReader reader = new StreamReader(@"C:\Users\Owner\source\repos\FlamesBotV2\Data.json"))    // Reads The Json File
                    { jsonString = await reader.ReadToEndAsync(); }
                    UserDatabase userDatabase = JsonConvert.DeserializeObject<UserDatabase>(jsonString);    // Deserialize The Json File

                    int slot_lvl = userDatabase.Users[userId].Slot_lvl;         // Gets The User's Slot Level
                    UserData user = userDatabase.Users[userId];         // Gets The User Id
                    Dictionary<string, string> slots = user.Slots;      // Gets The User's Slots

                    string result = "";
                    int currentNumber = 0;

                    foreach (var slotItem in slots)
                    {
                        string key = slotItem.Key;
                        string value = slotItem.Value;

                        string[] parts = key.Split('_');
                        string type = parts[0];
                        int number = int.Parse(parts[2]);

                        if (number != currentNumber)
                        {
                            result += "\n";
                            currentNumber = number;
                        }

                        result += $"`{type} slot [{number}]: {value}`\n";
                    }

                    var builder = new DiscordWebhookBuilder();
                    var slot_upgrade = new DiscordButtonComponent(ButtonStyle.Primary, "slot_upgrade", "Upgrade", false);       // Creating Slot Upgrade Button

                    if (slot_lvl <= 4 && slot_lvl > 0)
                    {
                        builder.AddComponents(slot_upgrade); // Adds The Upgrade Button Except When The Slot Level Is Equal Or Less Than 4 And Higher Than 0
                    }

                    var embed = new DiscordEmbedBuilder()

                    .WithTitle("**Slots**")
                    .WithDescription($"**Slots Level: {slot_lvl}/5\nCurrent Slots:\n{result}**")        // Displays The User's Slots
                    .WithColor(DiscordColor.Orange);

                    builder.AddEmbed(embed);        // Adds The Embed Messages

                    await ctx.EditResponseAsync(builder);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");      // Prints The Error In The Console
                    await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                    .WithContent("**An error occurred while processing your request.**")    // Acknowledges The User That An Error Occured
                    .AsEphemeral(true));    // Makes It Visible Only For The User Who Ran The Command
                }
            }
        }
        /* Slots Commands */



        /* Position */
        [SlashCommandGroup("position", "Position commands")]
        public class PositionGroupContainer : ApplicationCommandModule
        {
            [SlashCommand("change", "Change your position")]    // Position Change
            public async Task PositionChangeSlashCommand(InteractionContext ctx, [Option("place", "The place you want to be at")]
            [Choice("The Docs", "The Docs")]
            [Choice("The Suburbs", "The Suburbs")]
            [Choice("The Underground", "The Underground")]
            [Choice("West Side's Factories", "West Side's Factories")]
            [Choice("South Side's Factories", "South Side's Factories")]
            string place)
            {
                try
                {
                    string userId = ctx.User.Id.ToString();     // Gets The User's Id
                    string jsonString;
                    using (StreamReader reader = new StreamReader(@"C:\Users\Owner\source\repos\FlamesBotV2\Data.json"))    // Reads The Json File
                    { jsonString = await reader.ReadToEndAsync(); }
                    UserDatabase userDatabase = JsonConvert.DeserializeObject<UserDatabase>(jsonString);    // Deserialize The Json File

                    string position = userDatabase.Users[userId].Position;      // Gets The User's Current Position

                    userDatabase.Users[userId].Position = place;    // Updates The User's Position

                    SLCommands slCommands = new SLCommands();
                    await slCommands.UpdateJsonFileAsync(userDatabase);     // Updates The Json File

                    await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                        .WithContent($"**Position changed to {place}**")       // Acknowledges The User About The Changed Position
                        .AsEphemeral(true));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");      // Prints The Error In The Console
                    await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                    .WithContent("**An error occurred while processing your request.**")    // Acknowledges The User That An Error Occured
                    .AsEphemeral(true));    // Makes It Visible Only For The User Who Ran The Command
                }
            }



            [SlashCommand("info", "View the ups and downs of most positions")]      // Position Info
            public async Task PositionInfoSlashCommand(InteractionContext ctx, [Option("place", "The place you want to view")]
            [Choice("The Docs", "The Docs")]
            [Choice("The Suburbs", "The Suburbs")]
            [Choice("The Underground", "The Underground")]
            [Choice("West Side's Factories", "West Side's Factories")]
            [Choice("South Side's Factories", "South Side's Factories")]
            string place_info)
            {
                try
                {
                    await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

                    var embed = new DiscordEmbedBuilder()
                        .WithTitle("**Positions**")
                        .WithColor(DiscordColor.Orange);

                    string title = string.Empty;
                    string description = string.Empty;

                    switch (place_info)
                    {
                        case "The Docs":
                            title = "**The Docs**";     // Adds The Docs As Title
                            description = Vars.the_docs;    // Adds Description About The Docs
                            break;

                        case "The Suburbs":
                            title = "**The Suburbs**";      // Adds The Suburbs As Title
                            description = Vars.the_suburbs;     // Adds Description About The Suburbs
                            break;

                        case "The Underground":
                            title = "**The Underground**";      // Adds The Underground As Title
                            description = Vars.the_underground;     // Adds Description About The Underground
                            break;

                        case "West Side's Factories":
                            title = "**West Side's Factories**";    // Adds West Side's Factories As Title
                            description = Vars.west_side_factories;     // Adds Description About West's Side Factories
                            break;

                        case "South Side's Factories":
                            title = "**South Side's Factories**";       // Adds South's Side Factories As Title
                            description = Vars.south_side_factories;        // Adds Description About South's Side Factories
                            break;
                    }

                    embed.WithTitle(title);
                    embed.WithDescription(description);

                    await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");      // Prints The Error In The Console
                    await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                    .WithContent("**An error occurred while processing your request.**")    // Acknowledges The User That An Error Occured
                    .AsEphemeral(true));    // Makes It Visible Only For The User Who Ran The Command
                }
            }



            [SlashCommand("view", "View your position")]        // Position View
            public async Task PositionViewSlashCommand(InteractionContext ctx)
            {
                try
                {
                    string userId = ctx.User.Id.ToString();     // Gets The User's Id
                    string jsonString;
                    using (StreamReader reader = new StreamReader(@"C:\Users\Owner\source\repos\FlamesBotV2\Data.json"))    // Reads The Json File
                    { jsonString = await reader.ReadToEndAsync(); }
                    UserDatabase userDatabase = JsonConvert.DeserializeObject<UserDatabase>(jsonString);    // Deserialize The Json File

                    string position = userDatabase.Users[userId].Position;      // Gets The User's Current Position

                    if (position == "")
                    {
                        await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                            .WithContent("**You dont have position yet!\nType /position change and select one.**")
                            .AsEphemeral(true));        // Acknowledges The User If He Hasn't Set Position Yet

                    }
                    else
                    {
                        await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                        .WithContent($"**Current Position: {position}**")       // Shows The User's Current Position
                        .AsEphemeral(true));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");      // Prints The Error In The Console
                    await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                    .WithContent("**An error occurred while processing your request.**")    // Acknowledges The User That An Error Occured
                    .AsEphemeral(true));    // Makes It Visible Only For The User Who Ran The Command
                }
            }
        }
        /* Position Commands */



        /* Raid Command */
        [SlashCommand("raid", "Raid a place")]
        public async Task RaidSlashCommand(InteractionContext ctx, [Option("place", "The place you want to raid")]
        [Choice("The Docs","The Docs")]
        [Choice("The Suburbs","The Suburbs")]
        [Choice("The Underground","The Underground")]
        [Choice("West Side's Factories","West Side's Factories")]
        [Choice("South Side's Factories","South Side's Factories")] string place)
        {
            try
            {
                Random rng = new Random();     // Generates Random Number From 1 To 100
                double totalPercentage = 65 + 20 + 10 + 5;     // The Different Percentages
                double result = rng.NextDouble() * totalPercentage;    // Contains The Random Percentage In A Double

                await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
                var embed = new DiscordEmbedBuilder()
                .WithTitle("**Raid**");

                string description = string.Empty;
                DiscordColor color = DiscordColor.DarkGreen;

                if (result < 65)
                {
                    description = $"{place} got successfully raided!";      // Acknowledges The User That The Raid Was Successful
                }

                else if (result < 65 + 20)
                {
                    description = $"{place} failed to get raided!";         // Acknowledges The User That The Raid Failed
                    color = DiscordColor.Red;
                }

                else if (result < 65 + 20 + 5)
                {
                    Random random = new Random();
                    int randomIndex = random.Next(Vars.melee_weapons.Length);
                    string randomWeapon = Vars.melee_weapons[randomIndex];

                    description = $"{place} got successfully raided!\nYou also found {randomWeapon}!";      // Acknowledges The User That The Raid Was Successful And That The User Also Found Melee Weapon
                    color = DiscordColor.Yellow;
                }

                else
                {
                    Random random = new Random();
                    int randomIndex = random.Next(Vars.riot_shield_components.Length);
                    string random_component = Vars.riot_shield_components[randomIndex];

                    description = $"{place} got successfully raided!\nYou also found {random_component}!";      // Acknowledges The User That The Raid Was Successful And That The User Also Found Riot Shield Component
                    color = DiscordColor.Teal;
                }

                embed.WithDescription(description);
                embed.WithColor(color);

                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");      // Prints The Error In The Console
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                .WithContent("**An error occurred while processing your request.**")    // Acknowledges The User That An Error Occured
                .AsEphemeral(true));    // Makes It Visible Only For The User Who Ran The Command
            }
        }
        /* Raid Command */



        /* Riot Shield */
        [SlashCommandGroup("riot", "Riot Shield")]
        public class SubGroupContainer : ApplicationCommandModule
        {
            [SlashCommand("shield", "View your riot shield")]
            public async Task RiotShieldSlashCommand(InteractionContext ctx)
            {
                try
                {
                    string userId = ctx.User.Id.ToString();     // Gets The User's Id
                    string jsonString;
                    using (StreamReader reader = new StreamReader(@"C:\Users\Owner\source\repos\FlamesBotV2\Data.json"))    // Reads The Json File
                    { jsonString = await reader.ReadToEndAsync(); }
                    UserDatabase userDatabase = JsonConvert.DeserializeObject<UserDatabase>(jsonString);    // Deserialize The Json File

                    int riot_shield_lvl = userDatabase.Users[userId].Riot_shield_lvl;
                    string riot_shield_perk = userDatabase.Users[userId].Riot_shield_perk;

                    var builder1 = new DiscordWebhookBuilder();

                    var builder = new DiscordWebhookBuilder();

                    var riot_shield_upgrade = new DiscordButtonComponent(ButtonStyle.Primary, "riot_shield_upgrade", "Upgrade", false);     // Upgrade Button
                    var riot_shield_perk_toughness = new DiscordButtonComponent(ButtonStyle.Primary, "riot_shield_perk_toughness", "Toughness", false);     // Toughness Button
                    var riot_shield_perk_spikes = new DiscordButtonComponent(ButtonStyle.Primary, "riot_shield_perk_spikes", "Spikes", false);          // Spikes Button
                    var riot_shield_perk_camouflage = new DiscordButtonComponent(ButtonStyle.Primary, "riot_shield_perk_camouflage", "Camouflage", false);        // Camouflage Button

                    builder.AddComponents(riot_shield_upgrade);
                    builder1.AddComponents(riot_shield_perk_toughness, riot_shield_perk_spikes, riot_shield_perk_camouflage);

                    if (riot_shield_lvl == 5)       // If The User's Riot Shield Level Is 5
                    {
                        await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
                        var embed1 = new DiscordEmbedBuilder()
                            .WithTitle("**Riot Shield**");
                        embed1.WithDescription($"**Current Level: {riot_shield_lvl}/5\nCurrent Perk: {riot_shield_perk}**");       // Displays The User's Current Riot Shield Level & Perk If The User's Riot Shield Level Is 5
                        embed1.WithColor(DiscordColor.Orange);

                        builder1.AddEmbed(embed1);
                        await ctx.EditResponseAsync(builder1);
                    }

                    else        //If The User's Riot Shield Level Is Anything Else
                    {
                        await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
                        var embed = new DiscordEmbedBuilder()
                            .WithTitle("**Riot Shield**");
                        embed.WithDescription($"**Current Level: {riot_shield_lvl}/5\nCurrent Perk: {riot_shield_perk}**");       // Displays The User's Current Riot Shield Level & Perk If The User's Riot Shield Level Is Anything Except 5
                        embed.WithColor(DiscordColor.Orange);

                        builder.AddEmbed(embed);
                        await ctx.EditResponseAsync(builder);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");      // Prints The Error In The Console
                    await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                    .WithContent("**An error occurred while processing your request.**")    // Acknowledges The User That An Error Occured
                    .AsEphemeral(true));    // Makes It Visible Only For The User Who Ran The Command
                }
            }
        }
        /* Riot Shield */



        /* Notes Commands */
        [SlashCommandGroup("notes", "Note Commands")]
        public class NotesGroupContainer : ApplicationCommandModule
        {
            [SlashCommand("add", "Add a note")]     // Note Add Command
            public async Task NotesAddSlashCommand(InteractionContext ctx, [Option("note", "The note you want to add")] string note)
            {
                try
                {
                    string userId = ctx.User.Id.ToString();     // Gets The User's Id
                    string jsonString;
                    using (StreamReader reader = new StreamReader(@"C:\Users\Owner\source\repos\FlamesBotV2\Data.json"))    // Reads The Json File
                    { jsonString = await reader.ReadToEndAsync(); }
                    UserDatabase userDatabase = JsonConvert.DeserializeObject<UserDatabase>(jsonString);    // Deserialize The Json File

                    List<string> notes = userDatabase.Users[userId].Notes;      // Gets The User's Notes As List

                    if (notes.Count == 0)
                    {
                        userDatabase.Users[userId].Notes.Add("[1] " + note);     // Adds The Note To The User's Notes
                    }
                    else
                    {
                        int notes_count = notes.Count + 1;
                        userDatabase.Users[userId].Notes.Add($"[{notes_count}] " + note);
                    }

                    SLCommands slCommands = new SLCommands();
                    await slCommands.UpdateJsonFileAsync(userDatabase);     // Updates The Json File

                    await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                        .WithContent("**Note added successfully.**")        // Acknowledges The User About The Added Note
                        .AsEphemeral(true));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");      // Prints The Error In The Console
                    await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                    .WithContent("**An error occurred while processing your request.**")    // Acknowledges The User That An Error Occured
                    .AsEphemeral(true));    // Makes It Visible Only For The User Who Ran The Command
                }
            }



            [SlashCommand("change", "Change a note")]       // Notes Change
            public async Task NoteChangeSlashCommand(InteractionContext ctx, [Option("note_index", "The index of the note you want to change")] double noteIndex, [Option("new_note", "The content of the changed note")] string newNote)
            {
                try
                {
                    if (!IsWholeNumber(noteIndex))
                    {
                        await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
                        var embed = new DiscordEmbedBuilder()
                            .WithTitle("**Notes**")
                            .WithDescription("Please enter a whole number index.")
                            .WithColor(DiscordColor.Azure);
                        await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed));
                        return;
                    }
                    string userId = ctx.User.Id.ToString();     // Gets The User's Id
                    string jsonString;
                    using (StreamReader reader = new StreamReader(@"C:\Users\Owner\source\repos\FlamesBotV2\Data.json"))    // Reads The Json File
                    { jsonString = await reader.ReadToEndAsync(); }
                    UserDatabase userDatabase = JsonConvert.DeserializeObject<UserDatabase>(jsonString);    // Deserialize The Json File

                    List<string> notes = userDatabase.Users[userId].Notes;      // Gets The User's Notes As List

                    if (noteIndex >= 1 && noteIndex <= notes.Count)         // Makes Sure That The Note's Index Is Higher Than Or Equal To 1 Or Less Than Or Equal To The Notes's Count
                    {
                        // Subtract 1 from the noteIndex to account for 0-based indexing
                        int index = (int)noteIndex - 1;


                        notes[index] = $"[{noteIndex}] {newNote}";      // Changes The Content Of The Selected Note By Index

                        SLCommands slCommands = new SLCommands();
                        await slCommands.UpdateJsonFileAsync(userDatabase);     // Updates The Json File

                        await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                            .WithContent("**Note changed successfully.**")
                            .AsEphemeral(true));
                    }
                    else
                    {
                        await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                            .WithContent("Invalid note index. Please provide a valid index.")
                            .AsEphemeral(true));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");      // Prints The Error In The Console
                    await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                    .WithContent("**An error occurred while processing your request.**")    // Acknowledges The User That An Error Occured
                    .AsEphemeral(true));    // Makes It Visible Only For The User Who Ran The Command
                }
            }



            [SlashCommand("clear", "Clear your notes")]     // Notes Clear
            public async Task NotesClearSlashCommand(InteractionContext ctx, [Option("confirm", "Type \"confirm\" to confirm clearing all notes")] string confirm)      // Requires The User To Confirm In Order To Clear All Notes
            {
                try
                {
                    if (confirm == "confirm")
                    {
                        string userId = ctx.User.Id.ToString();     // Gets The User's Id
                        string jsonString;
                        using (StreamReader reader = new StreamReader(@"C:\Users\Owner\source\repos\FlamesBotV2\Data.json"))    // Reads The Json File
                        { jsonString = await reader.ReadToEndAsync(); }
                        UserDatabase userDatabase = JsonConvert.DeserializeObject<UserDatabase>(jsonString);    // Deserialize The Json File

                        List<string> notes = userDatabase.Users[userId].Notes;      // Gets The User's Notes As List

                        if (notes.Count == 0)
                        {
                            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                                .WithContent("**You don't have any notes so nothing was deleted.**")        // If There Aren't Any Notes It Acknowledges The User And Does't Delete Anything
                                .AsEphemeral(true));
                        }
                        else
                        {
                            notes.Clear();      // Clears All Notes

                            SLCommands slCommands = new SLCommands();
                            await slCommands.UpdateJsonFileAsync(userDatabase);     // Updates The Json File

                            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                            .WithContent("**All notes were successfully deleted.**")        // Acknowledges The User About The Deleted Notes
                            .AsEphemeral(true));
                        }
                    }
                    else
                    {
                        await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                        .WithContent("**You must type \"confirm\" in the option field in order to clear all notes!**")      // Tells The User To Input "confirm" In The Option In Order To Cleare All Notes
                        .AsEphemeral(true));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");      // Prints The Error In The Console
                    await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                    .WithContent("**An error occurred while processing your request.**")    // Acknowledges The User That An Error Occured
                    .AsEphemeral(true));    // Makes It Visible Only For The User Who Ran The Command
                }
            }



            [SlashCommand("remove", "Remove a note")] // Notes Remove
            public async Task NotesDeleteSlashCommand(InteractionContext ctx, [Option("note_index", "The index of the note you want to remove")] double noteIndex)
            {
                try
                {
                    if (!IsWholeNumber(noteIndex))
                    {
                        await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                               .WithContent("**Please enter a positive whole number Index.**") // Acknowledges The User That The Note Was Deleted
                               .AsEphemeral(true));
                        return;
                    }
                    else
                    {
                        string userId = ctx.User.Id.ToString(); // Gets The User's Id
                        string jsonString;
                        using (StreamReader reader = new StreamReader(@"C:\Users\Owner\source\repos\FlamesBotV2\Data.json")) // Reads The Json File
                        { jsonString = await reader.ReadToEndAsync(); }
                        UserDatabase userDatabase = JsonConvert.DeserializeObject<UserDatabase>(jsonString); // Deserialize The Json File

                        List<string> notes = userDatabase.Users[userId].Notes; // Gets The User's Notes As List

                        string noteIndexStr = $"[{noteIndex}]";     // Builds The Note Index String By Adding The noteIndex (The Number From The Option)

                        string foundNote = FindAndRemoveNoteByIndex(noteIndexStr, notes);   // Finds The Note By Its Index

                        if (foundNote != null)      // If The Note Is Not Null
                        {
                            SLCommands slCommands = new SLCommands();
                            await slCommands.UpdateJsonFileAsync(userDatabase);     // Updates The Json File

                            foundNote = foundNote.Substring(noteIndexStr.Length).Trim();

                            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                                .WithContent($"**Successfully deleted note: {foundNote}**") // Acknowledges The User That The Note Was Deleted
                                .AsEphemeral(true));
                        }
                        else
                        {
                            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                                .WithContent("**Note not found.**") // If The There Isn't Note With Such Index
                                .AsEphemeral(true));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}"); // Prints The Error In The Console
                    await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                        .WithContent("**An error occurred while processing your request.**") // Acknowledges The User That An Error Occured
                        .AsEphemeral(true)); // Makes It Visible Only For The User Who Ran The Command
                }
            }
            static string FindAndRemoveNoteByIndex(string index, List<string> notes)
            {
                string foundNote = null;    // By Default The Note Is Null

                for (int i = 0; i < notes.Count; i++)
                {
                    if (notes[i].StartsWith(index + " "))
                    {
                        foundNote = notes[i];
                        notes.RemoveAt(i);      // Removes The Note
                        break; // Stop searching after finding the first matching note
                    }
                }
                return foundNote;
            }



            [SlashCommand("view", "View your notes")]       // Notes View
            public async Task NotesViewSlashCommand(InteractionContext ctx)
            {
                try
                {
                    await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

                    string userId = ctx.User.Id.ToString();     // Gets The User's Id
                    string jsonString;
                    using (StreamReader reader = new StreamReader(@"C:\Users\Owner\source\repos\FlamesBotV2\Data.json"))    // Reads The Json File
                    { jsonString = await reader.ReadToEndAsync(); }
                    UserDatabase userDatabase = JsonConvert.DeserializeObject<UserDatabase>(jsonString);    // Deserialize The Json File

                    List<string> notes = userDatabase.Users[userId].Notes;      // Gets The User's Notes As List

                    if (notes.Count == 0)
                    {
                        var embed1 = new DiscordEmbedBuilder()
                        .WithTitle("**Notes**")
                        .WithDescription("Empty")       // Says Empty If There Aren't Any Notes
                        .WithColor(DiscordColor.DarkGreen);
                        await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed1));
                    }
                    else
                    {
                        var embed = new DiscordEmbedBuilder()
                        .WithTitle("**Notes**")
                        .WithDescription(string.Join("\n", notes))      // Lists All Notes
                        .WithColor(DiscordColor.DarkGreen);
                        await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");      // Prints The Error In The Console
                    await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                    .WithContent("**An error occurred while processing your request.**")    // Acknowledges The User That An Error Occured
                    .AsEphemeral(true));    // Makes It Visible Only For The User Who Ran The Command
                }

            }
            private bool IsWholeNumber(double number)
            {
                return Math.Abs(number % 1) < double.Epsilon;
            }
        }
        /* Notes Commands */



        /* Fish Command */
        [SlashCommand("fish", "Fish the fish")]
        public async Task FishSlashCommand(InteractionContext ctx)
        {
            try
            {
                string userId = ctx.User.Id.ToString();     // Gets The User's Id
                string jsonString;
                using (StreamReader reader = new StreamReader(@"C:\Users\Owner\source\repos\FlamesBotV2\Data.json"))    // Reads The Json File
                { jsonString = await reader.ReadToEndAsync(); }
                UserDatabase userDatabase = JsonConvert.DeserializeObject<UserDatabase>(jsonString);    // Deserialize The Json File

                Random random = new Random();       // Generate Random Number
                double totalPercentage = 60 + 20 + 10 + 7 + 2 + 0.99 + 0.01;    // The Different Percentages
                double randomNumber = random.NextDouble() * totalPercentage;    // Contains The Random Percentage In A Double

                string[] selectedArray;     // Declare The Randomly Selected Fish Array
                string fishtype;    // Declare Fish Type
                DiscordColor fishcolor;     // Declare The Embed Message's Color Depending On The Caught Fish

                int balance = userDatabase.Users[userId].Balance;    // Gets The User's Balance

                /* Total Number Of Caught Fish */
                int total_fish_count = userDatabase.Users[userId].Total_fish_count;
                int total_common_fish = userDatabase.Users[userId].Total_common_fish;
                int total_uncommon_fish = userDatabase.Users[userId].Total_uncommon_fish;
                int total_rare_fish = userDatabase.Users[userId].Total_rare_fish;
                int total_epic_fish = userDatabase.Users[userId].Total_epic_fish;
                int total_legendary_fish = userDatabase.Users[userId].Total_legendary_fish;
                int total_mythical_fish = userDatabase.Users[userId].Total_mythical_fish;
                int total_void_fish = userDatabase.Users[userId].Total_void_fish;
                /* Total Number Of Caught Fish */

                if (randomNumber < 60)
                {
                    selectedArray = Vars.common_fish;       // The User Caugh Common Fish
                    fishcolor = DiscordColor.NotQuiteBlack;
                    fishtype = "[COMMON]";
                    balance += 5;
                    total_common_fish += 1;
                    userDatabase.Users[userId].Total_common_fish = total_common_fish;

                }

                else if (randomNumber < 60 + 20)
                {
                    selectedArray = Vars.uncommon_fish;     // The User Caugh Unommon Fish
                    fishcolor = DiscordColor.Green;
                    fishtype = "[UNCOMMON]";
                    balance += 50;
                    total_uncommon_fish += 1;
                    userDatabase.Users[userId].Total_uncommon_fish = total_uncommon_fish;
                }
                else if (randomNumber < 60 + 20 + 10)
                {
                    selectedArray = Vars.rare_fish;         // The User Caugh Rare Fish
                    fishcolor = DiscordColor.Azure;
                    fishtype = "[RARE]";
                    balance += 500;
                    total_rare_fish += 1;
                    userDatabase.Users[userId].Total_rare_fish = total_rare_fish;
                }
                else if (randomNumber < 60 + 20 + 10 + 7)
                {
                    selectedArray = Vars.epic_fish;         // The User Caugh Epic Fish
                    fishcolor = DiscordColor.Purple;
                    fishtype = "[EPIC]";
                    balance += 3000;
                    total_epic_fish += 1;
                    userDatabase.Users[userId].Total_epic_fish = total_epic_fish;
                }
                else if (randomNumber < 60 + 20 + 10 + 7 + 2)
                {
                    selectedArray = Vars.legendary_fish;    // The User Caugh Legendary Fish
                    fishcolor = DiscordColor.Orange;
                    fishtype = "[LEGENDARY]";
                    balance += 20000;
                    total_legendary_fish += 1;
                    userDatabase.Users[userId].Total_legendary_fish = total_legendary_fish;
                }
                else if (randomNumber < 60 + 20 + 10 + 7 + 2 + 0.99)
                {
                    selectedArray = Vars.mythical_fish;     // The User Caugh Mythical Fish
                    fishcolor = DiscordColor.DarkRed;
                    fishtype = "[MYTHICAL]";
                    balance += 50000;
                    total_mythical_fish += 1;
                    userDatabase.Users[userId].Total_mythical_fish = total_mythical_fish;
                }

                else
                {
                    selectedArray = Vars.void_fish;     // The User Caugh Void Fish
                    fishcolor = DiscordColor.Black;
                    fishtype = "[VOID]";
                    balance += 100000;
                    total_void_fish += 1;
                    userDatabase.Users[userId].Total_void_fish = total_void_fish;
                }

                total_fish_count += 1;
                userDatabase.Users[userId].Total_fish_count = total_fish_count;     // Updates The User's Total Fish Count (Always)
                userDatabase.Users[userId].Balance = balance;        // Updates The User's Balance

                await UpdateJsonFileAsync(userDatabase);

                string randomFish = selectedArray[random.Next(selectedArray.Length)];   // Contains The Caught Fish In A String

                await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

                var embed = new DiscordEmbedBuilder()
                    .WithTitle("**Fish**")
                    .WithDescription($"**{fishtype}\nYou fished out: `{randomFish}`**")         // Shows The User The Fish He Fished Out
                    .WithColor(fishcolor);

                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");      // Prints The Error In The Console
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                    .WithContent("**An error occurred while processing your request.**")    // Acknowledges The User That An Error Occured
                    .AsEphemeral(true));    // Makes It Visible Only For The User Who Ran The Command
            }
        }
        /* Fish Command */



        /* Fish Commands */
        [SlashCommandGroup("fishing", "Fishing commands")]
        public class FishingGroupContainer : ApplicationCommandModule
        {
            [SlashCommand("shop", "Buy fish-related things")]        // Fishing Shop
            public async Task FishShopSlashCommand(InteractionContext ctx, [Option("item","The item you want to buy")]
            [Choice("Common Shards","Common Shards")]
            [Choice("Uncommon Shards","Uncommon Shards")]
            [Choice("Rare Shards","Rare Shards")]
            [Choice("Epic Shards","Epic Shards")]
            [Choice("Legendary Shards","Legendary Shards")] string item = null)     // The item Is Null By Default
            {
                try
                {
                    string userId = ctx.User.Id.ToString();     // Gets The User's Id
                    string jsonString;
                    using (StreamReader reader = new StreamReader(@"C:\Users\Owner\source\repos\FlamesBotV2\Data.json"))    // Reads The Json File
                    { jsonString = await reader.ReadToEndAsync(); }
                    UserDatabase userDatabase = JsonConvert.DeserializeObject<UserDatabase>(jsonString);    // Deserialize The Json File

                    int balance = userDatabase.Users[userId].Balance;    // Gets The User's Balance

                    string responseMessage = "Invalid item selected.";
                    bool purchaseSuccessful = false;

                    if (item == null)
                    {
                        /* Long Embed Message Displaying All The Items In The Fishing Shop */
                        await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

                        var embed = new DiscordEmbedBuilder()
                        .WithTitle("**Fish Shop**")
                        .WithDescription("**[Common Shards 100x]" +
                        "\nPrice: 10 000 coins**" +
                        "\nBoosts your chances of catching common fish by 60%\n" +
                        "\n**[Uncommon Shards 50x]" +
                        "\nPrice: 20 000 coins**" +
                        "\nBoosts your chances of catching uncommon fish by 50%\n" +
                        "\n**[Rare Shards 30x]" +
                        "\nPrice: 40 000 coins**" +
                        "\nBoosts your chances of catching rare fish by 40%\n" +
                        "\n**[Epic Shards 20x]" +
                        "\nPrice: 80 000 coins**" +
                        "\nBoosts your chances of catching epic fish by 30%\n" +
                        "\n**[Legendary Shards 10x]" +
                        "\nPrice: 100 000 coins**" +
                        "\nBoosts your chances of catching legendary fish by 20%\n" +
                        "\n**[Treasure Map]" +
                        "\nPrice: 200 000 coins**" +
                        "\nAncient map that will guide you towards a treasure")
                        .WithColor(DiscordColor.Azure);

                        await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed));
                        /* Long Embed Message Displaying All The Items In The Fishing Shop */
                    }
                    else
                    {
                        switch (item)
                        {
                            case "Common Shards":
                                if (balance >= 10000)
                                {
                                    balance -= 10000;       // Removes 10k Coins From The User's Balance
                                    userDatabase.Users[userId].Balance = balance;
                                    responseMessage = "Common Shards successfully bought!";     // Acknowledges The User That He Successfully Bought Common Shards
                                    purchaseSuccessful = true;
                                }
                                else
                                {
                                    responseMessage = "You don't have enough coins!";       // If The User Doesn't Have Enough Coins
                                }
                                break;

                            case "Uncommon Shards":
                                if (balance >= 20000)
                                {
                                    balance -= 20000;       // Removes 20k Coins From The User's Balance
                                    userDatabase.Users[userId].Balance = balance;
                                    responseMessage = "Uncommon Shards successfully bought!";   // Acknowledges The User That He Successfully Bought Unommon Shards
                                    purchaseSuccessful = true;

                                }
                                else
                                {
                                    responseMessage = "You don't have enough coins!";       // If The User Doesn't Have Enough Coins
                                }
                                break;

                            case "Rare Shards":
                                if (balance >= 40000)
                                {
                                    balance -= 40000;       // Removes 40k Coins From The User's Balance
                                    userDatabase.Users[userId].Balance = balance;
                                    responseMessage = "Rare Shards successfully bought!";       // Acknowledges The User That He Successfully Bought Rare Shards
                                    purchaseSuccessful = true;
                                }
                                else
                                {
                                    responseMessage = "You don't have enough coins!";       // If The User Doesn't Have Enough Coins
                                }
                                break;

                            case "Epic Shards":
                                if (balance >= 80000)
                                {
                                    balance -= 80000;       // Removes 80k Coins From The User's Balance
                                    userDatabase.Users[userId].Balance = balance;
                                    responseMessage = "Epic Shards successfully bought!";       // Acknowledges The User That He Successfully Bought Epic Shards
                                    purchaseSuccessful = true;
                                }
                                else
                                {
                                    responseMessage = "You don't have enough coins!";       // If The User Doesn't Have Enough Coins
                                }
                                break;

                            case "Legendary Shards":
                                if (balance >= 100000)
                                {
                                    balance -= 100000;      // Removes 100k Coins From The User's Balance
                                    userDatabase.Users[userId].Balance = balance;
                                    responseMessage = "Legendary Shards successfully bought!";  // Acknowledges The User That He Successfully Bought Legendary Shards
                                    purchaseSuccessful = true;
                                }
                                else
                                {
                                    responseMessage = "You don't have enough coins!";       // If The User Doesn't Have Enough Coins
                                }
                                break;
                        };

                        SLCommands slCommands = new SLCommands();
                        await slCommands.UpdateJsonFileAsync(userDatabase);

                        await SendResponse(ctx, responseMessage, purchaseSuccessful);       // Sends The Response

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");      // Prints The Error In The Console
                    await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                        .WithContent("**An error occurred while processing your request.**")    // Acknowledges The User That An Error Occured
                        .AsEphemeral(true));    // Makes It Visible Only For The User Who Ran The Command
                }
            }
            private async Task SendResponse(InteractionContext ctx, string message, bool success)       // The Method That Builds The Embed Message
            {
                await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

                var embed = new DiscordEmbedBuilder()
                    .WithTitle("**Fish Shop**")
                    .WithDescription(message)
                    .WithColor(success ? DiscordColor.Azure : DiscordColor.Red);

                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed));
            }



            [SlashCommand("stats", "View your fishing stats")]      // Fishing Stats
            public async Task FishStatsSlashCommand(InteractionContext ctx)
            {
                try
                {
                    string userId = ctx.User.Id.ToString();         // Gets The User's Id
                    string jsonString;
                    using (StreamReader reader = new StreamReader(@"C:\Users\Owner\source\repos\FlamesBotV2\Data.json"))    // Reads The Json File
                    { jsonString = await reader.ReadToEndAsync(); }
                    UserDatabase userDatabase = JsonConvert.DeserializeObject<UserDatabase>(jsonString);        // Deserialize The Json File

                    /* Declares Integers For The Total Number Of Caught Fish Of Each Type */
                    int total_fish_count1 = userDatabase.Users[userId].Total_fish_count;
                    int total_common_fish1 = userDatabase.Users[userId].Total_common_fish;
                    int total_uncommon_fish1 = userDatabase.Users[userId].Total_uncommon_fish;
                    int total_rare_fish1 = userDatabase.Users[userId].Total_rare_fish;
                    int total_epic_fish1 = userDatabase.Users[userId].Total_epic_fish;
                    int total_legendary_fish1 = userDatabase.Users[userId].Total_legendary_fish;
                    int total_mythical_fish1 = userDatabase.Users[userId].Total_mythical_fish;
                    int total_void_fish1 = userDatabase.Users[userId].Total_void_fish;
                    /* Declares Integers For The Total Number Of Caught Fish Of Each Type */

                    /* Long Embed Message Displaying How Many Fish Of Each Type The User Has Caught */
                    await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

                    var embed = new DiscordEmbedBuilder()
                        .WithTitle("**Fish Stats**")
                        .WithDescription($"**Total Fish Catched: `{total_fish_count1}`\n" +
                        $"Common Fish Caught: `{total_common_fish1}`\n" +
                        $"Uncommon Fish Caught: `{total_uncommon_fish1}`\n" +
                        $"Rare Fish Caught: `{total_rare_fish1}`\n" +
                        $"Epic Fish Caught: `{total_epic_fish1}`\n" +
                        $"Legendary Fish Caught: `{total_legendary_fish1}`\n" +
                        $"Mythical Fish Caught: `{total_mythical_fish1}`\n" +
                        $"Void Fish Caught: `{total_void_fish1}`**")
                        .WithColor(DiscordColor.Azure);

                    await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed));
                    /* Long Embed Message Displaying How Many Fish Of Each Type The User Has Caught */
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");      // Prints The Error In The Console
                    await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                        .WithContent("**An error occurred while processing your request.**")    // Acknowledges The User That An Error Occured
                        .AsEphemeral(true));    // Makes It Visible Only For The User Who Ran The Command
                }
            }
        }
        /* Fish Commands */



        /* Play Command */
        [SlashCommand("play", "Make Flames play music")]
        public async Task PlaySlashComand(InteractionContext ctx, [Option("input", "Input the name of the song and optionally the author")] string query)
        {
            try
            {
                var userVC = ctx.Member.VoiceState.Channel;
                var lavalinkInstance = ctx.Client.GetLavalink();

                var node = lavalinkInstance.ConnectedNodes.Values.First();
                await node.ConnectAsync(userVC);

                var conn = node.GetGuildConnection(ctx.Member.VoiceState.Guild);

                var searchQuery = await node.Rest.GetTracksAsync(query);
                var musicTrack = searchQuery.Tracks.First();

                await conn.PlayAsync(musicTrack);

                string musicDescription = $"Now Playing: {musicTrack.Title} \n" +
                                          $"Author: {musicTrack.Author} \n" +
                                          $"URL: {musicTrack.Uri}";

                await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

                var embed = new DiscordEmbedBuilder()
                   .WithTitle($"**Successfully joined channel {userVC.Name} and playing music**")
                   .WithDescription(musicDescription)
                   .WithColor(DiscordColor.Purple);

                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");      // Prints The Error In The Console
                if (ctx.Member.VoiceState == null || ctx.Member.VoiceState.Channel == null)
                {
                    await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                        .WithContent("**You must enter a VC!**")    // Tells The User To Enter A VC
                        .AsEphemeral(true));    // Makes It Visible Only For The User Who Ran The Command
                }
                else
                {
                    await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                    .WithContent("**An error occurred while processing your request.**")    // Acknowledges The User That An Error Occured
                    .AsEphemeral(true));    // Makes It Visible Only For The User Who Ran The Command
                }
            }
        }
        /* Play Command */



        /* Pause Command */
        [SlashCommand("pause", "Make Flames pause the music")]
        public async Task PauseSlashCommand(InteractionContext ctx)
        {
            try
            {
                var userVC = ctx.Member.VoiceState.Channel;
                var lavalinkInstance = ctx.Client.GetLavalink();

                var node = lavalinkInstance.ConnectedNodes.Values.First();
                var conn = node.GetGuildConnection(ctx.Member.VoiceState.Guild);

                await conn.PauseAsync();

                await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

                var embed = new DiscordEmbedBuilder()
                   .WithTitle("**Music Paused!**")
                   .WithColor(DiscordColor.Purple);

                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");      // Prints The Error In The Console
                if (ctx.Member.VoiceState == null || ctx.Member.VoiceState.Channel == null)
                {
                    await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                        .WithContent("**You must enter a VC!**")    // Tells The User To Enter A VC
                        .AsEphemeral(true));    // Makes It Visible Only For The User Who Ran The Command
                }
                else
                {
                    await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                    .WithContent("**An error occurred while processing your request.**")    // Acknowledges The User That An Error Occured
                    .AsEphemeral(true));    // Makes It Visible Only For The User Who Ran The Command
                }
            }
        }
        /* Pause Command */



        /* Resume Command */
        [SlashCommand("resume", "Make Flames resume the music")]
        public async Task ResumeSlashCommand(InteractionContext ctx)
        {
            try
            {
                var userVC = ctx.Member.VoiceState.Channel;
                var lavalinkInstance = ctx.Client.GetLavalink();

                var node = lavalinkInstance.ConnectedNodes.Values.First();
                var conn = node.GetGuildConnection(ctx.Member.VoiceState.Guild);

                await conn.ResumeAsync();

                await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

                var embed = new DiscordEmbedBuilder()
                   .WithTitle("**Music Resumed!**")
                   .WithColor(DiscordColor.Purple);

                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");      // Prints The Error In The Console
                if (ctx.Member.VoiceState == null || ctx.Member.VoiceState.Channel == null)
                {
                    await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                        .WithContent("**You must enter a VC!**")    // Tells The User To Enter A VC
                        .AsEphemeral(true));    // Makes It Visible Only For The User Who Ran The Command
                }
                else
                {
                    await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                    .WithContent("**An error occurred while processing your request.**")    // Acknowledges The User That An Error Occured
                    .AsEphemeral(true));    // Makes It Visible Only For The User Who Ran The Command
                }
            }
        }
        /* Resume Command */



        /* Stop Command */
        [SlashCommand("stop", "Make Flames stop the music")]
        public async Task StopSlashCommand(InteractionContext ctx)
        {
            try
            {
                var userVC = ctx.Member.VoiceState.Channel;
                var lavalinkInstance = ctx.Client.GetLavalink();

                var node = lavalinkInstance.ConnectedNodes.Values.First();
                var conn = node.GetGuildConnection(ctx.Member.VoiceState.Guild);

                await conn.StopAsync();
                await conn.DisconnectAsync();

                await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

                var embed = new DiscordEmbedBuilder()
                   .WithTitle("**Stopped the Track!**")
                   .WithDescription("Successfully disconnected from the VC")
                   .WithColor(DiscordColor.Purple);

                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");      // Prints The Error In The Console
                if (ctx.Member.VoiceState == null || ctx.Member.VoiceState.Channel == null)
                {
                    await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                        .WithContent("**You must enter a VC!**")    // Tells The User To Enter A VC
                        .AsEphemeral(true));    // Makes It Visible Only For The User Who Ran The Command
                }
                else
                {
                    await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                    .WithContent("**An error occurred while processing your request.**")    // Acknowledges The User That An Error Occured
                    .AsEphemeral(true));    // Makes It Visible Only For The User Who Ran The Command
                }
            }
        }
        /* Stop Command */



        /* Dm Command */
        [SlashCommand("dm", "Make Flames message specific user")]
        public async Task DmSlashCommand(InteractionContext ctx,
        [Option("message", "The message to send")] string message,
        [Option("user", "The user to send the message to")] DiscordUser user = null)    // By Default The user Is Null
        {
            try
            {
                user = user ?? ctx.User;
                var member = await ctx.Guild.GetMemberAsync(user.Id);

                var dmChannel = await member.CreateDmChannelAsync();
                await dmChannel.SendMessageAsync(message);      // DMs The Message To The User

                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                .WithContent("**Message sent!**")    // Acknowledges The User That The Message Was Sent
                .AsEphemeral(true));    // Makes It Visible Only For The User Who Ran The Command
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");      // Prints The Error In The Console
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                    .WithContent("**An error occurred while processing your request.**")    // Acknowledges The User That An Error Occured
                    .AsEphemeral(true));    // Makes It Visible Only For The User Who Ran The Command
            }
        }
        /* Dm Command */



        /* Warn Command */
        [SlashCommand("warn", "Warn specific user")]
        public async Task WarnSlashCommand(InteractionContext ctx,
        [Option("user", "The user you want to warn")] DiscordUser user,
        [Option("reason", "The reason why the user was being warned")] string reason)
        {
            try
            {
                DiscordUser user1 = ctx.User;       // The User Who Ran The Command
                var member = await ctx.Guild.GetMemberAsync(user.Id);   // Gets The User Who Will Be Warned

                var dmChannel = await member.CreateDmChannelAsync();

                var embed = new DiscordEmbedBuilder()       // Creates The Embed Message
                   .WithTitle("**Warning**")
                   .WithDescription($"**{member.Mention}, you got warned by {user1.Mention}\n Reason: {reason}**")
                   .WithColor(DiscordColor.Yellow);

                await dmChannel.SendMessageAsync(embed);      // DMs The Warning To The User

                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                .WithContent("**Warning sent!**")    // Acknowledges The User That The Message Was Sent
                .AsEphemeral(true));    // Makes It Visible Only For The User Who Ran The Command
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");      // Prints The Error In The Console
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                    .WithContent("**An error occurred while processing your request.**")    // Acknowledges The User That An Error Occured
                    .AsEphemeral(true));    // Makes It Visible Only For The User Who Ran The Command
            }
        }
        /* Warn Command */
    }
}
