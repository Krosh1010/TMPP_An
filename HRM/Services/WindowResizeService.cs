using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace HRM.Services
{
    public class WindowResizeService
    {
        private readonly TextBlock _titleText;
        private readonly StackPanel _actionButtonsPanel;
        private readonly Border _filterPanel;
        private readonly Button _showAddButton;
        private readonly Button _undoButton;
        private readonly Button _logoutButton;

        public WindowResizeService(
            TextBlock titleText,
            StackPanel actionButtonsPanel,
            Border filterPanel,
            Button showAddButton,
            Button undoButton,
            Button logoutButton)
        {
            _titleText = titleText;
            _actionButtonsPanel = actionButtonsPanel;
            _filterPanel = filterPanel;
            _showAddButton = showAddButton;
            _undoButton = undoButton;
            _logoutButton = logoutButton;
        }

        public void HandleWindowResize(SizeChangedEventArgs e)
        {
            if (e.NewSize.Width < 785)
            {
                SetCompactMode();
            }
            else
            {
                SetNormalMode();
            }
        }

        private void SetCompactMode()
        {
            _titleText.Text = "Employees";
            _titleText.Width = 10;

            _actionButtonsPanel.Orientation = Orientation.Vertical;
            _actionButtonsPanel.HorizontalAlignment = HorizontalAlignment.Left;

            _showAddButton.Content = "+";
            _undoButton.Content = "↩";
            _logoutButton.Content = "🚪";

            _showAddButton.Margin = new Thickness(0, 0, 0, 5);
            _undoButton.Margin = new Thickness(0, 0, 0, 5);
            _logoutButton.Margin = new Thickness(0);

            _filterPanel.MinWidth = 300;
            _filterPanel.MaxWidth = 350;
        }

        private void SetNormalMode()
        {
            _titleText.Text = "Employee Management";
            _titleText.Width = double.NaN; // Reset to auto

            _actionButtonsPanel.Orientation = Orientation.Horizontal;
            _actionButtonsPanel.HorizontalAlignment = HorizontalAlignment.Right;

            _showAddButton.Content = "Adaugă";
            _undoButton.Content = "Undo";
            _logoutButton.Content = "Logout";

            _showAddButton.Margin = new Thickness(0, 0, 5, 0);
            _undoButton.Margin = new Thickness(0, 0, 5, 0);
            _logoutButton.Margin = new Thickness(0);

            _filterPanel.MinWidth = 300;
            _filterPanel.MaxWidth = 500;
        }
    }
}
