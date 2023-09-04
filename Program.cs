/* NameSpaces */
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.Lavalink;
using DSharpPlus.Net;
using DSharpPlus.SlashCommands;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static FlamesBotV2.SLCommands;
/* NameSpaces */

namespace FlamesBotV2
{
    public sealed class Program
    {
        public static DiscordClient Client { get; private set; }
        public static CommandsNextExtension Commands { get; private set; }
        public static InteractivityExtension Interactivity { get; private set; }

        static async Task Main(string[] args)
        {
            /* Reads The Json File */
            var jsonReader = new JSONReader();
            await jsonReader.ReadJSON();
            ulong serverId = 1138196104761049199;
            /* Reads The Json File */

            /* Discord Bot Configuration */
            var config = new DiscordConfiguration()
            {
                Intents = DiscordIntents.All,
                Token = jsonReader.DiscordToken,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MinimumLogLevel = LogLevel.Debug
            };
            /* Discord Bot Configuration */

            Client = new DiscordClient(config);     // Sets The Client To Be Equal The Discord Bot Configuration

            Interactivity = Client.UseInteractivity(new InteractivityConfiguration()
            {
                Timeout = TimeSpan.FromMinutes(2)
            });

            Client.Ready += Client_Ready;       // Client Event Handler
            Client.ComponentInteractionCreated += InteractionEventHandler;  // Component Event Handler

            /* Commands Configuration */
            var commandsConfig = new CommandsNextConfiguration()
            {
                StringPrefixes = new string[] { jsonReader.DiscordPrefix },
                EnableMentionPrefix = true,
                EnableDms = true,
                EnableDefaultHelp = false,
            };
            /* Commands Configuration */

            /* Lavalink Configuration For Localhosting */
            var endpoint = new ConnectionEndpoint
            {
                Hostname = "127.0.0.1", // The Host Ip From The YAML File
                Port = 2333     // The Port From The YAML File
            };

            var lavalinkConfig = new LavalinkConfiguration
            {
                Password = "youshallnotpass",   // The Password From The YAML File
                RestEndpoint = endpoint,    // Rest Endpoint
                SocketEndpoint = endpoint       // Socket Endpoint 
            };
            /* Lavalink Configuration For Localhosting */

            var lavalink = Client.UseLavalink();

            Commands = Client.UseCommandsNext(commandsConfig);
            var SlashCommandsConfig = Client.UseSlashCommands();

            Commands.RegisterCommands<Commands>();      // Registers The Prefix Commands
            SlashCommandsConfig.RegisterCommands<SLCommands>();     // Registers The Slash Command

            await Client.ConnectAsync();

            /* ADDS ALL THE USERS ( EXCEPT BOTS) ALONG WITH ALL THE DATA */
            /* <----- REMOVE THE COMMENT UP AND DOWN
            var server = await Client.GetGuildAsync(serverId);

            if (server != null)
            {
                // Get all users in the server, excluding bots
                var users = server.Members.Values.Where(user => !user.IsBot);

                // Create a dictionary to store user IDs with extended data
                var userDict = new Dictionary<ulong, object>();

                foreach (var user in users)
                {
                    var userData = new
                    {
                        balance = 10000,
                        inv_lvl = 1,
                        slot_lvl = 1,
                        riot_shield_lvl = 1,
                        riot_shield_perk = "None",
                        name = user.Username, // You can change this based on how you want to get the user's name.
                        position = "",
                        total_fish_count = 0,
                        total_common_fish = 0,
                        total_uncommon_fish = 0,
                        total_rare_fish = 0,
                        total_epic_fish = 0,
                        total_legendary_fish = 0,
                        total_mythical_fish = 0,
                        total_void_fish = 0,
                        inv = new List<object>(),
                        notes = new List<object>(),
                        slots = new
                        {
                            melee_slot_1 = "",
                            range_slot_1 = "",
                            misc_slot_1 = ""
                        }
                    };

                    userDict[user.Id] = userData;
                }

                // Serialize the user dictionary to JSON
                var json = JsonConvert.SerializeObject(new { Users = userDict }, Formatting.Indented);

                // Write the JSON string to a file
                File.WriteAllText(@"C:\\Users\\Owner\\source\\repos\\FlamesBotV2\\Data.json", json);
            }
            else
            {
                Console.WriteLine("Server not found!");
            }
            */

            await lavalink.ConnectAsync(lavalinkConfig);    // Connects To Lavalink
            await Task.Delay(-1);
        }


