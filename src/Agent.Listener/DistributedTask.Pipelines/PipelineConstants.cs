﻿// This source file is maintained in two repos. Edits must be made to both copies.
// Unit tests live in the vsts-agent repo on GitHub.
//
// Repo 1) VSO repo under DistributedTask/Sdk/Server/Expressions
// Repo 2) vsts-agent repo on GitHub under src/Agent.Listener/DistributedTask.Pipelines
//
// The style of this source file aims to follow VSO/DistributedTask conventions.

using System;

namespace Microsoft.TeamFoundation.DistributedTask.Orchestration.Server.Pipelines
{
    internal static class PipelineConstants
    {
        internal const String Condition = "condition";
        internal const String ContinueOnError = "continueOnError";
        internal const String Data = "data";
        internal const String Enabled = "enabled";
        internal const String Export = "export";
        internal const String Import = "import";
        internal const String Inputs = "inputs";
        internal const String Jobs = "jobs";
        internal const String Name = "name";
        internal const String Parameters = "parameters";
        internal const String Phase = "phase";
        internal const String Phases = "phases";
        internal const String Resources = "resources";
        internal const String Steps = "steps";
        internal const String Target = "target";
        internal const String Task = "task";
        internal const String Template = "template";
        internal const String Timeout = "timeout";
        internal const String TimeoutInMinutes = "timeoutInMinutes";
        internal const String Type = "type";
        internal const String Value = "value";
        internal const String Variables = "variables";
        internal const String Verbatim = "verbatim";
    }
}