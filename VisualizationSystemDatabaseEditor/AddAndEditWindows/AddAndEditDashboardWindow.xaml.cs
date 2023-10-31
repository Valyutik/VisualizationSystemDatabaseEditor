using System;
using System.Linq;
using System.Windows;
using VisualizationSystemDatabaseEditor.Database;
using VisualizationSystemDatabaseEditor.Database.Models;

namespace VisualizationSystemDatabaseEditor.AddAndEditWindows;

public partial class AddAndEditDashboardWindow
{
    private readonly MainWindow _mainWindow;
    private readonly bool _isEditor;
    private readonly VisualizationSystemContext _visualizationSystemContext;
    private readonly int _indexDashboard;
    
    public AddAndEditDashboardWindow(MainWindow mainWindow, bool isEditor, int indexDashboard)
    {
        InitializeComponent();
        _mainWindow = mainWindow;
        _isEditor = isEditor;
        _indexDashboard = indexDashboard;
        _visualizationSystemContext = _mainWindow.VisualizationSystemContext;
    }
    
    public AddAndEditDashboardWindow(MainWindow mainWindow, bool isEditor)
    {
        InitializeComponent();
        _mainWindow = mainWindow;
        _isEditor = isEditor;
        _indexDashboard = 0;
        _visualizationSystemContext = _mainWindow.VisualizationSystemContext;
    }

    private void AddAndEditDashboardWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        AnalystComboBox.ItemsSource = _visualizationSystemContext.Users.Where(u => u.Role.Name == "Аналитик").ToList();
        if (!_isEditor) return;
        CaptionTextBlock.Text = "Редактирование дашборда";
        AddButton.Content = "Сохранить";
        var dashboard = _visualizationSystemContext.Dashboards.First(u => u.Id == _indexDashboard);
        NameTextBox.Text = dashboard.Name;
        AnalystComboBox.SelectedIndex = AnalystComboBox.Items.IndexOf(dashboard.Analyst);
    }

    private void AddAndEditDashboardWindow_OnClosed(object? sender, EventArgs e)
    {
        ReturnPreviousWindow(sender, null);
    }

    private void AddButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (!_isEditor)
        {
            var dashboard = new Dashboard
            {
                Name = NameTextBox.Text,
                AnalystId = _visualizationSystemContext.Users.First(u => u.Name == AnalystComboBox.Text).Id,
            };
            _visualizationSystemContext.Dashboards.Add(dashboard);
            try
            {
                _visualizationSystemContext.SaveChanges();
                MessageBox.Show($"Дашбоард {dashboard.Name} добавлен!");
                ReturnPreviousWindow(sender, null);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
        else
        {
            var dashboard = _visualizationSystemContext.Dashboards.First(u => u.Id == _indexDashboard);
            dashboard.Name = NameTextBox.Text;
            dashboard.AnalystId = _visualizationSystemContext.Users.First(u => u.Name == AnalystComboBox.Text).Id;
            try
            {
                _visualizationSystemContext.SaveChanges();
                MessageBox.Show($"Дашбоард {dashboard.Name} отредактирован!");
                ReturnPreviousWindow(sender, null);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
    }

    private void ReturnPreviousWindow(object? sender, RoutedEventArgs? e)
    {
        _mainWindow.Show();
        _mainWindow.MainWindow_OnLoaded(sender, null);
        Close();
    }
}