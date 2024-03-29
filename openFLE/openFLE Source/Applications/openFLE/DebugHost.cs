﻿//*********************************************************************************************************************
// DebugHost.cs
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
//
// This software incorporates work covered by the following copyright and permission notice: 
//
// •    TVA Code Library 4.0.4.3 - Tennessee Valley Authority, tvainfo@tva.gov
//      No copyright is claimed pursuant to 17 USC § 105. All Other Rights Reserved.
//
//      Licensed under TVA Custom License based on NASA Open Source Agreement (TVA Custom NOSA); 
//      you may not use TVA Code Library except in compliance with the TVA Custom NOSA. You may  
//      obtain a copy of the TVA Custom NOSA at http://tvacodelibrary.codeplex.com/license.
//
//      TVA Code Library is provided by the copyright holders and contributors "as is" and any express 
//      or implied warranties, including, but not limited to, the implied warranties of merchantability 
//      and fitness for a particular purpose are disclaimed.
//
//*********************************************************************************************************************
//
//  Code Modification History:
//  -------------------------------------------------------------------------------------------------------------------
//  09/10/2012 - Stephen C. Wills, Grid Protection Alliance
//       Generated original version of source code.
//
//*********************************************************************************************************************
//
using System;
using System.Windows.Forms;
using TVA.Reflection;

namespace openFLE
{
    public partial class DebugHost : Form
    {
        #region [ Members ]

        // Fields
        private string m_productName;

        #endregion

        #region [ Constructors ]

        public DebugHost()
        {
            InitializeComponent();
        }

        #endregion

        #region [ Methods ]

        private void DebugHost_Load(object sender, EventArgs e)
        {
            // Initialize text.
            m_productName = AssemblyInfo.EntryAssembly.Title;
            this.Text = string.Format(this.Text, m_productName);
            m_notifyIcon.Text = string.Format(m_notifyIcon.Text, m_productName);
            LabelNotice.Text = string.Format(LabelNotice.Text, m_productName);
            m_exitToolStripMenuItem.Text = string.Format(m_exitToolStripMenuItem.Text, m_productName);

            // Minimize the window.
            this.WindowState = FormWindowState.Minimized;

            // Start the windows service.
            m_serviceHost.StartDebugging(Environment.CommandLine.Split(' '));
        }

        private void DebugHost_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show(string.Format("Are you sure you want to stop {0} windows service? ",
                m_productName), "Stop Service", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // Stop the windows service.
                m_serviceHost.StopDebugging();
            }
            else
            {
                // Stop the application from exiting.
                e.Cancel = true;
            }
        }

        private void DebugHost_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                // Don't show the window in taskbar when minimized.
                this.ShowInTaskbar = false;
            }
        }

        private void ShowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Show the window in taskbar the in normal mode (visible).
            this.ShowInTaskbar = true;
            this.WindowState = FormWindowState.Normal;
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Close this window which will cause the application to exit.
            this.Close();
        }

        #endregion
    }
}
