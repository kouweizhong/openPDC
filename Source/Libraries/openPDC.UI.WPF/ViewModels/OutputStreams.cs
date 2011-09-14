﻿//******************************************************************************************************
//  OutputStreams.cs - Gbtc
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
//  09/09/2011 - Mehulbhai Thakkar
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using openPDCManager.UI.DataModels;
using TimeSeriesFramework.UI;
using TimeSeriesFramework.UI.Commands;
using TimeSeriesFramework.UI.DataModels;

namespace openPDCManager.UI.ViewModels
{
    internal class OutputStreams : PagedViewModelBase<OutputStream, int>
    {
        #region [ Members ]

        private Dictionary<Guid, string> m_nodelookupList;
        private Dictionary<string, string> m_downSamplingMethodLookupList;
        private Dictionary<string, string> m_dataFormatLookupList;
        private Dictionary<string, string> m_coordinateFormatLookupList;
        private Dictionary<int, string> m_typeLookupList;
        private RelayCommand m_initializeCommand;
        private RelayCommand m_copyCommand;
        private RelayCommand m_updateConfigurationCommand;
        private Dictionary<Guid, string> m_nodeLookupList;
        private string m_runtimeID;

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets <see cref="ICommand"/> object for update operation.
        /// </summary>
        public ICommand UpdateConfigurationCommand
        {
            get
            {
                if (m_updateConfigurationCommand == null)
                {
                    m_updateConfigurationCommand = new RelayCommand(UpdateConfiguration);
                }
                return m_updateConfigurationCommand;
            }
        }

        /// <summary>
        /// Gets <see cref="ICommand"/> object for copy operation.
        /// </summary>
        public ICommand CopyCommand
        {
            get
            {
                if (m_copyCommand == null)
                {
                    m_copyCommand = new RelayCommand(MakeCopy);
                }

                return m_copyCommand;
            }
        }

        /// <summary>
        /// Gets <see cref="ICommand"/> object for Initialize operation.
        /// </summary>
        public ICommand InitializeCommand
        {
            get
            {
                if (m_initializeCommand == null)
                {
                    m_initializeCommand = new RelayCommand(Initialize);
                }
                return m_initializeCommand;
            }
        }

        /// <summary>
        /// Gets <see cref="Dictionary{T1,T2}"/> type collection of <see cref="Node"/> defined in the database.
        /// </summary>
        public Dictionary<Guid, string> NodeLookupList
        {
            get
            {
                return m_nodelookupList;
            }
        }

        /// <summary>
        /// Gets <see cref="Dictionary{T1,T2}"/> type collection of <see cref="OutputStream"/> types.
        /// </summary>
        public Dictionary<int, string> TypeLookupList
        {
            get
            {
                return m_typeLookupList;
            }
        }

        /// <summary>
        /// Gets <see cref="Dictionary{T1,T2}"/> type collection of <see cref="OutputStream"/> down sampling methods.
        /// </summary>
        public Dictionary<string, string> DownSamplingMethodLookupList
        {
            get
            {
                return m_downSamplingMethodLookupList;
            }
        }

        /// <summary>
        /// Gets <see cref="Dictionary{T1,T2}"/> type collection of <see cref="OutputStream"/> data formats.
        /// </summary>
        public Dictionary<string, string> DataFormatLookupList
        {
            get
            {
                return m_dataFormatLookupList;
            }
        }

        /// <summary>
        /// Gets <see cref="Dictionary{T1,T2}"/> type collection of <see cref="OutputStream"/> coordinate formats.
        /// </summary>
        public Dictionary<string, string> CoordinateFormatLookupList
        {
            get
            {
                return m_coordinateFormatLookupList;
            }
        }

        /// <summary>
        /// Gets flag that determines if <see cref="PagedViewModelBase{T1, T2}.CurrentItem"/> is a new record.
        /// </summary>
        public override bool IsNewRecord
        {
            get
            {
                return CurrentItem.ID == 0;
            }
        }

        /// <summary>
        /// Gets or sets <see cref="OutputStream"/> RuntimeID.
        /// </summary>
        public string RuntimeID
        {
            get
            {
                return m_runtimeID;
            }
            set
            {
                m_runtimeID = value;
                OnPropertyChanged("RuntimeID");
            }
        }

        #endregion

        #region [ Constructor ]

        public OutputStreams(int itemsPerPage, bool autoSave = true)
            : base(itemsPerPage, autoSave)
        {
            m_nodelookupList = Node.GetLookupList(null);

            m_typeLookupList = new Dictionary<int, string>();
            m_typeLookupList.Add(0, "IEEE C37.118");
            m_typeLookupList.Add(1, "BPA");

            m_downSamplingMethodLookupList = new Dictionary<string, string>();
            m_downSamplingMethodLookupList.Add("LastReceived", "LastReceived");
            m_downSamplingMethodLookupList.Add("Closest", "Closest");
            m_downSamplingMethodLookupList.Add("Filtered", "Filtered");
            m_downSamplingMethodLookupList.Add("BestQuality", "BestQuality");

            m_dataFormatLookupList = new Dictionary<string, string>();
            m_dataFormatLookupList.Add("FloatingPoint", "FloatingPoint");
            m_dataFormatLookupList.Add("FixedInteger", "FixedInteger");

            m_coordinateFormatLookupList = new Dictionary<string, string>();
            m_coordinateFormatLookupList.Add("Polar", "Polar");
            m_coordinateFormatLookupList.Add("Rectangular", "Rectangular");
        }

