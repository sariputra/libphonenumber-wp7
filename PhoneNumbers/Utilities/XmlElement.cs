using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Linq;


namespace Talk.To.Utilities.Xml
{
    [DataContract]
    /*[KnownType(typeof(Message))]
    [KnownType(typeof(IQ))]
    [KnownType(typeof(BasicStanza))]
    [KnownType(typeof(Presence))]
    */
    //   [KnownType("GetKnownTypes")]
    public class XmlElement
    {
        [DataMember]
        public XElement _element;

        ///Creating Packets
        public XmlElement(String name)
            : this(name, null)
        {
        }


        public XElement Element
        {
            get
            {
                return _element;
            }

            set
            {
                _element = value;
            }
        }

        public string InnerText
        {
            get { return getMessage(); }
        }

        public XmlElement(String name, String xmlns, String namespaceprefix)
        {
            _element = new XElement(name);
        }

        public XmlElement(String name, String xmlns)
        {
            if (xmlns != null)
            {
                XNamespace _namespace = xmlns;
                _element = new XElement(_namespace + name, new XAttribute("xmlns", xmlns));
            }
            else
            {
                _element = new XElement(name);
            }
        }

        /*public static Type[] GetKnownTypes()
        {
          //  Type thisType = MethodBase.GetCurrentMethod().DeclaringType;
            //return thisType.Assembly.GetTypes().ToList<Type>().Where(t => t.IsSubclassOf(thisType)).ToArray();
        }
*/
        public XmlElement(XElement element)
        {
            _element = element;
        }

        public XElement getXElement()
        {
            return _element;
        }

        /// adding child
        public void addChild(XmlElement child)
        {
            XNamespace aw = _element.Name.Namespace;
            if (String.IsNullOrEmpty(child.getXElement().Name.Namespace.NamespaceName))
            {
                child.getXElement().Name = aw.GetName(child.getXElement().Name.LocalName);
            }
            _element.Add(child.getXElement());
        }

        public void addChild(String name, String xmlns)
        {

            XNamespace _nameSpace = xmlns;
            if (_nameSpace != null)
            {
                XElement element = new XElement(_nameSpace + name, new XAttribute("xmlns", xmlns));
                _element.Add(element);
            }
            else
            {
                XElement element = new XElement(_element.Name.Namespace + name);
                _element.Add(element);
            }
        }

        protected void add(XmlElement node)
        {
            _element.Add(node.getXElement());
        }

        ////////////////////// Getting attribute value and attributes hashtable
        public String GetAttribute(String name)
        {
            if (name == "xmlns")
            {
                return _element.Name.Namespace.NamespaceName;
            }
            XAttribute ret;
            if ((ret = _element.Attribute(name)) != null)
            {
                return ret.Value;
            }
            return null;
        }



        public int getChildrenCount()
        {
            return _element.Elements().Count();
        }

        ///////Returning name of the packet
        public String getName()
        {
            return _element.Name.LocalName;
        }

        public XmlElement getChild(String name)
        {

            IEnumerable<XElement> element = _element.Elements().Where(e => e.Name.LocalName == name);

            return element.Select(xElement => new XmlElement(xElement)).FirstOrDefault();
        }



        //Remove a particular child
        public bool removeChild(XmlElement child)
        {
            IEnumerable<XElement> element = _element.Elements().Where(e => e.Name.LocalName == child.getName());

            if (element.Any())
            {
                element.Remove();
                return true;
            }
            return false;
        }

        //Set an attribute
        public void setAttribute(String name, string value)
        {
            if (value != null)
            {
                if (name == "xmlns")
                {
                    XNamespace aw = value;

                    if (string.IsNullOrEmpty(_element.Name.Namespace.NamespaceName))
                        _element.Name = aw.GetName(_element.Name.LocalName);

                }
                else
                    _element.SetAttributeValue(name, value);
            }
        }

        public List<XmlElement> getChildren(String name)
        {
            IEnumerable<XElement> element = _element.Elements().Where(e => e.Name.LocalName == name);

            return element.Select(xElement => new XmlElement(xElement)).ToList();

        }

        public List<XmlElement> getChildren()
        {
            IEnumerable<XElement> element = _element.Elements();

            return element.Select(xElement => new XmlElement(xElement)).ToList();
        }

        //Get message
        public String getMessage()
        {
            return _element.Value;
            //  return _message;
        }

        //Set Message
        public void setMessage(String value)
        {
            _element.SetValue(value);
        }


        public bool hasAttribute(String name)
        {
            if (name == "xmlns")
            {
                return true;
            }
            if (_element.Attribute(name) != null)
                return true;
            return false;
        }

        public bool hasAttribute(String name, String value)
        {
            if (name == "xmlns")
            {
                if (_element.Name.Namespace.NamespaceName == value)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            XAttribute nAttribute = _element.Attribute(name);
            if (nAttribute != null)
            {
                return nAttribute.Value.Equals(value);
            }
            return false;
        }

        public bool hasChild(String name)
        {
            if (_element.Elements().Where(e => e.Name.LocalName == name).Count() > 0)
                return true;
            return false;
        }

        public void CloneAttributesAndChildrenFrom(XmlElement packetToCloneFrom)
        {
            IEnumerable<XAttribute> attribute = packetToCloneFrom.getXElement().Attributes();
            IEnumerable<XElement> element = packetToCloneFrom.getXElement().Elements();
            foreach (XElement xElement in element)
            {

            }
            _element.ReplaceAttributes(attribute);
            _element.Add(element);
        }

        public override string ToString()
        {
            return _element.ToString();
        }

        public static XmlElement ConvertPacketFromXML(XElement xmlElement)
        {

            foreach (XElement el in xmlElement.DescendantsAndSelf())
            {
                var xAttribute = el.Attribute("xmlns");
                if (xAttribute != null)
                {
                    XNamespace aw = xAttribute.Value;

                    if (el.Name.Namespace != aw)
                        el.Name = aw.GetName(el.Name.LocalName);
                }
                else
                {
                    if (el.Parent != null)
                    {
                        el.Name = el.Parent.Name.Namespace.GetName(el.Name.LocalName);
                    }
                }
            }


            XmlElement packet = new XmlElement(xmlElement);
            return packet;
        }

        private void updateXmlns(string name, string value)
        {
            if (value != null)
                _element.SetAttributeValue(name, value);
        }

        public string GetValue()
        {
            return _element.Value;
        }

        public void SetValue(string value)
        {
            _element.Value = value;
        }

        public void UpdateXmlnsIfEmpty(XmlElement peek)
        {
            XNamespace aw = peek.getXElement().Name.Namespace;

            if (string.IsNullOrEmpty(_element.Name.Namespace.NamespaceName))
                _element.Name = aw.GetName(_element.Name.LocalName);

        }

        public List<XmlElement> GetElementsByTagName(string name)
        {
            return getChildren(name);
        }

        public bool HasAttribute(string nationalPrefix)
        {
            return hasAttribute(nationalPrefix);
        }
    }
}
