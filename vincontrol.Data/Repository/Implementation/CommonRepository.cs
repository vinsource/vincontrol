using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vincontrol.Data.Model;
using vincontrol.Data.Repository.Interface;
using vincontrol.DomainObject;

namespace vincontrol.Data.Repository.Implementation
{
    public class CommonRepository : ICommonRepository
    {
        private VincontrolEntities _context;

        public CommonRepository(VincontrolEntities context)
        {
            _context = context;
        }

        #region ICommonRepository' Members

        public List<ExtendedSelectListItem> GetChromeYear()
        {
            var result = new List<ExtendedSelectListItem> { new ExtendedSelectListItem() { Text = "Select...", Value = "0", Selected = true } };
            var yearList = _context.YearMakes.Select(x => x.Year).Distinct().ToList();
            result.AddRange(yearList.OrderByDescending(y => y)
                    .Select(ym => new ExtendedSelectListItem
                            {
                                Text = ym.ToString(),
                                Value = ym.ToString()
                            })
                    .ToList());

            return result;
        }

        public List<ExtendedSelectListItem> GetChromeMake(int year)
        {
            var result = new List<ExtendedSelectListItem> { new ExtendedSelectListItem() { Text = "Make...", Value = "Make...", Selected = true } };
            var makeList = _context.YearMakes.Where(ym => ym.Year == year).Join(_context.Makes,
                                       v => v.MakeId,
                                       a => a.MakeId,
                                       (s, c) => new { c.Value, s.MakeId }).Select(o => new { o.Value, o.MakeId }).ToList();

            result.AddRange(makeList.OrderBy(i => i.Value).Select(ym => new ExtendedSelectListItem { Text = ym.Value, Value = ym.MakeId.ToString() }).ToList());

            return result;
        }

        public List<ExtendedSelectListItem> GetChromeModel(int year, int makeId)
        {
            var result = new List<ExtendedSelectListItem> { new ExtendedSelectListItem() { Text = "Model...", Value = "Model...", Selected = true } };
            var firstYearMake = _context.YearMakes.FirstOrDefault(i => i.Year == year && i.MakeId == makeId);
            var modelList =
                _context.Models.Where(m => m.YearMakeId == firstYearMake.YearMakeId)
                        .GroupBy(x => x.Value)
                        .Select(m => new { m.FirstOrDefault().Value, m.FirstOrDefault().ModelId })
                        .ToList();
            result.AddRange(modelList.Select(ym => new ExtendedSelectListItem { Text = ym.Value, Value = ym.ModelId.ToString() }).ToList());

            return result;
        }

        public List<ExtendedSelectListItem> GetChromeTrim(int year, int makeId, int modelId)
        {
            var result = new List<ExtendedSelectListItem> { new ExtendedSelectListItem() { Text = "Trim...", Value = "Trim...", Selected = true } };


            if (makeId == 110 && year <= 2009)
            {
                foreach (var tmp in _context.Trims.Where(t => t.ModelId == modelId))
                {
                    var code = tmp.Code;

                    if (code[code.Length - 1] == 'C' || code[code.Length - 1] == 'A' || code[code.Length - 1] == 'W')
                        code = code.Substring(0, code.Length - 1);

                    result.Add(new ExtendedSelectListItem()
                    {
                        Text = code,
                        Value = tmp.TrimId.ToString(),
                        ImageUrl = tmp.DefaultImageUrl
                    });


                };
            }
            else
            {
                foreach (var tmp in _context.Trims.Where(t => t.ModelId == modelId))
                {
                    result.Add(new ExtendedSelectListItem()
                    {
                        Text = String.IsNullOrEmpty(tmp.TrimName) ? "Base" : tmp.TrimName,
                        Value = tmp.TrimId.ToString(),
                        ImageUrl = tmp.DefaultImageUrl
                    });


                }
                ;
            }
            return result;
        }

        public List<ExtendedSelectListItem> GetTruckTypes()
        {
            return _context.TruckCategories.Select(i => i.TruckType).Distinct().Select(i => new ExtendedSelectListItem() { Text = i, Value = i, Selected = false }).OrderBy(i => i.Text).ToList();
        }

