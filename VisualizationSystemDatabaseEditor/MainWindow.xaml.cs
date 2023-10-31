using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using VisualizationSystemDatabaseEditor.AddAndEditWindows;
using VisualizationSystemDatabaseEditor.Database;

namespace VisualizationSystemDatabaseEditor;

public partial class MainWindow
{
    public readonly VisualizationSystemContext VisualizationSystemContext;
        
    public MainWindow()
    {
        InitializeComponent();
        VisualizationSystemContext = new VisualizationSystemContext();
    }

    public void MainWindow_OnLoaded(object? sender, RoutedEventArgs? e)
    {
        UsersDataGrid.ItemsSource = VisualizationSystemContext.Users.ToList();
        VisualizationsDataGrid.ItemsSource = VisualizationSystemContext.Visualizations.ToList();
        DashboardsDataGrid.ItemsSource = VisualizationSystemContext.Dashboards.ToList();
        DataSourcesDataGrid.ItemsSource = VisualizationSystemContext.DataSources.ToList();
        RoleFilterUserComboBox.ItemsSource = VisualizationSystemContext.Roles.ToList();
    }

    private void MainWindow_OnClosed(object? sender, EventArgs e)
    {
        VisualizationSystemContext.Dispose();
    }
        
    private void AddDataButton_OnClick(object sender, RoutedEventArgs e)
    {
        switch (MainTabControl.SelectedIndex)
        {
            case 0:
            {
                var addAndEditUserWindow = new AddAndEditUserWindow(this, false);
                addAndEditUserWindow.Show();
                Hide();
                break;
            }
            case 1:
            {
                var addAndEditVisualizationWindow = new AddAndEditVisualizationWindow(this, false);
                addAndEditVisualizationWindow.Show();
                Hide();
                break;
            }
            case 2:
            {
                var addAndEditDashboardWindow = new AddAndEditDashboardWindow(this, false);
                addAndEditDashboardWindow.Show();
                Hide();
                break;
            }
            case 3:
            {
                var addAndEditDataSourceWindow = new AddAndEditDataSourceWindow(this, false);
                addAndEditDataSourceWindow.Show();
                Hide();
                break;
            }
        }
    }

    private void OpenEditWindow(object sender, RoutedEventArgs e)
    {
        var dataGrid = MainTabControl.SelectedIndex switch
        {
            0 => UsersDataGrid,
            1 => VisualizationsDataGrid,
            2 => DashboardsDataGrid,
            3 => DataSourcesDataGrid,
            _ => throw new Exception()
        };
        var selectedCell = dataGrid.SelectedCells[0];
        var cellContent = selectedCell.Column.GetCellContent(selectedCell.Item);
        if (cellContent is not TextBlock block) return;
        if (!int.TryParse(block.Text, out var result)) return;
        Window window = MainTabControl.SelectedIndex switch
        {
            0 => new AddAndEditUserWindow(this, true, result),
            1 => new AddAndEditVisualizationWindow(this, true, result),
            2 => new AddAndEditDashboardWindow(this, true, result),
            3 => new AddAndEditDataSourceWindow(this, true, result),
            _ => throw new Exception()
        };
        window.Show();
        Hide();
    }

    #region Filters

    private void SetUserFilter()
    {
        string? role = null;
        UsersDataGrid.ItemsSource = RoleFilterUserComboBox.SelectedIndex switch
        {
            0 => role = "Администратор",
            1 => role = "Аналитик",
            2 => role = "Сотрудник государственной власти",
            _ => UsersDataGrid.ItemsSource
        };
            
        UsersDataGrid.ItemsSource = VisualizationSystemContext.Users.Where(u =>
            EF.Functions.Like(u.Name, $"%{NameFilterUserTextBox.Text}%") &&
            EF.Functions.Like(u.Email, $"%{EmailFilterUserTextBox.Text}%") &&
            EF.Functions.Like(u.PhoneNumber, $"%{PhoneNumberFilterUserTextBox.Text}%") &&
            EF.Functions.Like(u.Role.Name, $"%{role}%")).ToList();
    }

