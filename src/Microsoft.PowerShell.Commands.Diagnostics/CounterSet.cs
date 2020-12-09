// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;

namespace Microsoft.PowerShell.Commands.GetCounter
{
    public class CounterSet
    {
        internal CounterSet(string setName,
                            string machineName,
                            PerformanceCounterCategoryType categoryType,
                            string setHelp,
                            ref Dictionary<string, string[]> counterInstanceMapping)
        {
            _counterSetName = setName;
            if (machineName == null || machineName.Length == 0)
            {
                machineName = ".";
            }
            else
            {
                _machineName = machineName;
                if (!_machineName.StartsWith(@"\\", StringComparison.OrdinalIgnoreCase))
                {
                    _machineName = @"\\" + _machineName;
                }
            }

            _counterSetType = categoryType;
            _description = setHelp;
            _counterInstanceMapping = counterInstanceMapping;
        }

        public string CounterSetName
        {
            get
            {
                return _counterSetName;
            }
        }

        private readonly string _counterSetName = string.Empty;

        public string MachineName
        {
            get
            {
                return _machineName;
            }
        }

        private readonly string _machineName = ".";

        public PerformanceCounterCategoryType CounterSetType
        {
            get
            {
                return _counterSetType;
            }
        }

        private readonly PerformanceCounterCategoryType _counterSetType;

        public string Description
        {
            get
            {
                return _description;
            }
        }

        private readonly string _description = string.Empty;

        internal Dictionary<string, string[]> CounterInstanceMapping
        {
            get
            {
                return _counterInstanceMapping;
            }
        }

        private readonly Dictionary<string, string[]> _counterInstanceMapping;

        public StringCollection Paths
        {
            get
            {
                StringCollection retColl = new();
                foreach (string counterName in this.CounterInstanceMapping.Keys)
                {
                    string path;
                    if (CounterInstanceMapping[counterName].Length != 0)
                    {
                        path = (_machineName == ".") ?
                          ("\\" + _counterSetName + "(*)\\" + counterName) :
                          (_machineName + "\\" + _counterSetName + "(*)\\" + counterName);
                    }
                    else
                    {
                        path = (_machineName == ".") ?
                         ("\\" + _counterSetName + "\\" + counterName) :
                         (_machineName + "\\" + _counterSetName + "\\" + counterName);
                    }

                    retColl.Add(path);
                }

                return retColl;
            }
        }

        public StringCollection PathsWithInstances
        {
            get
            {
                StringCollection retColl = new();
                foreach (string counterName in CounterInstanceMapping.Keys)
                {
                    foreach (string instanceName in CounterInstanceMapping[counterName])
                    {
                        string path = (_machineName == ".") ?
                          ("\\" + _counterSetName + "(" + instanceName + ")\\" + counterName) :
                          (_machineName + "\\" + _counterSetName + "(" + instanceName + ")\\" + counterName);
                        retColl.Add(path);
                    }
                }

                return retColl;
            }
        }
    }
}
