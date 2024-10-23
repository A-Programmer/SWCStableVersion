using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PortalCommon
{
    [Serializable()]
    [XmlType(TypeName = "PageZone")]
    public class pcPageZone
    {
        [Key]
        public int ZoneID { get; set; }
        public string ZoneName { get; set; }
        public string ZoneTitle { get; set; }
        public string TemplateName { get; set; }
        public string CssClass { get; set; }
    }

    [Serializable()]
    [XmlType(TypeName = "ModulePageLayout")]
    public class pcPageLayout
    {
        public string BlockName { get; set; }
        public pcPageZone PageZone { get; set; }
        public int AppearanceOrder { get; set; }
    }

    [Serializable()]
    [XmlType(TypeName = "ModulePage")]
    public class pcModulePage
    {
        public string PageName { get; set; }
        public string PageTitle { get; set; }
        public string PageRoute { get; set; }
        public List<pcPageLayout> DefaultLayout { get; set; }
        public pcModulePage()
        {
            DefaultLayout = new List<pcPageLayout>();
        }

    }

    [Serializable()]
    [XmlType(TypeName = "ModuleBlock")]
    public class pcModuleBlock
    {
        public string BlockTitle { get; set; }
        public string BlockName { get; set; }
        public string BlockDescription { get; set; }
        public string BlockFilePath { get; set; }
        public bool IsAdminBlock { get; set; }
        
    }

    [Serializable()]
    [XmlType(TypeName = "ConnectionString")]
    public class pcConnectionString
    {
        public string Name { get; set; }
        public string ConnectionString { get; set; }
        public string ProviderName { get; set; }
    }

    [Serializable()]
    [XmlType(TypeName = "Permissions")]
    public class pcPermissions
    {
        public string PermissionName { get; set; }
        public string PermissionTitle { get; set; }
    }

    [Serializable()]
    [XmlType(TypeName = "Module")]
    public class pcModule
    {
        public string ModuleName { get; set; }
        public string ModuleTitle { get; set; }
        public string ModuleDescription { get; set; }
        public string AdminFilePath { get; set; }
        public bool IsVitalModule { get; set; }
        public bool IsActive { get; set; }
        public bool IsInstalled { get; set; }
        public bool HasConnectionStrings { get; set; }
        public List<pcConnectionString> ConnectionStrings { get; set; }
        public bool SupportsDatabaseInstall { get; set; }
        public bool SupportsDatabaseUninstall { get; set; }
        public List<pcModuleBlock> Blocks { get; set; }
        public List<pcModulePage> Pages { get; set; }
        public List<pcPermissions> Permissions { get; set; }

        public pcModule()
        {
            Blocks = new List<pcModuleBlock>();
            Pages = new List<pcModulePage>();
            ConnectionStrings = new List<pcConnectionString>();
            Permissions = new List<pcPermissions>();
        }
    }
}
