using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using VisualizationSystemDatabaseEditor.Database;
using VisualizationSystemDatabaseEditor.Database.Models;

namespace VisualizationSystemDatabaseEditor.AddAndEditWindows;

public partial class AddAndEditVisualizationWindow
{
    private readonly MainWindow _mainWindow;
    private readonly bool _isEditor;
    private readonly VisualizationSystemContext _visualizationSystemContext;
    private readonly int _indexVisualization;
    
    public AddAndEditVisualizationWindow(MainWindow mainWindow, bool isEditor, int indexVisualization)
    {
        InitializeComponent();
        _mainWindow = mainWindow;
        _isEditor = isEditor;
        _indexVisualization = indexVisualization;
        _visualizationSystemContext = _mainWindow.VisualizationSystemContext;
    }
    
    public AddAndEditVisualizationWindow(MainWindow mainWindow, bool isEditor)
    {
        InitializeComponent();
        _mainWindow = mainWindow;
        _isEditor = isEditor;
        _indexVisualization = 0;
        _visualizationSystemContext = _mainWindow.VisualizationSystemContext;
    }

    private void AddAndEditVisualizationWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        TypeComboBox.ItemsSource = _visualizationSystemContext.VisualizationTypes.ToList();
        AnalystComboBox.ItemsSource = _visualizationSystemContext.Users.Where(u => u.Role.Name == "Аналитик").ToList();
        DataSourcesComboBox.ItemsSource = _visualizationSystemContext.DataSources.ToList();
        if (!_isEditor) return;
        CaptionTextBlock.Text = "Редактирование визуализации";
        AddButton.Content = "Сохранить";
        var visualization = _visualizationSystemContext.Visualizations.First(u => u.Id == _indexVisualization);
        NameTextBox.Text = visualization.Name;
        TypeComboBox.SelectedIndex = visualization.TypeId - 1;
        AnalystComboBox.SelectedIndex = AnalystComboBox.Items.IndexOf(visualization.Analyst);
        DataSourcesComboBox.SelectedIndex = visualization.DataSourceId - 1;
        ParametersTextBox.Text = visualization.Parameters;
        VisualizationSettingsTextBox.Text = visualization.VisualizationSettings;
    }

    private void AddAndEditVisualizationWindow_OnClosed(object? sender, EventArgs e)
    {
        ReturnPreviousWindow(sender, null);
    }
    
    private void ReturnPreviousWindow(object? sender, RoutedEventArgs? e)
    {
        _mainWindow.Show();
        _mainWindow.MainWindow_OnLoaded(sender, null);
        Close();
    }
    
    private void AddButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (!_isEditor)
        {
            var visualization = new Visualization
            {
                Name = NameTextBox.Text,
                TypeId = TypeComboBox.SelectedIndex + 1,
                AnalystId = _visualizationSystemContext.Users.First(u => u.Name == AnalystComboBox.Text).Id,
                DataSourceId = DataSourcesComboBox.SelectedIndex + 1,
                Parameters = ParametersTextBox.Text,
                VisualizationSettings = VisualizationSettingsTextBox.Text
            };
            _visualizationSystemContext.Visualizations.Add(visualization);
            try
            {
                _visualizationSystemContext.SaveChanges();
                MessageBox.Show($"Визуализация {visualization.Name} добавлена!");
                ReturnPreviousWindow(sender, null);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
        else
        {
            var visualization = _visualizationSystemContext.Visualizations.First(u => u.Id == _indexVisualization);
            visualization.Name = NameTextBox.Text;
            visualization.TypeId = TypeComboBox.SelectedIndex + 1;
            visualization.AnalystId = _visualizationSystemContext.Users.First(u => u.Name == AnalystComboBox.Text).Id;
            visualization.DataSourceId = DataSourcesComboBox.SelectedIndex + 1;
            visualization.Parameters = ParametersTextBox.Text;
            visualization.VisualizationSettings = VisualizationSettingsTextBox.Text;
            try
            {
                _visualizationSystemContext.SaveChanges();
                MessageBox.Show($"Визуализация {visualization.Name} отредактирована!");
                ReturnPreviousWindow(sender, null);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
    }
}