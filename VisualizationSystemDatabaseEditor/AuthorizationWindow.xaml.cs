using System;
using System.Linq;
using System.Windows;
using VisualizationSystemDatabaseEditor.Database;

namespace VisualizationSystemDatabaseEditor;

public partial class AuthorizationWindow
{
    private readonly VisualizationSystemContext _visualizationSystemContext;
    
    public AuthorizationWindow()
    {
        InitializeComponent();
        _visualizationSystemContext = new VisualizationSystemContext();
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        try
        {
            var users = _visualizationSystemContext.Users.Where(u => u.Name == LoginTextBox.Text &&
                                                                     u.Password == PasswordBox.Password &&
                                                                     u.Role.Name == "Администратор");
            if (users.Any())
            {
                var mainWindow = new MainWindow();
                mainWindow.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Неправильный логин или пароль");
            }
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message);
            throw;
        }
    }

    private void AuthorizationWindow_OnClosed(object? sender, EventArgs e)
    {
        _visualizationSystemContext.Dispose();
    }
}