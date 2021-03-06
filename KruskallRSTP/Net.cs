﻿using System;
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
            int priority = 1;
            XmlNodeList list = xmlDocument.SelectNodes("//n:network/n:networkStructure/n:nodes/n:node", manager);
            foreach (XmlNode node in list) {
                String bridgeId = node.Attributes["id"].Value.ToString();
                double postionX = Convert.ToDouble(node.SelectSingleNode("n:coordinates/n:x", manager).InnerText, enUsCulture);
                double postionY = Convert.ToDouble(node.SelectSingleNode("n:coordinates/n:y", manager).InnerText, enUsCulture);
                Bridge bridge = new Bridge(bridgeId, priority++, postionX, postionY, new List<Port>());
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
            // always use dot separator for doubles
            CultureInfo enUsCulture = CultureInfo.GetCultureInfo("en-US");
            
            //create base xml informations
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
            XmlElement links = xmlDocument.CreateElement("links");
            networkStructure.AppendChild(links);

            List<Tripple<Port,Bridge,bool>> edgeGeneratorList = new List<Tripple<Port,Bridge,bool>>();

            //create nodes
            foreach (Bridge bridge in bridges) {
                //zapisujemy tylko włączone bridże
                if (!bridge.isEnabled) {
                    continue;
                }
                foreach (Port port in bridge.ports) {
                    edgeGeneratorList.Add(new Tripple<Port, Bridge, bool>(port, bridge, false));
                }
                XmlElement node = xmlDocument.CreateElement("node");
                node.SetAttribute("id", bridge.bridgeId);
                nodes.AppendChild(node);

                XmlElement coordinates = xmlDocument.CreateElement("coordinates");
                node.AppendChild(coordinates);

                XmlElement x = xmlDocument.CreateElement("x");
                x.InnerText = bridge.xPosition.ToString(enUsCulture);
                coordinates.AppendChild(x);

                XmlElement y = xmlDocument.CreateElement("y");
                y.InnerText = bridge.yPosition.ToString(enUsCulture);
                coordinates.AppendChild(y);

            }

            //create links
            foreach (Tripple<Port, Bridge, bool> tripple in edgeGeneratorList) {
                if (tripple.Third) {
                    continue;
                }
                if (tripple.First.destinationPort == null) {
                    continue;
                }
                //zapisujemy tylko włączone krawędzie
                if (!tripple.First.isEnabled) {
                    continue;
                }
                Tripple<Port, Bridge, bool> tripple3 = null;
                foreach (Tripple<Port, Bridge, bool> tripple2 in edgeGeneratorList) {
                    if (tripple2.First.Equals(tripple.First.destinationPort)) {
                        tripple3 = tripple2;
                        tripple2.Third = true;
                        break;
                    }
                }
                tripple.Third = true;

                //add link
                XmlElement link = xmlDocument.CreateElement("link");
                //drawLine(tripple.Second, tripple.First, tripple3.Second, tripple3.First, false);
                link.SetAttribute("id", tripple.Second.bridgeId + "_" + tripple3.Second.bridgeId);
                
                XmlElement source = xmlDocument.CreateElement("source");
                source.InnerText = tripple.Second.bridgeId;
                link.AppendChild(source);
                
                XmlElement target = xmlDocument.CreateElement("target");
                target.InnerText = tripple3.Second.bridgeId;
                link.AppendChild(target);

                XmlElement additionalModules = xmlDocument.CreateElement("additionalModules");
                link.AppendChild(additionalModules);

                XmlElement addModule = xmlDocument.CreateElement("addModule");
                additionalModules.AppendChild(addModule);

                XmlElement cost = xmlDocument.CreateElement("cost");
                cost.InnerText = tripple.First.time.ToString(enUsCulture);
                addModule.AppendChild(cost);
                
                links.AppendChild(link);
            }

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
