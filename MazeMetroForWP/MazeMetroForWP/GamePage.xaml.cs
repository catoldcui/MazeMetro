using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MazeMetroForWP.Model;


namespace MazeMetroForWP
{
    public partial class GamePage : PhoneApplicationPage
    {
        public static int sizeOfBlock = 25;
        public static int distance = 9;

        MapData mapData = new MapData();
        PersonModel personModel;
        Image personImg = new Image();

        public GamePage()
        {
            InitializeComponent();
            this.initPersonImg();
            this.initMap();
        }

        public void initPersonImg()
        {
            System.Diagnostics.Debug.WriteLine("设置人物位置");
            personModel = new PersonModel(ref mapData);

            personImg.Width = GamePage.sizeOfBlock;
            personImg.Height = GamePage.sizeOfBlock;
            personImg.Source = new BitmapImage(new Uri("Assets/person.png", UriKind.RelativeOrAbsolute));
            personImg.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            personImg.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            Grid.SetRow(personImg, 3);
            Grid.SetColumn(personImg, 8);

            mapGridView.Children.Add(personImg);
        }

        public void initMap()
        {
            this.mapGridView.Children.Clear();
            int x = personModel.curPos.x;
            int y = personModel.curPos.y;

            for (int i = 0; i < MapData.row; i++)
            {
                for (int j = 0; j < MapData.column; j++)
                {
                    double curDistance = Math.Pow(Math.Pow(x - i, 2) + Math.Pow(y - j, 2), 0.5);
                    if (curDistance > distance)
                    {
                        continue;
                    }
                    Image img = new Image();
                    int type = mapData.GetMapItemData(i, j);

                    if (type == 0)
                    {
                        img.Source = new BitmapImage(new Uri("Assets/floor.png", UriKind.RelativeOrAbsolute));
                    }
                    else if (type == 1)
                    {
                        img.Source = new BitmapImage(new Uri("Assets/wall.png", UriKind.RelativeOrAbsolute));
                    }
                    else if (type == -1)
                    {
                        img.Source = new BitmapImage(new Uri("Assets/wall.png", UriKind.RelativeOrAbsolute));
                    }

                    img.Width = GamePage.sizeOfBlock;
                    img.Height = GamePage.sizeOfBlock;

                    Grid.SetRow(img, i);
                    Grid.SetColumn(img, j);

                    mapGridView.Children.Add(img);
                }
            }
            Image img1 = new Image();
            img1.Source = new BitmapImage(new Uri("Assets/door.png", UriKind.RelativeOrAbsolute));
            img1.Width = GamePage.sizeOfBlock;
            img1.Height = GamePage.sizeOfBlock;
            Grid.SetRow(img1, MapData.row - 2);
            Grid.SetColumn(img1, MapData.column - 2);
            mapGridView.Children.Add(img1);

            this.setPerson();
        }

        /// <summary>
        /// 根据传进来的事件来进行操作人物
        /// </summary>
        /// <param name="d"></param>
        public void doEvent(int d)
        {
            bool result = false;
            switch (d)
            {
                case 0:
                    result = personModel.toNextPos(Direction.UP);
                    break;
                case 1:
                    result = personModel.toNextPos(Direction.DOWN);
                    break;
                case 2:
                    result = personModel.toNextPos(Direction.LEFT);
                    break;
                case 3:
                    result = personModel.toNextPos(Direction.RIGHT);
                    break;
            }

            if (result)
            {
                this.initMap();

                if (this.personModel.isEnd())
                {
                    if (distance == 2)
                    {
                        this.showWinMessage("恭喜你！成功完成一轮游戏！请重新开始！");
                        distance = 9;
                    }
                    else
                    {
                        this.showWinMessage("恭喜你！成功达到目的地！");
                    }
                    this.newGame();
                }
            }
        }

        public void showWinMessage(String mes)
        {
            MessageBox.Show(mes);
        }

        // start a new game
        public void newGame()
        {
            distance--;
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
            int type = -1;
            if (sender.Equals(UpBtn))
            {
                type = 0;
            }
            else if (sender.Equals(DownBtn))
            {
                type = 1;
            }
            else if (sender.Equals(leftBtn))
            {
                type = 2;
            }
            else if (sender.Equals(RightBtn))
            {
                type = 3;
            }

            doEvent(type);
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            distance = 9;
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }
    }
}