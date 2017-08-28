//*********************************************************************************************************************
// MeasurementDataSet.cs
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
//  05/23/2012 - J. Ritchie Carroll, Grid Protection Alliance
//       Generated original version of source code.
//
//*********************************************************************************************************************
//
using System;
using System.IO;

namespace FaultAlgorithms
{
    /// <summary>
    /// Represents a set of 3-phase line-to-neutral and line-to-line time-domain power data.
    /// </summary>
    public class MeasurementDataSet
    {
        #region [ Members ]

        // Constants

        private const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss.fff";

        // Fields

        /// <summary>
        /// Line-to-neutral A-phase data.
        /// </summary>
        public MeasurementData AN;

        /// <summary>
        /// Line-to-neutral B-phase data.
        /// </summary>
        public MeasurementData BN;

        /// <summary>
        /// Line-to-neutral C-phase data.
        /// </summary>
        public MeasurementData CN;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Creates a new <see cref="MeasurementDataSet"/>.
        /// </summary>
        public MeasurementDataSet()
        {
            AN = new MeasurementData();
            BN = new MeasurementData();
            CN = new MeasurementData();
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Writes all measurement data to a CSV file. 
        /// </summary>
        /// <param name="fileName">Export file name.</param>
        public void ExportVoltageDataToCSV(string fileName)
        {
            const string Header = "Time,AN,BN,CN,AB,BC,CA";

            using (FileStream fileStream = File.OpenWrite(fileName))
            {
                using (TextWriter fileWriter = new StreamWriter(fileStream))
                {
                    // Write the CSV header to the file
                    fileWriter.WriteLine(Header);

                    // Write the data to the file
                    for (int i = 0; i < AN.Times.Length; i++)
                    {
                        string time = new DateTime(AN.Times[i]).ToString(DateTimeFormat);

                        double an = AN.Values[i];
                        double bn = BN.Values[i];
                        double cn = CN.Values[i];

                        fileWriter.Write("{0},{1},{2},{3},", time, an, bn, cn);
                        fileWriter.WriteLine("{0},{1},{2}", an - bn, bn - cn, cn - an);
                    }
                }
            }
        }

        public void ExportCurrentDataToCSV(string fileName)
        {
            const string Header = "Time,AN,BN,CN";

            using (FileStream fileStream = File.OpenWrite(fileName))
            {
                using (TextWriter fileWriter = new StreamWriter(fileStream))
                {
                    // Write the CSV header to the file
                    fileWriter.WriteLine(Header);

                    // Write the data to the file
                    for (int i = 0; i < AN.Times.Length; i++)
                    {
                        string time = new DateTime(AN.Times[i]).ToString(DateTimeFormat);

                        double an = AN.Values[i];
                        double bn = BN.Values[i];
                        double cn = CN.Values[i];

                        fileWriter.WriteLine("{0},{1},{2},{3}", time, an, bn, cn);
                    }
                }
            }
        }

        #endregion

        #region [ Static ]

        // Static Methods

        public static void ExportToCSV(string fileName, MeasurementDataSet voltageData, MeasurementDataSet currentData)
        {
            const string Header = "Time,AN V,BN V,CN V,AB V,BC V,CA V,AN I,BN I,CN I";

            using (FileStream fileStream = File.OpenWrite(fileName))
            {
                using (TextWriter fileWriter = new StreamWriter(fileStream))
                {
                    // Write the CSV header to the file
                    fileWriter.WriteLine(Header);

                    // Write the data to the file
                    for (int i = 0; i < voltageData.AN.Times.Length; i++)
                    {
                        string time = new DateTime(voltageData.AN.Times[i]).ToString(DateTimeFormat);

                        double vAN = voltageData.AN.Values[i];
                        double vBN = voltageData.BN.Values[i];
                        double vCN = voltageData.CN.Values[i];

                        double iAN = currentData.AN.Values[i];
                        double iBN = currentData.BN.Values[i];
                        double iCN = currentData.CN.Values[i];

                        fileWriter.Write("{0},{1},{2},{3},", time, vAN, vBN, vCN);
                        fileWriter.Write("{0},{1},{2},", vAN - vBN, vBN - vCN, vCN - vAN);
                        fileWriter.WriteLine("{0},{1},{2}", iAN, iBN, iCN);
                    }
                }
            }
        }

        #endregion
    }
}