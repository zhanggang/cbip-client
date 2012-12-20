using System;
using System.Configuration;
using System.Xml;
using System.Collections.Generic;

namespace Console
{
	public class ConfigSerializerSectionHandler : IConfigurationSectionHandler 
	{
		public object Create(object parent, object configContext, XmlNode section) 
		{
            return LoadValuesFromConfigurationFile(section);
		}

        private object LoadValuesFromConfigurationFile(XmlNode node)
        {
            List<UserInfo> users = new List<UserInfo>();

            foreach (XmlNode n in node.ChildNodes)
            {
                if (n.NodeType == XmlNodeType.Comment)
                    continue;

                UserInfo user = new UserInfo();

                XmlAttribute attribute = n.Attributes["name"];
                if (!(attribute == null || string.IsNullOrEmpty(attribute.Value)))
                    user.LoginName = attribute.Value;

                attribute = n.Attributes["password"];
                if (!(attribute == null || string.IsNullOrEmpty(attribute.Value)))
                    user.Password = attribute.Value;
                
                users.Add(user);
            }

            return users;
        }
    }
}
