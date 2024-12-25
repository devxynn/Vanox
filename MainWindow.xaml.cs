using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Wpf.Ui;
using FaisalAPI;

namespace Vanox
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Wpf.Ui.Controls.FluentWindow
    {
        FaisalAPI.FAISAL api = new FaisalAPI.FAISAL();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Attach_Click(object sender, RoutedEventArgs e)
        {
            api.Attach();
        }

        private void Execute_Click(object sender, RoutedEventArgs e)
        {
            if (TabContainer.SelectedItem is TabItem selectedTab &&
                selectedTab.Content is Grid tabContent &&
                tabContent.Children[0] is TextBox editor)
            {
                api.SendScript(editor.Text);
            }
        }

        private void AddTab_Click(object sender, RoutedEventArgs e)
        {
            // Limit the number of tabs to 3
            if (TabContainer.Items.Count >= 4)
            {
                MessageBox.Show("You can only have up to 4 tabs.", "Limit Reached", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Create a new TabItem
            TabItem newTab = new TabItem();

            // Create a TextBox for the tab header
            var headerTextBox = new TextBox
            {
                Text = $"Tab {TabContainer.Items.Count + 1}",
                BorderThickness = new Thickness(0),
                Background = Brushes.Transparent,
                Foreground = Brushes.White,
                FontSize = 14,
                Padding = new Thickness(5, 2, 5, 2),
                IsReadOnly = true // Initially readonly
            };

            // Allow editing on double-click
            headerTextBox.MouseDoubleClick += (s, args) =>
            {
                headerTextBox.IsReadOnly = false;
                headerTextBox.Focus();
            };

            // End editing on losing focus or pressing Enter
            headerTextBox.LostFocus += (s, args) => headerTextBox.IsReadOnly = true;
            headerTextBox.KeyDown += (s, args) =>
            {
                if (args.Key == System.Windows.Input.Key.Enter)
                {
                    headerTextBox.IsReadOnly = true;
                }
            };

            // Set the header to the editable TextBox
            newTab.Header = headerTextBox;

            // Create the tab content
            newTab.Content = new Grid
            {
                Children =
                {
                    new Border
                    {
                        BorderBrush = new SolidColorBrush(Color.FromArgb(40, 255, 255, 255)),
                        BorderThickness = new Thickness(1),
                        CornerRadius = new CornerRadius(0, 7, 7, 7),
                        Background = new SolidColorBrush(Color.FromArgb(80, 0, 0, 0)),
                        Margin = new Thickness(9, -1, 10, 0),
                        Height = 296.7,
                        Child = new ICSharpCode.AvalonEdit.TextEditor
                        {
                            ShowLineNumbers = true,
                            SyntaxHighlighting = ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance.GetDefinition("lua.xshd"),
                            FontFamily = new FontFamily("Consolas"),
                            FontSize = 14,
                            Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)),
                            Foreground = new SolidColorBrush(Color.FromRgb(250, 250, 250)),
                            Margin = new Thickness(10, 10, 10, 50),
                            Padding = new Thickness(10),
                        }
                    },

                    new Wpf.Ui.Controls.Button
                    {
                        Content = "",
                        Icon = new Wpf.Ui.Controls.SymbolIcon
                        {
                            Symbol = Wpf.Ui.Controls.SymbolRegular.Delete24
                        },
                        VerticalAlignment = VerticalAlignment.Bottom,
                        HorizontalAlignment = HorizontalAlignment.Right,
                        Margin = new Thickness(0, 0, 20, 12)
                    }
                }
            };

            // Wire up delete button functionality
            Grid grid = (Grid)newTab.Content;
            Button deleteButton = (Button)grid.Children[1];
            deleteButton.Click += DeleteTab_Click;

            // Add the new tab to the TabContainer
            TabContainer.Items.Add(newTab);
            TabContainer.SelectedItem = newTab;
        }

        private void DeleteTab_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button deleteButton &&
                deleteButton.Parent is Grid tabContent &&
                tabContent.Parent is TabItem tab)
            {
                TabContainer.Items.Remove(tab);
            }
        }

        private void TabHeader_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.IsReadOnly = false;
                textBox.Focus();
                textBox.SelectAll();
            }
        }

        private void TabHeader_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.IsReadOnly = true;
            }
        }

        private void TabHeader_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (sender is TextBox textBox && e.Key == System.Windows.Input.Key.Enter)
            {
                textBox.IsReadOnly = true;
            }
        }

    }
}
