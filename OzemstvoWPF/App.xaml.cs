﻿using OzemstvoConsole;
using OzemstvoWPF.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Windows;
using System.Windows.Media;

namespace OzemstvoWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            //TODO: preventSecond();
        }

        private const string UniqueEventName = "Ozemstvo";
        private readonly Ozemstvo _ozemstvo = new();
        private ObservableCollection<RuleProperty> _rules = new();
        private ObservableCollection<BrowserProperty> _browsers = new();

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            _ozemstvo.Init();
            LoadRulesProperties();
            LoadBrowsersProperties();
            LoadDefaultBrowser();

            if (e.Args.Length > 0)
            {
                string target = e.Args[0];
                Uri.TryCreate(target, UriKind.Absolute, out Uri? uri);
                if (uri is null)
                {
                    Console.WriteLine("Invalid URL");
                    Shutdown();
                    return;
                }

                _ozemstvo.Browsers = GetBrowsers();
                _ozemstvo.Rules = GetRules();

                _ozemstvo.Run(new Uri(e.Args[0]));
                Shutdown();
            }
            else
            {
                MainWindow main = new MainWindow(_rules, _browsers);
                main.Show();
            }
        }

        private void preventSecond()
        {
            try
            {
                EventWaitHandle.OpenExisting(UniqueEventName);
                Shutdown();
            }
            catch (WaitHandleCannotBeOpenedException)
            {
                new EventWaitHandle(false, EventResetMode.AutoReset, UniqueEventName);
            }
        }

        public void SaveRulesProperties()
        {
            var rules = JsonSerializer.Serialize(_rules);
            Settings.Default.Rules = rules;
            Settings.Default.Save();
        }

        public void SaveBrowserProperties()
        {
            ObservableCollection<BrowserProperty> browsers = new();
            foreach (var browser in _browsers)
            {
                if (browser.UserDefined)
                {
                    browsers.Add(browser);
                }
            }
            Settings.Default.Browsers = JsonSerializer.Serialize(browsers);
            Settings.Default.Save();
        }

        public void SaveDefaultBrowser()
        {
            BrowserProperty defaultBrowser = _browsers.First(b => b.Default);
            if (defaultBrowser is null) return;
            Settings.Default.DefaultBrowserId = defaultBrowser.Id;
            Settings.Default.Save();
        }

        public void LoadRulesProperties()
        {
            var rules = JsonSerializer.Deserialize<ObservableCollection<RuleProperty>>(Settings.Default.Rules.ToString());
            if (rules is null) return;
            //foreach (var rule in rules)
            //{
            //    foreach (var match in rule.Matches)
            //    {
            //        if (string.IsNullOrEmpty(match.ColorConfig))
            //        {
            //            var random = new Random();
            //            var number = random.Next(1, 6);
            //            switch (number)
            //            {
            //                case 1:
            //                    match.ColorConfig = "#151F30";
            //                    break;
            //                case 2:
            //                    match.ColorConfig = "#103778";
            //                    break;
            //                case 3:
            //                    match.ColorConfig = "#0593A2";
            //                    break;
            //                case 4:
            //                    match.ColorConfig = "#FF7A48";
            //                    break;
            //                case 5:
            //                    match.ColorConfig = "#E3371E";
            //                    break;
            //            }
            //        }
            //    }
            //    if (string.IsNullOrEmpty(rule.ColorConfig))
            //    {
            //        var random = new Random();
            //        var number = random.Next(1, 6);
            //        switch (number)
            //        {
            //            case 1:
            //                rule.ColorConfig = "#151F30";
            //                break;
            //            case 2:
            //                rule.ColorConfig = "#103778";
            //                break;
            //            case 3:
            //                rule.ColorConfig = "#0593A2";
            //                break;
            //            case 4:
            //                rule.ColorConfig = "#FF7A48";
            //                break;
            //            case 5:
            //                rule.ColorConfig = "#E3371E";
            //                break;
            //        }
            //    }
            //}
            _rules = rules;
        }

        public void LoadBrowsersProperties()
        {
            foreach (var browser in _ozemstvo.Browsers)
            {
                _browsers.Add(new BrowserProperty
                {
                    Id = browser.Id,
                    Name = browser.Name,
                    Path = browser.Path,
                    UserDefined = false,
                    Default = browser.Default
                });
            }
            var browsers = JsonSerializer.Deserialize<ObservableCollection<BrowserProperty>>(Settings.Default.Browsers.ToString());
            if (browsers is null) return;
            foreach (var browser in browsers)
            {
                if (browser.Default) foreach (var b in _browsers) b.Default = false;
                _browsers.Add(browser);
            }

        }

        public void LoadDefaultBrowser()
        {
            string defaultBrowserId = Settings.Default.DefaultBrowserId.ToString();
            if (string.IsNullOrEmpty(defaultBrowserId)) return;
            foreach (var browser in _browsers)
            {
                browser.Default = defaultBrowserId == browser.Id;
            }
        }

        static public List<Rule> GetRules(List<Browser> browsers)
        {
            List<Rule> rules = new List<Rule>();
            var savedRules = JsonSerializer.Deserialize<ObservableCollection<RuleProperty>>(Settings.Default.Rules.ToString());
            if (savedRules is null) return rules;
            foreach (var rule in savedRules)
            {
                var browser = browsers.Find(b => b.Name == rule.Browser);
                if (browser is null) continue;
                
                List<Match> matches = new();
                foreach (var match in rule.Matches)
                {
                    Enum.TryParse(match.Type, true, out MatchType type);
                    matches.Add(new Match(match.Value, type));
                }

                rules.Add(
                    new Rule(rule.Name, browser, matches, rule.Template, rule.Id));
            }
            return rules;
        }

        public List<Rule> GetRules()
        {
            List<Rule> rules = new List<Rule>();
            foreach (var rule in _rules)
            {
                var browser = _ozemstvo.Browsers.Find(b => b.Name == rule.Browser);
                if (browser is null) continue;

                List<Match> matches = new();
                foreach (var match in rule.Matches)
                {
                    Enum.TryParse(match.Type, true, out MatchType type);
                    matches.Add(new Match(match.Value, type));
                }

                rules.Add(
                    new Rule(rule.Name, browser, matches, rule.Template, rule.Id));
            }
            return rules;
        }
        
        public List<Browser> GetBrowsers()
        {
            List<Browser> browsers = new List<Browser>();
            foreach (var browser in _browsers)
            {
                browsers.Add(new Browser(browser.Name, browser.Path, browser.Default));
            }
            return browsers;
        }
    }
}
