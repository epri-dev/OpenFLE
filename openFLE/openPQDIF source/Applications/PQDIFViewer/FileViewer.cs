//*********************************************************************************************************************
// FileViewer.cs
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
//  04/02/2012 - Mehulbhai Thakkar, Grid Protection Alliance
//       Generated original version of source code.
//
//*********************************************************************************************************************
//
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using openPQDIF.Logical;
using TVA;

namespace PQDIFViewer
{
    /// <summary>
    /// Represents UI.
    /// </summary>
    public partial class FileViewer : Form
    {
        #region [ Members ]

        // Fields
        private List<ObservationRecord> m_observationRecords;

        #endregion

        #region [ Constructor ]

        /// <summary>
        /// Creates an instance of <see cref="FileViewer"/> class.
        /// </summary>
        public FileViewer()
        {
            InitializeComponent();
            m_observationRecords = new List<ObservationRecord>();
        }

        #endregion

        #region [ Methods ]

        private void Form_Resize(object sender, EventArgs e)
        {
            DataChart.Width = (int)(Width - ChannelListBox.Width - 50);
            DataChart.Height = (int)(Height - 80);
        }

        private void ButtonBrowse_Click(object sender, System.EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "PQDIF Files (*.pqd)|*.pqd";
            dialog.Title = "Browse PQDIF File";
            if (dialog.ShowDialog() == DialogResult.Cancel)
                return;

            if (!File.Exists(dialog.FileName))
                return;

            using (LogicalParser logicalParser = new LogicalParser(dialog.FileName))
            {
                logicalParser.Open();
                m_observationRecords.Clear();

                while (logicalParser.HasNextObservationRecord())
                    m_observationRecords.Add(logicalParser.NextObservationRecord());
            }

            ObservationListBox.Items.Clear();
            ChannelListBox.Items.Clear();
            DataChart.Series.Clear();

            ObservationListBox.Items.AddRange(m_observationRecords
                .Select((observation, index) => string.Format("[{0}] {1}", index, observation.StartTime.ToString("yyyy-MM-dd HH:mm:ss")))
                .Cast<object>()
                .ToArray());

            ObservationListBox.SelectedIndex = 0;

            Text = string.Format("PQDIF.NET Viewer - [{0}]", dialog.SafeFileName);
        }

        private void ObservationListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ObservationListBox.SelectedIndex < 0)
                return;

            ObservationRecord observation = m_observationRecords[ObservationListBox.SelectedIndex];

            ChannelListBox.Items.Clear();
            ChannelListBox.Items.AddRange(observation.ChannelInstances
                .Select((channel, index) => string.Format("[{0}] {1}", index, channel.Definition.ChannelName.ToNonNullString()))
                .Cast<object>()
                .ToArray());

            ChannelListBox.SelectedIndex = 0;
        }

        private void ChannelListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ChannelListBox.SelectedIndex < 0)
                return;

            ObservationRecord observation = m_observationRecords[ObservationListBox.SelectedIndex];
            ChannelInstance channel = observation.ChannelInstances[ChannelListBox.SelectedIndex];
            SeriesInstance xValueSeries = channel.SeriesInstances[0];
            SeriesInstance[] yValueSeries = channel.SeriesInstances.Skip(1).ToArray();

            IList<object> xValues = xValueSeries.OriginalValues;
            IList<object> yValues;

            string channelName = channel.Definition.ChannelName;
            string valueTypeName;
            Series chartSeries;

            DataChart.Series.Clear();

            foreach (SeriesInstance series in yValueSeries)
            {
                yValues = series.OriginalValues;

                valueTypeName = SeriesValueType.ToString(series.Definition.ValueTypeID);
                chartSeries = DataChart.Series.Add(string.Format("{0} ({1})", channelName, valueTypeName));
                chartSeries.ChartType = SeriesChartType.Line;

                for (int i = 0; i < xValues.Count && i < yValues.Count; i++)
                    chartSeries.Points.AddXY(xValues[i], yValues[i]);
            }

            RecalculateAxes();
        }

        private void RecalculateAxes()
        {
            double xMin, xMax;
            double yMin, yMax;
            double xScale, yScale;

            // Get the absolute smallest and largest x-values and y-values of the data points on the chart
            xMin = DataChart.Series.Select(series => series.Points.Select(point => point.XValue).Min()).Min();
            xMax = DataChart.Series.Select(series => series.Points.Select(point => point.XValue).Max()).Max();
            yMin = DataChart.Series.Select(series => series.Points.Select(point => point.YValues.Min()).Min()).Min();
            yMax = DataChart.Series.Select(series => series.Points.Select(point => point.YValues.Max()).Max()).Max();

            // Determine scale factor
            xScale = GetChartScale(xMax - xMin);
            yScale = GetChartScale(yMax - yMin);

            // Apply scale to make axis labels more readable
            xMin = xScale * Math.Floor(xMin / xScale);
            xMax = xScale * Math.Ceiling(xMax / xScale);
            yMin = yScale * Math.Floor(yMin / yScale);
            yMax = yScale * Math.Ceiling(yMax / yScale);

            // If the difference between the min an max values is
            // zero, add some space so we do not encounter an error
            if (xMax - xMin == 0.0D)
            {
                xMin -= 0.5D;
                xMax += 0.5D;
            }

            if (yMax - yMin == 0.0D)
            {
                yMin -= 0.5D;
                yMax += 0.5D;
            }

            // Set min, max, and interval of each axis
            DataChart.ChartAreas[0].AxisX.Minimum = xMin;
            DataChart.ChartAreas[0].AxisX.Maximum = xMax;
            DataChart.ChartAreas[0].AxisY.Minimum = yMin;
            DataChart.ChartAreas[0].AxisY.Maximum = yMax;
            DataChart.ChartAreas[0].AxisX.Interval = (xMax - xMin) / 10.0D;
            DataChart.ChartAreas[0].AxisY.Interval = (yMax - yMin) / 10.0D;
        }

        private double GetChartScale(double diff)
        {
            double abs = Math.Abs(diff);
            double log = Math.Log10(abs);
            return (diff == 0.0D) ? 1.0D : Math.Pow(10.0D, Math.Floor(log) - 1.0D);
        }

        #endregion
    }
}