        private static async Task InteractionEventHandler(DiscordClient sender, ComponentInteractionCreateEventArgs e)
        {
            switch (e.Id)
            {
                case "hawk_eye_craft":
                    {
                        await e.Interaction.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
                        string userId = e.User.Id.ToString();
                        string jsonString = File.ReadAllText(@"C:\\Users\\Owner\\source\\repos\\FlamesBotV2\\Data.json");
                        var userDatabase = JsonConvert.DeserializeObject<UserDatabase>(jsonString);
                        if (CanAddMoreItems(userDatabase.Users[userId]))
                        {
                            userDatabase.Users[userId].Inv.Add("Hawk Eye");

                            var embed = new DiscordEmbedBuilder()
                            .WithTitle("**Armory**")
                            .WithDescription("**Hawk Eye Crafted!**")
                            .WithColor(DiscordColor.Yellow);

                            await e.Interaction.CreateFollowupMessageAsync(new DiscordFollowupMessageBuilder().AddEmbed(embed));

                            var serializerSettings = new JsonSerializerSettings { Formatting = Formatting.Indented };
                            string updatedJsonString = JsonConvert.SerializeObject(userDatabase, serializerSettings);
                            File.WriteAllText(@"C:\\Users\\Owner\\source\\repos\\FlamesBotV2\\Data.json", updatedJsonString);
                        }
                        else
                        {
                            var embed = new DiscordEmbedBuilder()
                                .WithTitle("**System**")
                                .WithDescription("**Your inventory is full.**")
                                .WithColor(DiscordColor.Azure);

                            await e.Interaction.CreateFollowupMessageAsync(new DiscordFollowupMessageBuilder().AddEmbed(embed));
                        }
                    }
                    break;

                case "glock_17_craft":
                    {
                        await e.Interaction.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
                        string userId = e.User.Id.ToString();
                        string jsonString = File.ReadAllText(@"C:\\Users\\Owner\\source\\repos\\FlamesBotV2\\Data.json");
                        var userDatabase = JsonConvert.DeserializeObject<UserDatabase>(jsonString);
                        if (CanAddMoreItems(userDatabase.Users[userId]))
                        {
                            userDatabase.Users[userId].Inv.Add("Glock 17");

                            var embed = new DiscordEmbedBuilder()
                            .WithTitle("**Armory**")
                            .WithDescription("**Glock 17 Crafted!**")
                            .WithColor(DiscordColor.Yellow);

                            await e.Interaction.CreateFollowupMessageAsync(new DiscordFollowupMessageBuilder().AddEmbed(embed));

                            var serializerSettings = new JsonSerializerSettings { Formatting = Formatting.Indented };   // Formats The Json File
                            string updatedJsonString = JsonConvert.SerializeObject(userDatabase, serializerSettings);       // Serializes The Json File
                            File.WriteAllText(@"C:\\Users\\Owner\\source\\repos\\FlamesBotV2\\Data.json", updatedJsonString);       // Updates The Json File
                        }
                        else
                        {
                            var embed = new DiscordEmbedBuilder()
                                .WithTitle("**System**")
                                .WithDescription("**Your inventory is full.**")
                                .WithColor(DiscordColor.Azure);

                            await e.Interaction.CreateFollowupMessageAsync(new DiscordFollowupMessageBuilder().AddEmbed(embed));
                        }
                    }
                    break;

                case "spas_12_craft":       // Glock 17 Button Interaction Code
                    {
                        await e.Interaction.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
                        string userId = e.User.Id.ToString();         // Gets The User's Id
                        string jsonString = File.ReadAllText(@"C:\Users\Owner\source\repos\FlamesBotV2\Data.json");     // The Full Path To The Json File
                        UserDatabase userDatabase = JsonConvert.DeserializeObject<UserDatabase>(jsonString);        // Deserialize The Json File
                        if (CanAddMoreItems(userDatabase.Users[userId]))
                        {
                            userDatabase.Users[userId].Inv.Add("SPAS-12");

                            var embed = new DiscordEmbedBuilder()
                            .WithTitle("**Armory**")
                            .WithDescription("**SPAS-12 Crafted!**")
                            .WithColor(DiscordColor.Yellow);

                            await e.Interaction.CreateFollowupMessageAsync(new DiscordFollowupMessageBuilder().AddEmbed(embed));

                            var serializerSettings = new JsonSerializerSettings { Formatting = Formatting.Indented };
                            string updatedJsonString = JsonConvert.SerializeObject(userDatabase, serializerSettings);
                            File.WriteAllText(@"C:\\Users\\Owner\\source\\repos\\FlamesBotV2\\Data.json", updatedJsonString);
                        }
                        else
                        {
                            var embed = new DiscordEmbedBuilder()
                                .WithTitle("**System**")
                                .WithDescription("**Your inventory is full.**")
                                .WithColor(DiscordColor.Azure);

                            await e.Interaction.CreateFollowupMessageAsync(new DiscordFollowupMessageBuilder().AddEmbed(embed));
                        }
                    }
                    break;

                case "m40a5_craft":         // M40A5 Button Interaction Code
                    {
                        await e.Interaction.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
                        string userId = e.User.Id.ToString();         // Gets The User's Id
                        string jsonString = File.ReadAllText(@"C:\Users\Owner\source\repos\FlamesBotV2\Data.json");     // The Full Path To The Json File
                        UserDatabase userDatabase = JsonConvert.DeserializeObject<UserDatabase>(jsonString);        // Deserialize The Json File
                        if (CanAddMoreItems(userDatabase.Users[userId]))
                        {
                            userDatabase.Users[userId].Inv.Add("M40A5");

                            var embed = new DiscordEmbedBuilder()
                            .WithTitle("**Armory**")
                            .WithDescription("**M40A5 Crafted!**")
                            .WithColor(DiscordColor.Yellow);

                            await e.Interaction.CreateFollowupMessageAsync(new DiscordFollowupMessageBuilder().AddEmbed(embed));

                            var serializerSettings = new JsonSerializerSettings { Formatting = Formatting.Indented };   // Formats The Json File
                            string updatedJsonString = JsonConvert.SerializeObject(userDatabase, serializerSettings);       // Serializes The Json File
                            File.WriteAllText(@"C:\\Users\\Owner\\source\\repos\\FlamesBotV2\\Data.json", updatedJsonString);       // Updates The Json File
                        }
                        else
                        {
                            var embed = new DiscordEmbedBuilder()
                                .WithTitle("**System**")
                                .WithDescription("**Your inventory is full.**")
                                .WithColor(DiscordColor.Azure);

                            await e.Interaction.CreateFollowupMessageAsync(new DiscordFollowupMessageBuilder().AddEmbed(embed));
                        }
                    }
                    break;

                case "ak_47_craft":         // AK-47 Button Interaction Code
                    {
                        await e.Interaction.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
                        string userId = e.User.Id.ToString();
                        string jsonString;
                        using (StreamReader reader = new StreamReader(@"C:\Users\Owner\source\repos\FlamesBotV2\Data.json"))    // Reads The Json File
                        { jsonString = await reader.ReadToEndAsync(); }
                        UserDatabase userDatabase = JsonConvert.DeserializeObject<UserDatabase>(jsonString);    // Deserialize The Json File
                        if (CanAddMoreItems(userDatabase.Users[userId]))
                        {
                            userDatabase.Users[userId].Inv.Add("AK-47");

                            var embed = new DiscordEmbedBuilder()
                            .WithTitle("**Armory**")
                            .WithDescription("**AK-47 Crafted!**")
                            .WithColor(DiscordColor.Yellow);

                            await e.Interaction.CreateFollowupMessageAsync(new DiscordFollowupMessageBuilder().AddEmbed(embed));

                            var serializerSettings = new JsonSerializerSettings { Formatting = Formatting.Indented };   // Formats The Json File
                            string updatedJsonString = JsonConvert.SerializeObject(userDatabase, serializerSettings);       // Serializes The Json File
                            File.WriteAllText(@"C:\\Users\\Owner\\source\\repos\\FlamesBotV2\\Data.json", updatedJsonString);       // Updates The Json File
                        }
                        else
                        {
                            var embed = new DiscordEmbedBuilder()
                                .WithTitle("**System**")
                                .WithDescription("**Your inventory is full.**")
                                .WithColor(DiscordColor.Azure);

                            await e.Interaction.CreateFollowupMessageAsync(new DiscordFollowupMessageBuilder().AddEmbed(embed));
                        }
                    }
                    break;
                /* Armory Buttons */



                /* Inventory Upgrade Button */
                case "inv_upgrade":
                    {
                        await e.Interaction.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
                        string userId = e.User.Id.ToString();         // Gets The User's Id
                        string jsonString;
                        using (StreamReader reader = new StreamReader(@"C:\Users\Owner\source\repos\FlamesBotV2\Data.json"))    // Reads The Json File
                        { jsonString = await reader.ReadToEndAsync(); }
                        UserDatabase userDatabase = JsonConvert.DeserializeObject<UserDatabase>(jsonString);    // Deserialize The Json File
                        int inv_lvl = userDatabase.Users[userId].Inv_lvl;
                        int balance = userDatabase.Users[userId].Balance;

                        if (userDatabase.Users.ContainsKey(userId))
                        {
                            switch (inv_lvl)
                            {
                                case 1:
                                    {
                                        if (balance >= 30000)
                                        {
                                            balance -= 30000;
                                            userDatabase.Users[userId].Balance = balance;
                                            userDatabase.Users[userId].Inv_lvl = 2; // Updates inv_lvl

                                            var serializerSettings = new JsonSerializerSettings { Formatting = Formatting.Indented };
                                            string updatedJsonString = JsonConvert.SerializeObject(userDatabase, serializerSettings);
                                            File.WriteAllText(@"C:\\Users\\Owner\\source\\repos\\FlamesBotV2\\Data.json", updatedJsonString);

                                            var embed = new DiscordEmbedBuilder()
                                                .WithTitle("**Upgrades**")
                                                .WithDescription("**Inventory Upgraded!\nCurrent Level: 2**")
                                                .WithColor(DiscordColor.Azure);
                                            await e.Interaction.CreateFollowupMessageAsync(new DiscordFollowupMessageBuilder().AddEmbed(embed));
                                        }
                                        else
                                        {
                                            var embed = new DiscordEmbedBuilder()
                                                 .WithTitle("**Upgrades**")
                                                 .WithDescription("**You don't have enough coins!**")
                                                 .WithColor(DiscordColor.Azure);
                                            await e.Interaction.CreateFollowupMessageAsync(new DiscordFollowupMessageBuilder().AddEmbed(embed));
                                        }
                                    }
                                    break;

                                case 2:
                                    if (balance >= 60000)
                                    {
                                        balance -= 60000;
                                        userDatabase.Users[userId].Balance = balance;
                                        userDatabase.Users[userId].Inv_lvl = 3; // Updates inv_lvl

                                        var serializerSettings = new JsonSerializerSettings { Formatting = Formatting.Indented };   // Formats The Json File
                                        string updatedJsonString = JsonConvert.SerializeObject(userDatabase, serializerSettings);       // Serializes The Json File
                                        File.WriteAllText(@"C:\\Users\\Owner\\source\\repos\\FlamesBotV2\\Data.json", updatedJsonString);       // Updates The Json File
                                        var embed = new DiscordEmbedBuilder()
                                            .WithTitle("**Upgrades**")
                                            .WithDescription("**Inventory Upgraded!\nCurrent Level: 3 [MAX]**")
                                            .WithColor(DiscordColor.Azure);
                                        await e.Interaction.CreateFollowupMessageAsync(new DiscordFollowupMessageBuilder().AddEmbed(embed));
                                    }
                                    else
                                    {
                                        var embed = new DiscordEmbedBuilder()
                                             .WithTitle("**Upgrades**")
                                             .WithDescription("**You don't have enough coins!**")
                                             .WithColor(DiscordColor.Azure);
                                        await e.Interaction.CreateFollowupMessageAsync(new DiscordFollowupMessageBuilder().AddEmbed(embed));
                                    }
                                    break;
                            }
                        }
                        else
                        {
                            var embed = new DiscordEmbedBuilder()
                                .WithTitle("**System**")
                                .WithDescription("**User ID wasn't found in the database.**")
                                .WithColor(DiscordColor.Azure);
                            await e.Interaction.CreateFollowupMessageAsync(new DiscordFollowupMessageBuilder().AddEmbed(embed));
                        }
                    }
                    break;
                /* Inventory Upgrade Button */



                /* Slots Ugpgrade Button */
                case "slot_upgrade":
                    if (e.Id == "slot_upgrade")
                    {
                        await e.Interaction.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
                        string userId = e.User.Id.ToString();         // Gets The User's Id
                        string jsonString;
                        using (StreamReader reader = new StreamReader(@"C:\Users\Owner\source\repos\FlamesBotV2\Data.json"))    // Reads The Json File
                        { jsonString = await reader.ReadToEndAsync(); }
                        UserDatabase userDatabase = JsonConvert.DeserializeObject<UserDatabase>(jsonString);    // Deserialize The Json File
                        int slot_lvl = int.Parse(userDatabase.Users[userId].Slot_lvl.ToString());
                        int balance = userDatabase.Users[userId].Balance;
                        if (userDatabase.Users.ContainsKey(userId))
                        {
                            if (slot_lvl == 5)
                            {
                                var embed1 = new DiscordEmbedBuilder()
                                .WithTitle("**Upgrades**")
                                .WithDescription($"**You are already max level!**")
                                .WithColor(DiscordColor.Azure);
                                await e.Interaction.CreateFollowupMessageAsync(new DiscordFollowupMessageBuilder().AddEmbed(embed1));
                                break;
                            }
                            else
                            {
                                switch (slot_lvl)
                                {
                                    case 1:
                                        {
                                            if (balance >= 40000)
                                            {
                                                balance -= 40000;
                                                userDatabase.Users[userId].Balance = balance;
                                                userDatabase.Users[userId].Slot_lvl = 2;
                                                userDatabase.Users[userId].Slots["melee_slot_2"] = "";
                                                userDatabase.Users[userId].Slots["misc_slot_2"] = "";

                                                var embed2 = new DiscordEmbedBuilder()
                                                  .WithTitle("**Upgrades**")
                                                  .WithDescription("**Slots Upgraded!\nCurrent Level: 2**")
                                                  .WithColor(DiscordColor.Azure);
                                                await e.Interaction.CreateFollowupMessageAsync(new DiscordFollowupMessageBuilder().AddEmbed(embed2));
                                            }
                                            else
                                            {
                                                var embed1 = new DiscordEmbedBuilder()
                                                 .WithTitle("**Upgrades**")
                                                 .WithDescription("**You don't have enough coins!**")
                                                 .WithColor(DiscordColor.Azure);
                                                await e.Interaction.CreateFollowupMessageAsync(new DiscordFollowupMessageBuilder().AddEmbed(embed1));
                                                break;
                                            }
                                        }
                                        break;

                                    case 2:
                                        {
                                            if (balance >= 50000)
                                            {
                                                balance -= 50000;
                                                userDatabase.Users[userId].Balance = balance;
                                                userDatabase.Users[userId].Slot_lvl = 3;
                                                userDatabase.Users[userId].Slots["range_slot_2"] = "";
                                                userDatabase.Users[userId].Slots["misc_slot_3"] = "";

                                                var embed2 = new DiscordEmbedBuilder()
                                                  .WithTitle("**Upgrades**")
                                                  .WithDescription($"**Slots Upgraded!\nCurrent Level: 3**")
                                                  .WithColor(DiscordColor.Azure);
                                                await e.Interaction.CreateFollowupMessageAsync(new DiscordFollowupMessageBuilder().AddEmbed(embed2));
                                            }
                                            else
                                            {
                                                var embed1 = new DiscordEmbedBuilder()
                                                 .WithTitle("**Upgrades**")
                                                 .WithDescription("**You don't have enough coins!**")
                                                 .WithColor(DiscordColor.Azure);
                                                await e.Interaction.CreateFollowupMessageAsync(new DiscordFollowupMessageBuilder().AddEmbed(embed1));
                                                break;
                                            }
                                        }
                                        break;

                                    case 3:
                                        {
                                            if (balance >= 60000)
                                            {
                                                balance -= 60000;
                                                userDatabase.Users[userId].Balance = balance;
                                                userDatabase.Users[userId].Slot_lvl = 4;
                                                userDatabase.Users[userId].Slots["melee_slot_3"] = "";

                                                var embed2 = new DiscordEmbedBuilder()
                                                  .WithTitle("**Upgrades**")
                                                  .WithDescription($"**Slots Upgraded!\nCurrent Level: 4**")
                                                  .WithColor(DiscordColor.Azure);
                                                await e.Interaction.CreateFollowupMessageAsync(new DiscordFollowupMessageBuilder().AddEmbed(embed2));
                                            }
                                            else
                                            {
                                                var embed1 = new DiscordEmbedBuilder()
                                                 .WithTitle("**Upgrades**")
                                                 .WithDescription("**You don't have enough coins!**")
                                                 .WithColor(DiscordColor.Azure);
                                                await e.Interaction.CreateFollowupMessageAsync(new DiscordFollowupMessageBuilder().AddEmbed(embed1));
                                                break;
                                            }
                                        }
                                        break;

                                    case 4:
                                        {
                                            if (balance >= 70000)
                                            {
                                                balance -= 70000;
                                                userDatabase.Users[userId].Balance = balance;
                                                userDatabase.Users[userId].Slot_lvl = 5;
                                                userDatabase.Users[userId].Slots["range_slot_3"] = "";

                                                var embed2 = new DiscordEmbedBuilder()
                                                  .WithTitle("**Upgrades**")
                                                  .WithDescription($"**Slots Upgraded!\nCurrent Level: 5 [MAX]**")
                                                  .WithColor(DiscordColor.Azure);
                                                await e.Interaction.CreateFollowupMessageAsync(new DiscordFollowupMessageBuilder().AddEmbed(embed2));
                                            }
                                            else
                                            {
                                                var embed1 = new DiscordEmbedBuilder()
                                                 .WithTitle("**Upgrades**")
                                                 .WithDescription("**You don't have enough coins!**")
                                                 .WithColor(DiscordColor.Azure);
                                                await e.Interaction.CreateFollowupMessageAsync(new DiscordFollowupMessageBuilder().AddEmbed(embed1));
                                                break;
                                            }
                                        }
                                        break;

                                    case 5:
                                        {
                                            var embed1 = new DiscordEmbedBuilder()
                                             .WithTitle("**Upgrades**")
                                             .WithDescription("**You are at already max level!**")
                                             .WithColor(DiscordColor.Azure);
                                            await e.Interaction.CreateFollowupMessageAsync(new DiscordFollowupMessageBuilder().AddEmbed(embed1));
                                        }
                                        break;
                                }
                            }
                            var serializerSettings = new JsonSerializerSettings { Formatting = Formatting.Indented };   // Formats The Json File
                            string updatedJsonString = JsonConvert.SerializeObject(userDatabase, serializerSettings);       // Serializes The Json File
                            File.WriteAllText(@"C:\\Users\\Owner\\source\\repos\\FlamesBotV2\\Data.json", updatedJsonString);       // Updates The Json File
                        }
                        else
                        {
                            var embed = new DiscordEmbedBuilder()
                                .WithTitle("**System**")
                                .WithDescription("**User ID wasn't found in the database.**")
                                .WithColor(DiscordColor.Azure);
                            await e.Interaction.CreateFollowupMessageAsync(new DiscordFollowupMessageBuilder().AddEmbed(embed));
                        }
                    }
                    break;
                /* Slots Ugpgrade Button */



                /* Riot Shield Buttons */
                case "riot_shield_upgrade":     // Riot Shield Upgrade Button
                    {
                        await e.Interaction.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
                        string userId = e.User.Id.ToString();         // Gets The User's Id
                        string jsonString;
                        using (StreamReader reader = new StreamReader(@"C:\Users\Owner\source\repos\FlamesBotV2\Data.json"))    // Reads The Json File
                        { jsonString = await reader.ReadToEndAsync(); }
                        UserDatabase userDatabase = JsonConvert.DeserializeObject<UserDatabase>(jsonString);    // Deserialize The Json File
                        int riot_shield_lvl = userDatabase.Users[userId].Riot_shield_lvl;
                        int balance = userDatabase.Users[userId].Balance;
                        switch (riot_shield_lvl)
                        {
                            case 1:
                                if (balance >= 50000)
                                {
                                    balance -= 50000;
                                    userDatabase.Users[userId].Balance = balance;
                                    userDatabase.Users[userId].Riot_shield_lvl = 2;

                                    var embed = new DiscordEmbedBuilder()
                                    .WithTitle("**Upgrades**")
                                    .WithDescription("**Riot Shield Upgraded!\nCurrent Level: 2**")
                                    .WithColor(DiscordColor.Azure);
                                    await e.Interaction.CreateFollowupMessageAsync(new DiscordFollowupMessageBuilder().AddEmbed(embed));
                                }
                                else
                                {
                                    var embed1 = new DiscordEmbedBuilder()
                                     .WithTitle("**Upgrades**")
                                     .WithDescription("**You don't have enough coins!**")
                                     .WithColor(DiscordColor.Azure);
                                    await e.Interaction.CreateFollowupMessageAsync(new DiscordFollowupMessageBuilder().AddEmbed(embed1));
                                    break;
                                }
                                break;

                            case 2:
                                if (balance >= 100000)
                                {
                                    balance -= 100000;
                                    userDatabase.Users[userId].Balance = balance;
                                    userDatabase.Users[userId].Riot_shield_lvl = 3;

                                    var embed = new DiscordEmbedBuilder()
                                    .WithTitle("**Upgrades**")
                                    .WithDescription("**Riot Shield Upgraded!\nCurrent Level: 3**")
                                    .WithColor(DiscordColor.Azure);
                                    await e.Interaction.CreateFollowupMessageAsync(new DiscordFollowupMessageBuilder().AddEmbed(embed));
                                }
                                else
                                {
                                    var embed1 = new DiscordEmbedBuilder()
                                     .WithTitle("**Upgrades**")
                                     .WithDescription("**You don't have enough coins!**")
                                     .WithColor(DiscordColor.Azure);
                                    await e.Interaction.CreateFollowupMessageAsync(new DiscordFollowupMessageBuilder().AddEmbed(embed1));
                                    break;
                                }
                                break;

                            case 3:
                                if (balance >= 150000)
                                {
                                    balance -= 150000;
                                    userDatabase.Users[userId].Balance = balance;
                                    userDatabase.Users[userId].Riot_shield_lvl = 4;

                                    var embed = new DiscordEmbedBuilder()
                                    .WithTitle("**Upgrades**")
                                    .WithDescription("**Riot Shield Upgraded!\nCurrent Level: 4**")
                                    .WithColor(DiscordColor.Azure);
                                    await e.Interaction.CreateFollowupMessageAsync(new DiscordFollowupMessageBuilder().AddEmbed(embed));
                                }
                                else
                                {
                                    var embed1 = new DiscordEmbedBuilder()
                                     .WithTitle("**Upgrades**")
                                     .WithDescription("**You don't have enough coins!**")
                                     .WithColor(DiscordColor.Azure);
                                    await e.Interaction.CreateFollowupMessageAsync(new DiscordFollowupMessageBuilder().AddEmbed(embed1));
                                    break;
                                }
                                break;

                            case 4:
                                if (balance >= 200000)
                                {
                                    balance -= 200000;
                                    userDatabase.Users[userId].Balance = balance;
                                    userDatabase.Users[userId].Riot_shield_lvl = 5;

                                    var embed = new DiscordEmbedBuilder()
                                    .WithTitle("**Upgrades**")
                                    .WithDescription("**Riot Shield Upgraded!\nCurrent Level: 5 [MAX]**")
                                        .WithColor(DiscordColor.Azure);
                                    await e.Interaction.CreateFollowupMessageAsync(new DiscordFollowupMessageBuilder().AddEmbed(embed));
                                }
                                else
                                {
                                    var embed1 = new DiscordEmbedBuilder()
                                     .WithTitle("**Upgrades**")
                                     .WithDescription("**You don't have enough coins!**")
                                     .WithColor(DiscordColor.Azure);
                                    await e.Interaction.CreateFollowupMessageAsync(new DiscordFollowupMessageBuilder().AddEmbed(embed1));
                                    break;
                                }
                                break;

                            case 5:
                                {
                                    var embed1 = new DiscordEmbedBuilder()
                                     .WithTitle("**Upgrades**")
                                     .WithDescription("**You are already max level!**")
                                     .WithColor(DiscordColor.Azure);
                                    await e.Interaction.CreateFollowupMessageAsync(new DiscordFollowupMessageBuilder().AddEmbed(embed1));
                                }
                                break;
                        }

                        var serializerSettings = new JsonSerializerSettings { Formatting = Formatting.Indented };   // Formats The Json File
                        string updatedJsonString = JsonConvert.SerializeObject(userDatabase, serializerSettings);       // Serializes The Json File
                        File.WriteAllText(@"C:\\Users\\Owner\\source\\repos\\FlamesBotV2\\Data.json", updatedJsonString);       // Updates The Json File
                    }
                    break;

                case "riot_shield_perk_toughness":      // Riot Shield Perk Toughness Button Interaction Code
                    {
                        await e.Interaction.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
                        string userId = e.User.Id.ToString();         // Gets The User's Id
                        string jsonString;
                        using (StreamReader reader = new StreamReader(@"C:\Users\Owner\source\repos\FlamesBotV2\Data.json"))    // Reads The Json File
                        { jsonString = await reader.ReadToEndAsync(); }
                        UserDatabase userDatabase = JsonConvert.DeserializeObject<UserDatabase>(jsonString);    // Deserialize The Json File

                        if (userDatabase.Users[userId].Inv.Contains("Titanium Plates"))
                        {
                            userDatabase.Users[userId].Inv.Remove("Titanium Plates");
                            userDatabase.Users[userId].Riot_shield_perk = "Toughness";
                            var embed = new DiscordEmbedBuilder()
                                .WithTitle("**Riot Shield**")
                                .WithDescription("**Toughness perk added to riot shield!**")
                                .WithColor(DiscordColor.Orange);
                            await e.Interaction.CreateFollowupMessageAsync(new DiscordFollowupMessageBuilder().AddEmbed(embed));

                            var serializerSettings = new JsonSerializerSettings { Formatting = Formatting.Indented };   // Formats The Json File
                            string updatedJsonString = JsonConvert.SerializeObject(userDatabase, serializerSettings);       // Serializes The Json File
                            File.WriteAllText(@"C:\\Users\\Owner\\source\\repos\\FlamesBotV2\\Data.json", updatedJsonString);       // Updates The Json File
                        }
                        else if (userDatabase.Users[userId].Riot_shield_perk == "Toughness")
                        {
                            var embed = new DiscordEmbedBuilder()
                              .WithTitle("**Riot Shield**")
                              .WithDescription("**You already have Toughness as your riot shield perk**")
                              .WithColor(DiscordColor.Orange);
                            await e.Interaction.CreateFollowupMessageAsync(new DiscordFollowupMessageBuilder().AddEmbed(embed));
                        }
                        else
                        {
                            var embed = new DiscordEmbedBuilder()
                              .WithTitle("**Riot Shield**")
                              .WithDescription("**You don't have Titanium Plates in your inventory**")
                              .WithColor(DiscordColor.Orange);
                            await e.Interaction.CreateFollowupMessageAsync(new DiscordFollowupMessageBuilder().AddEmbed(embed));
                        }
                    }
                    break;

                case "riot_shield_perk_spikes":         // Riot Shield Perk Spikes Button Interaction Code
                    {
                        await e.Interaction.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
                        string userId = e.User.Id.ToString();         // Gets The User's Id
                        string jsonString;
                        using (StreamReader reader = new StreamReader(@"C:\Users\Owner\source\repos\FlamesBotV2\Data.json"))    // Reads The Json File
                        { jsonString = await reader.ReadToEndAsync(); }
                        UserDatabase userDatabase = JsonConvert.DeserializeObject<UserDatabase>(jsonString);    // Deserialize The Json File


                        if (userDatabase.Users[userId].Inv.Contains("Iron Spikes"))
                        {
                            userDatabase.Users[userId].Inv.Remove("Iron Spikes");
                            userDatabase.Users[userId].Riot_shield_perk = "Spikes";
                            var embed = new DiscordEmbedBuilder()
                                .WithTitle("**Riot Shield**")
                                .WithDescription("**Spikes perk added to riot shield!**")
                                .WithColor(DiscordColor.Orange);
                            await e.Interaction.CreateFollowupMessageAsync(new DiscordFollowupMessageBuilder().AddEmbed(embed));

                            var serializerSettings = new JsonSerializerSettings { Formatting = Formatting.Indented };   // Formats The Json File
                            string updatedJsonString = JsonConvert.SerializeObject(userDatabase, serializerSettings);       // Serializes The Json File
                            File.WriteAllText(@"C:\\Users\\Owner\\source\\repos\\FlamesBotV2\\Data.json", updatedJsonString);       // Updates The Json File
                        }
                        else if (userDatabase.Users[userId].Riot_shield_perk == "Spikes")
                        {
                            var embed = new DiscordEmbedBuilder()
                              .WithTitle("**Riot Shield**")
                              .WithDescription("**You already have Spikes as your riot shield perk**")
                              .WithColor(DiscordColor.Orange);
                            await e.Interaction.CreateFollowupMessageAsync(new DiscordFollowupMessageBuilder().AddEmbed(embed));
                        }
                        else
                        {
                            var embed = new DiscordEmbedBuilder()
                              .WithTitle("**Riot Shield**")
                              .WithDescription("**You don't have Iron Spikes in your inventory**")
                              .WithColor(DiscordColor.Orange);
                            await e.Interaction.CreateFollowupMessageAsync(new DiscordFollowupMessageBuilder().AddEmbed(embed));
                        }
                    }
                    break;

                case "riot_shield_perk_camouflage":     // Riot Shield Perk Camouflage Button Interaction Code
                    {
                        await e.Interaction.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
                        string userId = e.User.Id.ToString();         // Gets The User's Id
                        string jsonString;
                        using (StreamReader reader = new StreamReader(@"C:\Users\Owner\source\repos\FlamesBotV2\Data.json"))    // Reads The Json File
                        { jsonString = await reader.ReadToEndAsync(); }
                        UserDatabase userDatabase = JsonConvert.DeserializeObject<UserDatabase>(jsonString);    // Deserialize The Json File

                        if (userDatabase.Users[userId].Inv.Contains("Camouflage Component"))
                        {
                            userDatabase.Users[userId].Inv.Remove("Camouflage Component");
                            userDatabase.Users[userId].Riot_shield_perk = "Camouflage";
                            var embed = new DiscordEmbedBuilder()
                                .WithTitle("**Riot Shield**")
                                .WithDescription("**Camouflage perk added to riot shield!**")
                                .WithColor(DiscordColor.Orange);
                            await e.Interaction.CreateFollowupMessageAsync(new DiscordFollowupMessageBuilder().AddEmbed(embed));

                            var serializerSettings = new JsonSerializerSettings { Formatting = Formatting.Indented };   // Formats The Json File
                            string updatedJsonString = JsonConvert.SerializeObject(userDatabase, serializerSettings);       // Serializes The Json File
                            File.WriteAllText(@"C:\\Users\\Owner\\source\\repos\\FlamesBotV2\\Data.json", updatedJsonString);       // Updates The Json File
                        }
                        else if (userDatabase.Users[userId].Riot_shield_perk == "Camouflage")
                        {
                            var embed = new DiscordEmbedBuilder()
                              .WithTitle("**Riot Shield**")
                              .WithDescription("**You already have Camouflage as your riot shield perk**")
                              .WithColor(DiscordColor.Orange);
                            await e.Interaction.CreateFollowupMessageAsync(new DiscordFollowupMessageBuilder().AddEmbed(embed));
                        }
                        else
                        {
                            var embed = new DiscordEmbedBuilder()
                              .WithTitle("**Riot Shield**")
                              .WithDescription("**You don't have Camouflage component in your inventory**")
                              .WithColor(DiscordColor.Orange);
                            await e.Interaction.CreateFollowupMessageAsync(new DiscordFollowupMessageBuilder().AddEmbed(embed));
                        }
                    }
                    break;
                /* Riot Shield Buttons */



                /* Armory Cancel Button */
                case "cancel_C":
                    {
                        await e.Message.DeleteAsync();
                        await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                            .WithContent("**Crafting cancelled**")
                            .AsEphemeral(true));
                    }
                    break;
                    /* Armory Cancel Button */
            }
        }
        private static Task Client_Ready(DiscordClient sender, DSharpPlus.EventArgs.ReadyEventArgs args)
        {
            return Task.CompletedTask;
        }

        public static bool CanAddMoreItems(UserData user)       // Checks If The User's Inventory Is Fulls
        {
            int maxItems = 0;
            switch (user.Inv_lvl)
            {
                case 1:
                    maxItems = 3;
                    break;
                case 2:
                    maxItems = 6;
                    break;
                case 3:
                    maxItems = 9;
                    break;
                default:
                    break;
            }
            return user.Inv.Count < maxItems;
        }
    }
}