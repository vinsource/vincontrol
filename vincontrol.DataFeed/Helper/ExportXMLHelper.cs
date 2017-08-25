using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml;
using vincontrol.Backend.Data;
using vincontrol.DataFeed.Model;

namespace vincontrol.DataFeed.Helper
{
    public class ExportXMLHelper
    {
        private CommonHelper _commonHelper;

        public ExportXMLHelper()
        {
            _commonHelper = new CommonHelper();
        }

        private const string Node_Root = "Mappings";
        private const string Node_Mapping = "Mapping";
        private const string Node_DBField = "DBField";
        private const string Node_XMLField = "XMLField";
        private const string Node_Replaces = "Replaces";
        private const string Node_Replace = "Replace";
        private const string Node_Conditions = "Conditions";
        private const string Node_Condition = "Condition";
        private const string Node_Expression = "Expression";
        private const string Attribute_Header = "Header";
        private const string Attribute_Delimeter = "Delimeter";
        private const string Attribute_Order = "Order";
        private const string Attribute_Name = "Name";
        private const string Attribute_From = "From";
        private const string Attribute_To = "To";
        private const string Attribute_DBField = "DBField";
        private const string Attribute_XMLField = "XMLField";
        private const string Attribute_Operator = "Operator";
        private const string Attribute_ComparedValue = "ComparedValue";
        private const string Attribute_TargetValue = "TargetValue";
        private const string Attribute_Type = "Type";
        private const string Attribute_ExportFileType = "ExportFileType";
        private const string Attribute_InventoryStatus = "InventoryStatus";
        private const string Attribute_SplitImage = "SplitImage";

        #region Declare Column Names
        public const string ListingId = "ListingID";
        public const string Year = "ModelYear";
        public const string Make = "Make";
        public const string Model = "Model";
        public const string Trim = "Trim";
        public const string Vin = "VINNumber";
        public const string DaysInInventory = "DaysInInventory";
        public const string StockNumber = "StockNumber";
        public const string SalePrice = "SalePrice";
        public const string MSRP = "MSRP";
        public const string Mileage = "Mileage";
        public const string ExteriorColor = "ExteriorColor";
        public const string InteriorColor = "InteriorColor";
        public const string InteriorSurface = "InteriorSurface";
        public const string BodyType = "BodyType";
        public const string EngineType = "EngineType";
        public const string DriveTrain = "DriveTrain";
        public const string Cylinders = "Cylinders";
        public const string Liters = "Liters";
        public const string FuelType = "FuelType";
        public const string Tranmission = "Tranmission";
        public const string Doors = "Doors";
        public const string Certified = "Certified";
        public const string CarsOptions = "CarsOptions";
        public const string Descriptions = "Descriptions";
        public const string Disclaimer = "Disclaimer";
        public const string CarImageUrl = "CarImageUrl";
        public const string ThumbnalImageurl = "ThumbnalImageurl";
        public const string DateInStock = "DateInStock";
        public const string DealershipName = "DealershipName";
        public const string DealershipAddress = "DealershipAddress";
        public const string DealershipCity = "DealershipCity";
        public const string DealershipState = "DealershipState";
        public const string DealershipZipCode = "DealershipZipCode";
        public const string DealershipPhone = "DealershipPhone";
        public const string DealershipId = "DealershipId";
        public const string DealerCost = "DealerCost";
        public const string ACV = "ACV";
        public const string DefaultImageUrl = "DefaultImageUrl";
        public const string NewUsed = "NewUsed";
        public const string Age = "Age";
        public const string AddToInventoryBy = "AddToInventoryBy";
        public const string AppraisalId = "AppraisalId";
        public const string DefaultImageURL = "DefaultImageURL";
        public const string FuelEconomyCity = "FuelEconomyCity";
        public const string FuelEconomyHighWay = "FuelEconomyHighWay";
        public const string VehicleType = "VehicleType";
        public const string TruckType = "TruckType";
        public const string TruckCategory = "TruckCategory";
        public const string TruckClass = "TruckClass";
        public const string IsTruck = "IsTruck";
        public const string Recon = "Recon";
        public const string VehicleStatus = "VehicleStatus";
        public const string StandardOptions = "StandardOptions";
        public const string RetailPrice = "RetailPrice";
        public const string DealerDiscount = "DealerDiscount";
        public const string WindowStickerPrice = "WindowStickerPrice";
        public const string ManufacturerRebate = "ManufacturerRebate";
        public const string CarFaxOwner = "CarFaxOwner";
        public const string KBBTrimId = "KBBTrimId";
        public const string KBBTrimOption = "KBBTrimOption";
        public const string WholeSale = "WholeSale";
        public const string PriorRental = "PriorRental";
        #endregion

        #region Public Methods

