using Discord;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;
using FFMpegSharp.FFMPEG;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using JikanDotNet;
using Microsoft.VisualBasic.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Models.MediaStreams;
using YoutubeExtractor;
using FFMpegSharp;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data;
using System.Net;
using HtmlAgilityPack;
using System.Net.Http;
using System.Collections.Specialized;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;
using ScrapySharp.Extensions;

namespace n
{
    public class Commands : ModuleBase
    {

        [Command("pong")]
        public async Task pong()
        {
            await ReplyAsync("<ping");

        }

        //[Command("Ping")]
        //public async Task Ping()
        //{
        //    var msg = await ReplyAsync("***Calculating Ping ....***");
        //    await ReplyAsync($"*** { } ms***");
        //    await Context.Message.DeleteAsync();
        //    await msg.DeleteAsync();
        //}

        [Command("square")]
        [Summary("Squares a number.")]
        public async Task SquareAsync(
            [Summary("The number to square.")]
        int num)
        {
            await Context.Channel.SendMessageAsync($"{num}^2 = {Math.Pow(num, 2)}");

        }
        [Command("emo")]
        public async Task emo()
        {
            IReadOnlyCollection<GuildEmote> emot;
            OleDbConnection cnnn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\\Emotes.mdb");
            if (cnnn.State == System.Data.ConnectionState.Closed)
                cnnn.Open();
            emot = Context.Guild.Emotes;
            for (int i = 0; i <= emot.Count - 1; i++)
            {
                OleDbCommand cmd = new OleDbCommand("INSERT INTO Emotes (Namee,ID) VALUES(\"" + emot.ElementAt(i).Name + "\",\"" + emot.ElementAt(i) + "\");", cnnn);
                cmd.ExecuteNonQuery();
            }
            cnnn.Close();
        }
        [Command("emot")]
        public async Task emot()
        {
            
            OleDbConnection cn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\\Emotes.mdb");
            if (cn.State == System.Data.ConnectionState.Closed)
                cn.Open();
            OleDbCommand cmd = new OleDbCommand("select Namee from Emotes ", cn);
            OleDbDataAdapter adp = new OleDbDataAdapter(cmd);
            DataTable dt = new DataTable();
            adp.Fill(dt);
            foreach(DataRow row in dt.Rows)
            {
                await ReplyAsync(":"+row["Namee"].ToString()+":") ;
            }
            cn.Close();
        }
        //[Command("ban")]
        //public async Task ban(IGuildUser user)
        //{
        //    await Context.Guild.AddBanAsync(user);
        //}
        [Command("ban")]
        public async Task ban(IGuildUser user)
        {
            await Context.Guild.AddBanAsync(user);
        }
        [Command("rmrole")]
        public async Task addrole(SocketGuildUser user)
        {

            await user.GetOrCreateDMChannelAsync();
            await ReplyAsync();
        }
        [Command("kick")]
        public async Task Kick(SocketGuildUser user, string reason = null)
        {
            //user.GuildPermissions.Has.reason
            if (user != null /*&& user.Mention.ToLower()!="everyone*/)
            {
                //string role = user.GuildPermissions.ToString();
                if (user.GuildPermissions.KickMembers || user.GuildPermissions.BanMembers || user.GuildPermissions.Administrator)
                {
                    var channel = await user.GetOrCreateDMChannelAsync();
                    await user.KickAsync();
                    await ReplyAsync(reason == null ? $"Kicked {user.Username}!" : $"Kicked {user.Username} for '{reason}'.");
                    await channel.SendMessageAsync(reason == null ? $"{user.Username}, you've been kicked from {Context.Guild.Name}" : $"{user.Username}, you've been kicked from {Context.Guild.Name} for '{reason}'.");
                }
                else
                    await ReplyAsync("You don't have the perms to do that.");
            }
            else
            {
                await ReplyAsync("You have to mention the person to kick, Nigger!");
            }

        }