        #endregion

        #region [ Methods ]

        public override int GetCurrentItemKey()
        {
            return CurrentItem.ID;
        }

        public override string GetCurrentItemName()
        {
            return CurrentItem.Name;
        }

        public override void Clear()
        {
            base.Clear();
            CurrentItem.NodeID = m_nodelookupList.First().Key;
        }

        public override void Load()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                ItemsSource = OutputStream.Load(null, false);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    Popup(ex.Message + Environment.NewLine + "Inner Exception: " + ex.InnerException.Message, "Load " + DataModelName + " Exception:", MessageBoxImage.Error);
                else
                    Popup(ex.Message, "Load " + DataModelName + " Exception:", MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }

        private void Initialize()
        {
            if (Confirm("Do you want to send Initialize Command?", "Output Stream: " + CurrentItem.Acronym))
            {
                try
                {
                    var result = CommonFunctions.SendCommandToService("Initialize " + RuntimeID);
                    Popup(result, "", System.Windows.MessageBoxImage.Information);
                    CommonFunctions.SendCommandToService("Invoke 0 ReloadStatistics");
                }
                catch (Exception ex)
                {
                    CommonFunctions.LogException(null, "WPF.SendInitialize", ex);
                    Popup("Failed to Send Initialize Command", ex.Message, System.Windows.MessageBoxImage.Error);
                }
            }
        }

        private void MakeCopy(object parameter)
        {
            try
            {
                if (Confirm("Do you want to make a copy of " + CurrentItem.Acronym + " output stream?", "This will only copy the output stream configuration, not associated devices."))
                {
                    OutputStream newOutputStream = new OutputStream()
                    {
                        ID = 0, // Set it to zero so it will be inserted instead of updated.
                        Enabled = false,
                        Acronym = CurrentItem.Acronym,
                        Name = "Copy of " + CurrentItem.Name,
                        AllowPreemptivePublishing = CurrentItem.AllowPreemptivePublishing,
                        AllowSortsByArrival = CurrentItem.AllowSortsByArrival,
                        AnalogScalingValue = CurrentItem.AnalogScalingValue,
                        AutoPublishConfigFrame = CurrentItem.AutoPublishConfigFrame,
                        AutoStartDataChannel = CurrentItem.AutoStartDataChannel,
                        CommandChannel = CurrentItem.CommandChannel,
                        ConnectionString = CurrentItem.ConnectionString,
                        CoordinateFormat = CurrentItem.CoordinateFormat,
                        CurrentScalingValue = CurrentItem.CurrentScalingValue,
                        DataChannel = CurrentItem.DataChannel,
                        DataFormat = CurrentItem.DataFormat,
                        DigitalMaskValue = CurrentItem.DigitalMaskValue,
                        DownSamplingMethod = CurrentItem.DownSamplingMethod,
                        FramesPerSecond = CurrentItem.FramesPerSecond,
                        IDCode = CurrentItem.IDCode,
                        IgnoreBadTimeStamps = CurrentItem.IgnoreBadTimeStamps,
                        LagTime = CurrentItem.LagTime,
                        LeadTime = CurrentItem.LeadTime,
                        LoadOrder = CurrentItem.LoadOrder,
                        NodeID = CurrentItem.NodeID,
                        NominalFrequency = CurrentItem.NominalFrequency,
                        PerformTimestampReasonabilityCheck = CurrentItem.PerformTimestampReasonabilityCheck,
                        TimeResolution = CurrentItem.TimeResolution,
                        Type = CurrentItem.Type,
                        UseLocalClockAsRealTime = CurrentItem.UseLocalClockAsRealTime,
                        VoltageScalingValue = CurrentItem.VoltageScalingValue
                    };

                    string originalAcronym = newOutputStream.Acronym;
                    int i = 1;
                    do
                    {
                        newOutputStream.Acronym = originalAcronym + i.ToString();
                        i++;
                    }
                    while (OutputStream.GetOutputStreamByAcronym(null, newOutputStream.Acronym) != null);

                    CurrentItem = newOutputStream;
                }
            }
            catch (Exception ex)
            {
                Popup("Failed to copy output stream.", ex.Message, System.Windows.MessageBoxImage.Error);
            }
        }

        private void UpdateConfiguration()
        {
            try
            {
                if (Confirm("Do you want to update configuration?", ""))
                {
                    string runtimeID = CommonFunctions.GetRuntimeID("OutputStream", CurrentItem.ID);
                    string result = CommonFunctions.SendCommandToService("reloadconfig");
                    result = CommonFunctions.SendCommandToService("Invoke " + runtimeID + " UpdateConfiguration");
                    Popup(result, "", MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                Popup("Failed to UpdateConfiguration", ex.Message, MessageBoxImage.Error);
            }
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == "CurrentItem")
            {
                if (CurrentItem == null)
                    RuntimeID = string.Empty;
                else
                    RuntimeID = TimeSeriesFramework.UI.CommonFunctions.GetRuntimeID("OutputStream", CurrentItem.ID);
            }
        }

        #endregion
    }
}
