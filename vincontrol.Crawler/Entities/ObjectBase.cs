using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using vincontrol.Data.Interface;

namespace vincontrol.Crawler.Entities
{
    public class ObjectBase
    {
        protected IUnitOfWork UnitOfWork;

        [XmlAttribute("name")]
        public string Name = string.Empty;

        public ObjectBase() { }

        public virtual void Execute()
        {
        }
    }

    public class CollectionBaseObject : CollectionBase
    {
        //for accessing fast
        Hashtable htName = new Hashtable();

        public ObjectBase this[int index]
        {
            get
            {
                return (ObjectBase)this.List[index];
            }
            set
            {
                this.List[index] = value;
            }
        }

        public ObjectBase this[string name]
        {
            get
            {
                return (ObjectBase)htName[name];
            }
            set
            {
                htName[name] = value;
            }
        }

        public void Add(ObjectBase value)
        {
            var collection = (this as ICollection);
            lock (collection.SyncRoot)
            {
                this.List.Add(value);
                htName.Add(value.Name, value);
            }
        }

        public virtual void Remove(ObjectBase value)
        {
            var collection = (this as ICollection);
            lock (collection.SyncRoot)
            {
                this.List.Remove(value);
                htName.Remove(value.Name);
            }
        }
    }

    public class BaseObjectType
    {
        private static Type[] _types;
        //contains all types and their names in this assembly
        private static Hashtable htTypeName = new Hashtable();
        //contains all XmlSerializer instance for 
        private static Hashtable htXmlSerializerList = new Hashtable();

        public static Type[] AllType
        {
            get { return _types ?? (_types = GetAllSystemType()); }
        }

        /// <summary>
        /// get all types of classes derived from BaseObject in this application for serializing
        /// </summary>
        private static Type[] GetAllSystemType()
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            Type baseType = typeof(ObjectBase);
            //contain all types of this application
            var list = new ArrayList();
            //get all class of Assembly for XmlSerializer
            foreach (Assembly t in assemblies)
            {
                Type[] types = t.GetTypes();
                foreach (Type t1 in types)
                {
                    //only get BaseObject and objects derived from it
                    if (!t1.IsSubclassOf(baseType)) continue;
                    list.Add(t1);
                    htTypeName.Add(t1.Name, t1);
                }
            }
            return (Type[])list.ToArray(typeof(Type));
        }

        public static XmlSerializer GetXmlSerializer(string systemType)
        {
            lock (htXmlSerializerList.SyncRoot)
            {
                if (_types == null)
                    _types = GetAllSystemType();

            }
            lock (htXmlSerializerList.SyncRoot)
            {
                if (htXmlSerializerList[systemType] == null)
                    htXmlSerializerList.Add(systemType, new XmlSerializer((Type)htTypeName[systemType], AllType));
            }
            return (XmlSerializer)htXmlSerializerList[systemType];
        }
    }
}
