using System;
using System.Windows;
using SchoolProtection.Core.Security;
using SchoolProtection.Core.Settings;
using SchoolProtection.Infrastructure.Services;

namespace SchoolProtection.Admin
{
    public partial class MainWindow : Window
    {
        private readonly RestoreEngine _restoreEngine;
        private readonly LoggerService _loggerService;
        private readonly ProtectionConfig _config;

        public MainWindow()
        {
            InitializeComponent();
            
            _config = new ProtectionConfig();
            _restoreEngine = new RestoreEngine(_config.CleanStateFilePath);
            _loggerService = new LoggerService(_config.LogFilePath);

            _loggerService.Log("Application started");
            RefreshLogs_Click(null, null);
        }

        private void SaveCleanState_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _restoreEngine.SaveCleanState();
                _loggerService.Log("Clean state saved successfully");
                MessageBox.Show("Clean state saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                RefreshLogs_Click(null, null);
            }
            catch (Exception ex)
            {
                _loggerService.Log($"Error saving clean state: {ex.Message}");
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RestoreNow_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _loggerService.Log("Restore started by teacher");
                _restoreEngine.RestoreToCleanState();
                _loggerService.Log("Restore completed successfully");
                MessageBox.Show("Desktop restored to clean state!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                RefreshLogs_Click(null, null);
            }
            catch (Exception ex)
            {
                _loggerService.Log($"Error during restore: {ex.Message}");
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EnableStudentMode_Click(object sender, RoutedEventArgs e)
        {
            _config.StudentModeEnabled = true;
            _loggerService.Log("Student Mode enabled");
            MessageBox.Show("Student Mode is now enabled.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            RefreshLogs_Click(null, null);
        }

        private void DisableStudentMode_Click(object sender, RoutedEventArgs e)
        {
            _config.StudentModeEnabled = false;
            _loggerService.Log("Student Mode disabled");
            MessageBox.Show("Student Mode is now disabled.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            RefreshLogs_Click(null, null);
        }

        private void RefreshLogs_Click(object sender, RoutedEventArgs e)
        {
            LogsTextBox.Text = _loggerService.GetLogs();
            LogsTextBox.ScrollToEnd();
        }
    }
}