        public string CreateMappingTemplate(MappingViewModel model)
        {
            var xmlDoc = new XmlDocument();

            var root = xmlDoc.CreateElement(Node_Root);
            root.SetAttribute(Attribute_Header, model.HasHeader.ToString());
            root.SetAttribute(Attribute_Delimeter, model.Delimeter);
            root.SetAttribute(Attribute_ExportFileType, model.ExportFileType);
            root.SetAttribute(Attribute_InventoryStatus, model.InventoryStatus);
            root.SetAttribute(Attribute_SplitImage, model.SplitImage.ToString());
            xmlDoc.AppendChild(root);

            foreach (var item in model.Mappings)
            {
                var mappingNode = xmlDoc.CreateElement(Node_Mapping);
                mappingNode.SetAttribute(Attribute_Order, item.Order.ToString());

                var dbFieldNode = xmlDoc.CreateElement(Node_DBField);
                dbFieldNode.SetAttribute(Attribute_Name, item.DBField);
                mappingNode.AppendChild(dbFieldNode);
                if (item.Replaces != null && item.Replaces.Any())
                {
                    var replacesNode = xmlDoc.CreateElement(Node_Replaces);
                    foreach (var replace in item.Replaces)
                    {
                        var replaceNode = xmlDoc.CreateElement(Node_Replace);
                        replaceNode.SetAttribute(Attribute_From, replace.From);
                        replaceNode.SetAttribute(Attribute_To, replace.To);
                        replacesNode.AppendChild(replaceNode);
                    }
                    dbFieldNode.AppendChild(replacesNode);
                }
                if (item.Conditions != null && item.Conditions.Any())
                {
                    var conditionsNode = xmlDoc.CreateElement(Node_Conditions);
                    foreach (var condition in item.Conditions)
                    {
                        var conditionNode = xmlDoc.CreateElement(Node_Condition);
                        conditionNode.SetAttribute(Attribute_XMLField, condition.XMLField);
                        conditionNode.SetAttribute(Attribute_Operator, condition.Operator);
                        conditionNode.SetAttribute(Attribute_ComparedValue, condition.ComparedValue);
                        conditionNode.SetAttribute(Attribute_DBField, condition.DBField);
                        conditionNode.SetAttribute(Attribute_TargetValue, condition.TargetValue);
                        conditionNode.SetAttribute(Attribute_Type, condition.Type);
                        conditionsNode.AppendChild(conditionNode);
                    }
                    dbFieldNode.AppendChild(conditionsNode);
                }

                if (item.Expression != null)
                {
                    var expressionNode = xmlDoc.CreateElement(Node_Expression);
                    expressionNode.SetAttribute("XMLField", item.Expression.XMLField);
                    expressionNode.SetAttribute("DBField1", item.Expression.DBField1);
                    expressionNode.SetAttribute("Operator1", item.Expression.Operator1);
                    expressionNode.SetAttribute("DBField2", item.Expression.DBField2);
                    expressionNode.SetAttribute("Operator2", item.Expression.Operator2);
                    expressionNode.SetAttribute("DBField3", item.Expression.DBField3);
                    mappingNode.AppendChild(expressionNode);
                }

                var xmlFieldNode = xmlDoc.CreateElement(Node_XMLField);
                xmlFieldNode.SetAttribute(Attribute_Name, item.XMLField);
                mappingNode.AppendChild(xmlFieldNode);
                
                root.AppendChild(mappingNode);
            }

            return root.OuterXml;
        }

        public MappingViewModel LoadMappingTemplateByCompanyId(int companyId)
        {
            using (var context = new vincontrolwarehouseEntities())
            {
                var existingImportTemplate = context.datafeedprofiles.FirstOrDefault(i => i.Id==companyId);
                if (existingImportTemplate != null)
                {
                    var result = !String.IsNullOrEmpty(existingImportTemplate.Mapping)
                                     ? LoadMappingTemplate(existingImportTemplate.Mapping)
                                     : new MappingViewModel();
                    result.ProfileName = existingImportTemplate.ProfileName;
                    result.FTPHost = existingImportTemplate.FTPServer;
                    result.FTPUserName = existingImportTemplate.DefaultUserName;
                    result.FTPPassword = existingImportTemplate.DefaultPassword;
                    result.Frequency = existingImportTemplate.Frequency??0;
                    result.RunningTime = existingImportTemplate.RunningTime;
                    result.Bundle = existingImportTemplate.Bundle??false;
                    result.FileName = existingImportTemplate.FileName;
                    result.Discontinued = existingImportTemplate.Discontinued??false;
                    result.CompanyName = existingImportTemplate.CompanyName;
                    result.Email = existingImportTemplate.Email;
                    return result;
                }
                return null;
            }
        }

        public MappingViewModel LoadMappingTemplateByCompany(int companyProfileId)
        {
            using (var context = new vincontrolwarehouseEntities())
            {
                var existingImportTemplate = context.datafeedprofiles.FirstOrDefault(i => i.Id == companyProfileId);

                return existingImportTemplate != null && !String.IsNullOrEmpty(existingImportTemplate.Mapping)
                           ? LoadMappingTemplate(existingImportTemplate.Mapping)
                           : LoadMappingTemplate(CreateMappingTemplate(_commonHelper.CreateSampleMappingData()));
            }
        }

        public MappingViewModel LoadMappingTemplate(string xmlContent)
        {
            var model = new MappingViewModel() { Mappings = new List<Mapping>() { } };
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlContent);
            var root = xmlDoc.DocumentElement;
            if (root != null)
            {
                model.Delimeter = _commonHelper.GetStringValueFromAttribute(root, Attribute_Delimeter);
                model.HasHeader = _commonHelper.GetBoolValueFromAttribute(root, Attribute_Header);
                model.ExportFileType = _commonHelper.GetStringValueFromAttribute(root, Attribute_ExportFileType);
                model.InventoryStatus = _commonHelper.GetStringValueFromAttribute(root, Attribute_InventoryStatus);
                model.SplitImage = _commonHelper.GetBoolValueFromAttribute(root, Attribute_SplitImage);
                XmlNodeList mappingNodes = root.SelectNodes(Node_Mapping);
                if (mappingNodes != null)
                {
                    foreach (XmlNode mappingNode in mappingNodes)
                    {
                        var order = _commonHelper.GetIntValueFromAttribute((XmlElement)mappingNode, Attribute_Order);
                        var dbFieldNode = mappingNode.SelectSingleNode(Node_DBField);
                        var xmlFieldNode = mappingNode.SelectSingleNode(Node_XMLField);
                        var expressionNode = mappingNode.SelectSingleNode(Node_Expression);

                        if (dbFieldNode != null && xmlFieldNode != null)
                        {
                            var mapping = new Mapping() { Order = order, DBField = _commonHelper.GetStringValueFromAttribute((XmlElement)dbFieldNode, Attribute_Name), XMLField = _commonHelper.GetStringValueFromAttribute((XmlElement)xmlFieldNode, Attribute_Name) };
                            if (expressionNode != null)
                            {
                                mapping.Expression = new Expression()
                                                         {
                                                             XMLField = _commonHelper.GetStringValueFromAttribute((XmlElement) expressionNode, "XMLField"),
                                                             DBField1 = _commonHelper.GetStringValueFromAttribute((XmlElement) expressionNode, "DBField1"),
                                                             Operator1 = _commonHelper.GetStringValueFromAttribute((XmlElement) expressionNode, "Operator1"),
                                                             DBField2 = _commonHelper.GetStringValueFromAttribute((XmlElement) expressionNode, "DBField2"),
                                                             Operator2 = _commonHelper.GetStringValueFromAttribute((XmlElement) expressionNode, "Operator2"),
                                                             DBField3 = _commonHelper.GetStringValueFromAttribute((XmlElement) expressionNode, "DBField3")
                                                         };
                            }

                            var replacesNode = dbFieldNode.SelectSingleNode(Node_Replaces);
                            if (replacesNode != null)
                            {
                                var replaceNodeList = replacesNode.SelectNodes(Node_Replace);
                                if (replaceNodeList != null)
                                {
                                    mapping.Replaces = new List<Replacement>() { };
                                    foreach (XmlNode item in replaceNodeList)
                                    {
                                        mapping.Replaces.Add(new Replacement() { From = _commonHelper.GetStringValueFromAttribute((XmlElement)item, Attribute_From), To = _commonHelper.GetStringValueFromAttribute((XmlElement)item, Attribute_To) });
                                    }
                                }
                            }

                            var conditionsNode = dbFieldNode.SelectSingleNode(Node_Conditions);
                            if (conditionsNode != null)
                            {
                                var conditionNodeList = conditionsNode.SelectNodes(Node_Condition);
                                if (conditionNodeList != null)
                                {
                                    mapping.Conditions = new List<Condition>() { };
                                    foreach (XmlNode item in conditionNodeList)
                                    {
                                        mapping.Conditions.Add(new Condition()
                                        {
                                            XMLField = _commonHelper.GetStringValueFromAttribute((XmlElement)item, Attribute_XMLField),
                                            Operator = _commonHelper.GetStringValueFromAttribute((XmlElement)item, Attribute_Operator),
                                            ComparedValue = _commonHelper.GetStringValueFromAttribute((XmlElement)item, Attribute_ComparedValue),
                                            DBField = _commonHelper.GetStringValueFromAttribute((XmlElement)item, Attribute_DBField),
                                            TargetValue = _commonHelper.GetStringValueFromAttribute((XmlElement)item, Attribute_TargetValue),
                                            Type = _commonHelper.GetStringValueFromAttribute((XmlElement)item, Attribute_Type)
                                        });
                                    }
                                }
                            }

                            model.Mappings.Add(mapping);
                        }
                    }
                }
            }

