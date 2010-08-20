﻿//******************************************************************************************************
//  DebugHost.cs - Gbtc
//
//  Copyright © 2010, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the Eclipse Public License -v 1.0 (the "License"); you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://www.opensource.org/licenses/eclipse-1.0.php
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  05/15/2009 - J. Ritchie Carroll
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Diagnostics;
using System.Windows.Forms;
using TVA.IO;
using TVA.Reflection;

namespace openPDC
{
    public partial class DebugHost : Form
    {
        #region [ Members ]

        // Fields
        private string m_productName;
        private Process m_remoteConsole;

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
            // Start remote console sesstion
            m_remoteConsole = Process.Start(FilePath.GetAbsolutePath("openPDCConsole.exe"));

            // Initialize text.
            m_productName = AssemblyInfo.EntryAssembly.Title;
            this.Text = string.Format(this.Text, m_productName);
            notifyIcon.Text = string.Format(notifyIcon.Text, m_productName);
            LabelNotice.Text = string.Format(LabelNotice.Text, m_productName);
            exitToolStripMenuItem.Text = string.Format(exitToolStripMenuItem.Text, m_productName);

            // Minimize the window.
            this.WindowState = FormWindowState.Minimized;

            // Start the windows service.
            serviceHost.StartDebugging(Environment.CommandLine.Split(' '));
        }

        private void DebugHost_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show(string.Format("Are you sure you want to stop {0} windows service? ",
                m_productName), "Stop Service", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // Stop the windows service.
                serviceHost.StopDebugging();

                // Close remote console session
                if (m_remoteConsole != null && !m_remoteConsole.HasExited)
                    m_remoteConsole.Kill();
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

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Show the window in taskbar the in normal mode (visible).
            this.ShowInTaskbar = true;
            this.WindowState = FormWindowState.Normal;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Close this window which will cause the application to exit.
            this.Close();
        }

        #endregion
    }
}