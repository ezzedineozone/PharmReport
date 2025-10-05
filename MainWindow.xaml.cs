using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using PharmReport.EF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using PharmReport.Models;
using PharmReport.Others.Converters;
using Microsoft.EntityFrameworkCore;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PharmReport
{

    public sealed partial class MainWindow : Window, System.ComponentModel.INotifyPropertyChanged
    {
        private bool _showArabicFields = false;

        public PharmReportProfile Profile { get; set; } = new PharmReportProfile();

        public bool ShowArabicFields
        {
            get => _showArabicFields;
            set
            {
                if (_showArabicFields == value) return;
                _showArabicFields = value;
                OnPropertyChanged(nameof(ShowArabicFields));
            }
        }

        public MainWindow(PharmReportProfile profile)
        {
            InitializeComponent();
            if (this.Content is FrameworkElement root)
            {
                root.DataContext = this;
            }
            this.Profile = profile ?? throw new ArgumentNullException(nameof(profile));
        }

        public event System.ComponentModel.PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));



        private void OnSidePanelButtonExited(object sender, PointerRoutedEventArgs e)
        {
            if (typeof(Microsoft.UI.Xaml.Controls.Button) == sender.GetType())
            {
                MainGrid.ColumnDefinitions[0].Width = new GridLength(80);
                AnimateSidePanel(80, 0.25);
                Button button = (Button)sender;
                SetSidePanelDescriptionsVisibility(SidePanel, Visibility.Collapsed);
            }
        }

        private void OnSidePanelButtonEntered(object sender, PointerRoutedEventArgs e)
        {
            if (typeof(Microsoft.UI.Xaml.Controls.Button) == sender.GetType())
            {
                MainGrid.ColumnDefinitions[0].Width = new GridLength(200);
                AnimateSidePanel(200,0.25);
                Button button = (Button)sender;
                SetSidePanelDescriptionsVisibility(SidePanel, Visibility.Visible);
            }
        }
        private void SetSidePanelDescriptionsVisibility(DependencyObject parent, Visibility visibility)
        {
            if (parent == null) return;

            int count = Microsoft.UI.Xaml.Media.VisualTreeHelper.GetChildrenCount(parent);

            for (int i = 0; i < count; i++)
            {
                var child = Microsoft.UI.Xaml.Media.VisualTreeHelper.GetChild(parent, i);
                if (child is Microsoft.UI.Xaml.Controls.TextBlock txtBlock
                    && txtBlock.Tag?.ToString() == "SidePanelButtonDescription")
                {
                    txtBlock.Visibility = visibility;
                }
                SetSidePanelDescriptionsVisibility(child, visibility);
            }
        }
        private void AnimateSidePanel(double targetWidth, double durationSeconds = 0.25)
        {
            var column = MainGrid.ColumnDefinitions[0];

            double fromWidth = column.ActualWidth;
            double toWidth = targetWidth;
            double elapsed = 0;

            void OnRendering(object sender, object e)
            {
                elapsed += 1.0 / 60.0;
                double t = Math.Min(elapsed / durationSeconds, 1);
                t = t * t * (3 - 2 * t);

                column.Width = new GridLength(fromWidth + (toWidth - fromWidth) * t);

                if (t >= 1)
                {
                    CompositionTarget.Rendering -= OnRendering;
                }
            }
            CompositionTarget.Rendering += OnRendering;
        }

    }


    public class FrmMainWindow : Window
    {
        private MainWindow _window;
        private PharmReportDBContext _dbContext;
        private PharmReportProfile _profile;

        public FrmMainWindow(MainWindow window, PharmReportDBContext dbContext)
        {
            // check if any are null and throw an exception
            if (window == null || dbContext == null)
            {
                throw new ArgumentNullException(nameof(window));
            }
            this._window = window;
            this._dbContext = dbContext;
            this._profile = window.Profile ?? new PharmReportProfile() { lastLogin = DateTime.Now };
            SetUpEventHandlers();
        }
        public void Show()
        {
            _window?.Activate();
        }



        private void SetUpEventHandlers()
        {
            if (_window is null)
            {
                throw new InvalidOperationException("Window is not initialized.");
            }
            _window.SavePharmacyButton.Click += SavePharmacyButton_Click;
        }

        private void SavePharmacyButton_Click(object sender, RoutedEventArgs e)
        {
            Upsert(_window.Profile.Pharmacy, _dbContext.Pharmacies);
            Upsert(_window.Profile.Pharmacist, _dbContext.Pharmacists);
            Upsert(_window.Profile, _dbContext.PharmReportProfiles);

            _dbContext.SaveChanges();
        }
        private void Upsert<TEntity>(TEntity entity, DbSet<TEntity> dbSet) where TEntity : class
        {
            var entry = _dbContext.Entry(entity);

            if (entry.IsKeySet)
            {
                dbSet.Update(entity);
            }
            else
            {
                dbSet.Add(entity);
            }
        }
    }
}