            return model;
        }
        
        public XmlElement CreateFieldElement(XmlDocument doc, string node, string name, string value)
        {
            var field = doc.CreateElement(node);
            field.SetAttribute(name, value);

            return field;
        }
        
        public string ApplyReplacement(IList<Replacement> replacements, string value)
        {
            if (replacements != null && replacements.Count > 0)
            {
                value = replacements.Aggregate(value, (current, replacement) => current.Replace(replacement.From, replacement.To));
            }

            return value;
        }

        public string GetValueAfterApplyCondition(VehicleViewModel model, Mapping mapping, string value)
        {
            if (mapping == null) return null;
            if (mapping.Conditions != null && mapping.Conditions.Any())
            {
                foreach (var condition in mapping.Conditions)
                {
                    switch (mapping.DBField)
                    {
                        case StockNumber:
                            if (!String.IsNullOrEmpty(model.StockNumber))
                            {
                                switch (condition.Operator)
                                {
                                    case Operators.Equal:
                                        if (model.StockNumber == condition.ComparedValue) return condition.TargetValue;
                                        break;
                                    case Operators.Different:
                                        if (model.StockNumber != condition.ComparedValue) return condition.TargetValue;
                                        break;
                                    case Operators.Contain:
                                        if (model.StockNumber.Contains(condition.ComparedValue))
                                            return condition.TargetValue;
                                        break;
                                    case Operators.StartWith:
                                        if (model.StockNumber.Length >= condition.ComparedValue.Length &&
                                            model.StockNumber.Substring(0, condition.ComparedValue.Length) == condition.ComparedValue)
                                            return condition.TargetValue;
                                        break;
                                    case Operators.EndWith:
                                        if (model.StockNumber.Length >= condition.ComparedValue.Length &&
                                            model.StockNumber.Substring(model.StockNumber.Length - condition.ComparedValue.Length, condition.ComparedValue.Length) ==
                                            condition.ComparedValue) return condition.TargetValue;
                                        break;
                                    default:
                                        break;
                                }
                            }
                            break;
                        case Make:
                            if (!String.IsNullOrEmpty(model.Make))
                            {
                                switch (condition.Operator)
                                {
                                    case Operators.Equal:
                                        if (model.Make == condition.ComparedValue) return condition.TargetValue;
                                        break;
                                    case Operators.Different:
                                        if (model.Make != condition.ComparedValue) return condition.TargetValue;
                                        break;
                                    case Operators.Contain:
                                        if (model.Make.Contains(condition.ComparedValue)) return condition.TargetValue;
                                        break;
                                    case Operators.StartWith:
                                        if (model.Make.Length > condition.ComparedValue.Length &&
                                            model.Make.Substring(0, condition.ComparedValue.Length) == condition.ComparedValue)
                                            return condition.TargetValue;
                                        break;
                                    case Operators.EndWith:
                                        if (model.Make.Length > condition.ComparedValue.Length &&
                                            model.Make.Substring(model.Make.Length - condition.ComparedValue.Length, condition.ComparedValue.Length) == condition.ComparedValue)
                                            return condition.TargetValue;
                                        break;
                                    default:
                                        break;
                                }
                            }
                            break;
                        case Model:
                            if (!String.IsNullOrEmpty(model.Model))
                            {
                                switch (condition.Operator)
                                {
                                    case Operators.Equal:
                                        if (model.Model == condition.ComparedValue) return condition.TargetValue;
                                        break;
                                    case Operators.Different:
                                        if (model.Model != condition.ComparedValue) return condition.TargetValue;
                                        break;
                                    case Operators.Contain:
                                        if (model.Model.Contains(condition.ComparedValue)) return condition.TargetValue;
                                        break;
                                    case Operators.StartWith:
                                        if (model.Model.Length >= condition.ComparedValue.Length &&
                                            model.Model.Substring(0, condition.ComparedValue.Length) == condition.ComparedValue)
                                            return condition.TargetValue;
                                        break;
                                    case Operators.EndWith:
                                        if (model.Model.Length >= condition.ComparedValue.Length &&
                                            model.Model.Substring(model.Model.Length - condition.ComparedValue.Length, condition.ComparedValue.Length) == condition.ComparedValue)
                                            return condition.TargetValue;
                                        break;
                                    default:
                                        break;
                                }
                            }
                            break;
                        case Trim:
                            if (!String.IsNullOrEmpty(model.Trim))
                            {
                                switch (condition.Operator)
                                {
                                    case Operators.Equal:
                                        if (model.Trim == condition.ComparedValue) return condition.TargetValue;
                                        break;
                                    case Operators.Different:
                                        if (model.Trim != condition.ComparedValue) return condition.TargetValue;
                                        break;
                                    case Operators.Contain:
                                        if (model.Trim.Contains(condition.ComparedValue)) return condition.TargetValue;
                                        break;
                                    case Operators.StartWith:
                                        if (model.Trim.Length >= condition.ComparedValue.Length &&
                                            model.Trim.Substring(0, condition.ComparedValue.Length) == condition.ComparedValue)
                                            return condition.TargetValue;
                                        break;
                                    case Operators.EndWith:
                                        if (model.Trim.Length >= condition.ComparedValue.Length &&
                                            model.Trim.Substring(model.Trim.Length - condition.ComparedValue.Length, condition.ComparedValue.Length) == condition.ComparedValue)
                                            return condition.TargetValue;
                                        break;
                                    default:
                                        break;
                                }
                            }
                            break;
                        case Vin:
                            if (!String.IsNullOrEmpty(model.Vin))
                            {
                                switch (condition.Operator)
                                {
                                    case Operators.Equal:
                                        if (model.Vin == condition.ComparedValue) return condition.TargetValue;
                                        break;
                                    case Operators.Different:
                                        if (model.Vin != condition.ComparedValue) return condition.TargetValue;
                                        break;
                                    case Operators.Contain:
                                        if (model.Vin.Contains(condition.ComparedValue)) return condition.TargetValue;
                                        break;
                                    case Operators.StartWith:
                                        if (model.Vin.Length >= condition.ComparedValue.Length &&
                                            model.Vin.Substring(0, condition.ComparedValue.Length) == condition.ComparedValue)
                                            return condition.TargetValue;
                                        break;
                                    case Operators.EndWith:
                                        if (model.Vin.Length >= condition.ComparedValue.Length &&
                                            model.Vin.Substring(model.Vin.Length - condition.ComparedValue.Length, condition.ComparedValue.Length) == condition.ComparedValue)
                                            return condition.TargetValue;
                                        break;
                                    default:
                                        break;
                                }
                            }
                            break;
                        case Mileage:
                            if (!String.IsNullOrEmpty(model.Mileage))
                            {
                                switch (condition.Operator)
                                {
                                    case Operators.Equal:
                                        if (model.Mileage == condition.ComparedValue) return condition.TargetValue;
                                        break;
                                    case Operators.Different:
                                        if (model.Mileage != condition.ComparedValue) return condition.TargetValue;
                                        break;
                                    case Operators.Contain:
                                        if (model.Mileage.Contains(condition.ComparedValue))
                                            return condition.TargetValue;
                                        break;
                                    case Operators.StartWith:
                                        if (model.Mileage.Length >= condition.ComparedValue.Length &&
                                            model.Mileage.Substring(0, condition.ComparedValue.Length) == condition.ComparedValue)
                                            return condition.TargetValue;
                                        break;
                                    case Operators.EndWith:
                                        if (model.Mileage.Length >= condition.ComparedValue.Length &&
                                            model.Mileage.Substring(model.Mileage.Length - condition.ComparedValue.Length, condition.ComparedValue.Length) ==
                                            condition.ComparedValue) return condition.TargetValue;
                                        break;
                                    default:
                                        break;
                                }
                            }
                            break;
                        case SalePrice:
                            if (!String.IsNullOrEmpty(model.SalePrice))
                            {
                                switch (condition.Operator)
                                {
                                    case Operators.Equal:
                                        if (model.SalePrice == condition.ComparedValue) return condition.TargetValue;
                                        break;
                                    case Operators.Different:
                                        if (model.SalePrice != condition.ComparedValue) return condition.TargetValue;
                                        break;
                                    case Operators.Contain:
                                        if (model.SalePrice.Contains(condition.ComparedValue))
                                            return condition.TargetValue;
                                        break;
                                    case Operators.StartWith:
                                        if (model.SalePrice.Length >= condition.ComparedValue.Length &&
                                            model.SalePrice.Substring(0, condition.ComparedValue.Length) == condition.ComparedValue)
                                            return condition.TargetValue;
                                        break;
                                    case Operators.EndWith:
                                        if (model.SalePrice.Length >= condition.ComparedValue.Length &&
                                            model.SalePrice.Substring(model.SalePrice.Length - condition.ComparedValue.Length, condition.ComparedValue.Length) ==
                                            condition.ComparedValue) return condition.TargetValue;
                                        break;
                                    default:
                                        break;
                                }
                            }
                            break;
                        case NewUsed:
                            if (!String.IsNullOrEmpty(model.NewUsed))
                            {
                                switch (condition.Operator)
                                {
                                    case Operators.Equal:
                                        if (model.NewUsed == condition.ComparedValue) return condition.TargetValue;
                                        break;
                                    case Operators.Different:
                                        if (model.NewUsed != condition.ComparedValue) return condition.TargetValue;
                                        break;
                                    case Operators.Contain:
                                        if (model.NewUsed.Contains(condition.ComparedValue))
                                            return condition.TargetValue;
                                        break;
                                    case Operators.StartWith:
                                        if (model.NewUsed.Length >= condition.ComparedValue.Length &&
                                            model.NewUsed.Substring(0, condition.ComparedValue.Length) == condition.ComparedValue)
                                            return condition.TargetValue;
                                        break;
                                    case Operators.EndWith:
                                        if (model.NewUsed.Length >= condition.ComparedValue.Length &&
                                            model.NewUsed.Substring(model.NewUsed.Length - condition.ComparedValue.Length, condition.ComparedValue.Length) ==
                                            condition.ComparedValue) return condition.TargetValue;
                                        break;
                                    default:
                                        break;
                                }
                            }
                            break;
                        case ExteriorColor:
                            if (!String.IsNullOrEmpty(model.ExteriorColor))
                            {
                                switch (condition.Operator)
                                {
                                    case Operators.Equal:
                                        if (model.ExteriorColor == condition.ComparedValue)
                                            return condition.TargetValue;
                                        break;
                                    case Operators.Different:
                                        if (model.ExteriorColor != condition.ComparedValue)
                                            return condition.TargetValue;
                                        break;
                                    case Operators.Contain:
                                        if (model.ExteriorColor.Contains(condition.ComparedValue))
                                            return condition.TargetValue;
                                        break;
                                    case Operators.StartWith:
                                        if (model.ExteriorColor.Length >= condition.ComparedValue.Length &&
                                            model.ExteriorColor.Substring(0, condition.ComparedValue.Length) == condition.ComparedValue)
                                            return condition.TargetValue;
                                        break;
                                    case Operators.EndWith:
                                        if (model.ExteriorColor.Length >= condition.ComparedValue.Length &&
                                            model.ExteriorColor.Substring(model.ExteriorColor.Length - condition.ComparedValue.Length, condition.ComparedValue.Length) ==
                                            condition.ComparedValue) return condition.TargetValue;
                                        break;
                                    default:
                                        break;
                                }
                            }
                            break;
                        case InteriorColor:
                            if (!String.IsNullOrEmpty(model.InteriorColor))
                            {
                                switch (condition.Operator)
                                {
                                    case Operators.Equal:
                                        if (model.InteriorColor == condition.ComparedValue)
                                            return condition.TargetValue;
                                        break;
                                    case Operators.Different:
                                        if (model.InteriorColor != condition.ComparedValue)
                                            return condition.TargetValue;
                                        break;
                                    case Operators.Contain:
                                        if (model.InteriorColor.Contains(condition.ComparedValue))
                                            return condition.TargetValue;
                                        break;
                                    case Operators.StartWith:
                                        if (model.InteriorColor.Length >= condition.ComparedValue.Length &&
                                            model.InteriorColor.Substring(0, condition.ComparedValue.Length) == condition.ComparedValue)
                                            return condition.TargetValue;
                                        break;
                                    case Operators.EndWith:
                                        if (model.InteriorColor.Length >= condition.ComparedValue.Length &&
                                            model.InteriorColor.Substring(model.InteriorColor.Length - condition.ComparedValue.Length, condition.ComparedValue.Length) ==
                                            condition.ComparedValue) return condition.TargetValue;
                                        break;
                                    default:
                                        break;
                                }
                            }
                            break;
                        case InteriorSurface:
                            if (!String.IsNullOrEmpty(model.InteriorSurface))
                            {
                                switch (condition.Operator)
                                {
                                    case Operators.Equal:
                                        if (model.InteriorSurface == condition.ComparedValue)
                                            return condition.TargetValue;
                                        break;
                                    case Operators.Different:
                                        if (model.InteriorSurface != condition.ComparedValue)
                                            return condition.TargetValue;
                                        break;
                                    case Operators.Contain:
                                        if (model.InteriorSurface.Contains(condition.ComparedValue))
                                            return condition.TargetValue;
                                        break;
                                    case Operators.StartWith:
                                        if (model.InteriorSurface.Substring(0, 1) == condition.ComparedValue)
                                            return condition.TargetValue;
                                        break;
                                    case Operators.EndWith:
                                        if (model.InteriorSurface.Substring(model.InteriorSurface.Length - 1, 1) ==
                                            condition.ComparedValue) return condition.TargetValue;
                                        break;
                                    default:
                                        break;
                                }
                            }
                            break;
                        case WindowStickerPrice:
                            if (!String.IsNullOrEmpty(model.WindowStickerPrice))
                            {
                                switch (condition.Operator)
                                {
                                    case Operators.Equal:
                                        if (model.WindowStickerPrice == condition.ComparedValue)
                                            return condition.TargetValue;
                                        break;
                                    case Operators.Different:
                                        if (model.WindowStickerPrice != condition.ComparedValue)
                                            return condition.TargetValue;
                                        break;
                                    case Operators.Contain:
                                        if (model.WindowStickerPrice.Contains(condition.ComparedValue))
                                            return condition.TargetValue;
                                        break;
                                    case Operators.StartWith:
                                        if (model.WindowStickerPrice.Substring(0, 1) == condition.ComparedValue)
                                            return condition.TargetValue;
                                        break;
                                    case Operators.EndWith:
                                        if (
                                            model.WindowStickerPrice.Substring(model.WindowStickerPrice.Length - 1, 1) ==
                                            condition.ComparedValue) return condition.TargetValue;
                                        break;
                                    default:
                                        break;
                                }
                            }
                            break;
                        case MSRP:
                            if (!String.IsNullOrEmpty(model.MSRP))
                            {
                                switch (condition.Operator)
                                {
                                    case Operators.Equal:
                                        if (model.MSRP == condition.ComparedValue) return condition.TargetValue;
                                        break;
                                    case Operators.Different:
                                        if (model.MSRP != condition.ComparedValue) return condition.TargetValue;
                                        break;
                                    case Operators.Contain:
                                        if (model.MSRP.Contains(condition.ComparedValue)) return condition.TargetValue;
                                        break;
                                    case Operators.StartWith:
                                        if (model.MSRP.Substring(0, 1) == condition.ComparedValue)
                                            return condition.TargetValue;
                                        break;
                                    case Operators.EndWith:
                                        if (model.MSRP.Substring(model.MSRP.Length - 1, 1) == condition.ComparedValue)
                                            return condition.TargetValue;
                                        break;
                                    default:
                                        break;
                                }
                            }
                            break;
                        case BodyType:
                            if (!String.IsNullOrEmpty(model.BodyType))
                            {
                                switch (condition.Operator)
                                {
                                    case Operators.Equal:
                                        if (model.BodyType == condition.ComparedValue) return condition.TargetValue;
                                        break;
                                    case Operators.Different:
                                        if (model.BodyType != condition.ComparedValue) return condition.TargetValue;
                                        break;
                                    case Operators.Contain:
                                        if (model.BodyType.Contains(condition.ComparedValue))
                                            return condition.TargetValue;
                                        break;
                                    case Operators.StartWith:
                                        if (model.BodyType.Substring(0, 1) == condition.ComparedValue)
                                            return condition.TargetValue;
                                        break;
                                    case Operators.EndWith:
                                        if (model.BodyType.Substring(model.BodyType.Length - 1, 1) ==
                                            condition.ComparedValue) return condition.TargetValue;
                                        break;
                                    default:
                                        break;
                                }
                            }
                            break;
                        case EngineType:
                            if (!String.IsNullOrEmpty(model.EngineType))
                            {
                                switch (condition.Operator)
                                {
                                    case Operators.Equal:
                                        if (model.EngineType == condition.ComparedValue) return condition.TargetValue;
                                        break;
                                    case Operators.Different:
                                        if (model.EngineType != condition.ComparedValue) return condition.TargetValue;
                                        break;
                                    case Operators.Contain:
                                        if (model.EngineType.Contains(condition.ComparedValue))
                                            return condition.TargetValue;
                                        break;
                                    case Operators.StartWith:
                                        if (model.EngineType.Substring(0, 1) == condition.ComparedValue)
                                            return condition.TargetValue;
                                        break;
                                    case Operators.EndWith:
                                        if (model.EngineType.Substring(model.EngineType.Length - 1, 1) ==
                                            condition.ComparedValue) return condition.TargetValue;
                                        break;
                                    default:
                                        break;
                                }
                            }
                            break;
                        case DriveTrain:
                            if (!String.IsNullOrEmpty(model.DriveTrain))
                            {
                                switch (condition.Operator)
                                {
                                    case Operators.Equal:
                                        if (model.DriveTrain == condition.ComparedValue) return condition.TargetValue;
                                        break;
                                    case Operators.Different:
                                        if (model.DriveTrain != condition.ComparedValue) return condition.TargetValue;
                                        break;
                                    case Operators.Contain:
                                        if (model.DriveTrain.Contains(condition.ComparedValue))
                                            return condition.TargetValue;
                                        break;
                                    case Operators.StartWith:
                                        if (model.DriveTrain.Substring(0, 1) == condition.ComparedValue)
                                            return condition.TargetValue;
                                        break;
                                    case Operators.EndWith:
                                        if (model.DriveTrain.Substring(model.DriveTrain.Length - 1, 1) ==
                                            condition.ComparedValue) return condition.TargetValue;
                                        break;
                                    default:
                                        break;
                                }
                            }
                            break;
                        case Cylinders:
                            if (!String.IsNullOrEmpty(model.Cylinders))
                            {
                                switch (condition.Operator)
                                {
                                    case Operators.Equal:
                                        if (model.Cylinders == condition.ComparedValue) return condition.TargetValue;
                                        break;
                                    case Operators.Different:
                                        if (model.Cylinders != condition.ComparedValue) return condition.TargetValue;
                                        break;
                                    case Operators.Contain:
                                        if (model.Cylinders.Contains(condition.ComparedValue))
                                            return condition.TargetValue;
                                        break;
                                    case Operators.StartWith:
                                        if (model.Cylinders.Substring(0, 1) == condition.ComparedValue)
                                            return condition.TargetValue;
                                        break;
                                    case Operators.EndWith:
                                        if (model.Cylinders.Substring(model.Cylinders.Length - 1, 1) ==
                                            condition.ComparedValue) return condition.TargetValue;
                                        break;
                                    default:
                                        break;
                                }
                            }
                            break;
                        case Liters:
                            if (!String.IsNullOrEmpty(model.Liters))
                            {
                                switch (condition.Operator)
                                {
                                    case Operators.Equal:
                                        if (model.Liters == condition.ComparedValue) return condition.TargetValue;
                                        break;
                                    case Operators.Different:
                                        if (model.Liters != condition.ComparedValue) return condition.TargetValue;
                                        break;
                                    case Operators.Contain:
                                        if (model.Liters.Contains(condition.ComparedValue))
                                            return condition.TargetValue;
                                        break;
                                    case Operators.StartWith:
                                        if (model.Liters.Substring(0, 1) == condition.ComparedValue)
                                            return condition.TargetValue;
                                        break;
                                    case Operators.EndWith:
                                        if (model.Liters.Substring(model.Liters.Length - 1, 1) ==
                                            condition.ComparedValue) return condition.TargetValue;
                                        break;
                                    default:
                                        break;
                                }
                            }
                            break;
                        case FuelType:
                            if (!String.IsNullOrEmpty(model.FuelType))
                            {
                                switch (condition.Operator)
                                {
                                    case Operators.Equal:
                                        if (model.FuelType == condition.ComparedValue) return condition.TargetValue;
                                        break;
                                    case Operators.Different:
                                        if (model.FuelType != condition.ComparedValue) return condition.TargetValue;
                                        break;
                                    case Operators.Contain:
                                        if (model.FuelType.Contains(condition.ComparedValue))
                                            return condition.TargetValue;
                                        break;
                                    case Operators.StartWith:
                                        if (model.FuelType.Substring(0, 1) == condition.ComparedValue)
                                            return condition.TargetValue;
                                        break;
                                    case Operators.EndWith:
                                        if (model.FuelType.Substring(model.FuelType.Length - 1, 1) ==
                                            condition.ComparedValue) return condition.TargetValue;
                                        break;
                                    default:
                                        break;
                                }
                            }
                            break;
                        case Tranmission:
                            if (!String.IsNullOrEmpty(model.Tranmission))
                            {
                                switch (condition.Operator)
                                {
                                    case Operators.Equal:
                                        if (model.Tranmission == condition.ComparedValue) return condition.TargetValue;
                                        break;
                                    case Operators.Different:
                                        if (model.Tranmission != condition.ComparedValue) return condition.TargetValue;
                                        break;
                                    case Operators.Contain:
                                        if (model.Tranmission.Contains(condition.ComparedValue))
                                            return condition.TargetValue;
                                        break;
                                    case Operators.StartWith:
                                        if (model.Tranmission.Substring(0, 1) == condition.ComparedValue)
                                            return condition.TargetValue;
                                        break;
                                    case Operators.EndWith:
                                        if (model.Tranmission.Substring(model.Tranmission.Length - 1, 1) ==
                                            condition.ComparedValue) return condition.TargetValue;
                                        break;
                                    default:
                                        break;
                                }
                            }
                            break;
                        case CarsOptions:
                            if (!String.IsNullOrEmpty(model.CarsOptions))
                            {
                                switch (condition.Operator)
                                {
                                    case Operators.Equal:
                                        if (model.CarsOptions == condition.ComparedValue) return condition.TargetValue;
                                        break;
                                    case Operators.Different:
                                        if (model.CarsOptions != condition.ComparedValue) return condition.TargetValue;
                                        break;
                                    case Operators.Contain:
                                        if (model.CarsOptions.Contains(condition.ComparedValue))
                                            return condition.TargetValue;
                                        break;
                                    case Operators.StartWith:
                                        if (model.CarsOptions.Substring(0, 1) == condition.ComparedValue)
                                            return condition.TargetValue;
                                        break;
                                    case Operators.EndWith:
                                        if (model.CarsOptions.Substring(model.CarsOptions.Length - 1, 1) ==
                                            condition.ComparedValue) return condition.TargetValue;
                                        break;
                                    default:
                                        break;
                                }
                            }
                            break;
                        case CarImageUrl:
                            if (!String.IsNullOrEmpty(model.CarImageUrl))
                            {
                                switch (condition.Operator)
                                {
                                    case Operators.Equal:
                                        if (model.CarImageUrl == condition.ComparedValue) return condition.TargetValue;
                                        break;
                                    case Operators.Different:
                                        if (model.CarImageUrl != condition.ComparedValue) return condition.TargetValue;
                                        break;
                                    case Operators.Contain:
                                        if (model.CarImageUrl.Contains(condition.ComparedValue))
                                            return condition.TargetValue;
                                        break;
                                    case Operators.StartWith:
                                        if (model.CarImageUrl.Substring(0, 1) == condition.ComparedValue)
                                            return condition.TargetValue;
                                        break;
                                    case Operators.EndWith:
                                        if (model.CarImageUrl.Substring(model.CarImageUrl.Length - 1, 1) ==
                                            condition.ComparedValue) return condition.TargetValue;
                                        break;
                                    default:
                                        break;
                                }
                            }
                            break;
                        case DealershipName:
                            if (!String.IsNullOrEmpty(model.DealershipName))
                            {
                                switch (condition.Operator)
                                {
                                    case Operators.Equal:
                                        if (model.DealershipName == condition.ComparedValue)
                                            return condition.TargetValue;
                                        break;
                                    case Operators.Different:
                                        if (model.DealershipName != condition.ComparedValue)
                                            return condition.TargetValue;
                                        break;
                                    case Operators.Contain:
                                        if (model.DealershipName.Contains(condition.ComparedValue))
                                            return condition.TargetValue;
                                        break;
                                    case Operators.StartWith:
                                        if (model.DealershipName.Substring(0, 1) == condition.ComparedValue)
                                            return condition.TargetValue;
                                        break;
                                    case Operators.EndWith:
                                        if (model.DealershipName.Substring(model.DealershipName.Length - 1, 1) ==
                                            condition.ComparedValue) return condition.TargetValue;
                                        break;
                                    default:
                                        break;
                                }
                            }
                            break;
                        case DealershipAddress:
                            if (!String.IsNullOrEmpty(model.DealershipAddress))
                            {
                                switch (condition.Operator)
                                {
                                    case Operators.Equal:
                                        if (model.DealershipAddress == condition.ComparedValue)
                                            return condition.TargetValue;
                                        break;
                                    case Operators.Different:
                                        if (model.DealershipAddress != condition.ComparedValue)
                                            return condition.TargetValue;
                                        break;
                                    case Operators.Contain:
                                        if (model.DealershipAddress.Contains(condition.ComparedValue))
                                            return condition.TargetValue;
                                        break;
                                    case Operators.StartWith:
                                        if (model.DealershipAddress.Substring(0, 1) == condition.ComparedValue)
                                            return condition.TargetValue;
                                        break;
                                    case Operators.EndWith:
                                        if (model.DealershipAddress.Substring(model.DealershipAddress.Length - 1, 1) ==
                                            condition.ComparedValue) return condition.TargetValue;
                                        break;
                                    default:
                                        break;
                                }
                            }
                            break;
                        case DealershipCity:
                            if (!String.IsNullOrEmpty(model.DealershipCity))
                            {
                                switch (condition.Operator)
                                {
                                    case Operators.Equal:
                                        if (model.DealershipCity == condition.ComparedValue)
                                            return condition.TargetValue;
                                        break;
                                    case Operators.Different:
                                        if (model.DealershipCity != condition.ComparedValue)
                                            return condition.TargetValue;
                                        break;
                                    case Operators.Contain:
                                        if (model.DealershipCity.Contains(condition.ComparedValue))
                                            return condition.TargetValue;
                                        break;
                                    case Operators.StartWith:
                                        if (model.DealershipCity.Substring(0, 1) == condition.ComparedValue)
                                            return condition.TargetValue;
                                        break;
                                    case Operators.EndWith:
                                        if (model.DealershipCity.Substring(model.DealershipCity.Length - 1, 1) ==
                                            condition.ComparedValue) return condition.TargetValue;
                                        break;
                                    default:
                                        break;
                                }
                            }
                            break;
                        case DealershipState:
                            if (!String.IsNullOrEmpty(model.DealershipState))
                            {
                                switch (condition.Operator)
                                {
                                    case Operators.Equal:
                                        if (model.DealershipState == condition.ComparedValue)
                                            return condition.TargetValue;
                                        break;
                                    case Operators.Different:
                                        if (model.DealershipState != condition.ComparedValue)
                                            return condition.TargetValue;
                                        break;
                                    case Operators.Contain:
                                        if (model.DealershipState.Contains(condition.ComparedValue))
                                            return condition.TargetValue;
                                        break;
                                    case Operators.StartWith:
                                        if (model.DealershipState.Substring(0, 1) == condition.ComparedValue)
                                            return condition.TargetValue;
                                        break;
                                    case Operators.EndWith:
                                        if (model.DealershipState.Substring(model.DealershipState.Length - 1, 1) ==
                                            condition.ComparedValue) return condition.TargetValue;
                                        break;
                                    default:
                                        break;
                                }
                            }
                            break;
                        case DealershipZipCode:
                            if (!String.IsNullOrEmpty(model.DealershipZipCode))
                            {
                                switch (condition.Operator)
                                {
                                    case Operators.Equal:
                                        if (model.DealershipZipCode == condition.ComparedValue)
                                            return condition.TargetValue;
                                        break;
                                    case Operators.Different:
                                        if (model.DealershipZipCode != condition.ComparedValue)
                                            return condition.TargetValue;
                                        break;
                                    case Operators.Contain:
                                        if (model.DealershipZipCode.Contains(condition.ComparedValue))
                                            return condition.TargetValue;
                                        break;
                                    case Operators.StartWith:
                                        if (model.DealershipZipCode.Substring(0, 1) == condition.ComparedValue)
                                            return condition.TargetValue;
                                        break;
                                    case Operators.EndWith:
                                        if (model.DealershipZipCode.Substring(model.DealershipZipCode.Length - 1, 1) ==
                                            condition.ComparedValue) return condition.TargetValue;
                                        break;
                                    default:
                                        break;
                                }
                            }
                            break;
                        case DealershipPhone:
                            if (!String.IsNullOrEmpty(model.DealershipPhone))
                            {
                                switch (condition.Operator)
                                {
                                    case Operators.Equal:
                                        if (model.DealershipPhone == condition.ComparedValue)
                                            return condition.TargetValue;
                                        break;
                                    case Operators.Different:
                                        if (model.DealershipPhone != condition.ComparedValue)
                                            return condition.TargetValue;
                                        break;
                                    case Operators.Contain:
                                        if (model.DealershipPhone.Contains(condition.ComparedValue))
                                            return condition.TargetValue;
                                        break;
                                    case Operators.StartWith:
                                        if (model.DealershipPhone.Substring(0, 1) == condition.ComparedValue)
                                            return condition.TargetValue;
                                        break;
                                    case Operators.EndWith:
                                        if (model.DealershipPhone.Substring(model.DealershipPhone.Length - 1, 1) ==
                                            condition.ComparedValue) return condition.TargetValue;
                                        break;
                                    default:
                                        break;
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            }

            return value;
        }

        public string ApplyExpression(VehicleViewModel model, Expression expression, string value)
        {
            if (expression != null)
            {
                var combinedValue = GetValueAfterApplyExpression(expression.Operator1,
                                                                 GetValueFormMappingField(model, expression.DBField1),
                                                                 GetValueFormMappingField(model, expression.DBField2));
                if (String.IsNullOrEmpty(combinedValue))
                {
                    combinedValue = GetValueAfterApplyExpression(expression.Operator2,
                                                                 GetValueFormMappingField(model, expression.DBField2),
                                                                 GetValueFormMappingField(model, expression.DBField3));
                }
                else
                {
                    combinedValue = GetValueAfterApplyExpression(expression.Operator2,
                                                                 combinedValue,
                                                                 GetValueFormMappingField(model, expression.DBField2));
                }

                return combinedValue ?? value;
            }

            return value;
        }

        public string GetValueFormMappingField(VehicleViewModel model, string name)
        {
            switch (name)
            {
                case StockNumber:
                    return model.StockNumber;
                case Year:
                    return model.Year.ToString();
                case Make:
                    return model.Make;
                case Model:
                    return model.Model;
                case Trim:
                    return model.Trim;
                case Vin:
                    return model.Vin;
                case DaysInInventory:
                    return model.DaysInInventory.ToString();
                case DateInStock:
                    return model.DateInStock.ToString();
                case Mileage:
                    return model.Mileage;
                case SalePrice:
                    return model.SalePrice;
                case NewUsed:
                    return model.NewUsed;
                case ExteriorColor:
                    return model.ExteriorColor;
                case InteriorColor:
                    return model.InteriorColor;
                case InteriorSurface:
                    return model.InteriorSurface;
                case MSRP:
                    return model.MSRP;
                case BodyType:
                    return model.BodyType;
                case EngineType:
                    return model.EngineType;
                case DriveTrain:
                    return model.DriveTrain;
                case Cylinders:
                    return model.Cylinders;
                case Liters:
                    return model.Liters;
                case FuelType:
                    return model.FuelType;
                case FuelEconomyHighWay:
                    return model.FuelEconomyHighWay;
                case FuelEconomyCity:
                    return model.FuelEconomyCity;
                case Tranmission:
                    return model.Tranmission;
                case Doors:
                    return model.Doors;
                case CarsOptions:
                    return model.CarsOptions;
                case Descriptions:
                    return model.Descriptions;
                case CarImageUrl:
                    return model.CarImageUrl;
                case DefaultImageURL:
                    return model.DefaultImageURL;
                case DealershipName:
                    return model.DealershipName;
                case DealershipAddress:
                    return model.DealershipAddress;
                case DealershipCity:
                    return model.DealershipCity;
                case DealershipState:
                    return model.DealershipState;
                case DealershipZipCode:
                    return model.DealershipZipCode;
                case DealershipPhone:
                    return model.DealershipPhone;
                case DealerCost:
                    return model.DealerCost;
                case DealerDiscount:
                    return model.DealerDiscount;
                case DealershipId:
                    return model.DealerId.ToString();
                case Disclaimer:
                    return model.Disclaimer;
                case Certified:
                    return model.Certified.ToString();
                default:
                    return string.Empty;
            }
        }

        #endregion

        private string GetValueAfterApplyExpression(string operatorType, string field1, string field2)
        {
            if (String.IsNullOrEmpty(operatorType)) return null;
            switch (operatorType)
            {
                case Operators.And:
                    return field1 + " " + field2;
                case Operators.Plus:
                    return (Convert.ToInt32(field1) + Convert.ToInt32(field2)).ToString();
                case Operators.Multiply:
                    return (Convert.ToInt32(field1) * Convert.ToInt32(field2)).ToString();
                case Operators.Devide:
                    return (Convert.ToInt32(field1) / Convert.ToInt32(field2)).ToString();
                default:
                    return null;
            }
        }
    }
}
