using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Data.OleDb;
using System.Reflection;
using System.Threading.Tasks;

namespace n
{
    public class CommandHandler
    {
        private CommandService _cmds;
        private DiscordSocketClient _client;
        public async Task Install(DiscordSocketClient c)
        {
            _client = c;
            _cmds = new CommandService();

            await _cmds.AddModulesAsync(Assembly.GetEntryAssembly(), null);

            _client.MessageReceived += HandleCommand;
            
            
            _client.UserJoined += AnnounceUserJoined;
            //_client.JoinedGuild += async guild =>
            //{
            //    var channel = guild.DefaultChannel;

            //    await channel.SendMessageAsync(">welcome to the server !", false);
            //};
            //_client.UserLeft += AnnounceUserLeft;
            //_client.UserLeft += AnnounceUserLeft;
        }

        public async Task AnnounceUserJoined(IGuildUser user)
        {
            ulong channelID = user.Guild.DefaultChannelId;
            var channel = _client.GetChannel(channelID) as SocketTextChannel;
            EmbedBuilder embed = new EmbedBuilder();
            embed.WithDescription("sup niggy\n");
            embed.WithTitle(user.Username + "#" + user.Discriminator + "\n");
            embed.WithImageUrl(user.GetAvatarUrl());
            await channel.SendMessageAsync("", false, embed.Build());
            OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Desk\n\n\bin\Debug\db.accdb");
            OleDbCommand cmd = con.CreateCommand();
            con.Open();
            cmd.CommandText = "Insert into rep Values('" + user.Username + "#" + user.Discriminator + "','0','" + DateTime.Now.AddHours(-24) + "')";
            cmd.Connection = con;
            cmd.ExecuteNonQuery();
            con.Close();
        }

        //public async Task AnnounceUserJoined(SocketGuildUser user)
        //{
        //    ulong channelID = user.Guild.DefaultChannelId;
        //    var chan = _client.GetChannel(channelID) as SocketTextChannel;
        //    var chan = _client.GetChannel() as SocketTextChannel;


        //    EmbedBuilder embeder = new EmbedBuilder();

        //    embeder.WithTitle(user.Username + "#" + user.Discriminator + "\n");
        //    embeder.AddField("Status: ", user.Status.ToString() + "\n", false);    // true - for inline
        //    embeder.AddField("Year Created: ", user.CreatedAt.Year.ToString() + "\n", false);
        //    embeder.WithThumbnailUrl(user.GetAvatarUrl());
        //    embeder.WithDescription("Welcome to the server !");
        //    embeder.WithImageUrl("https://gifimage.net/wp-content/uploads/2017/06/anime-blush-gif.gif");
        //    embeder.WithColor(Color.DarkTeal);
        //    await chan.SendMessageAsync("", false, embeder.Build());
        //}


        public async Task AnnounceUserLeft(SocketGuildUser user)
        {
            
            await Task.Delay(0);
        }
        public void code()
        {

        }
        public async Task HandleCommand(SocketMessage s)
        {


            var msg = s as SocketUserMessage;
            if (msg == null) return;

            var context = new CommandContext(_client, msg);

            int argPos = 0;
            string prefix = "?";
            if (msg.HasStringPrefix(prefix, ref argPos))
            {


                var result = await _cmds.ExecuteAsync(context, argPos, null);

                if (!result.IsSuccess)
                {
                    switch (result.ToString())
                    {
                        default:

                            await s.Channel.SendMessageAsync($"> Error: " + result.ToString());
                            break;

                        case "BadArgCount: The input text has too few parameters.":

                            await msg.DeleteAsync();

                            await s.Channel.SendMessageAsync($"User not Found or not enough parameters.");
                            break;
                        case "Exception: The server responded with error 403: Forbidden":
                            await msg.DeleteAsync();
                            await s.Channel.SendMessageAsync($"> Can't ban " + context.Message.MentionedUserIds.ToString() + " too op!");
                            break;
                        case "UnknownCommand: Unknown command.":

                            await msg.DeleteAsync();

                            await s.Channel.SendMessageAsync($">Nigga i got no command yet '{prefix}help will be available soon.");
                            break;

                    }
                }
            }
        }
    }
}