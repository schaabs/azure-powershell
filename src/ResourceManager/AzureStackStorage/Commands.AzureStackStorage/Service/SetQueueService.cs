﻿// ----------------------------------------------------------------------------------
//
// Copyright Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------


using System.Collections;
using System.Management.Automation;
using Microsoft.AzureStack.AzureConsistentStorage;
using Microsoft.AzureStack.AzureConsistentStorage.Models;

namespace Microsoft.AzureStack.AzureConsistentStorage.Commands
{
    /// <summary>
    /// 
    /// </summary>
    [Cmdlet(VerbsCommon.Set, Nouns.AdminQueueService, SupportsShouldProcess = true)]
    public sealed class SetQueueService : AdminCmdlet
    {
        /// <summary>
        /// Resource group name
        /// </summary>
        [Parameter(Position = 3, Mandatory = true, ValueFromPipelineByPropertyName = true)]
        [ValidateNotNull]
        public string ResourceGroupName { get; set; }

        /// <summary>
        ///     Farm Identifier
        /// </summary>
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, Position = 4)]
        [ValidateNotNull]
        public string FarmName { get; set; }

        /// <summary>
        /// CpuBasedKeepAliveThrottlingEnabled
        /// </summary>
        [Parameter]
        [SettingField]
        public bool? FrontEndCpuBasedKeepAliveThrottlingEnabled { get; set; }

        /// <summary>
        /// MemoryThrottlingEnabled
        /// </summary>
        [Parameter]
        [SettingField]
        public bool? FrontEndMemoryThrottlingEnabled { get; set; }


        protected override void Execute()
        {
            string confirmString;
            QueueServiceWritableSettings settings = Tools.ToSettingsObject<SetQueueService, QueueServiceWritableSettings>(this, out confirmString);
            if (ShouldProcess(
                Resources.SetServiceDescription.FormatInvariantCulture(Resources.QueueService, confirmString),
                Resources.SetServiceWarning.FormatInvariantCulture(Resources.QueueService, confirmString),
                Resources.ShouldProcessCaption))
            {
                QueueServiceGetResponse result = Client.QueueService.Patch(ResourceGroupName, FarmName, new QueueServicePatchParameters
                {
                    QueueService = new QueueServiceRequest
                    {
                        Settings = settings
                    }
                });
                WriteObject(new QueueServiceResponse(result.Resource));
            }
        }
    }
}