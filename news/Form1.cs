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

        private void button2_Click(object sender, EventArgs e)
        {
            Enumerable.Range(1, 50).Select(x => 2020 - x).ToList().ForEach(async x =>
            {
                await ProcessEvent(string.Format("https://en.wikipedia.org/wiki/{0}_Australian_Open_–_Women%27s_Singles", x));
            });

        }

        private async Task ProcessEvent(string url)
        {
            await timeConstraint;
            richTextBox1.AppendText("\t" + url + "\r\n");
            var page = await htmlWeb.LoadFromWebAsync(url);
            var tds = page.DocumentNode.SelectNodes("//td");
            int idx = 1;
            Dictionary<string, string> players = new Dictionary<string, string>();
            tds.ToList().ForEach(td =>
            {
                var flagspan = td.SelectSingleNode("(span|b|.)/span[@class='flagicon']/a");
                if (flagspan != null)
                {
                    var playerNodes = td.SelectNodes("(span|b|.)/(span|b|.)/a");
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
                richTextBox1.AppendText(idx++ + "\t" + players[p] + "\t" + p + "\r\n");
            });
        }
    }
}
