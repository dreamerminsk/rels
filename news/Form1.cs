using HtmlAgilityPack;
using RateLimiter;
using System;
using System.Linq;
using System.Web;
using System.Windows.Forms;
using ComposableAsync;
using System.Collections.Generic;
using System.Threading.Tasks;
using fstats.model;
using fstats.utils;

namespace fstats
{
    public partial class Form1 : Form
    {

        private TimeLimiter timeConstraint = TimeLimiter.GetFromMaxCountByInterval(1, TimeSpan.FromSeconds(16));

        private HtmlWeb htmlWeb = new HtmlWeb();

        private Dictionary<string, TeamStats> teamStats = new Dictionary<string, TeamStats>();

        private Dictionary<string, int> playerStats = new Dictionary<string, int>();

        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
        }

        public DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Enumerable.Range(1, 12).Select(x => 2020 - x).ToList().ForEach(async x =>
            {
                await timeConstraint;
                string url = string.Format(
                    "https://en.wikipedia.org/wiki/{0}–{1}_UEFA_Champions_League_knockout_phase",
                    x, (x + 1).ToString().Substring(2));
                var matches = await ParseMatches(url);
                matches?.ForEach(match => UpdateStats(match));
                ShowStats();
            });
            Enumerable.Range(1, 16).Select(x => 2008 - x).ToList().ForEach(async x =>
            {
                await timeConstraint;
                string url = string.Format(
                    "https://en.wikipedia.org/wiki/{0}–{1}_UEFA_Champions_League_knockout_stage",
                    x, (x + 1).ToString().Substring(2));
                if (x == 1999) url = "https://en.wikipedia.org/wiki/1999–2000_UEFA_Champions_League_knockout_stage";
                var matches = await ParseMatches(url);
                matches?.ForEach(match => UpdateStats(match));
                ShowStats();
            });
        }

        private void ShowStats()
        {
            listView1.BeginUpdate();
            listView1.Items.Clear();
            int i = 1;
            foreach (var item in teamStats.OrderBy(key => -key.Value.Pts))
            {
                var node = listView1.Items.Add(i++.ToString());
                node.SubItems.Add(item.Key);
                node.SubItems.Add(item.Value.Pld.ToString());
                node.SubItems.Add(item.Value.W.ToString());
                node.SubItems.Add(item.Value.D.ToString());
                node.SubItems.Add(item.Value.L.ToString());
                node.SubItems.Add(item.Value.GF.ToString());
                node.SubItems.Add(item.Value.GA.ToString());
                node.SubItems.Add(item.Value.GD.ToString());
                node.SubItems.Add(item.Value.Pts.ToString());
            }
            listView1.EndUpdate();
        }

        private void UpdateStats(MatchInfo match)
        {
            TeamStats home, away;
            teamStats.TryGetValue(match.HomeTeam, out home);
            teamStats.TryGetValue(match.AwayTeam, out away);
            if (home == null) home = new TeamStats();
            if (away == null) away = new TeamStats();
            home.Pld += 1; away.Pld += 1;
            if (match.HomeScore > match.AwayScore)
            {
                home.W += 1; home.Pts += 3; away.L += 1;
            }
            else if (match.AwayScore > match.HomeScore)
            {
                away.W += 1; away.Pts += 3; home.L += 1;
            }
            else
            {
                home.D += 1; home.Pts += 1; away.D += 1; away.Pts += 1;
            }
            home.GF += match.HomeScore; home.GA += match.AwayScore; home.GD += match.HomeScore - match.AwayScore;
            away.GF += match.AwayScore; away.GA += match.HomeScore; away.GD += match.AwayScore - match.HomeScore;
            teamStats.Remove(match.HomeTeam);
            teamStats.Add(match.HomeTeam, home);
            teamStats.Remove(match.AwayTeam);
            teamStats.Add(match.AwayTeam, away);
        }

        private async Task<List<MatchInfo>> ParseMatches(string url)
        {
            var page = await htmlWeb.LoadFromWebAsync(url);
            var h1s = page.DocumentNode.SelectNodes("//h1[@class='firstHeading']");
            h1s?.ToList().ForEach(h1 =>
            {
                richTextBox1.AppendText(string.Format("{0}\r\n", url));
                richTextBox1.AppendText(string.Format("{0}\r\n", h1.InnerText));
            });
            var rows = page.DocumentNode.SelectNodes("//table[@class='fevent']");
            return rows?.Select(row => GetMatch(row)).ToList();
        }

        private MatchInfo GetMatch(HtmlNode matchNode)
        {
            MatchInfo match = new MatchInfo();
            var ht = matchNode.SelectNodes("tbody/tr/th[@itemprop='homeTeam']/span/a");
            ht.ToList().ForEach(h =>
            {
                richTextBox1.AppendText(string.Format("\t{0}\r\n", h?.InnerText.Trim()));
                match.HomeTeam = h?.InnerText.Trim();
            });
            var at = matchNode.SelectNodes("tbody/tr/th[@itemprop='awayTeam']/span/a");
            at.ToList().ForEach(h =>
            {
                richTextBox1.AppendText(string.Format("\t{0}\r\n", h?.InnerText.Trim()));
                match.AwayTeam = h?.InnerText.Trim();
            });
            ht = matchNode.SelectNodes("tbody/tr/th[@class='fscore']");
            ht.ToList().ForEach(h =>
            {
                string scoreText = HttpUtility.HtmlDecode(h?.InnerText.Trim());
                var score = Numbers.ExtractNumbers(scoreText);
                richTextBox1.AppendText(string.Format("\t{0} - {1}\r\n", score[0], score[1]));
                match.HomeScore = score[0];
                match.AwayScore = score[1];
            });
            return match;
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            (await GetWomenSingles()).ToList().ForEach(async x =>
            {
                await ProcessEvent(x);
            });

        }

        private async Task ProcessEvent(string url)
        {
            await timeConstraint;
            richTextBox1.AppendText("\t" + url + "\r\n");
            var page = await htmlWeb.LoadFromWebAsync("https://en.wikipedia.org" + url);
            var tds = page.DocumentNode.SelectNodes("//td");
            int idx = 1;
            Dictionary<string, string> players = new Dictionary<string, string>();
            tds.ToList().ForEach(td =>
            {
                var flagspan = td.SelectSingleNode(".//span[@class='flagicon']/a");
                if (flagspan != null)
                {
                    var playerNodes = td.SelectNodes(".//a");
                    string player = "";
                    if (playerNodes != null)
                    {
                        playerNodes.ToList().ForEach(playerNode =>
                        {
                            player = playerNode.Attributes["title"]?.Value;
                        });
                    }
                    else
                    {
                        player = td.InnerHtml;
                    }
                    try
                    {
                        players.Add(player, flagspan.Attributes["title"].Value);
                    }
                    catch
                    {

                    }

                }
            });
            players.Keys.ToList().ForEach(p =>
            {
                if (playerStats.ContainsKey(p))
                {
                    playerStats[p] += 1;
                }
                else
                {
                    playerStats[p] = 1;
                }
                richTextBox1.AppendText(idx++ + "\t" + players[p] + "\t\t" + p + "\t" + playerStats[p] + "\r\n");
            });
        }

        private async Task<IEnumerable<string>> GetWomenSingles()
        {
            var results = await Task.WhenAll(GetAustraliaWomenSingles(), GetFranceWomenSingles(), GetGreatBritainWomenSingles());
            return results.SelectMany(result => result);
        }

        private async Task<List<string>> GetAustraliaWomenSingles()
        {
            var page = await htmlWeb.LoadFromWebAsync("https://en.wikipedia.org/wiki/List_of_Australian_Open_women%27s_singles_champions");
            var refs = page.DocumentNode.SelectNodes("//div[@class='navbox'][2]/table/tbody/tr[3]/td[@class='navbox-list navbox-odd']/div/ul/li/a");
            return refs.Select(r => HtmlEntity.DeEntitize(r?.Attributes["href"]?.Value)).ToList();
        }

        private async Task<List<string>> GetFranceWomenSingles()
        {
            var page = await htmlWeb.LoadFromWebAsync("https://en.wikipedia.org/wiki/List_of_French_Open_women%27s_singles_champions");
            var refs = page.DocumentNode.SelectNodes("//div[@class='navbox'][1]/table/tbody/tr[3]/td[@class='navbox-list navbox-odd']/div/ul/li/a");
            return refs.Select(r => HtmlEntity.DeEntitize(r?.Attributes["href"]?.Value)).ToList();
        }

        private async Task<List<string>> GetGreatBritainWomenSingles()
        {
            var page = await htmlWeb.LoadFromWebAsync("https://en.wikipedia.org/wiki/List_of_Wimbledon_ladies%27_singles_champions");
            var refs = page.DocumentNode.SelectNodes("//div[@class='navbox'][2]/table/tbody/tr[3]/td[@class='navbox-list navbox-odd']/div/ul/li/a");
            return refs.Select(r => HtmlEntity.DeEntitize(r?.Attributes["href"]?.Value)).ToList();
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            var page = await htmlWeb.LoadFromWebAsync("https://en.wikipedia.org/wiki/List_of_Australian_Open_women%27s_singles_champions");
            var refs = page.DocumentNode.SelectNodes("//div[@class='navbox'][2]/table/tbody/tr[3]/td[@class='navbox-list navbox-odd']/div/ul/li/a");
            refs.ToList().ForEach(r =>
            {
                richTextBox1.AppendText(HtmlEntity.DeEntitize(r?.Attributes["title"]?.Value) + "\r\n");
                //richTextBox1.AppendText(r.XPath + "\r\n");
            });
        }
    }
}