        public List<ExtendedSelectListItem> GetTruckTypes(string selectedTruckType)
        {
            return _context.TruckCategories.Select(i => i.TruckType).Distinct().Select(i => new ExtendedSelectListItem() { Text = i, Value = i, Selected = !String.IsNullOrEmpty(selectedTruckType) && i.Equals(selectedTruckType) }).OrderBy(i => i.Text).ToList();
        }
        
        public List<ExtendedSelectListItem> GetTruckCategories()
        {
            return _context.TruckCategories.AsEnumerable().Select(i => new ExtendedSelectListItem(){ Text = i.CategoryName, Value = i.TruckCategoryId.ToString(), Selected = false }).OrderBy(i => i.Text).ToList();
        }

        public List<ExtendedSelectListItem> GetTruckCategories(int selectedTruckCategory)
        {
            return _context.TruckCategories.Where(i => i.TruckCategoryId.Equals(selectedTruckCategory)).AsEnumerable().Select(i => new ExtendedSelectListItem() { Text = i.CategoryName, Value = i.TruckCategoryId.ToString(), Selected = i.TruckCategoryId.Equals(selectedTruckCategory) }).OrderBy(i => i.Text).ToList();
        }

        public List<ExtendedSelectListItem> GetTruckCategoriesByType(string truckType)
        {
            return _context.TruckCategories.Where(i => i.TruckType.Equals(truckType)).AsEnumerable().Select(i => new ExtendedSelectListItem() { Text = i.CategoryName, Value = i.TruckCategoryId.ToString(), Selected = false }).OrderBy(i => i.Text).ToList();
        }

        public List<ExtendedSelectListItem> GetTruckClasses()
        {
            return _context.TruckClasses.AsEnumerable().Select(i => new ExtendedSelectListItem() { Text = String.Format("{0} ({1})", i.ClassName, i.GVWR.TrimEnd()), Value = i.TruckClassId.ToString(), Selected = false }).OrderBy(i => i.Text).ToList();
        }

        public List<ExtendedSelectListItem> GetTruckClasses(int selectedTruckClass)
        {
            return _context.TruckClasses.AsEnumerable().Select(i => new ExtendedSelectListItem() { Text = String.Format("{0} ({1})", i.ClassName, i.GVWR.TrimEnd()), Value = i.TruckClassId.ToString(), Selected = i.TruckClassId.Equals(selectedTruckClass) }).OrderBy(i => i.Text).ToList();
        }

        public string GetChromeMakeName(int makeId)
        {
            var make = _context.Makes.FirstOrDefault(i => i.MakeId == makeId);
            return make != null ? make.Value : string.Empty;
        }

        public string GetChromeModelName(int modelId)
        {
            var model = _context.Models.FirstOrDefault(i => i.ModelId == modelId);
            return model != null ? model.Value : string.Empty;
        }

        public string GetChromeTrimName(int trimId)
        {
            var trim = _context.Trims.FirstOrDefault(i => i.TrimId == trimId);
            return trim != null ? trim.TrimName : string.Empty;
        }

        public Department GetDepartment(int departmentId)
        {
            return _context.Departments.FirstOrDefault(i => i.DepartmentId == departmentId);
        }

        public IQueryable<Department> GetAllDepartments()
        {
            return _context.Departments;
        }

        public BDC GetBDC(int bdcId)
        {
            return _context.BDCs.FirstOrDefault(i => i.BDCId == bdcId);
        }

        public IQueryable<BDC> GetBDCs()
        {
            return _context.BDCs;
        }

        public IQueryable<State> GetStates()
        {
            return _context.States;
        }

        public bool CheckStateExisting(string name)
        {
            return _context.States.Any(i => i.Name.Equals(name));
        }

        public bool CheckCityExisting(string city)
        {
            return _context.Cities.Any(i => i.Name.Equals(city));
        }

        public void AddNewState(State obj)
        {
            _context.AddToStates(obj);
        }

        public void AddNewCity(City obj)
        {
            _context.AddToCities(obj);
        }

        public void AddNewVSRSchedule(VSRSchedule obj)
        {
            _context.AddToVSRSchedules(obj);
        }

        #endregion
    }
}
