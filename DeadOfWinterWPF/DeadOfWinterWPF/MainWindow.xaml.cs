using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DeadOfWinterWPF.Classes;
using DeadOfWinterWPF.Controller;
using DeadOfWinterWPF.Exceptions;

namespace DeadOfWinterWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private StateController _stateController;
        private StateController _controller;

        public MainWindow()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void SurvivorComboBox_OnSelected(object sender, RoutedEventArgs e)
        {
            Survivor selectedSurvivor = _controller.Players[0].Survivors.Find(s => s.Name == SurvivorComboBox.SelectedValue.ToString());
            _controller.SelectedPlayer.SelectedSurvivor = selectedSurvivor;

            Health.Text = selectedSurvivor.Health.ToString();
            Attack.Text = selectedSurvivor.AttackAbility.ToString();
            Search.Text = selectedSurvivor.SearchAbility.ToString();
            CanMove.Text = selectedSurvivor.CanMove.ToString();
        }

        private void LocationComboBox_OnSelected(object sender, RoutedEventArgs e)
        {
            Location selectedLocation = _controller.Locations.Find(l => l.Name == LocationComboBox.SelectedValue.ToString());
            _controller.SelectedLocation = selectedLocation;
        }

        private void StartButton_OnClick(object sender, RoutedEventArgs e)
        {
            _controller.BeginTurn();
            var selectedDice =
                _controller.SelectedPlayer.Dice.Find(d => d.Number.ToString() == DiceComboBox.SelectedValue.ToString());
            _controller.SelectedPlayer.SelectedDice = selectedDice;

            DiceUsedText.Text = selectedDice.IsUsed.ToString();
            DiceValueText.Text = selectedDice.Value.ToString();
        }

        private void EndButton_OnClick(object sender, RoutedEventArgs e)
        {
            _controller.EndTurn();
        }

        private void DiceComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedDice =
                _controller.SelectedPlayer.Dice.Find(d => d.Number.ToString() == DiceComboBox.SelectedValue.ToString());
            _controller.SelectedPlayer.SelectedDice = selectedDice;

            DiceUsedText.Text = selectedDice.IsUsed.ToString();
            DiceValueText.Text = selectedDice.Value.ToString();
        }

        private void Move_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _controller.SelectedPlayer.SelectedSurvivor.Move(_controller
                    .SelectedLocation);
                Health.Text = _controller.SelectedPlayer.SelectedSurvivor.Health.ToString();
                CanMove.Text = _controller.SelectedPlayer.SelectedSurvivor.CanMove.ToString();
            }
            catch (InvalidActionException)
            {
                InvalidActionMessageBox();
            }
        }

        private void AttackButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _controller.SelectedPlayer.SelectedSurvivor.Attack(_controller.SelectedPlayer.SelectedDice);
                DiceUsedText.Text = _controller.SelectedPlayer.SelectedDice.IsUsed.ToString();
            }
            catch (InvalidActionException)
            {
                InvalidActionMessageBox();
            }
        }

        private void InitializeGame()
        {
            _stateController = new StateController();
            _controller = StateController.Instance;
            MakeLocations();
            Player player = new Player();
            _controller.Players.Add(player);
            _controller.SelectedPlayer = _controller.Players[0];
            _controller.BeginTurn();

            var diceList = _controller.SelectedPlayer.Dice.ConvertAll(d => d.Number.ToString());
            DiceComboBox.ItemsSource = diceList;
            DiceComboBox.SelectedItem = diceList[0];
            _controller.SelectedPlayer.SelectedDice = _controller.SelectedPlayer.Dice
                .Find(d => d.Number == int.Parse(DiceComboBox.SelectedValue.ToString()));

            var survivorList = _controller.Players[0].Survivors.ConvertAll(s => s.Name);
            SurvivorComboBox.ItemsSource = survivorList;
            SurvivorComboBox.SelectedItem = survivorList[0];
            _controller.SelectedPlayer.SelectedSurvivor = _controller.SelectedPlayer.Survivors
                .Find(s => s.Name == SurvivorComboBox.SelectedValue.ToString());

            var locationList = _controller.Locations.ConvertAll(l => l.Name);
            LocationComboBox.ItemsSource = locationList;
            LocationComboBox.SelectedItem = locationList[0];
            _controller.SelectedLocation =
                _controller.Locations.Find(l => l.Name == LocationComboBox.SelectedValue.ToString());
            var colony = (Colony) _controller.Locations.Find(l => l.GetType() == typeof(Colony));
            TrashText.Text = colony.AmountOfTrash.ToString();

            var hand = _controller.SelectedPlayer.Hand.Select(c => c.Name).ToList();
            HandComboBox.ItemsSource = hand;

        }

        private void MakeLocations()
        {
            Location policeStation = new Location("PoliceStation", 1, 2, 1, PoliceStation);
            Location colony = new Colony("Colony", 0, 3, 4, Colony);
            Location graveyard = new Location("Graveyard", 2, 10, 0, Graveyard);

            _controller.Locations.Add(policeStation);
            _controller.Locations.Add(colony);
            _controller.Locations.Add(graveyard);
        }

        private void InvalidActionMessageBox()
        {
            string messageBoxText = "Invalid Action";
            string caption = "About Me.";

            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Error;

            MessageBox.Show(messageBoxText, caption, button, icon);
        }

        private void SearchButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _controller.SelectedPlayer.SelectedSurvivor.Search(_controller.SelectedPlayer.SelectedDice);
                DiceUsedText.Text = _controller.SelectedPlayer.SelectedDice.IsUsed.ToString();
            }
            catch (InvalidActionException)
            {
                InvalidActionMessageBox();
            }
        }

        private void CleanButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _controller.SelectedPlayer.SelectedSurvivor.CleanTrash(_controller.SelectedPlayer.SelectedDice);
                DiceUsedText.Text = _controller.SelectedPlayer.SelectedDice.IsUsed.ToString();
                var colony = (Colony) _controller.Locations.Find(l => l.GetType() == typeof(Colony));
                TrashText.Text = colony.AmountOfTrash.ToString();
            }
            catch (InvalidActionException)
            {
                InvalidActionMessageBox();
            }
        }

        private void HandComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedCard = _controller.SelectedPlayer.Hand.ToList().Find(l => l.Name == LocationComboBox.SelectedValue.ToString());
            _controller.SelectedPlayer.SelectedCard = selectedCard;
        }
    }
     
}
