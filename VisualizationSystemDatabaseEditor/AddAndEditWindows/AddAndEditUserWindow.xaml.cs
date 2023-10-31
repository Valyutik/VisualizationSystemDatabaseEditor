using System;
using System.Linq;
using System.Windows;
using VisualizationSystemDatabaseEditor.Database;
using VisualizationSystemDatabaseEditor.Database.Models;

namespace VisualizationSystemDatabaseEditor.AddAndEditWindows;

public partial class AddAndEditUserWindow
{
    private readonly MainWindow _mainWindow;
    private readonly bool _isEditor;
    private readonly VisualizationSystemContext _visualizationSystemContext;
    private readonly int _indexUser;
    
    public AddAndEditUserWindow(MainWindow mainWindow, bool isEditor, int indexUser)
    {
        InitializeComponent();
        _mainWindow = mainWindow;
        _isEditor = isEditor;
        _indexUser = indexUser;
        _visualizationSystemContext = _mainWindow.VisualizationSystemContext;
    }
    
    public AddAndEditUserWindow(MainWindow mainWindow, bool isEditor)
    {
        InitializeComponent();
        _mainWindow = mainWindow;
        _isEditor = isEditor;
        _indexUser = 0;
        _visualizationSystemContext = _mainWindow.VisualizationSystemContext;
    }

    private void AddAndEditUserWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        RoleComboBox.ItemsSource = _visualizationSystemContext.Roles.ToList();
        if (!_isEditor) return;
        CaptionTextBlock.Text = "Редактирование пользователя";
        AddButton.Content = "Сохранить";
        var user = _visualizationSystemContext.Users.First(u => u.Id == _indexUser);;
        NameTextBox.Text = user.Name;
        EmailTextBox.Text = user.Email;
        PhoneNumberTextBox.Text = user.PhoneNumber;
        PasswordTextBox.Text = user.Password;
        RoleComboBox.SelectedIndex = user.RoleId - 1;
    }

    private void AddAndEditUserWindow_OnClosed(object sender, EventArgs e)
    {
        ReturnPreviousWindow(sender, null);
    }
    
    private void ReturnPreviousWindow(object? sender, RoutedEventArgs? e)
    {
        _mainWindow.Show();
        _mainWindow.MainWindow_OnLoaded(this, null);
        Close();
    }

    private void AddButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (!_isEditor)
        {
            var user = new User
            {
                Name = NameTextBox.Text,
                Email = EmailTextBox.Text,
                PhoneNumber = PhoneNumberTextBox.Text,
                Password = PasswordTextBox.Text,
                RoleId = RoleComboBox.SelectedIndex + 1
            };
            _visualizationSystemContext.Users.Add(user);
            try
            {
                _visualizationSystemContext.SaveChanges();
                MessageBox.Show($"Пользователь {user.Name} добавлен!");
                ReturnPreviousWindow(sender, null);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
        else
        {
            var user = _visualizationSystemContext.Users.First(u => u.Id == _indexUser);
            user.Name = NameTextBox.Text;
            user.Email = EmailTextBox.Text;
            user.PhoneNumber = PhoneNumberTextBox.Text;
            user.Password = PasswordTextBox.Text;
            user.RoleId = RoleComboBox.SelectedIndex + 1;
            try
            {
                _visualizationSystemContext.SaveChanges();
                MessageBox.Show($"Пользователь {user.Name} отредактирован!");
                ReturnPreviousWindow(sender, null);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
    }
}