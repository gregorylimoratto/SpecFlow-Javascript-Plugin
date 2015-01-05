using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TechTalk.SpecFlow.Generator.Configuration;
using TechTalk.SpecFlow.Generator.Interfaces;

namespace UnitTestProject1
{

    public class VsConfigReader : ISpecFlowConfigurationReader
    {
        public VsConfigReader()
        {
        }

        public SpecFlowConfigurationHolder ReadConfiguration()
        {
            string configFileContent = GetConfigFileContent();
            if (configFileContent == null)
                return new SpecFlowConfigurationHolder();
            return GetConfigurationHolderFromFileContent(configFileContent);
        }

        protected virtual string GetConfigFileContent()
        {
            var configFilePath = GetConfigFilePath();
            if (configFilePath == null)
                return null;
            return File.ReadAllText(configFilePath);
        }

        protected virtual string GetConfigFilePath()
        {
            return "../../app.config";
        }

        private SpecFlowConfigurationHolder GetConfigurationHolderFromFileContent(string configFileContent)
        {
            XmlDocument configDocument = new XmlDocument();
            configDocument.LoadXml(configFileContent);
            return new SpecFlowConfigurationHolder(configDocument.SelectSingleNode("/configuration/specFlow"));
        }

    }
}
