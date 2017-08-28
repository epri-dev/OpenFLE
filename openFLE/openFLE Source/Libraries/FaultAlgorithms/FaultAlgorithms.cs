//*********************************************************************************************************************
// FaultAlgorithms.cs
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
//  05/23/2012 - J. Ritchie Carroll, Grid Protection Alliance
//       Generated original version of source code.
//
//*********************************************************************************************************************
//
using System;
using System.Collections.Generic;
using System.Linq;
using TVA;

namespace FaultAlgorithms
{
    /// <summary>
    /// Function signature for fault detection algorithms.
    /// </summary>
    /// <param name="faultDataSet">Full collection of voltage, current, and cycle data.</param>
    /// <param name="parameters">Custom parameters for algorithm.</param>
    /// <returns><c>true</c> if fault was found in dataset; otherwise <c>false</c>.</returns>
    public delegate bool FaultDetectionAlgorithm(FaultLocationDataSet faultDataSet, string parameters);

    /// <summary>
    /// Function signature for fault location algorithms.
    /// </summary>
    /// <param name="faultDataSet">Full collection of voltage, current, and cycle data.</param>
    /// <param name="parameters">Custom parameters for algorithm.</param>
    /// <returns>Percentage of distance down the line where fault occured.</returns>
    public delegate double FaultLocationAlgorithm(FaultLocationDataSet faultDataSet, string parameters);

    /// <summary>
    /// Attribute used to annotate fault detection algorithms.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class FaultDetectionAlgorithmAttribute : Attribute
    {
    }

    /// <summary>
    /// Attribute used to annotate fault location algorithms.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class FaultLocationAlgorithmAttribute : Attribute
    {
    }

    /// <summary>
    /// Defines built-in fault detection and location algorithms
    /// </summary>
    static class SimpleFaultAlgorithms
    {
        // Defines a simple fault detection algorithm
        [FaultDetectionAlgorithm]
        private static bool SimpleFaultDetectionAlgorithm(FaultLocationDataSet faultDataSet, string parameters)
        {
            List<int> faultedCycles = new List<int>();
            CycleData bestFaultCycle;

            double anFaultLimit = faultDataSet.Cycles[0].AN.I.RMS * 5.0D;
            double bnFaultLimit = faultDataSet.Cycles[0].BN.I.RMS * 5.0D;
            double cnFaultLimit = faultDataSet.Cycles[0].CN.I.RMS * 5.0D;

            bool anFaultCycle;
            bool bnFaultCycle;
            bool cnFaultCycle;

            for (int i = 0; i < faultDataSet.Cycles.Count; i++)
            {
                CycleData cycle = faultDataSet.Cycles[i];

                anFaultCycle = (cycle.AN.I.RMS >= anFaultLimit && cycle.AN.I.RMS >= 500);
                bnFaultCycle = (cycle.BN.I.RMS >= bnFaultLimit && cycle.BN.I.RMS >= 500);
                cnFaultCycle = (cycle.CN.I.RMS >= cnFaultLimit && cycle.CN.I.RMS >= 500);

                if (anFaultCycle || bnFaultCycle || cnFaultCycle)
                    faultedCycles.Add(i);
            }

            faultDataSet.FaultedCycles = faultedCycles;
            faultDataSet.FaultCalculationCycle = GetBestCycle(faultDataSet);
            bestFaultCycle = faultDataSet.Cycles[faultDataSet.FaultCalculationCycle];

            anFaultCycle = (bestFaultCycle.AN.I.RMS >= anFaultLimit && bestFaultCycle.AN.I.RMS >= 500);
            bnFaultCycle = (bestFaultCycle.BN.I.RMS >= bnFaultLimit && bestFaultCycle.BN.I.RMS >= 500);
            cnFaultCycle = (bestFaultCycle.CN.I.RMS >= cnFaultLimit && bestFaultCycle.CN.I.RMS >= 500);

            if (anFaultCycle && bnFaultCycle && cnFaultCycle)
                faultDataSet.FaultType = FaultType.ABC;
            else if (anFaultCycle && bnFaultCycle)
                faultDataSet.FaultType = FaultType.AB;
            else if (bnFaultCycle && cnFaultCycle)
                faultDataSet.FaultType = FaultType.BC;
            else if (cnFaultCycle && anFaultCycle)
                faultDataSet.FaultType = FaultType.CA;
            else if (anFaultCycle)
                faultDataSet.FaultType = FaultType.AN;
            else if (bnFaultCycle)
                faultDataSet.FaultType = FaultType.BN;
            else if (cnFaultCycle)
                faultDataSet.FaultType = FaultType.CN;

            return faultedCycles.Count > 0;
        }

