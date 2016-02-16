// (c) 2009 RenaissanceRe IP Holdings Ltd.  All rights reserved.
// Author   :     John O'Sullivan
// Date     :     30-00-2010
// Desc     :     Extension methods that help build SSIS packages programmatically
// 
using System;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using Microsoft.SqlServer.Dts.Runtime;

namespace RenRe.Doris.Data.Ssis.Scaffolding
{
    public static class SsisScripterExtensionMethods
    {
        /// <summary>
        /// Creates an Oledb connection for a package
        /// </summary>
        /// <param name="package"></param>
        /// <param name="name"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static void AddOledbConnection(this Package package, string name, string connectionString)
        {
            var connection = package.Connections.Add(SsisConstants.SsisConnectionOleDbConnectionId);
            connection.ConnectionString = connectionString;
            connection.Name = name;
        }

        public static void AddAboNetConnection(this Package package, string name, string connectionString)
        {
            var connection = package.Connections.Add(SsisConstants.SsisConnectionAdoNetSql);
            connection.ConnectionString = connectionString;
            connection.Name = name;
        }

        public static void AddTextLog(this Package package, string name, string filePath)
        {
            //add the connection for the log file
            var connection = package.Connections.Add(SsisConstants.FileConnectionId);
            connection.Name = name;
            connection.ConnectionString = filePath;
            
            //add the log file to the package
            var logProvider = package.LogProviders.Add(SsisConstants.TextFileLoggingProvider);
            logProvider.ConfigString = connection.Name;
            package.LoggingOptions.SelectedLogProviders.Add(logProvider);
        }

        public static void SetStandardErrorLoggingOptions(this DtsContainer package)
        {
            //assume we have one and only one log file
            package.LoggingOptions.EventFilterKind = DTSEventFilterKind.Inclusion;
            package.LoggingOptions.EventFilter = new [] {SsisConstants.LoggingEventOnError, SsisConstants.LoggingEventOnTaskFailed, SsisConstants.LoggingEventOnWarning};
            package.LoggingMode = DTSLoggingMode.Enabled;
        }

        /// <summary>
        /// Adds a variable to a package
        /// </summary>
        /// <param name="package"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Variable AddVariable(this DtsContainer package, string name, object value)
        {
            return package.AddVariable(name, value, false, SsisConstants.PackageVarUserNamespace);
        }

        /// <summary>
        /// Adds a variable to a package
        /// </summary>
        /// <param name="package"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="nameSpace"></param>
        /// <returns></returns>
        public static Variable AddVariable(this DtsContainer package, string name, object value, string nameSpace)
        {
            return package.AddVariable(name, value, false, nameSpace);
        }

        /// <summary>
        /// Adds a variable to a package
        /// </summary>
        /// <param name="package"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="readyOnly"></param>
        /// <returns></returns>
        public static Variable AddVariable(this DtsContainer package, string name, object value, bool readyOnly)
        {
            return package.AddVariable(name, value, readyOnly, SsisConstants.PackageVarUserNamespace);
        }

        /// <summary>
        /// Adds a variable to a package
        /// </summary>
        /// <param name="package"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="readOnly"></param>
        /// <param name="nameSpace"></param>
        /// <returns></returns>
        public static Variable AddVariable(this DtsContainer package, string name, object value, bool readOnly, string nameSpace)
        {
            return package.Variables.Add(name, readOnly, nameSpace, value);
        }

        public static Variable AddVariable(this DtsContainer package, string name, object value, bool readOnly, bool evaluateAsExpression)
        {
            var variable = package.Variables.Add(name, readOnly, SsisConstants.PackageVarUserNamespace, null);
            variable.Value = string.Empty;
            variable.EvaluateAsExpression = evaluateAsExpression;
            variable.Expression = value.ToString();
            return variable;
        }

        /// <summary>
        /// Attach a connection to a component
        /// </summary>
        /// <param name="componentMetaData"></param>
        /// <param name="ssisConnectionManager"></param>
        public static void AttachConnection(this IDTSComponentMetaData100 componentMetaData, ISsisConnectionManager ssisConnectionManager)
        {
            if (componentMetaData.RuntimeConnectionCollection.Count > 0)
            {
                componentMetaData.RuntimeConnectionCollection[0].ConnectionManager = ssisConnectionManager.GetExtendedConnectionManager;
                componentMetaData.RuntimeConnectionCollection[0].ConnectionManagerID = ssisConnectionManager.ID;
                return;
            }

            throw new SsisScripterException("RuntimeConnectionCollection has no items - cannot add attach a connection to this component");
        }

