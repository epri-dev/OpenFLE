//*********************************************************************************************************************
// MainWindow.xaml.cs
//
// Copyright 2012 ELECTRIC POWER RESEARCH INSTITUTE, INC. All rights reserved.
//
// openFLE ("this software") is licensed under BSD 3-Clause license.
//
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the 
// following conditions are met:
//
// •    Redistributions of source code must retain the above copyright  notice, this list of conditions and 
//      the following disclaimer.
//
// •    Redistributions in binary form must reproduce the above copyright notice, this list of conditions and 
//      the following disclaimer in the documentation and/or other materials provided with the distribution.
//
// •    Neither the name of the Electric Power Research Institute, Inc. (“EPRI”) nor the names of its contributors 
//      may be used to endorse or promote products derived from this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, 
// INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE 
// DISCLAIMED. IN NO EVENT SHALL EPRI BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL 
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; 
// OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, 
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
// POSSIBILITY OF SUCH DAMAGE.
//
//*********************************************************************************************************************
//
//  Code Modification History:
//  -------------------------------------------------------------------------------------------------------------------
//  07/19/2012 - Stephen C. Wills, Grid Protection Alliance
//       Generated original version of source code.
//
//  11/19/2012 - Tina M Daniel, EPRI
//       Commented out display of license/splash screen
//
//*********************************************************************************************************************
//
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace openFLEManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region [ Constructors ]

        public MainWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region [ Properties ]

        public AboutWindow AboutWindow
        {
            get
            {
                return Resources["AboutWindow"] as AboutWindow;
            }
        }

        public ViewModel ViewModel
        {
            get
            {
                return Resources["ViewModel"] as ViewModel;
            }
        }

        #endregion

        #region [ Methods ]

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            #region licenseSplashScreen

            // 11-19-12, TMD: uncomment this code to show a license;
            // Replace text of "openFLE Splash Screen.rtf" to change license text

            //LicenseWindow licenseWindow;
            //bool? licenseResult;

            //licenseWindow = new LicenseWindow();
            //licenseWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            //licenseWindow.Owner = this;
            //licenseResult = licenseWindow.ShowDialog();

            //if (licenseResult.HasValue && licenseResult.Value)
            //{
            //    ViewModel.Load();
            //}
            //else
            //{
            //    Close();
            //}
            
            #endregion

            ViewModel.Load();
        }

        private void MainWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            AboutWindow.Close();
        }

        private void DropFolderBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            string dropFolder = ViewModel.DropFolder;
            BrowseFolders(ref dropFolder);
            ViewModel.DropFolder = dropFolder;
        }

        private void ResultsFolderBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            string resultsFolder = ViewModel.ResultsFolder;
            BrowseFolders(ref resultsFolder);
            ViewModel.ResultsFolder = resultsFolder;
        }

        private void BrowseFolders(ref string folder)
        {
            System.Windows.Forms.FolderBrowserDialog folderDialog = new System.Windows.Forms.FolderBrowserDialog();
            folderDialog.SelectedPath = folder;

            if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                folder = folderDialog.SelectedPath;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel.Save();
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to save configuration file. This may require elevated privileges.",
                    "openFLE Manager Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ConsoleButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.LaunchConsoleMonitor();
        }

        private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow.Owner = this;
            AboutWindow.ShowDialog();
        }

        private void AboutWindow_Closing(object sender, CancelEventArgs e)
        {
            if (IsLoaded)
            {
                e.Cancel = true;
                AboutWindow.Visibility = Visibility.Hidden;
            }
        }

        #endregion
    }
}
