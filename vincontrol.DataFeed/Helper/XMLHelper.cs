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
    public class XMLHelper
    {
        private CommonHelper _commonHelper;

        public XMLHelper()
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
            xmlDoc.AppendChild(root);

            foreach (var item in model.Mappings)
            {
                var mappingNode = xmlDoc.CreateElement(Node_Mapping);
                mappingNode.SetAttribute(Attribute_Order, item.Order.ToString());

                var dbFieldNode = xmlDoc.CreateElement(Node_DBField);
                dbFieldNode.SetAttribute(Attribute_Name, item.DBField);
                mappingNode.AppendChild(dbFieldNode);

                var xmlFieldNode = xmlDoc.CreateElement(Node_XMLField);
                xmlFieldNode.SetAttribute(Attribute_Name, item.XMLField);
                mappingNode.AppendChild(xmlFieldNode);
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
                    xmlFieldNode.AppendChild(replacesNode);
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
                    xmlFieldNode.AppendChild(conditionsNode);
                }

                root.AppendChild(mappingNode);
            }

            return root.OuterXml;
        }

        public MappingViewModel LoadMappingTemplateFromProfileId(int profileId)
        {
            using (var context = new vincontrolwarehouseEntities())
            {
                var existingImportTemplate = context.importdatafeedprofiles.FirstOrDefault(i => i.Id == profileId);
                if (existingImportTemplate != null)
                {
                    var mappingTemplate = LoadMappingTemplate(existingImportTemplate.Mapping);
                    mappingTemplate.SampleData = existingImportTemplate.SampleData;
                    mappingTemplate.ProfileName = existingImportTemplate.ProfileName;
                    mappingTemplate.CompanyName = existingImportTemplate.CompanyName;
                    mappingTemplate.RunningTime = existingImportTemplate.RunningTime;
                    mappingTemplate.Frequency = existingImportTemplate.Frequency??0;
                    mappingTemplate.Discontinued = existingImportTemplate.Discontinued ?? false;
                    mappingTemplate.SchemaURL = existingImportTemplate.SchemaURL;
                    mappingTemplate.UseSpecificMapping = existingImportTemplate.UseMappingInCode??false;
                    mappingTemplate.Email = existingImportTemplate.Email;
                    return mappingTemplate;
                }

                return LoadMappingTemplate(CreateMappingTemplate(_commonHelper.CreateSampleMappingData()));
            }
        }

        public MappingViewModel LoadMappingTemplate(int dealerId)
        {
            using (var context = new vincontrolwarehouseEntities())
            {
                //var existingImportTemplate = context.dealers_dealersetting.FirstOrDefault(i => i.DealershipId == dealerId);
                //if (existingImportTemplate != null && existingImportTemplate.importdatafeedprofile != null)
                //{
                //    var mappingTemplate = LoadMappingTemplate(existingImportTemplate.importdatafeedprofile.Mapping);
                //    mappingTemplate.SampleData = existingImportTemplate.importdatafeedprofile.SampleData;
                //    return mappingTemplate;
                //}

                return null;//LoadMappingTemplate(CreateMappingTemplate(_commonHelper.CreateSampleMappingData()));
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
                XmlNodeList mappingNodes = root.SelectNodes(Node_Mapping);
                if (mappingNodes != null)
                {
                    foreach (XmlNode mappingNode in mappingNodes)
                    {
                        var order = _commonHelper.GetIntValueFromAttribute((XmlElement)mappingNode, Attribute_Order);
                        var dbFieldNode = mappingNode.SelectSingleNode(Node_DBField);
                        var xmlFieldNode = mappingNode.SelectSingleNode(Node_XMLField);
                        
                        if (dbFieldNode != null && xmlFieldNode != null)
                        {
                            var mapping = new Mapping() { Order = order, DBField = _commonHelper.GetStringValueFromAttribute((XmlElement)dbFieldNode, Attribute_Name), XMLField = _commonHelper.GetStringValueFromAttribute((XmlElement)xmlFieldNode, Attribute_Name) };
                            var replacesNode = xmlFieldNode.SelectSingleNode(Node_Replaces);
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

                            var conditionsNode = xmlFieldNode.SelectSingleNode(Node_Conditions);
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

        public Mapping GetMappingField(IEnumerable<Mapping> mappings, string dbField)
        {
            return mappings.FirstOrDefault(i => i.DBField.ToLower().Equals(dbField.ToLower()));
        }
        
        public void ApplyCondition(VehicleViewModel model, Condition condition)
        {
            switch (condition.DBField)
            {
                case XMLHelper.StockNumber:
                    model.StockNumber = condition.TargetValue;
                    break;
                case XMLHelper.Year:
                    model.Year = Convert.ToInt32(condition.TargetValue);
                    break;
                case XMLHelper.Make:
                    model.Make = condition.TargetValue;
                    break;
                case XMLHelper.Model:
                    model.Model = condition.TargetValue;
                    break;
                case XMLHelper.Trim:
                    model.Trim = condition.TargetValue;
                    break;
                case XMLHelper.Vin:
                    model.Vin = condition.TargetValue;
                    break;
                case XMLHelper.DaysInInventory:
                    model.DaysInInventory = Convert.ToInt32(condition.TargetValue);
                    break;
                case XMLHelper.Mileage:
                    model.Mileage = condition.TargetValue;
                    break;
                case XMLHelper.SalePrice:
                    model.SalePrice = condition.TargetValue;
                    break;
                case XMLHelper.NewUsed:
                    model.NewUsed = condition.TargetValue;
                    break;
                case XMLHelper.ExteriorColor:
                    model.ExteriorColor = condition.TargetValue;
                    break;
                case XMLHelper.InteriorColor:
                    model.InteriorColor = condition.TargetValue;
                    break;
                case XMLHelper.InteriorSurface:
                    model.InteriorSurface = condition.TargetValue;
                    break;
                case XMLHelper.MSRP:
                    model.MSRP = condition.TargetValue;
                    break;
                case XMLHelper.BodyType:
                    model.BodyType = condition.TargetValue;
                    break;
                case XMLHelper.EngineType:
                    model.EngineType = condition.TargetValue;
                    break;
                case XMLHelper.DriveTrain:
                    model.DriveTrain = condition.TargetValue;
                    break;
                case XMLHelper.Cylinders:
                    model.Cylinders = condition.TargetValue;
                    break;
                case XMLHelper.Liters:
                    model.Liters = condition.TargetValue;
                    break;
                case XMLHelper.FuelType:
                    model.FuelType = condition.TargetValue;
                    break;
                case XMLHelper.FuelEconomyCity:
                    model.FuelEconomyCity = condition.TargetValue;
                    break;
                case XMLHelper.FuelEconomyHighWay:
                    model.FuelEconomyHighWay = condition.TargetValue;
                    break;
                case XMLHelper.Tranmission:
                    model.InteriorColor = condition.TargetValue;
                    break;
                case XMLHelper.Doors:
                    model.Doors = condition.TargetValue;
                    break;
                case XMLHelper.Certified:
                    model.Certified = Convert.ToBoolean(condition.TargetValue);
                    break;
                case XMLHelper.CarsOptions:
                    model.CarsOptions = condition.TargetValue;
                    break;
                case XMLHelper.Descriptions:
                    model.Descriptions = condition.TargetValue;
                    break;
                case XMLHelper.CarImageUrl:
                    model.CarImageUrl = condition.TargetValue;
                    break;
                case XMLHelper.DefaultImageURL:
                    model.DefaultImageURL = condition.TargetValue;
                    break;
                case XMLHelper.DealershipName:
                    model.DealershipName = condition.TargetValue;
                    break;
                case XMLHelper.DealershipAddress:
                    model.DealershipAddress = condition.TargetValue;
                    break;
                case XMLHelper.DealershipCity:
                    model.DealershipCity = condition.TargetValue;
                    break;
                case XMLHelper.DealershipState:
                    model.DealershipState = condition.TargetValue;
                    break;
                case XMLHelper.DealershipZipCode:
                    model.DealershipZipCode = condition.TargetValue;
                    break;
                case XMLHelper.DealershipPhone:
                    model.DealershipPhone = condition.TargetValue;
                    break;
                case XMLHelper.DealerCost:
                    model.DealerCost = condition.TargetValue;
                    break;
                case XMLHelper.DealerDiscount:
                    model.DealerDiscount = condition.TargetValue;
                    break;
                case XMLHelper.ACV:
                    model.ACV = condition.TargetValue;
                    break;
                default:
                    break;
            }
        }

        public void GetStringValueAfterApplyCondition(VehicleViewModel vehicleViewModel, MappingViewModel model, string name)
        {
            // parse conditions
            if (model.Mappings == null || model.Mappings.Count <= 0) return;
            
            foreach (var mapping in model.Mappings)
            {
                if (mapping.Conditions != null && mapping.Conditions.Any() && mapping.DBField.Equals(name))
                {
                    foreach (var condition in mapping.Conditions)
                    {
                        switch (name)
                        {
                            case StockNumber:
                                if (!String.IsNullOrEmpty(vehicleViewModel.StockNumber))
                                {
                                    switch (condition.Operator)
                                    {
                                        case Operators.Equal:
                                            if (vehicleViewModel.StockNumber == condition.ComparedValue) ApplyCondition(vehicleViewModel, condition); break;
                                        case Operators.Different:
                                            if (vehicleViewModel.StockNumber != condition.ComparedValue) ApplyCondition(vehicleViewModel, condition); break;
                                        case Operators.Contain:
                                            if (vehicleViewModel.StockNumber.Contains(condition.ComparedValue)) ApplyCondition(vehicleViewModel, condition); break;
                                        case Operators.StartWith:
                                            if (vehicleViewModel.StockNumber.Length >= condition.ComparedValue.Length && vehicleViewModel.StockNumber.Substring(0, condition.ComparedValue.Length) == condition.ComparedValue) ApplyCondition(vehicleViewModel, condition); break;
                                        case Operators.EndWith:
                                            if (vehicleViewModel.StockNumber.Length >= condition.ComparedValue.Length && vehicleViewModel.StockNumber.Substring(vehicleViewModel.StockNumber.Length - condition.ComparedValue.Length, condition.ComparedValue.Length) == condition.ComparedValue) ApplyCondition(vehicleViewModel, condition); break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            case Make:
                                if (!String.IsNullOrEmpty(vehicleViewModel.Make))
                                {
                                    switch (condition.Operator)
                                    {
                                        case Operators.Equal:
                                            if (vehicleViewModel.Make == condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.Different:
                                            if (vehicleViewModel.Make != condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.Contain:
                                            if (vehicleViewModel.Make.Contains(condition.ComparedValue))
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.StartWith:
                                            if (vehicleViewModel.Make.Length >= condition.ComparedValue.Length && vehicleViewModel.Make.Substring(0, condition.ComparedValue.Length) == condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.EndWith:
                                            if (vehicleViewModel.Make.Length >= condition.ComparedValue.Length && vehicleViewModel.Make.Substring(Make.Length - condition.ComparedValue.Length, condition.ComparedValue.Length) ==
                                                condition.ComparedValue) ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            case Model:
                                if (!String.IsNullOrEmpty(vehicleViewModel.Model))
                                {
                                    switch (condition.Operator)
                                    {
                                        case Operators.Equal:
                                            if (vehicleViewModel.Model == condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.Different:
                                            if (vehicleViewModel.Model != condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.Contain:
                                            if (vehicleViewModel.Model.Contains(condition.ComparedValue))
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.StartWith:
                                            if (vehicleViewModel.Model.Length >= condition.ComparedValue.Length && vehicleViewModel.Model.Substring(0, condition.ComparedValue.Length) == condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.EndWith:
                                            if (vehicleViewModel.Model.Length >= condition.ComparedValue.Length &&
                                                vehicleViewModel.Model.Substring(vehicleViewModel.Model.Length - condition.ComparedValue.Length, condition.ComparedValue.Length) ==
                                                condition.ComparedValue) ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            case Trim:
                                if (!String.IsNullOrEmpty(vehicleViewModel.Trim))
                                {
                                    switch (condition.Operator)
                                    {
                                        case Operators.Equal:
                                            if (vehicleViewModel.Trim == condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.Different:
                                            if (vehicleViewModel.Trim != condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.Contain:
                                            if (vehicleViewModel.Trim.Contains(condition.ComparedValue))
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.StartWith:
                                            if (vehicleViewModel.Trim.Length >= condition.ComparedValue.Length && vehicleViewModel.Trim.Substring(0, condition.ComparedValue.Length) == condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.EndWith:
                                            if (vehicleViewModel.Trim.Length >= condition.ComparedValue.Length && vehicleViewModel.Trim.Substring(vehicleViewModel.Trim.Length - condition.ComparedValue.Length, condition.ComparedValue.Length) ==
                                                condition.ComparedValue) ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            case Vin:
                                if (!String.IsNullOrEmpty(vehicleViewModel.Vin))
                                {
                                    switch (condition.Operator)
                                    {
                                        case Operators.Equal:
                                            if (vehicleViewModel.Vin == condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.Different:
                                            if (vehicleViewModel.Vin != condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.Contain:
                                            if (vehicleViewModel.Vin.Contains(condition.ComparedValue))
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.StartWith:
                                            if (vehicleViewModel.Vin.Length >= condition.ComparedValue.Length &&
                                                vehicleViewModel.Vin.Substring(0, condition.ComparedValue.Length) == condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.EndWith:
                                            if (vehicleViewModel.Vin.Length >= condition.ComparedValue.Length &&
                                                vehicleViewModel.Vin.Substring(vehicleViewModel.Vin.Length - condition.ComparedValue.Length, condition.ComparedValue.Length) ==
                                                condition.ComparedValue) ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            case Mileage:
                                if (!String.IsNullOrEmpty(vehicleViewModel.Mileage))
                                {
                                    switch (condition.Operator)
                                    {
                                        case Operators.Equal:
                                            if (vehicleViewModel.Mileage == condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.Different:
                                            if (vehicleViewModel.Mileage != condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.Contain:
                                            if (vehicleViewModel.Mileage.Contains(condition.ComparedValue))
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.StartWith:
                                            if (vehicleViewModel.Mileage.Length >= condition.ComparedValue.Length &&
                                                vehicleViewModel.Mileage.Substring(0, condition.ComparedValue.Length) == condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.EndWith:
                                            if (vehicleViewModel.Mileage.Length >= condition.ComparedValue.Length &&
                                                vehicleViewModel.Mileage.Substring(StockNumber.Length - condition.ComparedValue.Length, condition.ComparedValue.Length) ==
                                                condition.ComparedValue) ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            case SalePrice:
                                if (!String.IsNullOrEmpty(vehicleViewModel.SalePrice))
                                {
                                    switch (condition.Operator)
                                    {
                                        case Operators.Equal:
                                            if (vehicleViewModel.SalePrice == condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.Different:
                                            if (vehicleViewModel.SalePrice != condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.Contain:
                                            if (vehicleViewModel.SalePrice.Contains(condition.ComparedValue))
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.StartWith:
                                            if (vehicleViewModel.SalePrice.Length >= condition.ComparedValue.Length &&
                                                vehicleViewModel.SalePrice.Substring(0, condition.ComparedValue.Length) == condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.EndWith:
                                            if (vehicleViewModel.SalePrice.Length >= condition.ComparedValue.Length &&
                                                vehicleViewModel.SalePrice.Substring(
                                                    vehicleViewModel.SalePrice.Length - condition.ComparedValue.Length, condition.ComparedValue.Length) == condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            case NewUsed:
                                if (!String.IsNullOrEmpty(vehicleViewModel.NewUsed))
                                {
                                    switch (condition.Operator)
                                    {
                                        case Operators.Equal:
                                            if (vehicleViewModel.NewUsed == condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.Different:
                                            if (vehicleViewModel.NewUsed != condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.Contain:
                                            if (vehicleViewModel.NewUsed.Contains(condition.ComparedValue))
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.StartWith:
                                            if (vehicleViewModel.NewUsed.Length >= condition.ComparedValue.Length &&
                                                vehicleViewModel.NewUsed.Substring(0, condition.ComparedValue.Length) == condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.EndWith:
                                            if (vehicleViewModel.NewUsed.Length >= condition.ComparedValue.Length &&
                                                vehicleViewModel.NewUsed.Substring(vehicleViewModel.NewUsed.Length - condition.ComparedValue.Length,
                                                                                   condition.ComparedValue.Length) == condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            case ExteriorColor:
                                if (!String.IsNullOrEmpty(vehicleViewModel.ExteriorColor))
                                {
                                    switch (condition.Operator)
                                    {
                                        case Operators.Equal:
                                            if (vehicleViewModel.ExteriorColor == condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.Different:
                                            if (vehicleViewModel.ExteriorColor != condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.Contain:
                                            if (vehicleViewModel.ExteriorColor.Contains(condition.ComparedValue))
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.StartWith:
                                            if (vehicleViewModel.ExteriorColor.Length >= condition.ComparedValue.Length &&
                                                vehicleViewModel.ExteriorColor.Substring(0, condition.ComparedValue.Length) ==
                                                condition.ComparedValue) ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.EndWith:
                                            if (vehicleViewModel.ExteriorColor.Length >= condition.ComparedValue.Length &&
                                                vehicleViewModel.ExteriorColor.Substring(
                                                    vehicleViewModel.ExteriorColor.Length - condition.ComparedValue.Length, condition.ComparedValue.Length) ==
                                                condition.ComparedValue) ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            case InteriorColor:
                                if (!String.IsNullOrEmpty(vehicleViewModel.InteriorColor))
                                {
                                    switch (condition.Operator)
                                    {
                                        case Operators.Equal:
                                            if (vehicleViewModel.InteriorColor == condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.Different:
                                            if (vehicleViewModel.InteriorColor != condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.Contain:
                                            if (vehicleViewModel.InteriorColor.Contains(condition.ComparedValue))
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.StartWith:
                                            if (vehicleViewModel.InteriorColor.Length >= condition.ComparedValue.Length &&
                                                vehicleViewModel.InteriorColor.Substring(0, condition.ComparedValue.Length) ==
                                                condition.ComparedValue) ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.EndWith:
                                            if (vehicleViewModel.InteriorColor.Length >= condition.ComparedValue.Length &&
                                                vehicleViewModel.InteriorColor.Substring(
                                                    vehicleViewModel.InteriorColor.Length - condition.ComparedValue.Length, condition.ComparedValue.Length) ==
                                                condition.ComparedValue) ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            case InteriorSurface:
                                if (!String.IsNullOrEmpty(vehicleViewModel.InteriorSurface))
                                {
                                    switch (condition.Operator)
                                    {
                                        case Operators.Equal:
                                            if (vehicleViewModel.InteriorSurface == condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.Different:
                                            if (vehicleViewModel.InteriorSurface != condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.Contain:
                                            if (vehicleViewModel.InteriorSurface.Contains(condition.ComparedValue))
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.StartWith:
                                            if (vehicleViewModel.InteriorSurface.Substring(0, 1) ==
                                                condition.ComparedValue) ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.EndWith:
                                            if (
                                                vehicleViewModel.InteriorSurface.Substring(
                                                    vehicleViewModel.InteriorSurface.Length - 1, 1) ==
                                                condition.ComparedValue) ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            case WindowStickerPrice:
                                if (!String.IsNullOrEmpty(vehicleViewModel.WindowStickerPrice))
                                {
                                    switch (condition.Operator)
                                    {
                                        case Operators.Equal:
                                            if (vehicleViewModel.WindowStickerPrice == condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.Different:
                                            if (vehicleViewModel.WindowStickerPrice != condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.Contain:
                                            if (vehicleViewModel.WindowStickerPrice.Contains(condition.ComparedValue))
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.StartWith:
                                            if (vehicleViewModel.WindowStickerPrice.Substring(0, 1) ==
                                                condition.ComparedValue) ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.EndWith:
                                            if (
                                                vehicleViewModel.WindowStickerPrice.Substring(
                                                    vehicleViewModel.WindowStickerPrice.Length - 1, 1) ==
                                                condition.ComparedValue) ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            case MSRP:
                                if (!String.IsNullOrEmpty(vehicleViewModel.MSRP))
                                {
                                    switch (condition.Operator)
                                    {
                                        case Operators.Equal:
                                            if (vehicleViewModel.MSRP == condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.Different:
                                            if (vehicleViewModel.MSRP != condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.Contain:
                                            if (vehicleViewModel.MSRP.Contains(condition.ComparedValue))
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.StartWith:
                                            if (vehicleViewModel.MSRP.Substring(0, 1) == condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.EndWith:
                                            if (vehicleViewModel.MSRP.Substring(vehicleViewModel.MSRP.Length - 1, 1) ==
                                                condition.ComparedValue) ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            case BodyType:
                                if (!String.IsNullOrEmpty(vehicleViewModel.BodyType))
                                {
                                    switch (condition.Operator)
                                    {
                                        case Operators.Equal:
                                            if (vehicleViewModel.BodyType == condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.Different:
                                            if (vehicleViewModel.BodyType != condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.Contain:
                                            if (vehicleViewModel.BodyType.Contains(condition.ComparedValue))
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.StartWith:
                                            if (vehicleViewModel.BodyType.Substring(0, 1) == condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.EndWith:
                                            if (
                                                vehicleViewModel.BodyType.Substring(
                                                    vehicleViewModel.BodyType.Length - 1, 1) == condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            case EngineType:
                                if (!String.IsNullOrEmpty(vehicleViewModel.EngineType))
                                {
                                    switch (condition.Operator)
                                    {
                                        case Operators.Equal:
                                            if (vehicleViewModel.EngineType == condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.Different:
                                            if (vehicleViewModel.EngineType != condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.Contain:
                                            if (vehicleViewModel.EngineType.Contains(condition.ComparedValue))
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.StartWith:
                                            if (vehicleViewModel.EngineType.Substring(0, 1) == condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.EndWith:
                                            if (
                                                vehicleViewModel.EngineType.Substring(
                                                    vehicleViewModel.EngineType.Length - 1, 1) ==
                                                condition.ComparedValue) ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            case DriveTrain:
                                if (!String.IsNullOrEmpty(vehicleViewModel.DriveTrain))
                                {
                                    switch (condition.Operator)
                                    {
                                        case Operators.Equal:
                                            if (vehicleViewModel.DriveTrain == condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.Different:
                                            if (vehicleViewModel.DriveTrain != condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.Contain:
                                            if (vehicleViewModel.DriveTrain.Contains(condition.ComparedValue))
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.StartWith:
                                            if (vehicleViewModel.DriveTrain.Length >= condition.ComparedValue.Length &&
                                                vehicleViewModel.DriveTrain.Substring(0, condition.ComparedValue.Length) == condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.EndWith:
                                            if (vehicleViewModel.DriveTrain.Length >= condition.ComparedValue.Length &&
                                                vehicleViewModel.DriveTrain.Substring(
                                                    vehicleViewModel.DriveTrain.Length - condition.ComparedValue.Length, condition.ComparedValue.Length) ==
                                                condition.ComparedValue) ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            case Cylinders:
                                if (!String.IsNullOrEmpty(vehicleViewModel.Cylinders))
                                {
                                    switch (condition.Operator)
                                    {
                                        case Operators.Equal:
                                            if (vehicleViewModel.Cylinders == condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.Different:
                                            if (vehicleViewModel.Cylinders != condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.Contain:
                                            if (vehicleViewModel.Cylinders.Contains(condition.ComparedValue))
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.StartWith:
                                            if (vehicleViewModel.Cylinders.Length >= condition.ComparedValue.Length &&
                                                vehicleViewModel.Cylinders.Substring(0, condition.ComparedValue.Length) == condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.EndWith:
                                            if (vehicleViewModel.Cylinders.Length >= condition.ComparedValue.Length &&
                                                vehicleViewModel.Cylinders.Substring(
                                                    vehicleViewModel.Cylinders.Length - condition.ComparedValue.Length, condition.ComparedValue.Length) == condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            case Liters:
                                if (!String.IsNullOrEmpty(vehicleViewModel.Liters))
                                {
                                    switch (condition.Operator)
                                    {
                                        case Operators.Equal:
                                            if (vehicleViewModel.Liters == condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.Different:
                                            if (vehicleViewModel.Liters != condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.Contain:
                                            if (vehicleViewModel.Liters.Contains(condition.ComparedValue))
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.StartWith:
                                            if (vehicleViewModel.Liters.Length >= condition.ComparedValue.Length &&
                                                vehicleViewModel.Liters.Substring(0, condition.ComparedValue.Length) == condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.EndWith:
                                            if (vehicleViewModel.Liters.Length >= condition.ComparedValue.Length &&
                                                vehicleViewModel.Liters.Substring(vehicleViewModel.Liters.Length - condition.ComparedValue.Length, condition.ComparedValue.Length) ==
                                                condition.ComparedValue) ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            case FuelType:
                                if (!String.IsNullOrEmpty(vehicleViewModel.FuelType))
                                {
                                    switch (condition.Operator)
                                    {
                                        case Operators.Equal:
                                            if (vehicleViewModel.FuelType == condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.Different:
                                            if (vehicleViewModel.FuelType != condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.Contain:
                                            if (vehicleViewModel.FuelType.Contains(condition.ComparedValue))
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.StartWith:
                                            if (vehicleViewModel.FuelType.Length >= condition.ComparedValue.Length &&
                                                vehicleViewModel.FuelType.Substring(0, condition.ComparedValue.Length) == condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.EndWith:
                                            if (vehicleViewModel.FuelType.Length >= condition.ComparedValue.Length &&
                                                vehicleViewModel.FuelType.Substring(
                                                    vehicleViewModel.FuelType.Length - condition.ComparedValue.Length, condition.ComparedValue.Length) == condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            case Tranmission:
                                if (!String.IsNullOrEmpty(vehicleViewModel.Tranmission))
                                {
                                    switch (condition.Operator)
                                    {
                                        case Operators.Equal:
                                            if (vehicleViewModel.Tranmission == condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.Different:
                                            if (vehicleViewModel.Tranmission != condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.Contain:
                                            if (vehicleViewModel.Tranmission.Contains(condition.ComparedValue))
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.StartWith:
                                            if (vehicleViewModel.Tranmission.Length >= condition.ComparedValue.Length &&
                                                vehicleViewModel.Tranmission.Substring(0, condition.ComparedValue.Length) == condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.EndWith:
                                            if (vehicleViewModel.Tranmission.Length >= condition.ComparedValue.Length &&
                                                vehicleViewModel.Tranmission.Substring(
                                                    vehicleViewModel.Tranmission.Length - condition.ComparedValue.Length, condition.ComparedValue.Length) ==
                                                condition.ComparedValue) ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            case CarsOptions:
                                if (!String.IsNullOrEmpty(vehicleViewModel.CarsOptions))
                                {
                                    switch (condition.Operator)
                                    {
                                        case Operators.Equal:
                                            if (vehicleViewModel.CarsOptions == condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.Different:
                                            if (vehicleViewModel.CarsOptions != condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.Contain:
                                            if (vehicleViewModel.CarsOptions.Contains(condition.ComparedValue))
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.StartWith:
                                            if (vehicleViewModel.CarsOptions.Length >= condition.ComparedValue.Length &&
                                                vehicleViewModel.CarsOptions.Substring(0, condition.ComparedValue.Length) == condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.EndWith:
                                            if (vehicleViewModel.CarsOptions.Length >= condition.ComparedValue.Length &&
                                                vehicleViewModel.CarsOptions.Substring(
                                                    vehicleViewModel.CarsOptions.Length - condition.ComparedValue.Length, condition.ComparedValue.Length) ==
                                                condition.ComparedValue) ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            case CarImageUrl:
                                if (!String.IsNullOrEmpty(vehicleViewModel.CarImageUrl))
                                {
                                    switch (condition.Operator)
                                    {
                                        case Operators.Equal:
                                            if (vehicleViewModel.CarImageUrl == condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.Different:
                                            if (vehicleViewModel.CarImageUrl != condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.Contain:
                                            if (vehicleViewModel.CarImageUrl.Contains(condition.ComparedValue))
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.StartWith:
                                            if (vehicleViewModel.CarImageUrl.Length >= condition.ComparedValue.Length &&
                                                vehicleViewModel.CarImageUrl.Substring(0, condition.ComparedValue.Length) == condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.EndWith:
                                            if (vehicleViewModel.CarImageUrl.Length >= condition.ComparedValue.Length &&
                                                vehicleViewModel.CarImageUrl.Substring(
                                                    vehicleViewModel.CarImageUrl.Length - condition.ComparedValue.Length, condition.ComparedValue.Length) ==
                                                condition.ComparedValue) ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            case DealershipName:
                                if (!String.IsNullOrEmpty(vehicleViewModel.DealershipName))
                                {
                                    switch (condition.Operator)
                                    {
                                        case Operators.Equal:
                                            if (vehicleViewModel.DealershipName == condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.Different:
                                            if (vehicleViewModel.DealershipName != condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.Contain:
                                            if (vehicleViewModel.DealershipName.Contains(condition.ComparedValue))
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.StartWith:
                                            if (vehicleViewModel.DealershipName.Length >= condition.ComparedValue.Length &&
                                                vehicleViewModel.DealershipName.Substring(0, condition.ComparedValue.Length) ==
                                                condition.ComparedValue) ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.EndWith:
                                            if (vehicleViewModel.DealershipName.Length >= condition.ComparedValue.Length &&
                                                vehicleViewModel.DealershipName.Substring(
                                                    vehicleViewModel.DealershipName.Length - condition.ComparedValue.Length, condition.ComparedValue.Length) ==
                                                condition.ComparedValue) ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            case DealershipAddress:
                                if (!String.IsNullOrEmpty(vehicleViewModel.DealershipAddress))
                                {
                                    switch (condition.Operator)
                                    {
                                        case Operators.Equal:
                                            if (vehicleViewModel.DealershipAddress == condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.Different:
                                            if (vehicleViewModel.DealershipAddress != condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.Contain:
                                            if (vehicleViewModel.DealershipAddress.Contains(condition.ComparedValue))
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.StartWith:
                                            if (vehicleViewModel.DealershipAddress.Length >= condition.ComparedValue.Length && vehicleViewModel.DealershipAddress.Substring(0, condition.ComparedValue.Length) ==
                                                condition.ComparedValue) ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.EndWith:
                                            if (vehicleViewModel.DealershipAddress.Length >= condition.ComparedValue.Length &&
                                                vehicleViewModel.DealershipAddress.Substring(
                                                    vehicleViewModel.DealershipAddress.Length - condition.ComparedValue.Length, condition.ComparedValue.Length) ==
                                                condition.ComparedValue) ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            case DealershipCity:
                                if (!String.IsNullOrEmpty(vehicleViewModel.DealershipCity))
                                {
                                    switch (condition.Operator)
                                    {
                                        case Operators.Equal:
                                            if (vehicleViewModel.DealershipCity == condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.Different:
                                            if (vehicleViewModel.DealershipCity != condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.Contain:
                                            if (vehicleViewModel.DealershipCity.Contains(condition.ComparedValue))
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.StartWith:
                                            if (vehicleViewModel.DealershipCity.Length >= condition.ComparedValue.Length && vehicleViewModel.DealershipCity.Substring(0, condition.ComparedValue.Length) ==
                                                condition.ComparedValue) ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.EndWith:
                                            if (vehicleViewModel.DealershipCity.Length >= condition.ComparedValue.Length && 
                                                vehicleViewModel.DealershipCity.Substring(
                                                    vehicleViewModel.DealershipCity.Length - condition.ComparedValue.Length, condition.ComparedValue.Length) ==
                                                condition.ComparedValue) ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            case DealershipState:
                                if (!String.IsNullOrEmpty(vehicleViewModel.DealershipState))
                                {
                                    switch (condition.Operator)
                                    {
                                        case Operators.Equal:
                                            if (vehicleViewModel.DealershipState == condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.Different:
                                            if (vehicleViewModel.DealershipState != condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.Contain:
                                            if (vehicleViewModel.DealershipState.Contains(condition.ComparedValue))
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.StartWith:
                                            if (vehicleViewModel.DealershipState.Length >= condition.ComparedValue.Length && vehicleViewModel.DealershipState.Substring(0, condition.ComparedValue.Length) ==
                                                condition.ComparedValue) ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.EndWith:
                                            if (vehicleViewModel.DealershipState.Length >= condition.ComparedValue.Length && 
                                                vehicleViewModel.DealershipState.Substring(
                                                    vehicleViewModel.DealershipState.Length - condition.ComparedValue.Length, condition.ComparedValue.Length) ==
                                                condition.ComparedValue) ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            case DealershipZipCode:
                                if (!String.IsNullOrEmpty(vehicleViewModel.DealershipZipCode))
                                {
                                    switch (condition.Operator)
                                    {
                                        case Operators.Equal:
                                            if (vehicleViewModel.DealershipZipCode == condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.Different:
                                            if (vehicleViewModel.DealershipZipCode != condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.Contain:
                                            if (vehicleViewModel.DealershipZipCode.Contains(condition.ComparedValue))
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.StartWith:
                                            if (vehicleViewModel.DealershipZipCode.Length >= condition.ComparedValue.Length && vehicleViewModel.DealershipZipCode.Substring(0, condition.ComparedValue.Length) ==
                                                condition.ComparedValue) ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.EndWith:
                                            if (vehicleViewModel.DealershipZipCode.Length >= condition.ComparedValue.Length && 
                                                vehicleViewModel.DealershipZipCode.Substring(
                                                    vehicleViewModel.DealershipZipCode.Length - condition.ComparedValue.Length, condition.ComparedValue.Length) ==
                                                condition.ComparedValue) ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            case DealershipPhone:
                                if (!String.IsNullOrEmpty(vehicleViewModel.DealershipPhone))
                                {
                                    switch (condition.Operator)
                                    {
                                        case Operators.Equal:
                                            if (vehicleViewModel.DealershipPhone == condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.Different:
                                            if (vehicleViewModel.DealershipPhone != condition.ComparedValue)
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.Contain:
                                            if (vehicleViewModel.DealershipPhone.Contains(condition.ComparedValue))
                                                ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.StartWith:
                                            if (vehicleViewModel.DealershipPhone.Length >= condition.ComparedValue.Length &&
                                                vehicleViewModel.DealershipPhone.Substring(0, condition.ComparedValue.Length) ==
                                                condition.ComparedValue) ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        case Operators.EndWith:
                                            if (vehicleViewModel.DealershipPhone.Length >= condition.ComparedValue.Length &&
                                                vehicleViewModel.DealershipPhone.Substring(
                                                    vehicleViewModel.DealershipPhone.Length - condition.ComparedValue.Length, condition.ComparedValue.Length) ==
                                                condition.ComparedValue) ApplyCondition(vehicleViewModel, condition);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                        }
                    }
                }
            }
        }

        public void GetIntValueAfterApplyCondition(VehicleViewModel vehicleViewModel, MappingViewModel model, string name)
        {
            // parse conditions
            if (model.Mappings == null || model.Mappings.Count <= 0) return;

            foreach (var mapping in model.Mappings)
            {
                if (mapping.Conditions != null && mapping.Conditions.Any() && mapping.DBField.Equals(name))
                {
                    foreach (var condition in mapping.Conditions)
                    {
                        switch (name)
                        {
                            case Year:
                                switch (condition.Operator)
                                {
                                    case Operators.Equal:
                                        if (vehicleViewModel.Year == Convert.ToInt32(condition.ComparedValue)) ApplyCondition(vehicleViewModel, condition); break;
                                    case Operators.LessThan:
                                        if (vehicleViewModel.Year < Convert.ToInt32(condition.ComparedValue)) ApplyCondition(vehicleViewModel, condition); break;
                                    case Operators.LessThanOrEqual:
                                        if (vehicleViewModel.Year <= Convert.ToInt32(condition.ComparedValue)) ApplyCondition(vehicleViewModel, condition); break;
                                    case Operators.GreaterThan:
                                        if (vehicleViewModel.Year > Convert.ToInt32(condition.ComparedValue)) ApplyCondition(vehicleViewModel, condition); break;
                                    case Operators.GreaterThanOrEqual:
                                        if (vehicleViewModel.Year >= Convert.ToInt32(condition.ComparedValue)) ApplyCondition(vehicleViewModel, condition); break;
                                    default: break;
                                }
                                break;
                            case DaysInInventory:
                                switch (condition.Operator)
                                {
                                    case Operators.Equal:
                                        if (vehicleViewModel.DaysInInventory == Convert.ToInt32(condition.ComparedValue)) ApplyCondition(vehicleViewModel, condition); break;
                                    case Operators.LessThan:
                                        if (vehicleViewModel.DaysInInventory < Convert.ToInt32(condition.ComparedValue)) ApplyCondition(vehicleViewModel, condition); break;
                                    case Operators.LessThanOrEqual:
                                        if (vehicleViewModel.DaysInInventory <= Convert.ToInt32(condition.ComparedValue)) ApplyCondition(vehicleViewModel, condition); break;
                                    case Operators.GreaterThan:
                                        if (vehicleViewModel.DaysInInventory > Convert.ToInt32(condition.ComparedValue)) ApplyCondition(vehicleViewModel, condition); break;
                                    case Operators.GreaterThanOrEqual:
                                        if (vehicleViewModel.DaysInInventory >= Convert.ToInt32(condition.ComparedValue)) ApplyCondition(vehicleViewModel, condition); break;
                                    default: break;
                                }
                                break;
                        }
                    }
                }
            }
        }

        #endregion
    }
}
