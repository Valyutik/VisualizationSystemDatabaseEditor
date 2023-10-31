using System;
using System.Linq;
using System.Windows;
using VisualizationSystemDatabaseEditor.Database;
using VisualizationSystemDatabaseEditor.Database.Models;

namespace VisualizationSystemDatabaseEditor.AddAndEditWindows;

public partial class AddAndEditDataSourceWindow
{
    private readonly MainWindow _mainWindow;
    private readonly bool _isEditor;
    private readonly VisualizationSystemContext _visualizationSystemContext;
    private readonly int _indexDashboard;
    
    public AddAndEditDataSourceWindow(MainWindow mainWindow, bool isEditor, int indexDashboard)
    {
        InitializeComponent();
        _mainWindow = mainWindow;
        _isEditor = isEditor;
        _indexDashboard = indexDashboard;
        _visualizationSystemContext = _mainWindow.VisualizationSystemContext;
    }
    
    public AddAndEditDataSourceWindow(MainWindow mainWindow, bool isEditor)
    {
        InitializeComponent();
        _mainWindow = mainWindow;
        _isEditor = isEditor;
        _indexDashboard = 0;
        _visualizationSystemContext = _mainWindow.VisualizationSystemContext;
    }

    private void AddAndEditDataSourceWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        TypeComboBox.ItemsSource = _visualizationSystemContext.DataSourceTypes.ToList();
        if (!_isEditor) return;
        CaptionTextBlock.Text = "Редактирование источника данных";
        AddButton.Content = "Сохранить";
        var dataSource = _visualizationSystemContext.DataSources.First(u => u.Id == _indexDashboard);
        NameTextBox.Text = dataSource.Name;
        TypeComboBox.SelectedIndex = dataSource.TypeId - 1;
        LocationTextBox.Text = dataSource.Location;
        SecurityCredentialsTextBox.Text = dataSource.SecurityCredentials;
        FrequencyOfUseTextBox.Text = dataSource.FrequencyOfUse.ToString();
    }

    private void AddAndEditDataSourceWindow_OnClosed(object? sender, EventArgs e)
    {
        ReturnPreviousWindow(sender, null);
    }

    private void AddButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (!_isEditor)
        {
            var dataSource = new DataSource
            {
                Name = NameTextBox.Text,
                TypeId = TypeComboBox.SelectedIndex + 1,
                Location = LocationTextBox.Text,
                SecurityCredentials = SecurityCredentialsTextBox.Text
            };
            if (int.TryParse(FrequencyOfUseTextBox.Text, out var result))
            {
                dataSource.FrequencyOfUse = result;
            }
            _visualizationSystemContext.DataSources.Add(dataSource);
            try
            {
                _visualizationSystemContext.SaveChanges();
                MessageBox.Show($"Источник данных {dataSource.Name} добавлен!");
                ReturnPreviousWindow(sender, null);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
        else
        {
            var dataSource = _visualizationSystemContext.DataSources.First(u => u.Id == _indexDashboard);
            dataSource.Name = NameTextBox.Text;
            dataSource.TypeId = TypeComboBox.SelectedIndex + 1;
            dataSource.Location = LocationTextBox.Text;
            dataSource.SecurityCredentials = SecurityCredentialsTextBox.Text;
            try
            {
                _visualizationSystemContext.SaveChanges();
                MessageBox.Show($"Источник данных {dataSource.Name} отредактирован!");
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