        [Command("yt")]
        public async Task YTvid(string url)
        {
            var id = YoutubeClient.ParseVideoId(url); // "bnsUkE8i0tU"
            var client = new YoutubeClient();

            var video = await client.GetVideoAsync(id);
            var title = video.Title; // "Infected Mushroom - Spitfire [Monstercat Release]"
            var author = video.Author; // "Monstercat"
            var duration = video.Duration; // 00:07:14

            var streamInfoSet = await client.GetVideoMediaStreamInfosAsync(id);

            // Select one of the streams, e.g. highest quality muxed stream
            var streamInfo = streamInfoSet.Muxed.WithHighestVideoQuality();

            var ext = streamInfo.Container.GetFileExtension();

            // Download stream to file
            char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
            var validFilename = new string(title.Where(ch => !invalidFileNameChars.Contains(ch)).ToArray());
            await client.DownloadMediaStreamAsync(streamInfo, @"D:\nBot\Videos\" + validFilename + "."+ext);
            //string inputFile = @"D:\"+ validFilename + "."+ext,outputFile = @"D:\" + validFilename + ".mp3";

           //var convert =  new FFMpeg() .ExtractAudio(FFMpegSharp.VideoInfo.FromPath(inputFile),new FileInfo(outputFile));

        }

        //[Command("mute")]
        //public async Task mute(SocketGuildUser usr)
        //{

        //    ChannelPermission perm = null;

        //    await ReplyAsync(usr.Hierarchy.ToString());    
        //}


        [Command("usr") ]
        public async Task usr()
        {
            OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Desk\n\n\bin\Debug\db.accdb");
            OleDbCommand cmd = con.CreateCommand();
            con.Open();
            Array users = Context.Guild.GetUsersAsync().Result.ToArray();
            foreach(var user in users)
            {
                cmd.CommandText = "Insert into rep Values('" + user.ToString()+ "',0,0)";
                cmd.Connection = con;
                cmd.ExecuteNonQuery();
            }
            await ReplyAsync("All users added to DataBase!");
            con.Close();
        }
        [Command("rep")]
        public async Task rep(SocketGuildUser usr)
        {
            string timeout;
            OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Desk\n\n\bin\Debug\db.accdb");
            OleDbCommand cmd = con.CreateCommand();
            con.Open();
            cmd.CommandText = "SELECT timeout from rep where user = '" + Context.User.Username + "#" + Context.User.Discriminator + "';";
            cmd.Connection = con;
            OleDbDataReader reader = cmd.ExecuteReader();
            
            if (reader.Read())
            {
                timeout = String.Format("{0}", reader["timeout"]);
            }
            else timeout = " ";
            DateTime t1 = DateTime.Parse(timeout);
            con.Close();
            if (DateTime.Now > t1.AddHours(24))
            {
                OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Desk\n\n\bin\Debug\db.accdb");
                OleDbCommand cmdd = conn.CreateCommand();
                conn.Open();
                cmdd.CommandText = "UPDATE rep SET repc = repc + 1 WHERE user = '" + usr.Username + "#" + usr.Discriminator + "'; ";
                cmdd.Connection = conn;
                cmdd.ExecuteNonQuery();
                var time = DateTime.Now;
                cmdd.CommandText = "UPDATE rep SET timeout = '" + time + "' WHERE user = '" + Context.User.Username + "#" + Context.User.Discriminator + "'; ";
                cmdd.Connection = conn;
                cmdd.ExecuteNonQuery();
                await ReplyAsync("You gave " + usr.Mention + " a reputaion !");
                conn.Close();
            }
            else
                await ReplyAsync("You have to wait to for the next usage !");

        }

        [Command("cemj")]
        [Summary("Takes an image from a user and adds it as a guild emoji")]
        public async Task cemj(string name)
        {
            string path = Path.GetTempPath() + "\\image35.png";
            string url = Context.Message.Attachments.ElementAt(0).Url;
            using (WebClient client = new WebClient())
            {
                //client.DownloadFile(new Uri(url), @"c:\temp\image36.png");
                client.DownloadFileAsync(new Uri(url), path);

            }
            WaitFor(int.MaxValue);
            await Task.CompletedTask;
            Image img;
            img = new Image(path);
            await Context.Guild.CreateEmoteAsync(name, img);

        }

        private void WaitFor(int v)
        {
            while (v > 0) { v--; }
        }
        [Command("cl")]
        public async Task cl(string s)
        {
            await ReplyAsync(Context.Guild.OwnerId.ToString());
            if (Context.User.Id == Context.Guild.OwnerId)
            {
                string logdata = s;
                string path = @"D:\nBot\cl\CheckList.txt";
                TextWriter tw = new StreamWriter(path, true);
                tw.WriteLine(logdata);
                tw.Close();
            }
            else await ReplyAsync("only guild owner can do this command");
        }
        [Command("sql")]
        public async Task sql()
        {
            OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\bloop\Desktop\n\n\bin\Debug\db.accdb");
            OleDbCommand cmd = con.CreateCommand();
            con.Open();
            cmd.CommandText = "Insert into users(ServerName,Username)Values('" + Context.Guild.Name + "','" + Context.User.Username + "')";
            cmd.Connection = con;
            cmd.ExecuteNonQuery();
            await ReplyAsync("Record Submitted Congrats!");
            con.Close();
        }
        [Command("cr")]
                public async Task cr()
                {

            //var loginAddress = "https://www.iutiag.net/plan.php";
            //var loginData = new NameValueCollection
            //{
            //  { "userid", "017LTE17" },
            //  { "pwd", "017LTE17" }
            //};
            //var client = new CookieAwareWebClient();
            //client.Login(loginAddress, loginData);
            //string url = "https://www.iutiag.net/plan.php";
            //WebRequest myReq = WebRequest.Create(url);
            //CredentialCache mycache = new CredentialCache();
            //mycache.Add(new Uri(url), "Basic", new NetworkCredential("017LTE17", "017LTE17"));
            //myReq.Credentials = mycache;


                var url = "https://jaiminisbox.com/reader/read/the-second-coming-of-gluttony/en/0/1/page/1";
                var clientweb = new HttpClient();
                var html = await clientweb.GetStringAsync(url);
                var doc = new HtmlDocument();
                doc.LoadHtml(html);

            for (int i = 1; i < 37; i++)
            {
                var im = doc.DocumentNode.Descendants("img").ToList();
                foreach(var ii in im)
                {
                    await ReplyAsync(i.ToString());
                }
                
                var imgs = doc.DocumentNode.Descendants("img").Where(node => node.GetAttributeValue("class", "").Equals("page-" + i)).ToList();
                foreach (var img in imgs)
                {
                    //await ReplyAsync(div.Descendants("h4").FirstOrDefault().FirstChild.InnerText);
                    await ReplyAsync(img.Descendants("img").FirstOrDefault().ChildAttributes("src").FirstOrDefault().Value);
                }
            }
        }



        [Command("usrs")]
        public async Task usrs()
        {
            OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Desk\n\n\bin\Debug\db.accdb");
            OleDbCommand cmd = con.CreateCommand();
            con.Open();
            cmd.CommandText = "Select * from users)";
            cmd.Connection = con;
            cmd.ExecuteNonQuery();
            await ReplyAsync("Record Submitted Congrats!");
            con.Close();
        }
        [Command("who")]
        public async Task who(SocketUser user)
        {
            


            EmbedBuilder embeder = new EmbedBuilder();

            embeder.WithTitle(user.Username + "#" + user.Discriminator + "\n");
            embeder.AddField("Status: ", user.Status.ToString() + "\n", false);    // true - for inline
            embeder.AddField("AvatarID: ", user.AvatarId.ToString() + "\n", false);
            embeder.AddField("Year Created: ", user.CreatedAt.Year.ToString() + "\n", false);
            embeder.WithThumbnailUrl(user.GetAvatarUrl());
            //embeder.WithDescription("");
            embeder.WithColor(Color.Blue);
            await Context.Channel.SendMessageAsync("", false, embeder.Build());



        }





        [Command("mysql")]
        public async Task mysql()
        {
            {
                var usersg = Context.Guild.GetUsersAsync();
                var users = usersg.Result;
                string connStr = @"server=localhost;user id=root;database=discord";
                MySqlConnection conn = new MySqlConnection(connStr);
                try
                {
                    await ReplyAsync("Connecting to MySQL...");
                    conn.Open();
                    await ReplyAsync(conn.State.ToString());
                    foreach (var user in users)
                    {
                        string sql = "INSERT INTO `users`(`user`) VALUES ('" + user + "');";
                        MySqlCommand cmd = new MySqlCommand(sql, conn);
                        cmd.ExecuteScalar();
                    }
                }
                catch (Exception ex)
                {
                    await ReplyAsync(ex.ToString());
                }
                conn.Close();
            }
        }

        [Command("feet")]
        public async Task feet(SocketUser user)
        {
            string msg = "https://kingged.com/wp-content/uploads/2018/11/sell-feet-pics.jpg";
            await user.SendMessageAsync(msg, false);
        }



        [Command("dm")]
        public async Task dm(SocketUser user, string msg)
        {
            string info = "Sedning " + user.Username + " " + msg + " . ";
            await user.SendMessageAsync(msg, false);
            await ReplyAsync(info);
        }

        [Command("copy")]
        [Summary("Say what i say.")]
        public async Task copy(
            [Summary("Repeats.")]
        string s)
        {
            await Context.Channel.SendMessageAsync(s);

        }


        [Command("crr")]
        public async Task crr()
        {
            for (int i = 2; i < 23; i++)
            {
                var url = "https://merakiscans.com/manga/my-wife-is-a-demon-queen/1/";
                var webGet = new HtmlWeb();
                if (webGet.Load(url) is HtmlDocument document)
                {
                    var nodes = document.DocumentNode.CssSelect(".img").ToList();
                    foreach (var node in nodes)
                    {
                        string original_text = node.CssSelect(".webtoon").Single().OuterHtml;
                        string matchString = Regex.Match(original_text, "<img.+?src=[\"'](.+?)[\"'].+?>", RegexOptions.IgnoreCase).Groups[1].Value;
                        await ReplyAsync("https://merakiscans.com/"+matchString);
                    }
                }
                System.Threading.Thread.Sleep(1000);
            }
        }
        [Command("mera")]
        public async Task mera(string url,int pages)
        {
            for (int i = 0; i <=pages; i++)
            {
                string imgurl = url + i.ToString("00")+ ".png";
                await ReplyAsync(imgurl);
                
                System.Threading.Thread.Sleep(1000);
            }
        }
        [Command("weeb")]
        [Summary("calculate how weeb i am.")]
        public async Task weeb()
        {
            Random rnd = new Random();
            int weeb = rnd.Next(0, 10);
            await Context.Channel.SendMessageAsync("you are " + weeb + " / 10 weeb");

        }
        [Command("mal")]
                [Summary("calculate how weeb i am.")]
                public async Task mal(string keys)
                {

            // Initialize JikanWrapper
            IJikan jikan = new Jikan(true);

            // Send request to search anime with "haibane" key word
            AnimeSearchResult animeSearchResult = await jikan.SearchAnime(keys);
            await ReplyAsync(animeSearchResult.Results.First().Title);

            //// Send request to search anime with "gundam" key word, second page of results
            //animeSearchResult = await jikan.SearchAnime(keys, 2);

            //AnimeSearchConfig animeSearchConfig = new AnimeSearchConfig()
            //{
            //    Type = AnimeType.Movie,
            //    Score = 7
            //};
            //await ReplyAsync(animeSearchResult.Results.First().Title);
            //// Send request to search anime with "gundam" key word, movies with score bigger than 7 only.
            //animeSearchResult = await jikan.SearchAnime(keys, animeSearchConfig);

            //animeSearchConfig = new AnimeSearchConfig()
            //{
            //    Genres = { GenreSearch.Action, GenreSearch.Adventure },
            //    GenreIncluded = true
            //};
            //await ReplyAsync(animeSearchResult.Results.First().Title);
            //// Send request to search anime with "samurai" key word, with action and/or adventure genre.
            //animeSearchResult = await jikan.SearchAnime(keys, animeSearchConfig);

            //animeSearchConfig = new AnimeSearchConfig()
            //{
            //    Genres = { GenreSearch.Mecha, GenreSearch.Romance },
            //    GenreIncluded = false
            //};
            //await ReplyAsync(animeSearchResult.Results.First().Title);
            //// Send request to search anime with "samurai" key word, without mecha and/or romance genre.
            //animeSearchResult = await jikan.SearchAnime(keys, animeSearchConfig);
            //animeSearchConfig = new AnimeSearchConfig()

            //{
            //    Rating = AgeRating.RX
            //};
            //await ReplyAsync(animeSearchResult.Results.First().Title);
            //// Send request to search anime with "xxx" key word, adult anime only, second page of results
            //animeSearchResult = await jikan.SearchAnime(keys, 2, animeSearchConfig);
            //await ReplyAsync(animeSearchResult.Results.First().Title);

        }
            [Command("yts")]
            [Summary("Search for YT video.")]
             public async Task yts(string key)
              {

            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {   ApiKey = "AIzaSyCoS_lL8xPGLdPNAkXKDW59XIodBiMYlVE",ApplicationName = this.GetType().ToString()});

            var searchListRequest = youtubeService.Search.List("snippet");
            searchListRequest.Q = key; // Replace with your search term.
            searchListRequest.MaxResults = 5;

            // Call the search.list method to retrieve results matching the specified query term.
            var searchListResponse = await searchListRequest.ExecuteAsync();

            List<string> videos = new List<string>();
            List<string> channels = new List<string>();
            List<string> playlists = new List<string>();

            // Add each result to the appropriate list, and then display the lists of
            // matching videos, channels, and playlists.
            foreach (var searchResult in searchListResponse.Items)
            {
                switch (searchResult.Id.Kind)
                {
                    case "youtube#video":
                        videos.Add(String.Format("{0} ({1})", searchResult.Snippet.Title, "https://www.youtube.com/watch?v=" + searchResult.Id.VideoId));
                        break;

                    case "youtube#channel":
                        channels.Add(String.Format("{0} ({1})", searchResult.Snippet.Title, searchResult.Id.ChannelId));
                        break;

                    case "youtube#playlist":
                        playlists.Add(String.Format("{0} ({1})", searchResult.Snippet.Title, searchResult.Id.PlaylistId));
                        break;
                }
            }

            await ReplyAsync(String.Format("Videos:\n{0}\n", string.Join("\n", videos)));
            //await ReplyAsync(String.Format("Channels:\n{0}\n", string.Join("\n", channels)));
            //await ReplyAsync(String.Format("Playlists:\n{0}\n", string.Join("\n", playlists)));
        }
    }


}

