using App4.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using Windows.System;
using App4.Model;

// “基本页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234237 上有介绍

namespace App4
{
    /// <summary>
    /// 基本页，提供大多数应用程序通用的特性。
    /// </summary>
    public sealed partial class GamePage : LayoutAwarePage
    {
        public static int sizeOfBlock = 25;

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        MapData mapData = new MapData();
        PersonModel personModel;
        Image personImg = new Image();
        
        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        public GamePage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;

            this.initPersonImg();
            this.initMap();
        }

        public void initPersonImg()
        {
            personModel = new PersonModel(ref mapData);

            personImg.Width = GamePage.sizeOfBlock;
            personImg.Height = GamePage.sizeOfBlock;
            personImg.Source = new BitmapImage(new Uri("ms-appx:///Assets/person.png"));
        }

        /// <summary>
        /// 使用在导航过程中传递的内容填充页。在从以前的会话
        /// 重新创建页时，也会提供任何已保存状态。
        /// </summary>
        /// <param name="navigationParameter">最初请求此页时传递给
        /// <see cref="Frame.Navigate(Type, Object)"/> 的参数值。
        /// </param>
        /// <param name="pageState">此页在以前会话期间保留的状态
        /// 字典。首次访问页面时为 null。</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// 保留与此页关联的状态，以防挂起应用程序或
        /// 从导航缓存中放弃此页。值必须符合
        /// <see cref="SuspensionManager.SessionState"/> 的序列化要求。
        /// </summary>
        /// <param name="pageState">要使用可序列化状态填充的空字典。</param>
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        public void initMap()
        {
            for (int i = 0; i < MapData.row; i++)
            {
                for (int j = 0; j < MapData.column; j++)
                {
                    Image img = new Image();
                    int type = mapData.GetMapItemData(i, j);

                    if (type == 0)
                    {
                        img.Source = new BitmapImage(new Uri("ms-appx:///Assets/floor.png"));
                    }
                    else if(type == 1)
                    {
                        img.Source = new BitmapImage(new Uri("ms-appx:///Assets/wall.png"));
                    }
                    else if (type == -1)
                    {
                        img.Source = new BitmapImage(new Uri("ms-appx:///Assets/wall.png"));
                    }
                    
                    img.Width = GamePage.sizeOfBlock;
                    img.Height = GamePage.sizeOfBlock;

                    Grid.SetRow(img, i);
                    Grid.SetColumn(img, j);

                    mapGridView.Children.Add(img);
                }
            }
            Image img1 = new Image();
            img1.Source = new BitmapImage(new Uri("ms-appx:///Assets/door.png"));
            img1.Width = GamePage.sizeOfBlock;
            img1.Height = GamePage.sizeOfBlock;
            Grid.SetRow(img1, MapData.row - 2);
            Grid.SetColumn(img1, MapData.column - 2);
            mapGridView.Children.Add(img1);

            this.setPerson();
        }

        /// <summary>
        /// 真实键盘事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("e.Key:" + e.Key);
            doEvent(e.Key);
        }

        /// <summary>
        /// 根据传进来的事件来进行操作人物
        /// </summary>
        /// <param name="d"></param>
        public void doEvent(VirtualKey d) 
        {
            switch (d)
            {
                case VirtualKey.Up:
                    personModel.toNextPos(Direction.UP);
                    break;
                case VirtualKey.Down:
                    personModel.toNextPos(Direction.DOWN);
                    break;
                case VirtualKey.Right:
                    personModel.toNextPos(Direction.RIGHT);
                    break;
                case VirtualKey.Left:
                    personModel.toNextPos(Direction.LEFT);
                    break;
            }
            this.setPerson();

            if (this.personModel.isEnd())
            {
                this.showWinMessage();
                this.newGame();
            }
        }

        public async void showWinMessage()
        {
            MessageDialog inf = new MessageDialog("恭喜你！成功达到目的地！");
            await inf.ShowAsync();
        }

        // start a new game
        public void newGame()
        {
            mapGridView.Children.Clear();
            mapData = new MapData();
            initPersonImg();
            this.initMap();
        }

        private void setPerson()
        {
            Position pos = personModel.curPos;
            mapGridView.Children.Remove(personImg);
            Grid.SetRow(personImg, pos.x);
            Grid.SetColumn(personImg, pos.y);
            mapGridView.Children.Add(personImg);
        }

        /// <summary>
        /// 右下角四个button事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            VirtualKey v = VirtualKey.Up;
            if (sender.Equals(UpBtn))
            {
                v = VirtualKey.Up;
            }
            else if (sender.Equals(DownBtn))
            {
                v = VirtualKey.Down;
            }
            else if (sender.Equals(leftBtn))
            {
                v = VirtualKey.Left;
            }
            else if (sender.Equals(RightBtn))
            {
                v = VirtualKey.Right;
            }

            doEvent(v);
        }
    }
}
