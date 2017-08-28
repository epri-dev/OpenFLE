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
//*********************************************************************************************************************
//
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Documents;

namespace Setup
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LicenseTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            byte[] licenseData;
            TextRange textRange;
            MemoryStream licenseStream;

            licenseData = Properties.Resources.License;
            textRange = new TextRange(LicenseTextBox.Document.ContentStart, LicenseTextBox.Document.ContentEnd);

            using (licenseStream = new MemoryStream(licenseData))
            {
                textRange.Load(licenseStream, DataFormats.Rtf);
            }
        }

        private void AgreeButton_Click(object sender, RoutedEventArgs e)
        {
            Process msi;

            try
            {
                Visibility = Visibility.Hidden;
                msi = Process.Start("openFLESetup.msi");

                if ((object)msi != null)
                    msi.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not start openFLESetup.msi: " + ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            Close();
        }

        private void DeclineButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
