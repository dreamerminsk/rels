using HtmlAgilityPack;
using RateLimiter;
using System;
using System.Linq;
using System.Web;
using System.Windows.Forms;
using ComposableAsync;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace news
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
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
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
                matches.ForEach(match => UpdateStats(match));
            });
            Enumerable.Range(1, 16).Select(x => 2008 - x).ToList().ForEach(async x =>
            {
                await timeConstraint;
                string url = string.Format(
                    "https://en.wikipedia.org/wiki/{0}–{1}_UEFA_Champions_League_knockout_stage",
                    x, (x + 1).ToString().Substring(2));
                var matches = await ParseMatches(url);
                matches.ForEach(match => UpdateStats(match));
            });
        }
        private void UpdateStats(MatchInfo match)
        {
            TeamStats home, away;
            teamStats.TryGetValue(match.HomeTeam, out home);
            teamStats.TryGetValue(match.HomeTeam, out away);
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
                richTextBox1.AppendText(string.Format("\t{0}\r\n", h?.Attributes["title"].Value));
                match.HomeTeam = h?.Attributes["title"]?.Value;
            });
            var at = matchNode.SelectNodes("tbody/tr/th[@itemprop='awayTeam']/span/a");
            at.ToList().ForEach(h =>
            {
                richTextBox1.AppendText(string.Format("\t{0}\r\n", h?.Attributes["title"].Value));
                match.AwayTeam = h?.Attributes["title"]?.Value;
            });
            ht = matchNode.SelectNodes("tbody/tr/th[@class='fscore']");
            ht.ToList().ForEach(h =>
            {
                string scoreText = HttpUtility.HtmlDecode(h?.InnerText.Trim());
                var score = ExtractNumbers(scoreText);
                richTextBox1.AppendText(string.Format("\t{0} - {1}\r\n", score[0], score[1]));
                match.HomeScore = score[0];
                match.AwayScore = score[1];
            });
            return match;
        }

        private List<int> ExtractNumbers(string text)
        {
            List<int> numbers = new List<int>();
            string number = "0";
            foreach (char letter in text)
            {
                if (Char.IsDigit(letter))
                {
                    number += letter;
                }
                else
                {
                    numbers.Add(int.Parse(number));
                    number = "0";
                }
            }
            numbers.Add(int.Parse(number));
            return numbers;
        }
    }

    public class MatchInfo
    {
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public int HomeScore { get; set; } = 0;
        public int AwayScore { get; set; } = 0;
    }

    public class TeamStats
    {
        public int Pld { get; set; } = 0;
        public int W { get; set; } = 0;
        public int D { get; set; } = 0;
        public int L { get; set; } = 0;
        public int GF { get; set; } = 0;
        public int GA { get; set; } = 0;
        public int GD { get; set; } = 0;
        public int Pts { get; set; } = 0;
    }
}
