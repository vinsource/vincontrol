using vincontrol.DomainObject;

namespace vincontrol.Application.ViewModels.Chart
{
    public class ChartSelectionViewModel
    {
        public int Id { get; set; }
        public bool IsCarsCom { get; set; }
        public string Options { get; set; }
        public string Vin { get; set; }
        public string Trims { get; set; }
        public bool? IsCertified { get; set; }
        public bool IsAll { get; set; }
        public bool IsFranchise { get; set; }
        public bool IsIndependant { get; set; }
        public string Screen { get; set; }
        //public string Mileage { get; set; }
        //public string SalePrice { get; set; }
        public string PdfContent { get; set; }
        public ChartSelection CarsCom { get; set; }
        public string FilterOptions { get; set; }
        public int Type;
        public short InventoryStatus { get; set; }
        public Car CurrentCar { get; set; }
    }

    public class ChartSelection
    {
        public bool IsCarsCom { get; set; }
        public string Options { get; set; }
        public string Trims { get; set; }
        public bool IsCertified { get; set; }
        public bool IsAll { get; set; }
        public bool IsFranchise { get; set; }
        public bool IsIndependant { get; set; }
        public string Screen { get; set; }
    }

    public class Car
    {
        public int? Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Trim { get; set; }
        public bool IsCertified { get; set; }
        public decimal CertifiedAmount { get; set; }
        public bool ACar { get; set; }
        public decimal MileageAdjustment { get; set; }
        public string Note { get; set; }
    }
}