    private void SetVisualizationFilter()
    {
        VisualizationsDataGrid.ItemsSource = VisualizationSystemContext.Visualizations.Where(u =>
            EF.Functions.Like(u.Name, $"%{NameFilterVisualizationTextBox.Text}%") &&
            EF.Functions.Like(u.Analyst.Name, $"%{AnalystFilterVisualizationTextBox.Text}%") &&
            EF.Functions.Like(u.Type.Name, $"%{TypeFilterVisualizationTextBox.Text}%") &&
            EF.Functions.Like(u.DataSource.Name, $"%{DataSourceFilterVisualizationTextBox.Text}%")).ToList();
    }

    private void SetDashboardFilter()
    {
        DashboardsDataGrid.ItemsSource = VisualizationSystemContext.Dashboards.Where(u =>
            EF.Functions.Like(u.Name, $"%{NameFilterDashboardTextBox.Text}%") &&
            EF.Functions.Like(u.Analyst.Name, $"%{AnalystFilterDashboardTextBox.Text}%")).ToList();
    }

    private void SetDataSourceFilter()
    {
        DataSourcesDataGrid.ItemsSource = VisualizationSystemContext.DataSources.Where(u =>
            EF.Functions.Like(u.Name, $"%{NameFilterDataSourceTextBox.Text}%") &&
            EF.Functions.Like(u.Type.Name, $"%{TypeFilterDataSourceTextBox.Text}%") &&
            EF.Functions.Like(u.Location, $"%{LocationFilterDataSourceTextBox.Text}%")).ToList();
    }
        
    private void SearchButton_OnClick(object sender, RoutedEventArgs? e)
    {
        switch (MainTabControl.SelectedIndex)
        {
            case 0:
                SetUserFilter();
                break;
            case 1:
                SetVisualizationFilter();
                break;
            case 2:
                SetDashboardFilter();
                break;
            case 3:
                SetDataSourceFilter();
                break;
        }
    }

    private void ResetFilterButton_OnClick(object sender, RoutedEventArgs e)
    {
        NameFilterUserTextBox.Clear();
        EmailFilterUserTextBox.Clear();
        PhoneNumberFilterUserTextBox.Clear();
        RoleFilterUserComboBox.SelectedIndex = -1;
        NameFilterVisualizationTextBox.Clear();
        TypeFilterVisualizationTextBox.Clear();
        AnalystFilterVisualizationTextBox.Clear();
        DataSourceFilterVisualizationTextBox.Clear();
        NameFilterDashboardTextBox.Clear();
        AnalystFilterDashboardTextBox.Clear();
        NameFilterDataSourceTextBox.Clear();
        TypeFilterDataSourceTextBox.Clear();
        LocationFilterDataSourceTextBox.Clear();
        SearchButton_OnClick(sender, null);
    }

    #endregion

    private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
    {
        var dataGrid = MainTabControl.SelectedIndex switch
        {
            0 => UsersDataGrid,
            1 => VisualizationsDataGrid,
            2 => DashboardsDataGrid,
            3 => DataSourcesDataGrid,
            _ => throw new Exception()
        };
        var selectedCell = dataGrid.SelectedCells[0];
        var cellContent = selectedCell.Column.GetCellContent(selectedCell.Item);
        if (cellContent is not TextBlock block) return;
        if (!int.TryParse(block.Text, out var result)) return;
        switch (MainTabControl.SelectedIndex)
        {
            case 0:
                VisualizationSystemContext.Users.Remove(
                    VisualizationSystemContext.Users.First(u => u.Id == result));
                break;
            case 1:
                VisualizationSystemContext.Visualizations.Remove(
                    VisualizationSystemContext.Visualizations.First(u => u.Id == result));
                break;
            case 2:
                VisualizationSystemContext.Dashboards.Remove(
                    VisualizationSystemContext.Dashboards.First(u => u.Id == result));
                break;
            case 3:
                VisualizationSystemContext.DataSources.Remove(
                    VisualizationSystemContext.DataSources.First(u => u.Id == result));
                break;
        }
            
        try
        {
            VisualizationSystemContext.SaveChanges();
            MessageBox.Show("Запись удалена");
            MainWindow_OnLoaded(sender, null);
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message);
        }
    }
}