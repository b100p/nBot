using Discord;
using Discord.Rest;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace n
{
    class Program
    {
        int i = 0;
        string token = "NjgyOTQzMTY5Mjk3Nzc2NjY3.XlkWoA.fTqR2ePSqkTewzlPYjw5dzJiX-w";//this is the token it's used to identify our bot it's a secret key
        public DiscordSocketClient Client;//this is basically where the commection to the bot happens 
        public CommandHandler Handler; // i mean do i need to say it ?
        static void Main(string[] args) => new Program().Start().GetAwaiter().GetResult(); // so basically this means that now the program will be excuted async cause all the discord library is async
        public async Task Start()
        {
            Client = new DiscordSocketClient();
            Handler = new CommandHandler(); 
            await Client.LoginAsync(TokenType.Bot, token);// this here waits for the connection btween the bot an the console to be established 
            await Client.StartAsync();//starts the connection
            await Handler.Install(Client);
            Client.Log += Log;
            Client.MessageReceived += MessageReceived;
            Client.MessageUpdated += MessageUpdated;
            //Client.MessageReceived += logger;
            Client.MessageReceived += MessageReceivedd;
            Client.MessageReceived += ping;
            Client.MessageReceived += lol;
            Client.JoinedGuild += join;
            //Client.MessageReceived += REC;
            
            await Client.SetGameAsync("with your mom.");

            //await Client.SetStatusAsync"Playing with your mom.");
            await Task.Delay(-1);
        }

        private Task join(SocketGuild server)
        {
            var users = server.Users;
            foreach(var user in users)
            {
                OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Desk\n\n\bin\Debug\db.accdb");
                OleDbCommand cmd = con.CreateCommand();
                con.Open();
                cmd.CommandText = "Insert into rep Values('" + user.Username+"#"+user.Discriminator + "','0','"+ DateTime.Now.AddHours(-24)+"')";
                cmd.Connection = con;
                cmd.ExecuteNonQuery();
                con.Close();
            }
            return Task.CompletedTask;
        }

        private Task Log(LogMessage msg)
        { 
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
        private async Task MessageReceivedd(SocketMessage message)
        {
            Console.WriteLine(message);
        }
        private async Task MessageReceived(SocketMessage message)
        {
            OleDbConnection cn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\\Emotes.mdb");
            if (cn.State == System.Data.ConnectionState.Closed)
                cn.Open();
            OleDbCommand cmd = new OleDbCommand("select * from Emotes ", cn);
            OleDbDataAdapter adp = new OleDbDataAdapter(cmd);
            DataTable dt = new DataTable();
            adp.Fill(dt);
            foreach (DataRow row in dt.Rows)
            {
                if (message.Content.Contains(row["Namee"].ToString()))
                {
                    await message.Channel.SendMessageAsync(row["Namee"].ToString());
                    //OleDbCommand cmd2 = new OleDbCommand("UPDATE Emotes set Count = Count + 1 where ID =\""+row["ID"].ToString()+"\"", cn);
                    //cmd2.ExecuteNonQuery();
                    await message.Channel.SendMessageAsync("Emote Counted");
                }
            }
            cn.Close();
        }

        private async Task MessageUpdated(Cacheable<IMessage, ulong> before, SocketMessage after, ISocketMessageChannel channel)
        {
            // If the message was not in the cache, downloading it will result in getting a copy of `after`.
            var message = await before.GetOrDownloadAsync();
            Console.WriteLine($"{message} -> {after}");

        }

        private async Task ping(SocketMessage msg)
        {
            if (msg.Content == "@ping")
            {

                await msg.Channel.SendMessageAsync("***nBot's ping is :" + Client.Latency + " ms***");
            }
        }
        private async Task lol(SocketMessage msg)
        {
            if (msg.Content == "pong")
            {
                await msg.Channel.SendMessageAsync("<ping");
            }
        }

        //private async Task<Task> logger(SocketMessage m)
        //{
        
        //    string logdata = m.Channel.ToString() + " " + m.Author.ToString() + " : " + m.Content.ToString() /*+ " - " + m.Attachments.ElementAt(0).Url*/;
        //    string time = DateTime.Today.Date.ToString().Replace("/", "-");
        //    char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
        //    var validFilename = new string(time.Where(ch => !invalidFileNameChars.Contains(ch)).ToArray());
        //    string path = @"D:\nBot\log\" + validFilename + ".txt";
        //    //if (!File.Exists(path))
        //    //{
        //    //File.Create(path);
        //    TextWriter tw = new StreamWriter(path, true);
        //    tw.WriteLine(logdata);
        //    tw.Close();
        //    //}

        //    //else if (File.Exists(path))
        //    //{
        //    //    using (var tw = new StreamWriter(path, true))
        //    //    {
        //    //        tw.WriteLine(logdata);
        //    //    }
        //    //}

        //    //TextWriter sw = new StreamWriter(@"F:\\file11.txt");
        //    //if (mis Image)
        //    //{
        //    //}else
        //    //while (i < 5)
        //    //{
        //        i++;
        //        Console.WriteLine(logdata);
        //        return Task.CompletedTask;
                
        //    //}
        //    //await m.Channel.SendMessageAsync(Client.Guilds.ToString());
          
        //    //i = 0;
        //    //return Task.CompletedTask;
            
        //}
        //private async Task REC(SocketMessage message)
        //{
        //    AddUsers(message);
        //    await Reply(message);
        //}

        //private void AddUsers(SocketMessage message)
        //{
        //    int i;
        //    for (i = 0; users[i] != null && i < users.Length; i++) ;
        //    if (i >= users.Length) { i = 0; }

        //    users[i] = message.Author.Username;

        //}

        //private async Task Reply(SocketMessage message)
        //{
        //    int pos = -1;
        //    if (message.Content.ToLower() == "hi" && !message.Author.IsBot) { await message.Channel.SendMessageAsync("yo!"); }
        //    else if (message.Content.Contains("hi") && message.Content.Contains("nbot") && !message.Author.IsBot) { await message.Channel.SendMessageAsync("hello!"); }
        //    else if (message.Content.Contains(botName)) { await message.Channel.SendMessageAsync("yes?"); }
        //    //
        //    string[] array = message.Content.Split(' ');
        //    pos = Array.IndexOf(array, "is");
        //    if (pos > -1)
        //    {
        //        string uname = array[pos - 1];
        //        if (Array.IndexOf(users, uname.ToLower()) > -1)
        //        {
        //            int userpos = Array.IndexOf(users, uname.ToLower());
        //            int i = 0;
        //            for (i = 0; adjs[userpos, i] != null && i < adjs.GetLength(0); i++) ;

        //            if (i >= adjs.GetLength(0)) { i = 0; }

        //            if (array[pos + 1] != null)
        //            {
        //                if (array[pos + 1].ToLower() != "a" && array[pos + 1].ToLower() != "an")
        //                {
        //                    if (array[pos + 2] != null) adjs[userpos, i] = array[pos + 2];
        //                }
        //                else adjs[userpos, i] = array[pos + 1];
        //            }


        //        }
        //    }
        //}
        //string[] users = new string[10];
        //string[,] adjs = new string[10, 4];
        ////List<List<string>> users = new List<List<string>>();
        //private string botName = "nbot";
    }
}
