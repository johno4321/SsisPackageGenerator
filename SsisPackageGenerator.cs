// (c) 2009 RenaissanceRe IP Holdings Ltd.  All rights reserved.
// Author   :     John O'Sullivan
// Date     :     30-00-2010
// Desc     :     
//
using System;
using System.Text;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using RenRe.Doris.Data.IoC;
using RenRe.Doris.Data.NH.Mappings;
using RenRe.Doris.Data.Repository;
using RenRe.Doris.Data.Ssis;
using StructureMap;

namespace RenRe.Doris.SsisPackageGenerator
{
    public class SsisPackageGenerator
    {
        private const int TotalNumberOfArgs = 9;

        private readonly ImportSchemaRepository _repository;
        private readonly string _importName;
        private readonly string _generatedScriptPath;
        private readonly string _sourceConnectionString;
        private readonly string _destinationConnectionString;
        private readonly string _stagingDatabaseConnectionString;
        private readonly string _importSchemaName;
        private readonly long _defaultImportRunId;
        private readonly string _defaultSourceIds;
        private readonly string _logFileDir;

        public SsisPackageGenerator(ImportSchemaRepository repository,string importName, string generatedScriptPath, string sourceConnectionString, string destinationConnectionString, string stagingDatabaseConnectionString, string importSchemaName, long defaultImportRunId, string defaultSourceIds, string logFileDir)
        {
            _repository = repository;
            _importName = importName;
            _generatedScriptPath = generatedScriptPath;
            _sourceConnectionString = sourceConnectionString;
            _destinationConnectionString = destinationConnectionString;
            _stagingDatabaseConnectionString = stagingDatabaseConnectionString;
            _importSchemaName = importSchemaName;
            _defaultImportRunId = defaultImportRunId;
            _defaultSourceIds = defaultSourceIds;
            _logFileDir = logFileDir;
        }
        
        public void GeneratePackage()
        {
            try
            {
                var importSchema = _repository.LoadOne();

                Console.WriteLine("import schema elements:");
                foreach(var importSchemaElement in importSchema.Elements)
                {
                    Console.WriteLine(importSchemaElement.Name);
                }

                var ssisImportScripterFactory = new SsisImportScripterFactory();
                var ssisImportScripter = ssisImportScripterFactory.GetSsisImportScripter
                (
                    _importName,
                    _importSchemaName,
                    _generatedScriptPath,
                    _sourceConnectionString,
                    _destinationConnectionString,
                    _stagingDatabaseConnectionString,
                    _defaultImportRunId,
                    _defaultSourceIds,
                    _logFileDir,
                    importSchema
                );

                ssisImportScripter.Generate();
            }
            catch (Exception err)
            {
                Console.WriteLine("Exception caught during SSIS package generation.");
                Console.WriteLine(err.ToString());
                Console.WriteLine(ToString());
            }

            Console.WriteLine("Generated package " + _generatedScriptPath);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("Import Name : \t" + _importName + Environment.NewLine);
            sb.Append("Generated Script Path : \t" + _generatedScriptPath + Environment.NewLine);
            sb.Append("Source Connection String : \t" + _sourceConnectionString + Environment.NewLine);
            sb.Append("Destination Connection String : \t" + _destinationConnectionString + Environment.NewLine);
            sb.Append("Staging Database Connection String : \t" + _stagingDatabaseConnectionString + Environment.NewLine);
            sb.Append("Import Schema Name : \t" + _importSchemaName + Environment.NewLine);
            sb.Append("Default Import Run Id : \t" + _defaultImportRunId + Environment.NewLine);
            sb.Append("Default Source Ids : \t" + _defaultSourceIds + Environment.NewLine);
            sb.Append("Log File Dir : \t" + _logFileDir + Environment.NewLine);
            
            return sb.ToString();
        }
        
        public static void Main(string[] args)
        {
            if (args.Length != TotalNumberOfArgs)
            {
                Console.WriteLine("I got " + args.Length + " arguments to this process. I need " + TotalNumberOfArgs);
                for (var i = 0; i < args.Length; i++)
                {
                    Console.WriteLine("Arg : " + i + " is : " + args[i]);
                }
                Console.WriteLine("Usage:");
                Console.WriteLine("GeneratorTool <importName> <generatedScriptPath> <sourceConnectionString> <destinationConnectionString> <stagingDatabaseConnectionString> <importSchemaName> <defaultImportRunId> <defaultSourceIds> <logFileDir>");
                return;
            }

            var importName = args[0];
            var generatedScriptPath = args[1];
            var sourceConnectionString = args[2];
            var destinationConnectionString = args[3];
            var stagingDatabaseConnectionString = args[4];
            var importSchemaName = args[5];
            var defaultImportRunId = Convert.ToInt64(args[6]);
            var defaultSourceIds = args[7];
            var logFileDir = args[8];
            Console.WriteLine("ctor for SsisPackageGenerator about to be called");

            var session = CreateSession(stagingDatabaseConnectionString);
            var importSchemaRepository = new ImportSchemaRepository(session,importName);
            var ssisPackageGenerator = new SsisPackageGenerator(importSchemaRepository,importName, generatedScriptPath, sourceConnectionString,
                                                                destinationConnectionString,
                                                                stagingDatabaseConnectionString, importSchemaName,
                                                                defaultImportRunId, defaultSourceIds, logFileDir);
            ssisPackageGenerator.GeneratePackage();
        }

        private static ISession CreateSession(string connectionString)
        {
            NHibernate.Cfg.Environment.BytecodeProvider = new DorisByteCodeProvider(ObjectFactory.Container);

            var cfg = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2005.ConnectionString(connectionString))
                .Mappings(m =>
                {
                    m.FluentMappings.AddFromAssemblyOf<RmsEdmMetaDataMap>().Conventions
                                    .AddFromAssemblyOf<RmsEdmMetaDataMap>();

                    m.HbmMappings.AddFromAssemblyOf<RmsEdmMetaDataMap>();
                }); 
            var sessionFactory = cfg.BuildSessionFactory();
            return sessionFactory.OpenSession();
        }
    }
}