        // Defines a simple fault location algorithm
        [FaultLocationAlgorithm]
        private static double SimpleFaultLocationAlgorithm(FaultLocationDataSet faultDataSet, string parameters)
        {
            CycleData bestFaultCycle = faultDataSet.Cycles[faultDataSet.FaultCalculationCycle];
            ComplexNumber v, i, z;

            switch (faultDataSet.FaultType)
            {
                case FaultType.AN:
                    v = bestFaultCycle.AN.V.Complex;
                    i = bestFaultCycle.AN.I.Complex;
                    z = faultDataSet.Zs;
                    break;

                case FaultType.BN:
                    v = bestFaultCycle.BN.V.Complex;
                    i = bestFaultCycle.BN.I.Complex;
                    z = faultDataSet.Zs;
                    break;

                case FaultType.CN:
                    v = bestFaultCycle.CN.V.Complex;
                    i = bestFaultCycle.CN.I.Complex;
                    z = faultDataSet.Zs;
                    break;

                case FaultType.AB:
                    v = bestFaultCycle.AN.V.Complex - bestFaultCycle.BN.V.Complex;
                    i = bestFaultCycle.AN.I.Complex - bestFaultCycle.BN.I.Complex;
                    z = faultDataSet.Z1;
                    break;

                case FaultType.BC:
                    v = bestFaultCycle.BN.V.Complex - bestFaultCycle.CN.V.Complex;
                    i = bestFaultCycle.BN.I.Complex - bestFaultCycle.CN.I.Complex;
                    z = faultDataSet.Z1;
                    break;

                case FaultType.CA:
                    v = bestFaultCycle.CN.V.Complex - bestFaultCycle.AN.V.Complex;
                    i = bestFaultCycle.CN.I.Complex - bestFaultCycle.AN.I.Complex;
                    z = faultDataSet.Z1;
                    break;

                case FaultType.ABC:
                    Conductor[] conductors = { bestFaultCycle.AN, bestFaultCycle.BN, bestFaultCycle.CN };
                    Conductor bestConductor = conductors.OrderBy(conductor => conductor.IError).First();

                    v = bestConductor.V.Complex;
                    i = bestConductor.I.Complex;
                    z = faultDataSet.Z1;
                    break;

                default:
                    throw new ArgumentOutOfRangeException("Unknown fault type: " + faultDataSet.FaultType);
            }

            // Calculate fault distance.
            faultDataSet.FaultDistance = faultDataSet.LineDistance * (v.Magnitude / i.Magnitude) / z.Magnitude;

            return faultDataSet.FaultDistance;
        }

        private static int GetBestCycle(FaultLocationDataSet faultDataSet)
        {
            return faultDataSet.Cycles
                .Select((cycle, index) => new Tuple<CycleData, int>(cycle, index))
                .Where(tuple => faultDataSet.FaultedCycles.Contains(tuple.Item2))
                .OrderByDescending(tuple => tuple.Item1.AN.I.RMS + tuple.Item1.BN.I.RMS + tuple.Item1.CN.I.RMS)
                .Select(tuple => tuple.Item2)
                .First();
        }
    }
}