        /// <summary>
        /// Adds a component to a data flow
        /// </summary>
        /// <param name="dataFlowMainPipe"></param>
        /// <param name="name"></param>
        /// <param name="componentName"></param>
        /// <returns></returns>
        public static IDTSComponentMetaData100 AddComponentToDataFlow(this IDTSPipeline100 dataFlowMainPipe, string name, string componentName)
        {
            if(dataFlowMainPipe != null)
            {
                //create the component
                var componentMetaData = dataFlowMainPipe.ComponentMetaDataCollection.New();
                componentMetaData.Name = name;
                componentMetaData.ComponentClassID = componentName;
                return componentMetaData;
            }

            throw new SsisScripterException("The IDTSPipeline100 object passed to this method was null");
        }

        /// <summary>
        /// Instantiates a data flow component
        /// </summary>
        /// <param name="componentMetaData"></param>
        /// <returns></returns>
        public static IDTSDesigntimeComponent100 InstantiateComponent(this IDTSComponentMetaData100 componentMetaData)
        {
            if (componentMetaData != null)
            {
                //Get the design time instance of the component.
                var componentInstance = componentMetaData.Instantiate();

                //Initialize the component
                componentInstance.ProvideComponentProperties();

                return componentInstance;
            }

            throw new SsisScripterException("The IDTSComponentMetaData100 object passed to this method was null");
        }

        /// <summary>
        /// Refresh meta data for a given component instance
        /// </summary>
        /// <param name="componentInstance"></param>
        public static void RefreshMetaData(this IDTSDesigntimeComponent100 componentInstance)
        {
            try
            {
                if (componentInstance != null)
                {
                    componentInstance.AcquireConnections(null);
                    componentInstance.ReinitializeMetaData();
                    componentInstance.ReleaseConnections();
                    return;
                }    
            }
            catch(Exception err)
            {
                throw new SsisScripterException("Exception caught when component refreshing meta data", err);
            }

            throw new SsisScripterException("The IDTSDesigntimeComponent100 object passed to this method was null");
        }

        /// <summary>
        /// Needs to look like this:
        /// "Parameter0",{70CD7BCB-8914-453D-A0D8-2BD213A332BD};
        /// </summary>
        /// <param name="sqlParameterIndex"></param>
        /// <param name="variable"></param>
        /// <returns></returns>
        public static string GetSqlCommandParameterMapping(this Variable variable, string sqlParameterIndex)
        {
            var variableGuid = new Guid(variable.ID).ToString("B").ToUpper();
            return "\"" + sqlParameterIndex + "\"," + variableGuid + ";";
        }

        /// <summary>
        /// Wraps up a Ssis parameter qualifiedName so that it can be injected into an SSIS expression
        /// </summary>
        /// <param name="qualifiedName"></param>
        /// <returns></returns>
        public static string GetSsisParameterForExpression(this string qualifiedName)
        {
            return "@[" + qualifiedName + "]";
        }

        /// <summary>
        /// Adds a precedence constraint to a package - the connectTo parm determines if we are connecting the targetTaskComponent 
        /// to or from the sourceExecutable. This method assumes we want a Successful connection
        /// </summary>
        /// <param name="container"></param>
        /// <param name="sourceExecutable"></param>
        /// <param name="targetExecutable"></param>
        public static void AddPrecedenceConstraint(this Package container, Executable sourceExecutable, Executable targetExecutable)
        {
            container.AddPrecedenceConstraint(sourceExecutable, targetExecutable, DTSExecResult.Success);
        }

        /// <summary>
        /// Adds a precedence constraint to a package - the connectTo parm determines if we are connecting the targetTaskComponent 
        /// to or from the sourceExecutable.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="sourceExecutable"></param>
        /// <param name="targetExecutable"></param>
        /// <param name="desiredResult"></param>
        public static void AddPrecedenceConstraint(this Package container, Executable sourceExecutable, Executable targetExecutable, DTSExecResult desiredResult)
        {
            var precedenceConstraint = container.PrecedenceConstraints.Add(sourceExecutable, targetExecutable);
            precedenceConstraint.Value = desiredResult;
        }
    }
}
