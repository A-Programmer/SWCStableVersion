using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Soap;

namespace PortalCommon
{
    public class SerializationUtils
    {
        public SerializationUtils() { }

        #region XML Serialization
        /// <summary>
        /// Converts an object to an xml string.
        /// </summary>
        /// <param name="objToXml">Object wanted to be converted to xml string</param>
        /// <returns>Xml string of the object</returns>		
        public static string ToXml(Object objToXml, bool includeNameSpace)
        {
            StreamWriter stWriter = null;
            XmlSerializer xmlSerializer;
            string buffer;
            try
            {
                xmlSerializer = new XmlSerializer(objToXml.GetType());
                MemoryStream memStream = new MemoryStream();
                stWriter = new StreamWriter(memStream);
                if (!includeNameSpace)
                {
                    System.Xml.Serialization.XmlSerializerNamespaces xs = new XmlSerializerNamespaces();
                    xs.Add("", "");//To remove namespace and any other inline information tag
                    xmlSerializer.Serialize(stWriter, objToXml, xs);
                }
                else
                {
                    xmlSerializer.Serialize(stWriter, objToXml);
                }
                buffer = Encoding.UTF8.GetString(memStream.GetBuffer());
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                if (stWriter != null) stWriter.Close();
            }
            return buffer;
        }

