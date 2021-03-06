﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SeasOfWrath
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LevelOne : ContentPage
    {
        #region --- variables ---
        private int _numOfRows = 7;
        private int _numOfCols = 4;
        private int _food = 6;
        private int _health = 3;
        private List<BoardObject> _dangers;
        private List<BoardObject> _islands;
        private Boolean _found;
        private Boolean _moveTaken;
        private Boolean _movingTurn;
        #endregion

        #region --- Initialize Level ---
        public LevelOne()
        {
            InitializeComponent();
            _movingTurn = false;
            _moveTaken = false;
            _found = false;
            FoodLabel.Text = _food.ToString();
            HealthLabel.Text = _health.ToString();
            MoveIndicator.Text = "Inspect Tile";
            AddLandmarks();
        }

        private void AddLandmarks()
        {
            if (_dangers == null) _dangers = new List<BoardObject>();
            if (_islands == null) _islands = new List<BoardObject>();

            BoardObject danger, island;

            danger = new BoardObject()
            {
                Xpos = 0,
                Ypos = 0
            };
            _dangers.Add(danger);

            danger = new BoardObject()
            {
                Xpos = 0,
                Ypos = 1
            };
            _dangers.Add(danger);

            danger = new BoardObject()
            {
                Xpos = 1,
                Ypos = 0
            };
            _dangers.Add(danger);

            danger = new BoardObject()
            {
                Xpos = 1,
                Ypos = 1
            };
            _dangers.Add(danger);

            danger = new BoardObject()
            {
                Xpos = 1,
                Ypos = 3
            };
            _dangers.Add(danger);

            danger = new BoardObject()
            {
                Xpos = 3,
                Ypos = 1
            };
            _dangers.Add(danger);

            danger = new BoardObject()
            {
                Xpos = 3,
                Ypos = 3
            };
            _dangers.Add(danger);

            danger = new BoardObject()
            {
                Xpos = 4,
                Ypos = 3
            };
            _dangers.Add(danger);

            danger = new BoardObject()
            {
                Xpos = 5,
                Ypos = 1
            };
            _dangers.Add(danger);


            danger = new BoardObject()
            {
                Xpos = 6,
                Ypos = 2
            };
            _dangers.Add(danger);

            danger = new BoardObject()
            {
                Xpos = 6,
                Ypos = 3
            };
            _dangers.Add(danger);

            
            island = new BoardObject()
            {
                Xpos = 3,
                Ypos = 2
            };
            _islands.Add(island);

            island = new BoardObject()
            {
                Xpos = 5,
                Ypos = 0
            };
            _islands.Add(island);

        }
        #endregion

        #region --- ArrowButtons_Clicked ---
        private void UpButton_Clicked(object sender, EventArgs e)
        {
            HandlePlayerTurn(1);
        }
        private void RightArrow_Clicked(object sender, EventArgs e)
        {
            HandlePlayerTurn(2);
        }
        private void DownArrow_Clicked(object sender, EventArgs e)
        {
            HandlePlayerTurn(3);
        }
        private void LeftArrow_Clicked(object sender, EventArgs e)
        {
            HandlePlayerTurn(4);
        }
        #endregion

        private async void HandlePlayerTurn(int button)
        {
            Image player = GameGrid.FindByName<Image>("ImagePlayer");
            double xStep = GameGrid.Width / 4;
            double yStep = GameGrid.Height / 7;

            if (_movingTurn == true)
            {
                switch (button)
                {
                    case 1:
                        await MoveUp(player, yStep);
                        break;
                    case 2:
                        await MoveRight(player, xStep);
                        break;
                    case 3:
                        await MoveDown(player, yStep);
                        break;
                    case 4:
                        await MoveLeft(player, xStep);
                        break;
                    default:
                        Console.WriteLine("Move unknown.");
                        break;
                }

                MoveIndicator.Text = "Inspect Tile";
            }

            else if (_movingTurn == false)
            {
                switch (button)
                {
                    case 1:
                        await CheckUp(player);
                        break;
                    case 2:
                        CheckRight(player);
                        break;
                    case 3:
                        CheckDown(player);
                        break;
                    case 4:
                        CheckLeft(player);
                        break;
                    default:
                        Console.WriteLine("Move unknown.");
                        break;
                }
                MoveIndicator.Text = "Move";
            }

            if (_moveTaken == true)
            {
                await CheckForLandmarksEvent(player);
                await DecrementFood();
                _moveTaken = false;
            }

        }

        #region --- Move Up/Right/Down/Left ---
        private async Task MoveUp(Image player, double yStep)
        {
            int newRow = (int)player.GetValue(Grid.RowProperty) - 1;
            yStep *= -1;

            if (newRow < 0)
            {

            }
            else
            {
                await player.TranslateTo(0, yStep, 500, Easing.CubicInOut);

                player.SetValue(Grid.RowProperty, newRow);
                player.TranslationY = 0;
                _movingTurn = false;
                _moveTaken = true;
                ClearTiles();
            }
        }

        private async Task MoveDown(Image player, double yStep)
        {
            int newRow = (int)player.GetValue(Grid.RowProperty) + 1;

            if (newRow >= _numOfRows)
            {

            }
            else
            {
                await player.TranslateTo(0, yStep, 500, Easing.CubicInOut);
                player.SetValue(Grid.RowProperty, newRow);
                player.TranslationY = 0;
                _movingTurn = false;
                _moveTaken = true;
                ClearTiles();
            }

        }
        private async Task MoveRight(Image player, double xStep)
        {
            int newCol = (int)player.GetValue(Grid.ColumnProperty) + 1;
            if (newCol >= _numOfCols)
            {

            }
            else
            {
                await player.TranslateTo(xStep, 0, 500, Easing.CubicInOut);
                player.SetValue(Grid.ColumnProperty, newCol);
                player.TranslationX = 0; _movingTurn = false;
                _movingTurn = false;
                _moveTaken = true;
                ClearTiles();
            }
        }
        private async Task MoveLeft(Image player, double xStep)
        {
            int newCol = (int)player.GetValue(Grid.ColumnProperty) - 1;
            xStep *= -1;
            if (newCol < 0)
            {

            }
            else
            {
                await player.TranslateTo(xStep, 0, 500, Easing.CubicInOut);
                player.SetValue(Grid.ColumnProperty, newCol);
                player.TranslationX = 0;
                _movingTurn = false;
                _moveTaken = true;
                ClearTiles();
            }
        }
        #endregion

        #region --- Check Up/Right/Down/Left ---
        private async Task CheckUp(Image player)
        {
            // check if the square contains an island or danger tile
            int checkRow = (int)player.GetValue(Grid.RowProperty) - 1;
            int checkCol = (int)player.GetValue(Grid.ColumnProperty);

            if (checkRow < 0)
            {
                bool answer = await DisplayAlert("Congratulations!", "You have won!" +
                            "Would you like to play again?", "Yes", "No");

                if (answer == true)
                {
                    await Navigation.PushAsync(new DevLevelPage());
                }

                else
                {
                    await Navigation.PushAsync(new MainPage());
                }

            }
            else
            {
                CheckForLandmarks(checkRow, checkCol);
            }
        }

        private void CheckDown(Image player)
        {
            // check if the square contains an island or danger tile
            int checkRow = (int)player.GetValue(Grid.RowProperty) + 1;
            int checkCol = (int)player.GetValue(Grid.ColumnProperty);

            if (checkRow >= _numOfRows)
            {

            }
            else
            {
                CheckForLandmarks(checkRow, checkCol);
            }
        }

        private void CheckRight(Image player)
        {
            // check if the square contains an island or danger tile
            int checkRow = (int)player.GetValue(Grid.RowProperty);
            int checkCol = (int)player.GetValue(Grid.ColumnProperty) + 1;

            if (checkCol >= _numOfCols)
            {

            }
            else
            {
                CheckForLandmarks(checkRow, checkCol);
            }
        }

        private void CheckLeft(Image player)
        {
            // check if the square contains an island or danger tile
            int checkRow = (int)player.GetValue(Grid.RowProperty);
            int checkCol = (int)player.GetValue(Grid.ColumnProperty) - 1;

            if (checkCol < 0)
            {

            }
            else
            {
                CheckForLandmarks(checkRow, checkCol);
            }
        }
        #endregion

        #region --- Check LandMarks ---
        private void CheckForLandmarks(int x, int y)
        {
            foreach (var d in _dangers)
            {
                if ((d.Xpos == x) && (d.Ypos == y))
                {
                    Image danger = GameGrid.FindByName<Image>("ImageDanger");
                    danger.SetValue(Grid.RowProperty, x);
                    danger.SetValue(Grid.ColumnProperty, y);
                    danger.IsVisible = true;
                    _found = true;
                    _movingTurn = true;
                }
            }

            foreach (var i in _islands)
            {
                if ((i.Xpos == x) && (i.Ypos == y))
                {
                    Image island = GameGrid.FindByName<Image>("ImageIsland");
                    island.SetValue(Grid.RowProperty, x);
                    island.SetValue(Grid.ColumnProperty, y);
                    island.IsVisible = true;
                    _found = true;
                    _movingTurn = true;
                }
            }

            //If no event tiles were found, place an empty tile
            if (_found == false)
            {
                SetWaveTile(x, y);
            }
            //If an event tile was found, resets the variable for the next check stage
            else { _found = false; }
        }

        private async Task CheckForLandmarksEvent(Image player)
        {
            int x = (int)player.GetValue(Grid.RowProperty);
            int y = (int)player.GetValue(Grid.ColumnProperty);

            foreach (var d in _dangers)
            {
                if ((d.Xpos == x) && (d.Ypos == y))
                {
                    _health -= 1;
                    HealthLabel.Text = _health.ToString();

                    if (_health == 0)
                    {
                        bool answer = await DisplayAlert("Game Over", "You have run out of health. " +
                            "Would you like to try again?", "Yes", "No");

                        if (answer == true)
                        {
                            await Navigation.PushAsync(new DevLevelPage());
                        }

                        else
                        {

                            await Navigation.PushAsync(new MainPage());
                        }

                    }
                }
            }

            foreach (var i in _islands)
            {
                if ((i.Xpos == x) && (i.Ypos == y))
                {
                    //If player lands on an island, replenishes food
                    _food = 7;
                    FoodLabel.Text = _food.ToString();
                }
            }
        }
        #endregion

        private void SetWaveTile(int Ypos, int Xpos)
        {
            //set the tile to a picture of empty waves
            Image wave = GameGrid.FindByName<Image>("ImageWaves");
            wave.SetValue(Grid.RowProperty, Ypos);
            wave.SetValue(Grid.ColumnProperty, Xpos);
            wave.IsVisible = true;
            _movingTurn = true;
        }

        private void ClearTiles()
        {
            //set the tile to a picture of empty waves
            Image danger = GameGrid.FindByName<Image>("ImageDanger");
            Image island = GameGrid.FindByName<Image>("ImageIsland");
            Image waves = GameGrid.FindByName<Image>("ImageWaves");
            danger.IsVisible = false;
            island.IsVisible = false;
            waves.IsVisible = false;
        }

        private async Task DecrementFood()
        {
            _food -= 1;
            FoodLabel.Text = _food.ToString();

            if (_food == 0)
            {
                bool answer = await DisplayAlert("Game Over", "You have run out of food. " +
                            "Would you like to try again?", "Yes", "No");

                if (answer == true)
                {
                    await Navigation.PushAsync(new DevLevelPage());
                }

                else
                {

                    await Navigation.PushAsync(new MainPage());
                }
            }
        }

        private async Task LoadSave()
        {
            //Save save1 = JsonConvert.DeserializeObject<Save>(File.ReadAllText("SeasOfWrath.SaveGame.txt"));
            //string test = save1.Xpos;
            //bool answer = await DisplayAlert("Test", test, "Yes", "No");
            /**
            string jsonText;
            ObservableCollection<Save> myList = new ObservableCollection<Save>();

            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(MainPage)).Assembly;
            // create the stream
            Stream stream = assembly.GetManifestResourceStream("SeasOfWrath.SaveGame.txt");
            using (var reader = new StreamReader(stream))
            {
                jsonText = reader.ReadToEnd();
                // include JSON library now
            }

            myList = JsonConvert.DeserializeObject<ObservableCollection<Save>>(jsonText);
            **/
        }
    }
}