using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using northwindAPI.Data.ModelDTO;
using northwindAPI.Models;

namespace northwindAPI.MapperService
{
    public class MapperConfig:Profile
    {
        public MapperConfig()
        {
            
            CreateMap<Category,CategoryDTO>().ReverseMap();
            CreateMap<Customer,CustomerDTO>().ReverseMap();
            CreateMap<Employee,EmployeeDTO>().ReverseMap();
            CreateMap<Order,OrderDTO>().ReverseMap();
            CreateMap<OrderDetail,OrderDetailDTO>().ReverseMap();
            CreateMap<Product,ProductDTO>().ReverseMap();
            CreateMap<Region,RegionDTO>().ReverseMap();
            CreateMap<Shipper,ShipperDTO>().ReverseMap();
            CreateMap<Supplier,SupplierDTO>().ReverseMap();
            CreateMap<Territory,TerritoryDTO>().ReverseMap();
        }
    }
}