        /// <summary>
        /// Serializes a given object to the file given with the path
        /// </summary>
        /// <param name="objToXml">Object wanted to be serialized to the file</param>
        /// <param name="filePath">Path of the file</param>		
        public static void ToXml(Object objToXml, string filePath, bool includeNameSpace)
        {
            StreamWriter stWriter = null;
            XmlSerializer xmlSerializer;
            try
            {
                xmlSerializer = new XmlSerializer(objToXml.GetType());
                stWriter = new StreamWriter(filePath);
                if (!includeNameSpace)
                {
                    System.Xml.Serialization.XmlSerializerNamespaces xs = new XmlSerializerNamespaces();
                    xs.Add("", "");//To remove namespace and any other inline information tag
                    xmlSerializer.Serialize(stWriter, objToXml, xs);
                }
                else
                {
                    xmlSerializer.Serialize(stWriter, objToXml);
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                if (stWriter != null) stWriter.Close();
            }
        }

        /// <summary>
        /// Serializes a given object to the file given with the path
        /// </summary>
        /// <param name="objToXml">Object wanted to be serialized to the file</param>
        /// <param name="includeStartDocument">Includes the <?xml version="1.0" encoding="utf-8" ?> line if it is true, otherwise start doucment line is excluded from the exported xml.</param>
        /// <param name="filePath">Path of the file</param>		
        public static void ToXml(Object objToXml, string filePath, bool includeNameSpace, bool includeStartDocument)
        {
            SpecialXmlWriter stWriter = null;
            XmlSerializer xmlSerializer;
            try
            {
                xmlSerializer = new XmlSerializer(objToXml.GetType());
                stWriter = new SpecialXmlWriter(filePath, null, includeStartDocument);
                System.Xml.Serialization.XmlSerializerNamespaces xs = new XmlSerializerNamespaces();
                xs.Add("", "");//To remove namespace and any other inline information tag
                xmlSerializer.Serialize(stWriter, objToXml, xs);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                if (stWriter != null) stWriter.Close();
            }
        }

        /// <summary>
        /// Deserializes the object given with the type from the given string
        /// </summary>
        /// <param name="xmlString">String containing the serialized xml form of the object</param>
        /// <param name="type">Type of the object to be deserialized</param>
        /// <returns>Deserialized object</returns>
        public static object XmlTo(string xmlString, Type type)
        {
            XmlSerializer xmlSerializer;
            MemoryStream memStream = null;
            try
            {
                xmlSerializer = new XmlSerializer(type);
                //byte[] bytes = new byte[xmlString.Length];
                //Encoding.UTF8.GetBytes(xmlString, 0, xmlString.Length, bytes, 0);
                byte[] bytes = Encoding.UTF8.GetBytes(xmlString);
                memStream = new MemoryStream(bytes);
                object objectFromXml = xmlSerializer.Deserialize(memStream);
                return objectFromXml;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                if (memStream != null) memStream.Close();
            }
        }

        /// <summary>
        /// Deserializes the object given with the type from the given string
        /// </summary>
        /// <param name="xmlString">String containing the serialized xml form of the object</param>
        /// <param name="type">Type of the object to be deserialized</param>
        /// <returns>Deserialized object</returns>
        public static object XmlToFromFile(string filePath, Type type)
        {
            XmlSerializer xmlSerializer;
            FileStream fileStream = null;
            try
            {
                xmlSerializer = new XmlSerializer(type);
                fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                object objectFromXml = xmlSerializer.Deserialize(fileStream);
                return objectFromXml;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                if (fileStream != null) fileStream.Close();
            }
        }
        #endregion XML Serialization

        #region Binary Serialization
        public static object BinaryTo(string binaryString)
        {
            IFormatter formatter;
            MemoryStream memStream = null;
            Object objectFromBin = null;

            try
            {
                byte[] bytes = new byte[binaryString.Length];
                Encoding.UTF8.GetBytes(binaryString, 0, binaryString.Length, bytes, 0);
                memStream = new MemoryStream(bytes);
                formatter = new SoapFormatter();
                objectFromBin = formatter.Deserialize(memStream);
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                if (memStream != null) memStream.Close();
            }
            return objectFromBin;



        }
        #endregion

        #region SOAP Serialization

        /// <summary>
        /// DeSerializes a string into a  object 
        /// </summary>
        /// <param name="strFieldObject">String to be deserialized</param>
        /// <returns>Deserialized field object</returns>
        public static object SoapTo(string soapString)
        {
            IFormatter formatter;
            MemoryStream memStream = null;
            Object objectFromSoap = null;
            try
            {
                byte[] bytes = new byte[soapString.Length];
                Encoding.UTF8.GetBytes(soapString, 0, soapString.Length, bytes, 0);
                memStream = new MemoryStream(bytes);
                formatter = new SoapFormatter();
                objectFromSoap = formatter.Deserialize(memStream);
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                if (memStream != null) memStream.Close();
            }
            return objectFromSoap;
        }

        /// <summary>
        /// DeSerializes a string into a  object 
        /// </summary>
        /// <param name="strFieldObject">String to be deserialized</param>
        /// <returns>Deserialized field object</returns>
        public static object SoapToFromFile(string filePath)
        {
            IFormatter formatter;
            FileStream fileStream = null;
            Object objectFromSoap = null;
            try
            {
                fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                formatter = new SoapFormatter();
                objectFromSoap = formatter.Deserialize(fileStream);
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                if (fileStream != null) fileStream.Close();
            }
            return objectFromSoap;
        }

        /// <summary>
        /// Serializes the field object into a string
        /// </summary>
        /// <param name="objField">Field Object to be serialized</param>
        /// <returns>Serialized field object</returns>
        public static string ToSoap(Object objToSoap)
        {
            IFormatter formatter;
            MemoryStream memStream = null;
            string strObject = "";
            try
            {
                memStream = new MemoryStream();
                formatter = new SoapFormatter();
                formatter.Serialize(memStream, objToSoap);
                strObject = Encoding.UTF8.GetString(memStream.GetBuffer());
                int index = strObject.IndexOf("\0");//Check for the null terminator character
                if (index > 0)
                {
                    strObject = strObject.Substring(0, index);
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                if (memStream != null) memStream.Close();
            }
            return strObject;
        }

        /// <summary>
        /// Serializes the field object into a string
        /// </summary>
        /// <param name="objField">Field Object to be serialized</param>
        /// <returns>Serialized field object</returns>
        public static void ToSoap(Object objToSoap, string filePath)
        {
            IFormatter formatter;
            FileStream fileStream = null;
            try
            {
                fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                formatter = new SoapFormatter();
                formatter.Serialize(fileStream, objToSoap);
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                if (fileStream != null) fileStream.Close();
            }
        }

        /// <summary>
        /// DeSerializes a string into a  object 
        /// </summary>
        /// <param name="strFieldObject">String to be deserialized</param>
        /// <returns>Deserialized field object</returns>
        public static object SoapToFromFile(string filePath, SerializationBinder binder)
        {
            IFormatter formatter;
            FileStream fileStream = null;
            Object objectFromSoap = null;
            try
            {
                fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                formatter = new SoapFormatter();
                formatter.Binder = binder;
                objectFromSoap = formatter.Deserialize(fileStream);
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                if (fileStream != null) fileStream.Close();
            }
            return objectFromSoap;
        }
    }


        #endregion XML Serialization

    #region SpecialXmlWriter

    /// <summary>
    /// This class can be used to implement special affects while producing xml documents.
    /// At the moment it is only used for excluding the xml start line(<?xml version="1.0" encoding="utf-8" ?> )
    /// </summary>
    public class SpecialXmlWriter : XmlTextWriter
    {
        bool m_includeStartDocument = true;
        public SpecialXmlWriter(TextWriter tw, bool includeStartDocument)
            : base(tw)
        {
            m_includeStartDocument = includeStartDocument;
        }
        public SpecialXmlWriter(Stream sw, Encoding encoding, bool includeStartDocument)
            : base(sw, null)
        {
            m_includeStartDocument = includeStartDocument;
        }
        public SpecialXmlWriter(string filePath, Encoding encoding, bool includeStartDocument)
            : base(filePath, null)
        {
            m_includeStartDocument = includeStartDocument;
        }
        public override void WriteStartDocument()
        {
            if (m_includeStartDocument)
            {
                base.WriteStartDocument();
            }
        }
    }

    #endregion SpecialXmlWriter
    }

