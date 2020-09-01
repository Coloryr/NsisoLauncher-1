﻿using NsisoLauncher.Config;
using NsisoLauncher.Utils;
using NsisoLauncherCore.Net.MojangApi.Api;
using NsisoLauncherCore.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace NsisoLauncher.ViewModels.Pages
{
    public class UserPageViewModel : INotifyPropertyChanged
    {
        public Windows.MainWindowViewModel MainWindowVM { get; set; }
        public bool IsLoggedIn { get; set; } = true;
        public bool IsNotLoggedIn { get => !IsLoggedIn; }
        public string LoggedInUsername { get; set; }
        public UserNode LoggedInUser { get; set; }
        public Brush StateColor { get; set; } = new SolidColorBrush(Color.FromRgb(255, 0, 0));
        public string State { get; set; }
        public string AuthName { get; set; }
        public Dictionary<string, Uuid> UUIDList { get; set; }

        public ICommand OfflineLoginCmd { get; set; }
        public ICommand MojangLoginCmd { get; set; }
        public ICommand OtherLoginCmd { get; set; }

        public ICommand LogoutCmd { get; set; }

        public User User { get; set; }

        public UserPageViewModel()
        {
            if (App.MainWindowVM != null)
            {
                MainWindowVM = App.MainWindowVM;
            }

            this.PropertyChanged += UserPageViewModel_PropertyChanged;

            User = App.Config?.MainConfig?.User;

            if (User != null)
            {
                if (!string.IsNullOrEmpty(User.SelectedUser))
                {
                    this.LoggedInUser = User.GetSelectedUser();
                }
            }

            OfflineLoginCmd = new DelegateCommand(async (a) =>
            {
                await OfflineLogin();
            });

            LogoutCmd = new DelegateCommand((a) =>
            {
                Logout();
            });

            //LoggedInUser = new UserNode()
            //{
            //    UserName = "nsisogf@163.com",
            //    SelectProfileUUID = "888",
            //    Profiles = new Dictionary<string, Uuid>()
            //{
            //    { "666", new Uuid() { PlayerName = "siso" } },
            //    { "777", new Uuid() { PlayerName = "2号角色22" } },
            //    { "888", new Uuid() { PlayerName = "3号角色33333" } },
            //    { "999", new Uuid() { PlayerName = "4号角色44444444" } },
            //}
            //};
        }

        private void UserPageViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "LoggedInUser")
            {
                if (LoggedInUser != null)
                {
                    IsLoggedIn = true;
                    UUIDList = LoggedInUser.Profiles;
                    LoggedInUsername = LoggedInUser.UserName;
                    State = "在线";
                    if (User.AuthenticationDic.ContainsKey(LoggedInUser.AuthModule))
                    {
                        AuthName = User.AuthenticationDic[LoggedInUser.AuthModule].Name;
                    }
                    else
                    {
                        AuthName = "未知";
                    }
                    StateColor = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                }
                else
                {
                    IsLoggedIn = false;
                    UUIDList = null;
                    LoggedInUsername = null;
                    State = null;
                    AuthName = null;
                    StateColor = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                }
            }
        }

        private async Task OfflineLogin()
        {
            string username = await App.MainWindowVM.ShowInputAsync("输入游戏用户名", "游戏中显示的名字将会是此用户名");
            string uuidValue = Guid.NewGuid().ToString();
            UserNode userNode = new UserNode()
            {
                UserName = username,
                AccessToken = Guid.NewGuid().ToString(),
                AuthModule = "offline",
                Profiles = new Dictionary<string, Uuid>() { { uuidValue, new Uuid() { PlayerName = username, Value = uuidValue } } },
                SelectProfileUUID = uuidValue
            };
            string userId = Guid.NewGuid().ToString();
            User.UserDatabase.Add(userId, userNode);
            this.LoggedInUser = userNode;
            User.SelectedUser = userId;
        }

        private void Logout()
        {
            if (LoggedInUser.AuthModule == "offline")
            {
            }
            else
            {
                //todo 线上注销
            }
            User.SelectedUser = null;
            LoggedInUser = null;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
