using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Globalization;

namespace KruskallRSTP {
    class Net {
        private readonly int NUMBER_OF_BRIDGES = 10;
        public List<Bridge> bridges { get; private set; }

        /**
         * 
         * make random net
         * don't do it at home!
         * 
         */
        public Net() {
            bridges = new List<Bridge>();
            Port port = null;
            for (int i = 0; i < NUMBER_OF_BRIDGES; i++) {
                List<Port> ports = new List<Port>();
                for (int j = 0; j < 10; j++) {
                    if (j == 0) {
                        port = new Port(new MAC(0, i, j), port, 0);
                    } else {
                        port = new Port(new MAC(0, i, j), null, 0);
                    }
                    ports.Add(port);
                }
                Bridge bridge = new Bridge("Andzia" + i.ToString(), i, i, ports);
                bridges.Add(bridge);
                bridge.PropertyChanged += sc_PropertyChanged;
            }
        }

        public Net(XmlDocument xmlDocument) {
            // always use dot separator for doubles
            CultureInfo enUsCulture = CultureInfo.GetCultureInfo("en-US");
            bridges = new List<Bridge>();
            XmlNamespaceManager manager = new XmlNamespaceManager(xmlDocument.NameTable);
            if (xmlDocument.DocumentElement.Attributes["xmlns"] != null) {
                manager.AddNamespace("n", xmlDocument.DocumentElement.Attributes["xmlns"].Value);
            } else {
                manager.AddNamespace("n", "");
            }
            XmlNodeList list = xmlDocument.SelectNodes("//n:network/n:networkStructure/n:nodes/n:node", manager);
            foreach (XmlNode node in list) {
                String bridgeId = node.Attributes["id"].Value.ToString();
                double postionX = Convert.ToDouble(node.SelectSingleNode("n:coordinates/n:x", manager).InnerText, enUsCulture);
                double postionY = Convert.ToDouble(node.SelectSingleNode("n:coordinates/n:y", manager).InnerText, enUsCulture);
                Bridge bridge = new Bridge(bridgeId, postionX, postionY, new List<Port>());
                bridges.Add(bridge);
            }
            list = xmlDocument.SelectNodes("//n:network/n:networkStructure/n:links/n:link", manager);
            int i = 1;
            foreach (XmlNode link in list) {
                String bridgeId1 = link.SelectSingleNode("n:source", manager).InnerText;
                String bridgeId2 = link.SelectSingleNode("n:target", manager).InnerText;
                int cost = (int)Convert.ToDouble(link.SelectSingleNode("n:additionalModules/n:addModule/n:cost", manager).InnerText, enUsCulture);

                //tutaj można jesszcze ekstra zabezpieczać przed złymi xmlami
                //że jest target a dest nie znaleziony itp
                if (bridgeId1 != null && bridgeId2 != null) {
                    Port port1 = new Port(new MAC(0, i, i + 1),
                                          null,
                                          cost);
                    Port port2 = new Port(new MAC(0, i + 1, i++),
                                          port1,
                                          cost);
                    foreach (Bridge bridge in bridges) {
                        if (bridgeId1.Equals(bridge.bridgeId)) {
                            bridge.ports.Add(port1);
                            break;
                        }
                    }
                    foreach (Bridge bridge in bridges) {
                        if (bridgeId2.Equals(bridge.bridgeId)) {
                            bridge.ports.Add(port2);
                            break;
                        }
                    }
                }
            }
        }

        public void save(string filename) {
            //CultureInfo enUsCulture = CultureInfo.GetCultureInfo("en-US");
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.AppendChild(xmlDocument.CreateXmlDeclaration("1.0", null, null));
            XmlElement network = xmlDocument.CreateElement("network");
            network.SetAttribute("version", "1.0");
            xmlDocument.AppendChild(network);
            XmlElement networkStructure = xmlDocument.CreateElement("networkStructure");
            network.AppendChild(networkStructure);
            XmlElement nodes = xmlDocument.CreateElement("nodes");
            nodes.SetAttribute("coordinatesType", "geographical");
            networkStructure.AppendChild(nodes);

            foreach (Bridge bridge in bridges) {
                XmlElement node = xmlDocument.CreateElement("node");
                node.SetAttribute("id", bridge.bridgeId);
                nodes.AppendChild(node);

                XmlElement coordinates = xmlDocument.CreateElement("coordinates");
                node.AppendChild(coordinates);

                XmlElement x = xmlDocument.CreateElement("x");
                x.InnerText = bridge.xPosition.ToString();
                coordinates.AppendChild(x);

                XmlElement y = xmlDocument.CreateElement("y");
                y.InnerText = bridge.yPosition.ToString();
                coordinates.AppendChild(y);

            }

            XmlElement links = xmlDocument.CreateElement("links");
            networkStructure.AppendChild(links);

            
            


            // Save to the XML file
            xmlDocument.Save(filename);
        }

        void sc_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            if (e.PropertyName.Equals("isEnabled")) {
                //ponów kruskala;
            }
        }
    }
}
